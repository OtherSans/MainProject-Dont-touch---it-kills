using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeController : MonoBehaviour
{
    public int MovementLevel { get; private set; }
    public int AttackLevel { get; private set; }
    public int ConsumeLevel { get; private set; }

    [Header("Base stat increases")]
    [SerializeField]
    private float movementSpeedIncrease = 0.1f;

    [SerializeField]
    private float attackDamageIncrease = 2f;

    [SerializeField]
    private int consumeChargeIncrease = 1;

    [Header("References")]
    [SerializeField]
    private DraggingController movement;

    [SerializeField]
    private SwordAttack swordAttack;

    [SerializeField]
    private ConsumeChargeController consumeCharges;

    private readonly HashSet<string> ownedUpgrades = new HashSet<string>();

    // -------------------------
    // Predator
    // -------------------------

    private bool predatorUnlocked;

    private float predatorSpeedBonus;
    private float predatorDuration;

    private Coroutine predatorCoroutine;

    // -------------------------
    // Combo Impact
    // -------------------------

    public bool ComboImpactUnlocked { get; private set; }

    public float ComboDamageMultiplier { get; private set; }
    public float ComboDuration { get; private set; }

    // -------------------------
    // Charge Refund
    // -------------------------

    public bool ChargeRefundUnlocked { get; private set; }

    public float ChargeRefundChance { get; private set; }

    private void Awake()
    {
        if (movement == null)
        {
            movement =
                GetComponent<DraggingController>();
        }

        if (swordAttack == null)
        {
            swordAttack =
                GetComponentInChildren<SwordAttack>();
        }

        if (consumeCharges == null)
        {
            consumeCharges =
                GetComponent<ConsumeChargeController>();
        }
    }
    private void OnEnable()
    {
        EnemyKillEvents.EnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyKillEvents.EnemyKilled -= OnEnemyKilled;
    }
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

        ApplyBaseStatIncrease(upgrade.category);

        ApplyUniqueEffect(upgrade);

        Debug.Log(
            $"UPGRADE APPLIED: {upgrade.upgradeName}"
            );

        return true;
    }

    public List<string> GetOwnedUpgradeIds()
    {
        return new List<string>(ownedUpgrades);
    }
    private void ApplyUniqueEffect(
        UpgradeDefinition upgrade)
    {
        switch (upgrade.id)
        {
            case "movement_predator":

                predatorUnlocked = true;
                predatorSpeedBonus =
                    upgrade.effectValue;

                predatorDuration =
                    upgrade.duration;

                break;

            case "attack_combo_impact":

                if (swordAttack != null)
                {
                    swordAttack.UnlockComboImpact(
                        upgrade.effectValue,
                        upgrade.duration
                    );
                }

                break;

            case "consume_charge_refund":

                if (consumeCharges != null)
                {
                    consumeCharges.UnlockChargeRefund(
                        upgrade.effectValue
                    );
                }

                break;

            default:

                Debug.LogWarning(
                    $"Unknown upgrade id: {upgrade.id}"
                );

                break;
        }
    }

    private void ApplyBaseStatIncrease(
        UpgradeCategory category)
    {
        switch (category)
        {
            case UpgradeCategory.Movement:

                if (movement != null)
                {
                    movement.IncreaseMovementSpeed(
                        movementSpeedIncrease
                    );
                }

                break;

            case UpgradeCategory.Attack:

                if (swordAttack != null)
                {
                    swordAttack.IncreaseDamage(
                        attackDamageIncrease
                    );
                }

                break;

            case UpgradeCategory.Consume:

                if (consumeCharges != null)
                {
                    consumeCharges.IncreaseMaxCharges(
                        consumeChargeIncrease
                    );
                }

                break;
        }
    }
    private void IncreaseMovementStat()
    {
        MovementLevel++;

        DraggingController movement =
        GetComponent<DraggingController>();

        if (movement == null)
            return;

        movement.IncreaseMovementSpeed(
            movementSpeedIncrease
        );
    }
    private void IncreaseAttackStat()
    {
        AttackLevel++;

        DraggingController movement =
        GetComponent<DraggingController>();

        if (movement == null)
            return;

        movement.IncreaseMovementSpeed(
            movementSpeedIncrease
        );
    }
    private void IncreaseConsumeStat()
    {
        ConsumeLevel++;

        ConsumeChargeController consume =
        GetComponent<ConsumeChargeController>();

        if (consume == null)
            return;

        consume.IncreaseMaxCharges(
            consumeChargeIncrease
        );
    }
    public void RestoreUpgrades(
    List<string> upgradeIds,
    UpgradeDefinition[] allUpgrades)
    {
        if (upgradeIds == null)
            return;

        foreach (string id in upgradeIds)
        {
            if (ownedUpgrades.Contains(id))
                continue;

            foreach (UpgradeDefinition upgrade in allUpgrades)
            {
                if (upgrade == null)
                    continue;

                if (upgrade.id != id)
                    continue;

                ApplyUpgrade(upgrade);
                break;
            }
        }
    }
    private void OnEnemyKilled()
    {
        if (!predatorUnlocked)
            return;

        if (movement == null)
            return;

        if (predatorCoroutine != null)
        {
            StopCoroutine(
                predatorCoroutine
            );
        }

        predatorCoroutine =
            StartCoroutine(
                PredatorRoutine()
            );
    }

    private IEnumerator PredatorRoutine()
    {
        movement.SetTemporarySpeedMultiplier(
            1f + predatorSpeedBonus
        );

        yield return new WaitForSeconds(
            predatorDuration
        );

        movement.SetTemporarySpeedMultiplier(
            1f
        );

        predatorCoroutine = null;
    }
}
