using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeController : MonoBehaviour
{
    private readonly HashSet<string> ownedUpgrades = new HashSet<string>();

    public bool HasUpgrade(string id)
    {
        return ownedUpgrades.Contains(id);
    }
    public bool ApplyUpgrade(UpgradeDefinition upgrade)
    {
        if (upgrade == null)
            return false;

        if (ownedUpgrades.Contains(upgrade.id))
            return false;

        ownedUpgrades.Add(upgrade.id);

        ApplyEffect(upgrade);

        Debug.Log(
            $"UPGRADE RECEIVED: {upgrade.upgradeName}"
            );

        return true;
    }

    private void ApplyEffect(UpgradeDefinition upgrade)
    {
        switch (upgrade.id)
        {
            case "movement_speed":
                ApplyMovementSpeed();
                break;
            case "attack_length":
                ApplyAttackLength();
                break;
            case "consume_heal":
                ApplyConsumeHeal();
                break;
        }
    }
    private void ApplyMovementSpeed()
    {
        Debug.Log("Movement Upgrade Applied");
    }
    private void ApplyAttackLength()
    {
        Debug.Log("Attack Upgrade Applied");
    }
    private void ApplyConsumeHeal()
    {
        Debug.Log("Consume Upgrade Applied");
    }
}
