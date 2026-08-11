using UnityEngine;

public class SpikesController : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField, Min(0f)]
    private float damage = 1f;

    [Header("Knockback")]
    [SerializeField, Min(0f)]
    private float knockbackForce = 8f;

    [SerializeField, Min(0f)]
    private float knockbackDuration = 0.15f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
        other.GetComponent<PlayerController>();

        if (player == null)
            return;

        HealthController health =
            player.GetComponent<HealthController>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Vector2 direction =
            (player.transform.position -
             transform.position).normalized;

        player.ApplyKnockback(
            direction,
            knockbackForce,
            knockbackDuration
        );
    }
}
