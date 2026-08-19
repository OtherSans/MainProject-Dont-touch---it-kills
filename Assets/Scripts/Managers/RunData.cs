using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public float currentHealth;

    public int currency;

    public int currentCharges;
    public int maxCharges;

    public int movementLevel;
    public int attackLevel;
    public int consumeLevel;

    public float movementSpeedMultiplier = 1f;

    public float bonusAttackDamage;

    public List<string> ownedUpgradeIds =
        new List<string>();
}