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

    // Используется ключом:
    // дверь уничтожается, но XP за исследование НЕ даётся.
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

    // Используется, когда игрок сам ломает дверь мечом.
    public void Break()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (roomFog != null)
            roomFog.Reveal();

        if (roomController != null)
        {
            roomController.ActivateRoom();
            roomController.GiveDiscoveryExperience();
        }

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
