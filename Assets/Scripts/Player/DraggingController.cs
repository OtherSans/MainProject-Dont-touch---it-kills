using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.U2D.Physics.PhysicsQuery;

public class DraggingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField, Range(0f, 1f)]
    private float edgeDirectionSensitivity = 0.02f;

    private Vector2 storedEdgeDirection;


    [Header("Drag")]
    [SerializeField, Range(0, 2f)] private float dragSensitivity = 1f;
    [SerializeField, Min(0.01f)]
    private float maxDragDistancePerFrame = 0.5f;

    [Header("Screen edge movement")]
    [Tooltip("Ширина области у края экрана, которая включает автодвижение")]
    [SerializeField, Min(1f)]
    private float edgeSize = 50f;

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
    [SerializeField, Min(0f)]
    private float virtualCursorReturnSpeed = 10f;

    private Vector2 virtualMouseScreenPosition;
    private bool isUsingVirtualCursor;

    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Vector2 edgeMoveDirection;


    private Vector3 lastMouseWorldPos;


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
        if (!isDragging)
        {
            edgeMoveDirection = Vector2.zero;
            return;
        }

        Vector2 currentEdgeDirection =
            GetScreenEdgeDirection();

        if (currentEdgeDirection != Vector2.zero)
        {
            edgeMoveDirection =
                currentEdgeDirection.normalized;

            lastMouseWorldPos =
                GetMouseWorldPosition();

            return;
        }

        edgeMoveDirection = Vector2.zero;

        Vector3 currentMouseWorldPosition =
            GetMouseWorldPosition();

        Vector2 mouseDelta =
            currentMouseWorldPosition -
            lastMouseWorldPos;

        Vector2 dragMovement =
            mouseDelta * dragSensitivity;

        dragMovement = Vector2.ClampMagnitude(
            dragMovement,
            maxDragDistancePerFrame
        );

        MoveWithCollisions(dragMovement);

        lastMouseWorldPos =
            currentMouseWorldPosition;
    }
    private void FixedUpdate()
    {
        if (!isDragging)
            return;

        if (edgeMoveDirection == Vector2.zero)
            return;

        Vector2 movement =
            edgeMoveDirection *
            edgeMoveSpeed *
            Time.fixedDeltaTime;

        MoveWithCollisions(movement);
    }
    private void DragCheck(bool dragCheck)
    {
        isDragging = dragCheck;

        edgeMoveDirection = Vector2.zero;
        storedEdgeDirection = Vector2.zero;

        isUsingVirtualCursor = false;
        wasNearEdge = false;

        if (!isDragging)
            return;

        lastMouseWorldPos = GetMouseWorldPosition();
        virtualMouseScreenPosition = Input.mousePosition;
    }
    private Vector2 GetScreenEdgeDirection()
    {
        Vector2 realMousePosition = Input.mousePosition;

        bool nearLeft =
            realMousePosition.x <= edgeSize;

        bool nearRight =
            realMousePosition.x >= Screen.width - edgeSize;

        bool nearBottom =
            realMousePosition.y <= edgeSize;

        bool nearTop =
            realMousePosition.y >= Screen.height - edgeSize;

        bool nearAnyEdge =
            nearLeft || nearRight || nearBottom || nearTop;

        // Курсор вернулся внутрь экрана.
        if (!nearAnyEdge)
        {
            wasNearEdge = false;
            isUsingVirtualCursor = false;

            // Важно: полностью синхронизируем виртуальный курсор.
            virtualMouseScreenPosition = realMousePosition;

            return Vector2.zero;
        }

        /*
         * Курсор только что вошёл в краевую зону.
         * Старая виртуальная позиция больше не используется.
         */
        if (!wasNearEdge)
        {
            wasNearEdge = true;
            isUsingVirtualCursor = true;
            virtualMouseScreenPosition = realMousePosition;
        }

        Vector2 mouseDelta =
            Mouse.current != null
                ? Mouse.current.delta.ReadValue()
                : Vector2.zero;

        virtualMouseScreenPosition +=
            mouseDelta * virtualCursorSensitivity;

        /*
         * Принудительно удерживаем виртуальный курсор
         * за той границей, возле которой находится настоящий курсор.
         */
        if (nearLeft)
        {
            virtualMouseScreenPosition.x =
                Mathf.Min(
                    virtualMouseScreenPosition.x,
                    edgeSize
                );
        }
        else if (nearRight)
        {
            virtualMouseScreenPosition.x =
                Mathf.Max(
                    virtualMouseScreenPosition.x,
                    Screen.width - edgeSize
                );
        }

        if (nearBottom)
        {
            virtualMouseScreenPosition.y =
                Mathf.Min(
                    virtualMouseScreenPosition.y,
                    edgeSize
                );
        }
        else if (nearTop)
        {
            virtualMouseScreenPosition.y =
                Mathf.Max(
                    virtualMouseScreenPosition.y,
                    Screen.height - edgeSize
                );
        }

        Vector2 screenCenter = new Vector2(
            Screen.width * 0.5f,
            Screen.height * 0.5f
        );

        Vector2 fromCenter =
            virtualMouseScreenPosition - screenCenter;

        fromCenter = Vector2.ClampMagnitude(
            fromCenter,
            maxVirtualCursorDistance
        );

        virtualMouseScreenPosition =
            screenCenter + fromCenter;

        Vector2 playerScreenPosition =
            mainCamera.WorldToScreenPoint(rb.position);

        Vector2 direction =
            virtualMouseScreenPosition -
            playerScreenPosition;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return Vector2.zero;

        return direction.normalized;
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

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = transform.position.z; // сохраняем исходную глубину персонажа
        return worldPos;
    }
}
