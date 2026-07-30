using System.Threading;
using UnityEngine;

public class CaptureCombatRoom : RoomController
{
    [Header("Capture")]
    [SerializeField] private FsmFinishController finishPoint;

    [Header("Barriers")]
    [SerializeField] private RoomBarrier[] roomBarriers;
    private bool isSubscribed;
    protected override void OnRoomEntered()
    {
        OpenBarriers();

        if (IsCompleted)
        {
            OpenBarriers();
            return;
        }

        Subscribe();

        if (finishPoint != null && finishPoint.IsCaptured)
            HandleCaptured();

    }
    protected override void OnPlayerArrived()
    {
        if (IsCompleted)
        {
            OpenBarriers();
            return;
        }

        CloseBarriers();
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
        OpenBarriers();
        CompleteRoom();
    }
    private void CloseBarriers()
    {
        if (roomBarriers == null)
            return;

        foreach (RoomBarrier barrier in roomBarriers)
        {
            if (barrier != null)
                barrier.Close();
        }
    }

    private void OpenBarriers()
    {
        if (roomBarriers == null)
            return;

        foreach (RoomBarrier barrier in roomBarriers)
        {
            if (barrier != null)
                barrier.Open();
        }
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
