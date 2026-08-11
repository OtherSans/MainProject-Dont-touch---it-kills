using UnityEngine;

public class StatUpgradeItem : MonoBehaviour
{
    [SerializeField, Min(0f)] private int amount = 1;

    public void GiveToPlayer(PlayerController player)
    {
        if (player == null)
            return;

        StatUpgradeController statUpgradeContr = player.GetComponent<StatUpgradeController>();
        if (statUpgradeContr == null)
            return;
        statUpgradeContr.AddUpgradeItems(amount);
    }
}
