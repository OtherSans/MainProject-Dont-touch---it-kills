using System;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private EnemyController[] enemies;
    private PlayerController player;

    private bool isActivated;

    public bool IsActivated => isActivated;

    private LevelRoomManager levelRoomManager;

    [SerializeField]
    private BreakableDoor[] doors;

    [SerializeField]
    private RoomFog roomFog;

    private void Start()
    {
        levelRoomManager =
        FindAnyObjectByType<LevelRoomManager>();

        if (levelRoomManager != null)
        {
            levelRoomManager.RegisterRoom(
                this
            );
        }

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

    public void ForceOpenRoom()
    {
        if (roomFog != null)
        {
            roomFog.Reveal();
        }

        if (doors != null)
        {
            foreach (BreakableDoor door in doors)
            {
                if (door == null)
                    continue;

                door.ForceBreak();
            }
        }

        ActivateRoom();
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
    private void OnDestroy()
    {
        if (levelRoomManager != null)
        {
            levelRoomManager.UnregisterRoom(
                this
            );
        }
    }
}
