using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private RoomController targetRoom;

    [Header("Current Room")]
    [SerializeField] private Transform exitMovePoint;

    [Header("Target Room")]
    [Tooltip("Точка снаружи целевой комнаты, куда переносится игрок")]
    [SerializeField] private Transform targetOutsidePoint;

    [Tooltip("Точка внутри целевой комнаты, куда игрок автоматически залетает")]
    [SerializeField] private Transform targetEntryPoint;

    private void Reset()
    {
        Collider2D trigger = GetComponent<Collider2D>();
        trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (roomManager.IsTransitioning)
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        if (roomManager == null || 
            targetRoom == null || 
            exitMovePoint == null || 
            targetOutsidePoint == null ||
            targetEntryPoint == null)
        {
            Debug.LogError(
                $"RoomTransition {name} is not configured.",
                this
            );

            return;
        }

        //isTriggered = true;
        roomManager.StartTransition(
            targetRoom,
            exitMovePoint,
            targetOutsidePoint,
            targetEntryPoint
        );
    }
}
