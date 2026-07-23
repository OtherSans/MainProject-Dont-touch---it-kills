using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private SwordController swordContr;
    [SerializeField] private float maxAttackDamage;
    [SerializeField] private float minAttackDamage;
    [SerializeField] private float maxSwordSpeed;
    [SerializeField] private string targetTag;
    //[SerializeField] private float hitStopDuration;
    //[SerializeField] private float shakeDur;
    //[SerializeField] private float shakeStrength;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (swordContr.SkeweredEnemy != null)
            return;

        if (collision.gameObject.CompareTag(targetTag))
        {
            var healthContr = collision.gameObject.GetComponent<HealthController>();
            var spriteFlashContr = collision.gameObject.GetComponent<SpriteFlash>();
            float speed = swordContr.TipVelocity.magnitude;
            float damage = Mathf.Lerp(minAttackDamage, maxAttackDamage, Mathf.Clamp01(speed / maxSwordSpeed));
            healthContr.TakeDamage(damage);
            spriteFlashContr.Flash();
            swordContr.PlayImpact();

            //CameraShake.Instance.Shake(shakeDur, shakeStrength);
            //HitStop.Instance.StopHit(hitStopDuration);
        }
    }
}
