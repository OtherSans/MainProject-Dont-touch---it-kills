using System;
using UnityEngine;

public class CurrencyCollector : MonoBehaviour
{
    public int Currency { get; private set; }
    public event Action<int> CurrencyChanged;

    public void AddCurrency(int amount)
    {
        if (amount <= 0)
            return;

        Currency += amount;
        CurrencyChanged?.Invoke(Currency);
    }

    public bool TrySpendCurrency(int amount)
    {
        if (amount <= 0 || Currency < amount)
            return false;

        Currency -= amount;
        CurrencyChanged?.Invoke(Currency);
        return true;
    }
}
