using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private HealthController healthContr;
    private void Start()
    {
        mainCam = Camera.main;
        UpdateHealthBar();
    }
    private void Update()
    {
        transform.rotation = mainCam.transform.rotation;
        transform.position = target.position + offset;
    }
    public void UpdateHealthBar()
    {
        healthSlider.value = healthContr.RemainingHealthPercents;
    }
}