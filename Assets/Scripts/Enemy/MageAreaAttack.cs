using System.Collections;
using UnityEngine;

public class MageAreaAttack : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField, Min(0f)]
    private float warningDuration = 1f;

    [SerializeField, Min(0f)]
    private float activeDuration = 0.15f;

    [Header("Damage")]
    [SerializeField, Min(0f)]
    private float damage = 1f;

    [SerializeField]
    private LayerMask playerMask;

    [Header("Visuals")]
    [SerializeField]
    private GameObject warningVisual;

    [SerializeField]
    private GameObject explosionVisual;

    private Collider2D attackCollider;
    private bool hasDealtDamage;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
        attackCollider.isTrigger = true;
        attackCollider.enabled = false;

        if (warningVisual != null)
            warningVisual.SetActive(true);

        if (explosionVisual != null)
            explosionVisual.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(
            warningDuration
        );

        if (warningVisual != null)
            warningVisual.SetActive(false);

        if (explosionVisual != null)
            explosionVisual.SetActive(true);

        hasDealtDamage = false;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(
            activeDuration
        );

        attackCollider.enabled = false;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamagePlayer(other);
    }

    private void TryDamagePlayer(Collider2D other)
    {
        if (hasDealtDamage)
            return;

        int layerBit =
            1 << other.gameObject.layer;

        if ((playerMask.value & layerBit) == 0)
            return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        HealthController health =
            player.GetComponent<HealthController>();

        if (health == null)
            return;

        hasDealtDamage = true;

        health.TakeDamage(damage);
    }
}
