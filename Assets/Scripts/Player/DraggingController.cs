using System;
using UnityEngine;
using static Unity.U2D.Physics.PhysicsQuery;

public class DraggingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;


    [Tooltip("Множитель скорости перемещения. 1 = персонаж двигается точно так же, как курсор")]
    [SerializeField, Range(0, 1)] private float dragSensitivity = 1f;

    [Tooltip("Слои, которые должны блокировать игрока")]
    [SerializeField]
    private LayerMask obstacleMask;

    [Tooltip("Небольшое расстояние между игроком и стеной")]
    [SerializeField]
    private float collisionOffset = 0.01f;

    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D playerCollider;


    private Vector3 lastMouseWorldPos;
    private bool isDragging;

    private readonly RaycastHit2D[] castResults = new RaycastHit2D[8];

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();


    }
    private void OnEnable()
    {
        if(playerController != null)
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
            return;

        Vector3 currentMouseWorldPos = GetMouseWorldPosition();
        Vector2 mouseDelta = currentMouseWorldPos - lastMouseWorldPos;

        MoveWithCollisions(mouseDelta * dragSensitivity);

        lastMouseWorldPos = currentMouseWorldPos;
    }
    private void DragCheck(bool dragCheck)
    {
        isDragging = dragCheck;
        if(isDragging)
            lastMouseWorldPos = GetMouseWorldPosition();
    }
    private void MoveWithCollisions(Vector2 movement)
    {
        float distance = movement.magnitude;

        if (distance <= Mathf.Epsilon)
            return;

        Vector2 direction = movement / distance;

        ContactFilter2D filter = new ContactFilter2D();
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
            float hitDistance = castResults[i].distance - collisionOffset;

            if (hitDistance < allowedDistance)
                allowedDistance = Mathf.Max(0f, hitDistance);
        }

        rb.MovePosition(rb.position + direction * allowedDistance);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = transform.position.z; // сохраняем исходную глубину персонажа
        return worldPos;
    }
}
