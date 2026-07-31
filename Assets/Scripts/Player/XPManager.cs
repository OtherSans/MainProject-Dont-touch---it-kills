using System;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField, Min(1)] private int currentLevel = 1;
    [SerializeField, Min(0)] private int currentExperience;
    [SerializeField, Min(1)] private int baseExperienceToLevelUp = 100;
    [SerializeField, Min(0)] private int experienceIncreasePerLevel = 30;

    public int CurrentLevel => currentLevel;
    public int CurrentExperience => currentExperience;
    public int ExperienceToNextLevel => CalculateExperienceToNextLevel();

    public float NormalizedExperience =>
        (float)currentExperience / ExperienceToNextLevel;

    public event Action<int, int> ExperienceChanged;
    public event Action<int> LevelIncreased;

    private void Start()
    {
        NotifyExperienceChanged();
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0)
            return;

        currentExperience += amount;

        while (currentExperience >= ExperienceToNextLevel)
        {
            currentExperience -= ExperienceToNextLevel;
            currentLevel++;

            LevelIncreased?.Invoke(currentLevel);
        }

        NotifyExperienceChanged();
    }

    private int CalculateExperienceToNextLevel()
    {
        return baseExperienceToLevelUp
             + (currentLevel - 1) * experienceIncreasePerLevel;
    }

    private void NotifyExperienceChanged()
    {
        ExperienceChanged?.Invoke(
            currentExperience,
            ExperienceToNextLevel
        );
    }
}
