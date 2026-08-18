using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private LevelRoomManager roomManager;

    [SerializeField]
    private LevelRewardSpawner rewardSpawner;

    [SerializeField]
    private GameObject lockedVisual;

    [SerializeField]
    private GameObject completedVisual;

    private bool isCompleted;

    private void Awake()
    {
        if (roomManager == null)
        {
            roomManager =
                FindAnyObjectByType<LevelRoomManager>();
        }

        if (rewardSpawner == null)
        {
            rewardSpawner =
                FindAnyObjectByType<LevelRewardSpawner>();
        }
    }
    private void ShowCompletedState()
    {
        if (lockedVisual != null)
            lockedVisual.SetActive(false);

        if (completedVisual != null)
            completedVisual.SetActive(true);
    }
    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if (isCompleted)
            return;

        PlayerLevelKeyController keyController =
            other.GetComponent<PlayerLevelKeyController>();

        if (keyController == null)
            return;

        if (!keyController.HasKey)
        {
            Debug.Log("Для выхода нужен ключ.");
            return;
        }

        CompleteLevel(
            keyController
        );
    }

    private void CompleteLevel(
    PlayerLevelKeyController keyController)
    {
        if (!keyController.TryUseKey())
            return;

        isCompleted = true;
        // Показываем, что уровень завершён
        ShowCompletedState();


        if (roomManager != null)
        {
            roomManager.GiveKeyCompletionExperience();
            roomManager.PetrifyAllEnemies();
        }
        // Создаём награду
        if (rewardSpawner != null)
        {
            rewardSpawner.SpawnReward();
        }


        Debug.Log("LEVEL COMPLETED");
    }
}
