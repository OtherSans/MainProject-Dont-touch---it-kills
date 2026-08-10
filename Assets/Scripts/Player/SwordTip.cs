using UnityEngine;

public class SwordTip : MonoBehaviour
{
    [SerializeField] private SwordController sword;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float hitStopDuration;

    [SerializeField]
    private PlayerController playerController;

    private SkewerableDoor blockingDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryProcessHit(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryProcessHit(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        SkewerableDoor door =
            other.GetComponentInParent<SkewerableDoor>();

        if (door == null)
            return;

        if (door != blockingDoor)
            return;

        blockingDoor = null;

        sword.SetDoorExtensionBlocked(false);

        // Снова разрешаем раскачивание.
        sword.OnWallHitEnd();
    }

    private void TryProcessHit(Collider2D other)
    {
        /*
         * Сначала всегда проверяем дверь.
         * Пока дверь закрыта, враг за ней вообще
         * не должен обрабатываться SwordTip.
         */
        SkewerableDoor door =
            other.GetComponentInParent<SkewerableDoor>();

        if (door != null)
        {
            ProcessDoor(door);
            return;
        }

        ProcessEnemy(other);
    }

    private void ProcessDoor(SkewerableDoor door)
    {
        if (door == null)
            return;

        /*
         * Если дверь уже находится на мече,
         * она больше не является препятствием.
         */
        if (door.IsSkewered)
        {
            ClearDoorBlock(door);
            return;
        }

        /*
         * Нельзя нанизать дверь.
         *
         * Например:
         * - меч слишком медленный;
         * - недостаточно вытянут;
         * - на мече уже есть враг;
         * - меч сейчас не выдвигается.
         *
         * В таком случае дверь работает как стена.
         */
        if (!sword.CanSkewerDoor())
        {
            BlockSwordWithDoor(door);
            return;
        }

        /*
         * Пробуем нанизать.
         */
        bool success =
            sword.TrySkewerDoor(
                door,
                playerController
            );

        if (success)
        {
            /*
             * Дверь снята с прохода —
             * больше меч не блокируем.
             */
            ClearDoorBlock(door);
        }
        else
        {
            /*
             * Если по какой-либо причине
             * TrySkewerDoor не сработал,
             * меч всё равно не проходит насквозь.
             */
            BlockSwordWithDoor(door);
        }
    }

    private void BlockSwordWithDoor(
        SkewerableDoor door)
    {

        blockingDoor = door;

        sword.SetDoorExtensionBlocked(true);

        // Блокируем раскачивание сквозь дверь.
        if (door.SwordWallMaterial != null)
        {
            sword.OnWallHit(
                door.SwordWallMaterial
            );
        }
    }

    private void ClearDoorBlock(
        SkewerableDoor door)
    {
        if (blockingDoor != null &&
        blockingDoor != door)
        {
            return;
        }

        blockingDoor = null;

        sword.SetDoorExtensionBlocked(false);
        sword.OnWallHitEnd();
    }

    private void ProcessEnemy(Collider2D other)
    {
        if (!other.TryGetComponent(
                out EnemyController enemy))
        {
            return;
        }

        if (enemy.PetrifiedController.IsPetrified)
            return;

        // У врага слишком много HP — нанизать нельзя.
        if (!enemy.CanBeSkewered())
            return;

        if (!sword.CanSkewer())
            return;

        if (!sword.TrySkewer(enemy))
            return;

        SpriteFlash spriteFlashContr =
            enemy.GetComponent<SpriteFlash>();

        if (spriteFlashContr != null)
            spriteFlashContr.Flash();

        cameraShake.Shake(shakeStrength);

        HitStop.Instance.StopHit(
            hitStopDuration
        );
    }
}
