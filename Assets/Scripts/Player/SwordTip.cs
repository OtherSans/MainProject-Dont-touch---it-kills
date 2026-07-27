using UnityEngine;

public class SwordTip : MonoBehaviour
{
    [SerializeField] private SwordController sword;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float hitStopDuration;


    private void OnTriggerEnter2D(Collider2D other)
    {
        TrySkewerEnemy(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        TrySkewerEnemy(other);
    }

    private void TrySkewerEnemy(Collider2D other)
    {
        if (!other.TryGetComponent(out EnemyController enemy))
            return;

        if (!sword.CanSkewer())
            return;

        if (!sword.TrySkewer(enemy))
            return;

        var spriteFlashContr = enemy.gameObject.GetComponent<SpriteFlash>();
        if(spriteFlashContr != null)
            spriteFlashContr.Flash();

        CameraShake.Instance.Shake(shakeDuration, shakeStrength);
        HitStop.Instance.StopHit(hitStopDuration);
        sword.TrySkewer(enemy);
    }
}
