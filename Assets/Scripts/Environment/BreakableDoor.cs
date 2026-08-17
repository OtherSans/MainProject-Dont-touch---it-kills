using UnityEngine;

public class BreakableDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyVfxPrefab;

    [SerializeField]
    private RoomController roomController;

    [SerializeField]
    private RoomFog roomFog;

    private bool isDestroyed;
    private void Awake()
    {
        if (roomController == null)
        {
            roomController =
                GetComponentInParent<RoomController>();
        }

        if (roomFog == null)
        {
            roomFog =
                GetComponentInParent<RoomFog>();
        }
    }
    public void ForceBreak()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (destroyVfxPrefab != null)
        {
            Instantiate(
                destroyVfxPrefab,
                transform.position,
                transform.rotation
            );
        }

        Destroy(gameObject);
    }
    public void Break()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (roomFog != null)
            roomFog.Reveal();

        if (roomController != null)
            roomController.ActivateRoom();

        if (destroyVfxPrefab != null)
        {
            Instantiate(
                destroyVfxPrefab,
                transform.position,
                transform.rotation
            );
        }

        Destroy(gameObject);
    }
}
