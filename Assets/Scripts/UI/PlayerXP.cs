using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text experienceText;

    private XPManager experienceManager;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager не найден.", this);
            return;
        }

        experienceManager = GameManager.Instance.Experience;

        experienceManager.ExperienceChanged += UpdateExperience;
        experienceManager.LevelIncreased += UpdateLevel;

        UpdateExperience(
            experienceManager.CurrentExperience,
            experienceManager.ExperienceToNextLevel
        );

        UpdateLevel(experienceManager.CurrentLevel);
    }

    private void OnDestroy()
    {
        if (experienceManager == null)
            return;

        experienceManager.ExperienceChanged -= UpdateExperience;
        experienceManager.LevelIncreased -= UpdateLevel;
    }

    private void UpdateExperience(
        int currentExperience,
        int experienceToNextLevel
    )
    {
        if (experienceSlider != null)
        {
            experienceSlider.minValue = 0;
            experienceSlider.maxValue = experienceToNextLevel;
            experienceSlider.value = currentExperience;
        }

        if (experienceText != null)
        {
            experienceText.text =
                $"XP: {currentExperience} / {experienceToNextLevel}";
        }
    }

    private void UpdateLevel(int level)
    {
        if (levelText != null)
            levelText.text = $"Player LVL {level}";
    }
}
