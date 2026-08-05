using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomAlarmController : MonoBehaviour
{
    [SerializeField]
    private List<EnemyController> sleepingEnemies = new();

    [SerializeField]
    private PatrolerController patroler;

    private bool isPrepared;
    private bool isAlarmRaised;

    public bool IsAlarmRaised => isAlarmRaised;

    public event Action AlarmRaised;

    private void Awake()
    {
        if (sleepingEnemies.Count > 0)
            return;

        EnemyController[] foundEnemies =
            GetComponentsInChildren<EnemyController>(true);

        foreach (EnemyController enemy in foundEnemies)
        {
            if (enemy == null)
                continue;

            if (enemy is PatrolerController)
                continue;

            sleepingEnemies.Add(enemy);
        }
    }

    public void PrepareRoom()
    {
        if (isPrepared)
            return;

        isPrepared = true;
        isAlarmRaised = false;

        foreach (EnemyController enemy in sleepingEnemies)
        {
            if (enemy != null)
                enemy.EnterSleepState();
        }

        if (patroler != null)
            patroler.BeginPatrol();
    }

    public void RaiseAlarm()
    {
        if (isAlarmRaised)
            return;

        isAlarmRaised = true;

        foreach (EnemyController enemy in sleepingEnemies)
        {
            if (enemy != null)
                enemy.WakeUp();
        }

        if (patroler != null)
            patroler.StartFleeing();

        AlarmRaised?.Invoke();
    }
}
