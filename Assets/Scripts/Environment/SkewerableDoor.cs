using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkewerableDoor : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private Rigidbody2D rigidbody2D;

    [Header("Stun explosion")]

    [SerializeField, Min(0f)]
    private float stunRadius = 3f;

    [SerializeField, Min(0f)]
    private float stunDuration = 2f;

    [SerializeField]
    private LayerMask enemyMask;

    [SerializeField, Min(1)]
    private int maximumEnemies = 20;

    private readonly Collider2D[] stunResults =
        new Collider2D[32];

    [Header("Settings")]
    [SerializeField] private bool destroyOnInteract = true;

    [Header("Position on sword")]
    [SerializeField]
    private Vector3 skeweredLocalPosition = Vector3.zero;
    [SerializeField]
    private float skeweredLocalRotation = 90f;

    [SerializeField, Min(0f)]
    private float destroyDelay;

    private SwordController currentSword;
    private PlayerController currentPlayer;

    private bool isSkewered;
    private bool isDestroyed;

    public bool IsSkewered => isSkewered;

    private void Awake()
    {
        if (blockingCollider == null)
            blockingCollider = GetComponent<Collider2D>();

        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public bool TrySkewer(
        SwordController sword,
        PlayerController player,
        Transform skewerPoint)
    {
        if (isSkewered || isDestroyed)
            return false;

        if (sword == null || player == null || skewerPoint == null)
            return false;

        currentSword = sword;
        currentPlayer = player;
        isSkewered = true;

        // Дверь перестаёт перекрывать проход.
        if (blockingCollider != null)
            blockingCollider.enabled = false;

        if (rigidbody2D != null)
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2D.simulated = false;
        }

        // Закрепляем дверь на конце меча.
        transform.SetParent(skewerPoint);

        transform.localPosition = skeweredLocalPosition;

        transform.localRotation = Quaternion.Euler(
            0f,
            0f,
            skeweredLocalRotation
        );

        // Теперь Space взаимодействует с дверью.
        currentPlayer.SetInteractable(this);

        return true;
    }
    private void StunNearbyEnemies()
    {
        Vector2 center = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
    transform.position,
    stunRadius,
    enemyMask
);

        HashSet<EnemyController> stunnedEnemies = new();

        foreach (Collider2D hit in hits)
        {
            EnemyController enemy =
                hit.GetComponentInParent<EnemyController>();

            if (enemy == null)
                continue;

            if (!stunnedEnemies.Add(enemy))
                continue;

            enemy.Stun(stunDuration);

        }
    }
    public void Interact(PlayerController player)
    {
        if (!isSkewered || isDestroyed)
            return;

        if (player != currentPlayer)
            return;
        StunNearbyEnemies();
        DestroyDoor();
    }

    private void DestroyDoor()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (currentPlayer != null)
            currentPlayer.ClearInteractable(this);

        if (currentSword != null)
            currentSword.RemoveSkeweredDoor(this);

        transform.SetParent(null);

        if (destroyOnInteract)
        {
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (currentPlayer != null)
            currentPlayer.ClearInteractable(this);
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(
            transform.position,
            stunRadius
        );
    }
}
