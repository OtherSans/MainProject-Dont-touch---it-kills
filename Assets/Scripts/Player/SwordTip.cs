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
        BreakableDoor door =
            other.GetComponentInParent<BreakableDoor>();

        if (door != null)
        {
            ProcessDoor(door);
            return;
        }

        ProcessEnemy(other);
    }

    private void ProcessDoor(BreakableDoor door)
    {
        if (door == null)
            return;

        if (!sword.CanBreakDoor())
            return;

        sword.PlayImpact();

        door.Break();
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
