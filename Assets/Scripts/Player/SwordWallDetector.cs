using UnityEngine;

public class SwordWallDetector : MonoBehaviour
{
    [SerializeField]
    private SwordController swordController;

    [SerializeField]
    private LayerMask wallMask;

    private WallSurface currentSurface;

    private void Reset()
    {
        swordController = GetComponentInParent<SwordController>();
    }

    private void Awake()
    {
        if (swordController == null)
        {
            swordController = GetComponentInParent<SwordController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsWall(other.gameObject.layer))
            return;

        WallSurface wallSurface =
        other.GetComponentInParent<WallSurface>();

        if (wallSurface == null || wallSurface.Material == null)
        {
            Debug.LogWarning(
                $"У стены {other.name} отсутствует WallSurface или WallMaterial");

            return;
        }
        currentSurface = wallSurface;
        swordController.OnWallHit(wallSurface.Material);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsWall(other.gameObject.layer))
            return;
        WallSurface surface =
        other.GetComponentInParent<WallSurface>();

        if (surface != currentSurface)
            return;

        currentSurface = null;
        swordController.OnWallHitEnd();
    }

    private bool IsWall(int layer)
    {
        return (wallMask.value & (1 << layer)) != 0;
    }
}
