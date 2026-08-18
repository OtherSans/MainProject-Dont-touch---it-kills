using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class StatUpgradePanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;

    [SerializeField] private UpgradeChoiceView movementChoice;
    [SerializeField] private UpgradeChoiceView attackChoice;
    [SerializeField] private UpgradeChoiceView consumeChoice;

    [Header("Upgrades")]
    [SerializeField] private UpgradeDefinition[] upgrades;
    [Header("Player")]
    [SerializeField] private StatUpgradeController statUpgradeController;
    [SerializeField] private PlayerUpgradeController playerUpgradeController;

    private bool isOpen;
    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);

        movementChoice.Button.onClick.AddListener(
            () => SelectUpgrade(
                movementChoice.Upgrade)
            );
        attackChoice.Button.onClick.AddListener(
            () => SelectUpgrade(
                attackChoice.Upgrade)
            );
        consumeChoice.Button.onClick.AddListener(
            () => SelectUpgrade(
                consumeChoice.Upgrade)
            );
    }
    public void Open()
    {
        if (statUpgradeController == null)
            return;
        if (playerUpgradeController == null)
            return;

        if (!statUpgradeController.CanUseUpgradeItem)
        {
            Debug.Log("Нет предмета или очка улучшения.");
            return;
        }
        UpgradeDefinition movement = GetRandomUpgrade(UpgradeCategory.Movement);
        UpgradeDefinition attack = GetRandomUpgrade(UpgradeCategory.Attack);
        UpgradeDefinition consume = GetRandomUpgrade(UpgradeCategory.Consume);

        if(movement == null ||
            attack == null ||
            consume == null)
        {
            Debug.LogWarning("Не удалось собрать три варианта апгрейда");
            return;
        }

        movementChoice.SetUpgrade(movement);
        attackChoice.SetUpgrade(attack);
        consumeChoice.SetUpgrade(consume);

        panel.SetActive(true);
        isOpen = true;

        Time.timeScale = 0f;
    }

    private UpgradeDefinition GetRandomUpgrade(UpgradeCategory category)
    {
        List<UpgradeDefinition> candidates = new List<UpgradeDefinition>();

        foreach(UpgradeDefinition upgrade in upgrades)
        {
            if (upgrade == null)
                continue;
            if (upgrade.category != category)
                continue;

            if (playerUpgradeController.HasUpgrade(upgrade.id))
                continue;
            candidates.Add(upgrade);
        }

        if (candidates.Count == 0)
            return null;

        return candidates[Random.Range(
            0,
            candidates.Count
            )];
    }
    private void SelectUpgrade(UpgradeDefinition upgrade)
    {
        if (!isOpen)
            return;
        if (upgrade == null)
            return;
        if (!statUpgradeController.CanUseUpgradeItem)
            return;

        bool applied = playerUpgradeController.ApplyUpgrade(upgrade);

        if (!applied)
            return;
        statUpgradeController.ConfirmUpgrade();

        Close();
    }
    public void Close()
    {
        panel.SetActive(false);
        isOpen = false;

        Time.timeScale = 1f;
    }
    
}
