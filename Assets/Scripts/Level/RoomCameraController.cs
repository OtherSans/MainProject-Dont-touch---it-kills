using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Camera targetCamera;

    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float followSmoothTime = 0.12f;

    private BoxCollider2D currentBounds;
    private Vector3 smoothVelocity;
    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (player == null || cameraTarget == null)
            return;

        Vector3 desiredPosition = player.position;

        if (currentBounds != null)
            desiredPosition = ClampPositionToBounds(desiredPosition);

        desiredPosition.z = cameraTarget.position.z;

        if(followSmoothTime <= 0f)
        {
            cameraTarget.position = desiredPosition;
            return;
        }
        cameraTarget.position = Vector3.SmoothDamp(
            cameraTarget.position,
            desiredPosition,
            ref smoothVelocity,
            followSmoothTime);
    }
    public void SetRoomBounds(BoxCollider2D newBounds)
    {
        currentBounds = newBounds;
        smoothVelocity = Vector3.zero;

        SnapToPlayer();
    }
    public void CLearRoomBounds()
    {
        currentBounds = null;
    }

    public void SnapToPlayer()
    {
        if (player == null || cameraTarget == null)
            return;

        Vector3 position = player.position;

        if (currentBounds != null)
            position = ClampPositionToBounds(position);

        position.z = cameraTarget.position.z;
        cameraTarget.position = position;

        smoothVelocity = Vector3.zero;
    }
    private Vector3 ClampPositionToBounds(Vector3 desiredPosition)
    {
        Bounds bounds = currentBounds.bounds;

        float verticalExtent = targetCamera.orthographicSize;
        float horizontalExtent = verticalExtent * targetCamera.aspect;

        float minX = bounds.min.x + horizontalExtent;
        float maxX = bounds.max.x - horizontalExtent;

        float minY = bounds.min.y + verticalExtent;
        float maxY = bounds.max.y - verticalExtent;

        float x = GetClampedAxis(
            desiredPosition.x,
            minX,
            maxX,
            bounds.center.x
        );

        float y = GetClampedAxis(
            desiredPosition.y,
            minY,
            maxY,
            bounds.center.y
        );

        return new Vector3(x, y, desiredPosition.z);
    }

    private static float GetClampedAxis(
        float value,
        float min,
        float max,
        float fallback)
    {
        // Если комната меньше размеров камеры,
        // камера остаётся в центре этой комнаты.
        if (min > max)
            return fallback;

        return Mathf.Clamp(value, min, max);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (currentBounds == null)
            return;

        Gizmos.DrawWireCube(
            currentBounds.bounds.center,
            currentBounds.bounds.size
        );
    }
#endif
}
