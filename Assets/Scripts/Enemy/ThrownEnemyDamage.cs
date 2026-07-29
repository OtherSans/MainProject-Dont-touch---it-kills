using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class ThrownEnemyDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField, Min(0f)] private float damage = 2f;
    [SerializeField, Min(0.5f)] private float shakeStrength = 1f;
    [SerializeField, Min(0.01f)] private float hitStopDuration = 0.03f;

    [Header("Requirements")]
    [SerializeField] private float minHitSpeed = 3f;

    [Header("References")]
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private EnemyController ownerEnemy;
    [SerializeField] private CameraShake cameraShake;

    private readonly HashSet<HealthController> damagedTargets = new();

    private bool isActive;

    private void Awake()
    {
        if (enemyRb == null)
        enemyRb = GetComponent<Rigidbody2D>();
        if (ownerEnemy == null)
            ownerEnemy = GetComponent<EnemyController>();
    }
    public void EnableDamage()
    {
        isActive = true;
        damagedTargets.Clear();
    }
    public void DisableDamage()
    {
        isActive = false;
        damagedTargets.Clear();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }
    private void TryDamage(Collider2D other)
    {
        if (!isActive)
            return;
        if (enemyRb.linearVelocity.magnitude < minHitSpeed)
            return;
        EnemyController targetEnemy = other.GetComponent<EnemyController>();

        if (targetEnemy == null)
            return;

        if (targetEnemy == ownerEnemy)
            return;
            
        HealthController targetHealth = other.GetComponent<HealthController>();

        if (targetHealth == null)
            targetHealth =
                targetEnemy.GetComponentInChildren<HealthController>();

        if (targetHealth == null)
            return;

        // Один летящий враг наносит конкретной цели урон только один раз.
        if (!damagedTargets.Add(targetHealth))
            return;

        targetHealth.TakeDamage(damage);
        var spriteFlashContr = other.GetComponent<SpriteFlash>();
        if (spriteFlashContr != null)
            spriteFlashContr.Flash();

        if (cameraShake != null)
            cameraShake.Shake(shakeStrength);
        HitStop.Instance.StopHit(hitStopDuration);
    }
}
