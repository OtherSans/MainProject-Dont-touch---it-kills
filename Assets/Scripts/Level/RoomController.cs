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

    [Header("Experience")]
    [SerializeField, Min(0)]
    private int discoveryExperience = 10;

    private bool discoveryExperienceGiven;

    private bool openedByKey;

    public bool OpenedByKey => openedByKey;

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
    public void GiveDiscoveryExperience()
    {
        if (discoveryExperienceGiven)
            return;

        if (discoveryExperience <= 0)
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "GameManager не найден.",
                this
            );

            return;
        }

        discoveryExperienceGiven = true;

        GameManager.Instance.Experience.AddExperience(
            discoveryExperience
        );

        Debug.Log(
            $"{name}: комната открыта, +{discoveryExperience} XP"
        );
    }
    public void PetrifyAllEnemies()
    {
        EnemyController[] enemies =
            GetComponentsInChildren<EnemyController>(true);

        Debug.Log(
            $"{name}: enemies found = {enemies.Length}"
        );

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null)
                continue;

            Debug.Log(
                $"PETRIFY: {enemy.name}"
            );

            enemy.Fsm.SetState<FsmEnemyStatePetrified>();
        }
    }

    public void ForceOpenRoom()
    {
        // Если игрок уже сам открыл комнату,
        // ключ не должен считать её своей.
        if (!discoveryExperienceGiven)
        {
            openedByKey = true;
        }

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

    public int GiveKeyCompletionExperience()
    {
        if (!openedByKey)
            return 0;

        if (discoveryExperienceGiven)
            return 0;

        if (discoveryExperience <= 0)
            return 0;

        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "GameManager не найден.",
                this
            );

            return 0;
        }

        discoveryExperienceGiven = true;

        GameManager.Instance.Experience.AddExperience(
            discoveryExperience
        );

        Debug.Log(
            $"{name}: XP за доставку ключа +{discoveryExperience}"
        );

        return discoveryExperience;
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
