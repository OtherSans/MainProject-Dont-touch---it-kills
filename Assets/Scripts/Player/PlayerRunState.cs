using UnityEngine;

public class PlayerRunState : MonoBehaviour
{
    [SerializeField]
    private HealthController health;

    [SerializeField]
    private CurrencyCollector currency;

    [SerializeField]
    private ConsumeChargeController consume;

    [SerializeField]
    private PlayerUpgradeController upgrades;

    [SerializeField]
    private UpgradeDefinition[] allUpgrades;

    [SerializeField]
    private DraggingController movement;

    [SerializeField]
    private SwordAttack swordAttack;

    private void Awake()
    {
        health = GetComponent<HealthController>();

        currency = GetComponent<CurrencyCollector>();

        consume = GetComponent<ConsumeChargeController>();

        upgrades = GetComponent<PlayerUpgradeController>();

        movement = GetComponent<DraggingController>();

        swordAttack =
    GetComponentInChildren<SwordAttack>();
    }

    private void Start()
    {
        Restore();
    }

    private void Restore()
    {
        if (GameManager.Instance == null)
            return;

        RunData data =
            GameManager.Instance.Run;

        if (upgrades != null)
        {
            upgrades.RestoreUpgrades(
                data.ownedUpgradeIds,
                allUpgrades
            );
        }

        // восстановление валюты,
        // HP, charges, upgrades и т.д.
    }

    public void Save()
    {
        if (GameManager.Instance == null)
            return;

        RunData data =
            GameManager.Instance.Run;

        data.currency =
            currency.Currency;

        data.currentCharges =
            consume.CurrentCharges;

        data.maxCharges =
            consume.MaxCharges;

        data.movementLevel =
            upgrades.MovementLevel;

        data.attackLevel =
            upgrades.AttackLevel;

        data.consumeLevel =
            upgrades.ConsumeLevel;

        data.ownedUpgradeIds =
            upgrades.GetOwnedUpgradeIds();
    }
}
