using UnityEngine;

public class SwordTip : MonoBehaviour
{
    [SerializeField] private SwordController sword;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private float hitStopDuration;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!sword.CanSkewer())
            return;

        if (!other.TryGetComponent(out EnemyController enemy))
            return;
        var spriteFlashContr = enemy.gameObject.GetComponent<SpriteFlash>();
        spriteFlashContr.Flash();
        CameraShake.Instance.Shake(shakeDuration, shakeStrength);
        HitStop.Instance.StopHit(hitStopDuration);
        sword.TrySkewer(enemy);
    }
}
