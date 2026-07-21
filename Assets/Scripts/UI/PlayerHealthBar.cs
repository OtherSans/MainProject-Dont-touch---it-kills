using UnityEngine.UI;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private HealthController healthContr;

    public void UpdatePlayerHealth()
    {
        healthSlider.value = healthContr.RemainingHealthPercents;
    }
}
