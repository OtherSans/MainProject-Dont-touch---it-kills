using System.Threading;
using UnityEngine;

public class CaptureCombatRoom : RoomController
{
    [SerializeField]
    private RoomAlarmController roomAlarmController;

    [Header("Capture")]
    [SerializeField] private FsmFinishController finishPoint;

    [Header("Barriers")]
    [SerializeField] private RoomBarrier[] roomBarriers;

    [Header("Experience")]
    [SerializeField, Min(0)] private int completionExperienceReward = 50;

    [Header("Reward")]
    [SerializeField] private RewardSpawner rewardSpawner;

    private bool isSubscribed;
    protected override void OnRoomEntered()
    {
        roomAlarmController.PrepareRoom();

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
        OpenBarriers();


        rewardSpawner.SpawnReward();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Experience.AddExperience(
                completionExperienceReward
            );
        }
        else
        {
            Debug.LogError("GameManager не найден.", this);
        }

        Debug.Log(
            $"Room {RoomID} completed. " +
            $"Received {completionExperienceReward} XP"
        );
    }
    private void OnDestroy()
    {
        Unsubscribe();
    }
}
