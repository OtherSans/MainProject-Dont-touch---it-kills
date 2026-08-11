using System;
using UnityEngine;

public class StatUpgradeController : MonoBehaviour
{
    [Header("Upgrade Points")]
    [SerializeField, Min(0f)] private int availableUpgradePoints;
    [Header("Upgrade Items")]
    [SerializeField, Min(0f)] private int upgradeItems;

    public int AvailableUpgradePoints => availableUpgradePoints;
    public int UpgradeItems => upgradeItems;

    public bool CanUseUpgradeItem => availableUpgradePoints > 0 && upgradeItems > 0;

    public event Action<int> UpgradePointsChanged;
    public event Action<int> UpgradeItemsChanged;

    public void AddUpgradePoints(int amount = 1)
    {
        if (amount <= 0)
            return;

        availableUpgradePoints += amount;

        UpgradePointsChanged?.Invoke(availableUpgradePoints);
    }
    public void AddUpgradeItems(int amount = 1)
    {
        if (amount <= 0)
            return;

        upgradeItems += amount;

        UpgradeItemsChanged?.Invoke(upgradeItems);
    }
    public bool TryStartUpgrade()
    {
        if (!CanUseUpgradeItem)
            return false;

        /*
        * Пока характеристики нет,
        * здесь просто подтверждаем, что улучшение
        * можно открыть.
        *
        * Потом здесь будет открываться UI выбора.
        */
        Debug.Log(
            "Можно выбрать характеристику."
        );

        return true;
    }
    public void ConfirmUpgrade()
    {
        if (!CanUseUpgradeItem)
            return;

        availableUpgradePoints--;
        upgradeItems--;

        UpgradePointsChanged?.Invoke(availableUpgradePoints);
        UpgradeItemsChanged?.Invoke(upgradeItems);

        Debug.Log("Улучшение потрачено");
    }
    private void OnGUI()
    {
        GUI.Label(
            new Rect(20, 20, 300, 30),
            $"Upgrade Items: {upgradeItems}"
        );

        GUI.Label(
            new Rect(20, 50, 300, 30),
            $"Upgrade Points: {availableUpgradePoints}"
        );
    }
}
