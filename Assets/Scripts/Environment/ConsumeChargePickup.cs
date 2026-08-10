using UnityEngine;

public class ConsumeChargePickup : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int chargesToRestore = 1;

    private BreakableObject breakable;

    private void Awake()
    {
        breakable =
            GetComponent<BreakableObject>();
    }

    private void OnEnable()
    {
        breakable.OnBroken += RestoreCharges;
    }

    private void OnDisable()
    {
        breakable.OnBroken -= RestoreCharges;
    }
    public bool CanBeDamaged(PlayerController player)
    {
        if (player == null)
            return false;

        ConsumeChargeController chargeController =
            player.GetComponent<ConsumeChargeController>();

        if (chargeController == null)
            return false;

        return chargeController.CurrentCharges <
               chargeController.MaxCharges;
    }
    private void RestoreCharges(
    PlayerController player)
    {
        if (player == null)
            return;

        ConsumeChargeController chargeController =
            player.GetComponent<ConsumeChargeController>();

        if (chargeController == null)
            return;

        chargeController.TryAddCharges(
            chargesToRestore
        );
    }
}
