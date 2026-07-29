using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Camera mainCamera;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 8f;

    private BoxCollider2D currentBounds;
    private bool hasBounds;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (player == null || cameraTarget == null)
            return;

        Vector3 targetPosition = GetClampedPosition(player.position);

        cameraTarget.position = Vector3.Lerp(
            cameraTarget.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }

    public void SetRoomBounds(BoxCollider2D newBounds)
    {
        currentBounds = newBounds;
        hasBounds = currentBounds != null;
    }

    public void ClearRoomBounds()
    {
        currentBounds = null;
        hasBounds = false;
    }

    public void SnapToPlayer()
    {
        if (player == null || cameraTarget == null)
            return;

        cameraTarget.position = GetClampedPosition(player.position);
    }

    private Vector3 GetClampedPosition(Vector3 playerPosition)
    {
        Vector3 result = playerPosition;

        // Z оставляем прежним, чтобы не сломать глубину камеры.
        result.z = cameraTarget.position.z;

        if (!hasBounds || currentBounds == null || mainCamera == null)
            return result;

        Bounds bounds = currentBounds.bounds;

        float halfCameraHeight = mainCamera.orthographicSize;
        float halfCameraWidth =
            halfCameraHeight * mainCamera.aspect;



        float roomWidth = bounds.size.x;
        float roomHeight = bounds.size.y;

        float cameraWidth = halfCameraWidth * 2f;
        float cameraHeight = halfCameraHeight * 2f;

        // Если комната шире экрана, ограничиваем камеру по X.
        if (roomWidth > cameraWidth)
        {
            float minX = bounds.min.x + halfCameraWidth;
            float maxX = bounds.max.x - halfCameraWidth;

            result.x = Mathf.Clamp(
                playerPosition.x,
                minX,
                maxX
            );
        }
        else
        {
            // Если комната уже экрана — ставим камеру по центру.
            result.x = bounds.center.x;
        }

        // Если комната выше экрана, ограничиваем камеру по Y.
        if (roomHeight > cameraHeight)
        {
            float minY = bounds.min.y + halfCameraHeight;
            float maxY = bounds.max.y - halfCameraHeight;

            result.y = Mathf.Clamp(
                playerPosition.y,
                minY,
                maxY
            );
        }
        else
        {
            result.y = bounds.center.y;
        }

        return result;
    }
}
