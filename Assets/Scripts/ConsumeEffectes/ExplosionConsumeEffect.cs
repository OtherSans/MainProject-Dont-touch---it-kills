using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionConsumeEffect : ConsumeEffect
{
    [Header("Explosion")]
    [SerializeField, Min(0f)] private float explosionRadius = 4f;

    [Header("Enemy")]
    [SerializeField, Min(0f)]
    private float enemyDamage = 2f;

    [SerializeField, Min(0f)]
    private float enemyKnockbackForce = 8f;

    [SerializeField, Min(0f)]
    private float enemyKnockbackDuration = 0.25f;

    [Header("Player")]
    [SerializeField, Min(0f)]
    private float playerKnockbackForce = 7f;

    [SerializeField, Min(0f)]
    private float playerKnockbackDuration = 0.15f;

    [Header("Detection")]
    [SerializeField]
    private LayerMask enemyMask;

    [Header("Visual")]
    [SerializeField]
    private GameObject explosionVfxPrefab;

    [SerializeField, Min(0f)]
    private float explosionVfxLifetime = 2f;

    [Header("Camera Shake")]
    [SerializeField]
    private CameraShake cameraShake;

    [SerializeField, Min(0f)]
    private float shakeStrength = 1f;

    private EnemyController ownerEnemy;

    private void Awake()
    {
        ownerEnemy = GetComponent<EnemyController>();

        if (cameraShake == null)
        {
            cameraShake =
                FindAnyObjectByType<CameraShake>();
        }
    }
    public override void Apply(PlayerController player)
    {
        Vector2 explosionCenter = transform.position;

        SpawnExplosionVfx(
        explosionCenter
    );

        if (cameraShake != null)
        {
            cameraShake.Shake(
                shakeStrength
            );
        }

        AffectEnemies(
            explosionCenter
        );

        KnockbackPlayer(
            player,
            explosionCenter
        );

    }
    private void SpawnExplosionVfx(
    Vector2 position)
    {
        if (explosionVfxPrefab == null)
            return;

        GameObject effect =
            Instantiate(
                explosionVfxPrefab,
                position,
                Quaternion.identity
            );

        Destroy(
            effect,
            explosionVfxLifetime
        );
    }
    private void AffectEnemies(Vector2 center)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
                center,
                explosionRadius,
                enemyMask
            );
        HashSet<EnemyController> affectedEnemies = new();

        foreach(Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            EnemyController enemy = hit.GetComponentInParent<EnemyController>();

            if (enemy == null)
                continue;

            if (enemy == ownerEnemy)
                continue;

            if (!affectedEnemies.Add(enemy))
                continue;

            Collider2D enemyCollider = enemy.Collider;

            if (enemyCollider == null)
                continue;

            Vector2 closestPoint = enemyCollider.ClosestPoint(center);

            float distance = Vector2.Distance(center, closestPoint);

            if (distance > explosionRadius)
                continue;

            HealthController health = enemy.GetComponent<HealthController>();

            if(health != null)
            {
                health.TakeDamage(enemyDamage);
            }

            enemy.Knockback.KnockbackFrom(center,
                enemyKnockbackForce,
                enemyKnockbackDuration);
        }
    }

    private void KnockbackPlayer(PlayerController player, Vector2 center)
    {
        if (player == null)
            return;
        Vector2 direction = (Vector2)player.transform.position - center;

        if (direction.sqrMagnitude < 0.001f)
            direction = Random.insideUnitCircle.normalized;

        player.ApplyKnockback(direction,
            playerKnockbackForce,
            playerKnockbackDuration);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}

 
