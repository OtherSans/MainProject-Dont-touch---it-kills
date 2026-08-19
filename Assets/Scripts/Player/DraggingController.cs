using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.U2D.Physics.PhysicsQuery;

public class DraggingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;




    [Header("Movement Stats")]

    [SerializeField, Min(0.1f)]
    private float permanentSpeedMultiplier = 1f;

    private float temporarySpeedMultiplier = 1f;

    private float CurrentSpeedMultiplier =>
        permanentSpeedMultiplier *
        temporarySpeedMultiplier;

    private Vector2 storedEdgeDirection;

    [Header("Camera edge movement")]

    [Tooltip("Насколько близко персонаж должен быть к краю камеры")]
    [SerializeField, Range(0.001f, 0.2f)]
    private float playerViewportEdge = 0.05f;

    [Tooltip("Минимальное расстояние курсора от персонажа для автодвижения")]
    [SerializeField, Min(0f)]
    private float minimumCursorDistance = 20f;


    [Header("Drag")]
    [SerializeField, Range(0, 2f)] private float dragSensitivity = 1f;
    [SerializeField, Min(0.01f)]
    private float maxDragSpeed = 30f;

    [Header("Screen edge movement")]
    [Tooltip("Скорость движения, когда курсор находится у края экрана")]
    [SerializeField, Min(0f)]
    private float edgeMoveSpeed = 7f;

    [Tooltip("Слои, которые должны блокировать игрока")]
    [SerializeField]
    private LayerMask obstacleMask;

    [Tooltip("Небольшое расстояние между игроком и стеной")]
    [SerializeField]
    private float collisionOffset = 0.01f;

    [Header("Virtual cursor")]
    [SerializeField, Min(0f)]
    private float virtualCursorSensitivity = 1f;

    [SerializeField, Min(0f)]
    private float maxVirtualCursorDistance = 1000f;

    private Vector2 virtualMouseScreenPosition;
    private bool isUsingVirtualCursor;

    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Vector2 desiredDragVelocity;

    private Vector2 edgeMoveDirection;

    private Vector2 pendingDragMovement;
    private bool isDragging;

    private bool wasNearEdge;

    private readonly RaycastHit2D[] castResults = new RaycastHit2D[8];

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();


    }
    private void OnEnable()
    {
        if (playerController != null)
        {

            playerController.OnDragEvent += DragCheck;

        }
    }
    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnDragEvent -= DragCheck;
        }

    }
    private void Update()
    {
        if (playerController.IsKnockedBack)
        {
            edgeMoveDirection = Vector2.zero;
            desiredDragVelocity = Vector2.zero;
            return;
        }

        if (!isDragging)
        {
            edgeMoveDirection = Vector2.zero;
            desiredDragVelocity = Vector2.zero;
            return;
        }

        Vector2 currentEdgeDirection =
            GetScreenEdgeDirection();

        // Если работает движение у края экрана,
        // обычный drag в этот момент не используем.
        if (currentEdgeDirection != Vector2.zero)
        {
            edgeMoveDirection =
                currentEdgeDirection.normalized;

            desiredDragVelocity =
                Vector2.zero;

            return;
        }

        edgeMoveDirection =
            Vector2.zero;

        Vector2 mousePixelDelta =
            Mouse.current != null
                ? Mouse.current.delta.ReadValue()
                : Vector2.zero;

        float worldUnitsPerPixel =
            mainCamera.orthographicSize * 2f /
            Screen.height;

        Vector2 rawMovement =
            mousePixelDelta *
            worldUnitsPerPixel *
            dragSensitivity;

        if (Time.deltaTime <= Mathf.Epsilon)
        {
            desiredDragVelocity =
                Vector2.zero;

            return;
        }

        /*
         * Переводим движение мыши за render frame
         * в скорость world units / second.
         */
        desiredDragVelocity =
            rawMovement / Time.deltaTime;

        desiredDragVelocity =
            Vector2.ClampMagnitude(
                desiredDragVelocity,
                maxDragSpeed *
                CurrentSpeedMultiplier
            );
    }
    private void FixedUpdate()
    {
        if (playerController.IsKnockedBack)
        {
            edgeMoveDirection = Vector2.zero;
            desiredDragVelocity = Vector2.zero;
            return;
        }

        if (!isDragging)
        {
            desiredDragVelocity = Vector2.zero;
            return;
        }

        Vector2 movement;

        if (edgeMoveDirection != Vector2.zero)
        {
            movement =
                edgeMoveDirection *
                edgeMoveSpeed *
                CurrentSpeedMultiplier *
                Time.fixedDeltaTime;
        }
        else
        {
            movement =
                desiredDragVelocity *
                Time.fixedDeltaTime;
        }

        if (movement.sqrMagnitude <=
            Mathf.Epsilon)
        {
            return;
        }

        MoveWithCollisions(movement);
    }
    private void DragCheck(bool dragCheck)
    {
        if (playerController.IsKnockedBack)
        {
            edgeMoveDirection = Vector2.zero;
            desiredDragVelocity = Vector2.zero;
            return;
        }

        isDragging = dragCheck;

        edgeMoveDirection = Vector2.zero;
        desiredDragVelocity = Vector2.zero;
        storedEdgeDirection = Vector2.zero;

        isUsingVirtualCursor = false;
        wasNearEdge = false;

        if (!isDragging)
            return;

        virtualMouseScreenPosition =
            Mouse.current != null
                ? Mouse.current.position.ReadValue()
                : (Vector2)Input.mousePosition;
    }
    private Vector2 GetScreenEdgeDirection()
    {
        Vector3 playerViewportPosition =
        mainCamera.WorldToViewportPoint(rb.position);

        bool playerNearLeft =
            playerViewportPosition.x <= playerViewportEdge;

        bool playerNearRight =
            playerViewportPosition.x >= 1f - playerViewportEdge;

        bool playerNearBottom =
            playerViewportPosition.y <= playerViewportEdge;

        bool playerNearTop =
            playerViewportPosition.y >= 1f - playerViewportEdge;

        bool playerNearAnyEdge =
            playerNearLeft ||
            playerNearRight ||
            playerNearBottom ||
            playerNearTop;

        if (!playerNearAnyEdge)
        {
            ResetEdgeMovement();
            return Vector2.zero;
        }

        Vector2 mouseScreenPosition =
            Mouse.current != null
                ? Mouse.current.position.ReadValue()
                : (Vector2)Input.mousePosition;

        Vector2 playerScreenPosition =
            mainCamera.WorldToScreenPoint(rb.position);

        Vector2 directionToCursor =
            mouseScreenPosition - playerScreenPosition;

        if (directionToCursor.magnitude < minimumCursorDistance)
        {
            ResetEdgeMovement();
            return Vector2.zero;
        }

        /*
         * Проверяем, действительно ли курсор тянет персонажа
         * наружу через тот край, к которому он подошёл.
         */
        bool pushingOutside =
            playerNearLeft && directionToCursor.x < 0f ||
            playerNearRight && directionToCursor.x > 0f ||
            playerNearBottom && directionToCursor.y < 0f ||
            playerNearTop && directionToCursor.y > 0f;

        if (!pushingOutside)
        {
            ResetEdgeMovement();
            return Vector2.zero;
        }

        if (!wasNearEdge)
        {
            wasNearEdge = true;
            isUsingVirtualCursor = true;
            virtualMouseScreenPosition = mouseScreenPosition;
        }

        Vector2 mouseDelta =
            Mouse.current != null
                ? Mouse.current.delta.ReadValue()
                : Vector2.zero;

        virtualMouseScreenPosition +=
            mouseDelta * virtualCursorSensitivity;

        /*
         * Не позволяем виртуальному курсору перейти
         * на противоположную от активной границы сторону.
         */
        if (playerNearLeft)
        {
            virtualMouseScreenPosition.x = Mathf.Min(
                virtualMouseScreenPosition.x,
                playerScreenPosition.x
            );
        }
        else if (playerNearRight)
        {
            virtualMouseScreenPosition.x = Mathf.Max(
                virtualMouseScreenPosition.x,
                playerScreenPosition.x
            );
        }

        if (playerNearBottom)
        {
            virtualMouseScreenPosition.y = Mathf.Min(
                virtualMouseScreenPosition.y,
                playerScreenPosition.y
            );
        }
        else if (playerNearTop)
        {
            virtualMouseScreenPosition.y = Mathf.Max(
                virtualMouseScreenPosition.y,
                playerScreenPosition.y
            );
        }

        Vector2 direction =
            virtualMouseScreenPosition -
            playerScreenPosition;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return Vector2.zero;

        return direction.normalized;
    }

    private void ResetEdgeMovement()
    {
        wasNearEdge = false;
        isUsingVirtualCursor = false;
        storedEdgeDirection = Vector2.zero;
        virtualMouseScreenPosition =
            Mouse.current != null
                ? Mouse.current.position.ReadValue()
                : (Vector2)Input.mousePosition;
    }
    private void MoveWithCollisions(Vector2 movement)
    {
        float distance = movement.magnitude;

        if (distance <= Mathf.Epsilon)
            return;

        Vector2 direction = movement / distance;

        ContactFilter2D filter =
            new ContactFilter2D();

        filter.SetLayerMask(obstacleMask);
        filter.useTriggers = false;

        int hitCount = playerCollider.Cast(
            direction,
            filter,
            castResults,
            distance + collisionOffset
        );

        float allowedDistance = distance;

        for (int i = 0; i < hitCount; i++)
        {
            float hitDistance =
                castResults[i].distance -
                collisionOffset;

            allowedDistance = Mathf.Min(
                allowedDistance,
                Mathf.Max(0f, hitDistance)
            );
        }

        rb.MovePosition(
            rb.position +
            direction * allowedDistance
        );
    }
    public void IncreaseMovementSpeed(
    float percent)
    {
        if (percent <= 0f)
            return;

        permanentSpeedMultiplier += percent;

        Debug.Log(
            $"Movement speed increased by {percent * 100f}%. " +
            $"Multiplier: {permanentSpeedMultiplier:F2}"
        );
    }

    public void SetTemporarySpeedMultiplier(
    float multiplier)
    {
        temporarySpeedMultiplier =
            Mathf.Max(0f, multiplier);
    }
}
