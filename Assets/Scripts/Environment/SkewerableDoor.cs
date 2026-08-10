using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D))]
public class SkewerableDoor : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField]
    private Transform visual;
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private NavMeshObstacle navMeshObstacle;
    [SerializeField]
    private SkewerableDoorWallSensor wallSensor;
    [SerializeField]
    private float skeweredVisualRotation = 90f;

    private Quaternion originalVisualRotation;


    [SerializeField]
    private WallMaterial swordWallMaterial;

    public WallMaterial SwordWallMaterial =>
        swordWallMaterial;


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
    [SerializeField, Min(0.05f)]
    private float pullOutDuration = 0.2f;
    [SerializeField]
    private AnimationCurve pullOutCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

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
        if (visual != null)
        {
            originalVisualRotation =
                visual.localRotation;
        }

        if (swordWallMaterial == null)
            swordWallMaterial = GetComponent<WallMaterial>();

        if (wallSensor == null)
        {
            wallSensor =
                GetComponentInChildren<SkewerableDoorWallSensor>(true);
        }

        if (blockingCollider == null)
            blockingCollider = GetComponent<Collider2D>();

        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void OpenPassage()
    {
        if (doorCollider != null)
            doorCollider.enabled = false;

        if (navMeshObstacle != null)
            navMeshObstacle.enabled = false;
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

        OpenPassage();

        if (rigidbody2D != null)
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            rigidbody2D.simulated = false;
        }

        StartCoroutine(
    PullDoorToSword(skewerPoint)
);


        // Теперь Space взаимодействует с дверью.
        currentPlayer.SetInteractable(this);

        return true;
    }
    private void StunNearbyEnemies()
    {
        Vector2 center = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            center,
            stunRadius,
            enemyMask
        );

        HashSet<EnemyController> stunnedEnemies = new();

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            EnemyController enemy =
                hit.GetComponentInParent<EnemyController>();

            if (enemy == null)
                continue;

            if (!stunnedEnemies.Add(enemy))
                continue;

            /*
             * OverlapCircle мог попасть в большой дочерний
             * триггер врага. Поэтому дополнительно проверяем
             * расстояние именно до основного коллайдера.
             */
            Collider2D enemyBodyCollider = enemy.Collider;

            if (enemyBodyCollider == null ||
                !enemyBodyCollider.enabled)
            {
                continue;
            }

            Vector2 closestPoint =
                enemyBodyCollider.ClosestPoint(center);

            float distanceToEnemy =
                Vector2.Distance(center, closestPoint);

            if (distanceToEnemy > stunRadius)
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
    public void BreakFromWallImpact()
    {
        if (!isSkewered || isDestroyed)
            return;

        StunNearbyEnemies();
        DestroyDoor();
    }
    private void DestroyDoor()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (wallSensor != null)
            wallSensor.DisableSensor();

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

    private IEnumerator PullDoorToSword(
    Transform skewerPoint)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 targetPosition =
            skewerPoint.TransformPoint(skeweredLocalPosition);

        Quaternion targetRotation =
            skewerPoint.rotation *
            Quaternion.Euler(
                0f,
                0f,
                skeweredLocalRotation
            );

        // Направление, в котором дверь вырывается.
        Vector3 pullDirection =
            (skewerPoint.position - startPosition).normalized;

        // Сначала дверь немного выдёргивается из проёма.
        Vector3 breakPosition =
            startPosition +
            pullDirection * 0.25f;

        float timer = 0f;

        while (timer < pullOutDuration)
        {
            timer += Time.deltaTime;

            float normalizedTime =
                Mathf.Clamp01(
                    timer / pullOutDuration
                );

            float t =
                pullOutCurve.Evaluate(normalizedTime);

            // ПЕРВАЯ ФАЗА:
            // дверь вырывается из проёма.
            if (t < 0.25f)
            {
                float phase =
                    t / 0.25f;

                transform.position =
                    Vector3.Lerp(
                        startPosition,
                        breakPosition,
                        phase
                    );
            }

            // ВТОРАЯ ФАЗА:
            // дверь летит к кончику меча.
            else
            {
                float phase =
                    (t - 0.25f) / 0.75f;

                transform.position =
                    Vector3.Lerp(
                        breakPosition,
                        targetPosition,
                        phase
                    );
            }

            // Одновременно постепенно поворачиваем дверь.
            transform.rotation =
                Quaternion.Lerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }

        transform.SetParent(skewerPoint);

        transform.localPosition =
            skeweredLocalPosition;

        transform.localRotation =
            Quaternion.identity;

        if (visual != null)
        {
            visual.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    skeweredVisualRotation
                );
        }

        if (wallSensor != null)
            wallSensor.EnableSensor();
    }
}