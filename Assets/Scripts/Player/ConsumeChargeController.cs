using System;
using UnityEngine;

public class ConsumeChargeController : MonoBehaviour
{
    [SerializeField, Min(0f)] private int maxCharges = 3;
    [SerializeField, Min(0f)] private int currentCharges = 3;

    public int MaxCharges => maxCharges;
    public int CurrentCharges => currentCharges;

    public bool HasCharges => currentCharges > 0;

    public event Action<int, int> ChargesChanged;

    private void Awake()
    {
        currentCharges = Mathf.Clamp(currentCharges, 0, maxCharges);
    }

    public bool TrySpendCharge()
    {
        if (currentCharges <= 0)
            return false;

        currentCharges --;

        ChargesChanged?.Invoke(currentCharges, maxCharges);

        return true;
    }
    public void RestoreAllCharges()
    {
        currentCharges = maxCharges;
        ChargesChanged?.Invoke(currentCharges, maxCharges);
    }
}
