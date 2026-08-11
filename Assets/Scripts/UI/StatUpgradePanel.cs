using UnityEngine;

public class StatUpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private StatUpgradeController upgradeController;

    private bool isOpen;
    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }
    public void Open()
    {
        if (upgradeController == null)
            return;

        if(!upgradeController.CanUseUpgradeItem)
        {
            Debug.Log("Нет предмета или очка улучшения.");
            return;
        }

        panel.SetActive(true);
        isOpen = true;

        Time.timeScale = 0f;
    }
    public void Close()
    {
        panel.SetActive(false);
        isOpen = false;

        Time.timeScale = 1f;
    }
    public void SelectMovement()
    {
        if (!TrySpendUpgrade())
            return;

        Debug.Log("Выбрано улучшение Movement");

        // Потом:
        // movementUpgrade.Apply();

        Close();
    }
    public void SelectAttack()
    {
        if (!TrySpendUpgrade())
            return;

        Debug.Log("Выбрано улучшение Attack");

        // Потом:
        // attackUpgrade.Apply();

        Close();
    }

    public void SelectConsume()
    {
        if (!TrySpendUpgrade())
            return;

        Debug.Log("Выбрано улучшение Consume");

        // Потом:
        // consumeUpgrade.Apply();

        Close();
    }

    private bool TrySpendUpgrade()
    {
        if (upgradeController == null)
            return false;

        if (!upgradeController.CanUseUpgradeItem)
            return false;

        upgradeController.ConfirmUpgrade();

        return true;
    }
}
