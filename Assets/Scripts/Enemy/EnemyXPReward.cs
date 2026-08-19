using UnityEngine;

[RequireComponent(typeof(HealthController))]
public class EnemyExperienceReward : MonoBehaviour
{
    [SerializeField, Min(0)] private int experienceReward = 10;

    private HealthController healthController;
    private bool rewardGiven;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    private void OnEnable()
    {
        if (healthController != null)
            healthController.OnDied.AddListener(HandleEnemyDied);
    }

    private void OnDisable()
    {
        if (healthController != null)
            healthController.OnDied.RemoveListener(HandleEnemyDied);
    }

    private void HandleEnemyDied()
    {
        if (rewardGiven)
            return;

        rewardGiven = true;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager не найден.", this);
            return;
        }

        GameManager.Instance.Experience.AddExperience(experienceReward);

        EnemyKillEvents.ReportKill();
    }
}
