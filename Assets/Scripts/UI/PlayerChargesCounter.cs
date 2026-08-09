using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;

public class PlayerChargesCounter : MonoBehaviour
{
    [SerializeField] private ConsumeChargeController chargesController;
    [SerializeField] private TextMeshProUGUI chargesText;

    private void OnEnable()
    {
        chargesController.ChargesChanged += UpdateChargesText;
        UpdateChargesText(chargesController.CurrentCharges, chargesController.MaxCharges);
    }

    private void OnDisable()
    {
        chargesController.ChargesChanged -= UpdateChargesText;
    }
    private void UpdateChargesText(int curAmount, int maxAmount)
    {
        chargesText.text = $"Charges {curAmount.ToString()} / {maxAmount.ToString()}";
    }
}
