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
        CollectEnemies();
    }

    private void CollectEnemies()
    {
        sleepingEnemies.Clear();

        EnemyController[] foundEnemies =
            GetComponentsInChildren<EnemyController>(true);

        foreach (EnemyController enemy in foundEnemies)
        {
            if (enemy == null)
                continue;

            if (enemy is PatrolerController)
            {
                if (patroler == null)
                    patroler = enemy as PatrolerController;

                continue;
            }

            sleepingEnemies.Add(enemy);
        }

        Debug.Log(
            $"{name}: найдено спящих врагов — {sleepingEnemies.Count}",
            this
        );
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

        Debug.Log(
            $"{name}: тревога! Пробуждаем {sleepingEnemies.Count} врагов.",
            this
        );

        foreach (EnemyController enemy in sleepingEnemies)
        {
            if (enemy == null)
                continue;

            Debug.Log($"Пробуждение: {enemy.name}", enemy);
            enemy.WakeUp();
        }

        if (patroler != null)
            patroler.StartFleeing();

        AlarmRaised?.Invoke();
    }
}
