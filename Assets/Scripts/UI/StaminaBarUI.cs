using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField]
    private StaminaController staminaController;

    [SerializeField]
    private Slider staminaSlider;

    private void OnEnable()
    {
        if (staminaController == null)
            return;

        staminaController.StaminaChanged += UpdateBar;

        UpdateBar(
            staminaController.CurrentStamina,
            staminaController.MaxStamina
        );
    }

    private void OnDisable()
    {
        if (staminaController != null)
            staminaController.StaminaChanged -= UpdateBar;
    }

    private void UpdateBar(float current, float maximum)
    {
        if (staminaSlider == null)
            return;

        staminaSlider.minValue = 0f;
        staminaSlider.maxValue = maximum;
        staminaSlider.value = current;
    }
}
