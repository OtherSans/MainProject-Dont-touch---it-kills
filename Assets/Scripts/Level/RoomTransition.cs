using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private RoomController targetRoom;

    private bool isTransitioning;

    private void Reset()
    {
        Collider2D trigger = GetComponent<Collider2D>();
        trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTransitioning)
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        if (roomManager == null || targetRoom == null)
        {
            Debug.LogError(
                $"RoomTransition {name} is not configured.",
                this
            );

            return;
        }

        isTransitioning = true;
        roomManager.EnterRoom(targetRoom);
    }
}
