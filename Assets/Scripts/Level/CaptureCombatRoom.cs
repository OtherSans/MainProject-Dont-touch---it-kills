using UnityEngine;

public class CaptureCombatRoom : RoomController
{
    [SerializeField] private FsmFinishController finishPoint;
    private bool isSubscribed;
    protected override void OnRoomEntered()
    {
        if (IsCompleted)
            return;
        Subscribe();
        if (finishPoint.IsCaptured)
            HandleCaptured();

    }
    protected override void OnRoomExited()
    {
        Unsubscribe();
    }
    private void Subscribe()
    {
        if (isSubscribed || finishPoint == null)
            return;
        finishPoint.Captured += HandleCaptured;
        isSubscribed = true;
    }
    private void Unsubscribe()
    {
        if (!isSubscribed || finishPoint == null)
            return;
        finishPoint.Captured -= HandleCaptured;
        isSubscribed = false;
    }
    private void HandleCaptured()
    {
        CompleteRoom();
    }
    protected override void OnRoomCompleted()
    {
        Unsubscribe();

        Debug.Log($"Room {RoomID} completed");
    }
    private void OnDestroy()
    {
        Unsubscribe();
    }
}
