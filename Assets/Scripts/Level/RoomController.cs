using System;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private EnemyController[] enemies;
    private PlayerController player;

    private bool isActivated;

    public bool IsActivated => isActivated;

    private void Start()
    {
        enemies =
            GetComponentsInChildren<EnemyController>(true);

        player =
            FindAnyObjectByType<PlayerController>();

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null)
                continue;

            enemy.Initialize(
                player,
                this
            );

            // ВАЖНО:
            // после полной инициализации принудительно усыпляем.
            enemy.EnterSleepState();
        }
    }

    public void ActivateRoom()
    {
        if (isActivated)
            return;

        isActivated = true;

        Debug.Log(
            $"{name}: ROOM ACTIVATED"
        );

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null)
                continue;

            enemy.OnRoomActivated();
        }
    }
}
