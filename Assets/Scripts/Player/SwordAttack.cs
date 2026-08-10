using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private SwordController swordContr;

    [SerializeField, Min(0f)]
    private float minDamageSpeed = 3f;

    [SerializeField] private RoomAlarmController roomAlarmController;
    [SerializeField] private float maxAttackDamage;
    [SerializeField] private float minAttackDamage;
    [SerializeField] private float maxSwordSpeed;
    [SerializeField] private string targetTag;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (swordContr.SkeweredEnemy != null)
            return;

        if (swordContr.TipSpeed < minDamageSpeed)
            return;

        if (collision.gameObject.CompareTag(targetTag))
        {
            if (!collision.gameObject.GetComponent<EnemyController>().CanReceiveSwordHit())
                return;

            roomAlarmController?.RaiseAlarm();

            var healthContr = collision.gameObject.GetComponent<HealthController>();
            var spriteFlashContr = collision.gameObject.GetComponent<SpriteFlash>();
            float speed = swordContr.TipVelocity.magnitude;
            float damage = Mathf.Lerp(minAttackDamage, maxAttackDamage, Mathf.Clamp01(speed / maxSwordSpeed));
            healthContr.TakeDamage(damage);
            spriteFlashContr.Flash();
            swordContr.PlayImpact();
        }
    }
}
