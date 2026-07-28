using TMPro;
using UnityEngine;

public class PlayerCurrencyCounter : MonoBehaviour
{
    [SerializeField] private CurrencyCollector currencyCollector;
    [SerializeField] private TextMeshProUGUI currencyText;

    private void OnEnable()
    {
        currencyCollector.CurrencyChanged += UpdateCurrencyText;
        UpdateCurrencyText(currencyCollector.Currency);
    }

    private void OnDisable()
    {
        currencyCollector.CurrencyChanged -= UpdateCurrencyText;
    }
    private void UpdateCurrencyText(int amount)
    {
        currencyText.text = amount.ToString();
    }
}
