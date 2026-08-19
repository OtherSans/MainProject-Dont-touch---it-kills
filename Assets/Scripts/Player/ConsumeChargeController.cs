using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConsumeChargeController : MonoBehaviour
{
    [SerializeField, Min(0f)] private int maxCharges = 3;
    [SerializeField, Min(0f)] private int currentCharges = 3;

    public int MaxCharges => maxCharges;
    public int CurrentCharges => currentCharges;

    public bool HasCharges => currentCharges > 0;

    public event Action<int, int> ChargesChanged;

    private bool chargeRefundUnlocked;
    private float chargeRefundChance;

    private void Awake()
    {
        currentCharges = Mathf.Clamp(currentCharges, 0, maxCharges);
    }
    public bool TryAddCharges(int amount)
    {
        if (amount <= 0)
            return false;

        if (currentCharges >= maxCharges)
            return false;

        currentCharges = Mathf.Clamp(
            currentCharges + amount,
            0,
            maxCharges
        );

        ChargesChanged?.Invoke(
            currentCharges,
            maxCharges
        );

        return true;
    }

    public bool TrySpendCharge()
    {
        if (currentCharges <= 0)
            return false;

        if (chargeRefundUnlocked)
        {
            float roll =
                Random.value;

            if (roll < chargeRefundChance)
            {
                Debug.Log(
                    "CHARGE REFUND! Заряд не потрачен."
                );

                return true;
            }
        }

        currentCharges--;

        ChargesChanged?.Invoke(
            currentCharges,
            maxCharges
        );

        return true;
    }
    public void RestoreAllCharges()
    {
        currentCharges = maxCharges;
        ChargesChanged?.Invoke(currentCharges, maxCharges);
    }

    public void IncreaseMaxCharges(int amount, bool giveNewCharges = true)
    {
        if (amount <= 0f)
            return;

        maxCharges += amount;

        if(giveNewCharges)
        {
            currentCharges += amount;
        }

        currentCharges = Mathf.Clamp(currentCharges, 0, maxCharges);

        ChargesChanged?.Invoke(currentCharges,maxCharges);

        Debug.Log(
        $"Max consume charges increased by {amount}. " +
        $"Charges: {currentCharges}/{maxCharges}"
    );
    }

    public void UnlockChargeRefund(float chance)
    {
        chargeRefundUnlocked = true;

        chargeRefundChance =
            Mathf.Clamp01(chance);

        Debug.Log(
            $"CHARGE REFUND UNLOCKED | Chance: {chargeRefundChance * 100f}%"
        );
    }
}
