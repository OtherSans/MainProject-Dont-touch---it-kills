using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour, IInteractable
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

    private bool playerInside;

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
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
            return;

        playerInside = true;

        player.SetInteractable(this);

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null)
            return;

        playerInside = false;

        player.ClearInteractable(this);
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

    public void Interact(PlayerController player)
    {
        if (!isCompleted)
            return;
        PlayerRunState runState =
    player.GetComponent<PlayerRunState>();

        if (runState != null)
            runState.Save();

        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int currentScene =
        SceneManager.GetActiveScene().buildIndex;

        int nextScene =
            currentScene + 1;

        if (nextScene >=
            SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log(
                "Это последняя сцена на сегодня."
            );

            return;
        }

        SceneManager.LoadScene(nextScene);
    }
}
