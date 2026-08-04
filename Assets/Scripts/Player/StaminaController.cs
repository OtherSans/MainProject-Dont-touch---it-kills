using System;
using UnityEngine;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField, Min(1f)]
    private float maxStamina = 100f;

    [SerializeField, Min(0f)]
    private float currentStamina = 100f;

    [Header("Recovery")]
    [SerializeField, Min(0f)]
    private float recoveryPerSecond = 25f;

    [SerializeField, Min(0f)]
    private float recoveryDelay = 1f;

    [Header("Attack restriction")]
    [Tooltip("Сколько стамины должно быть, чтобы разрешить новый выпад")]
    [SerializeField, Min(0f)]
    private float minimumStaminaToAttack = 15f;

    private float recoveryTimer;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    public float NormalizedStamina =>
        maxStamina <= 0f
            ? 0f
            : currentStamina / maxStamina;

    public bool IsEmpty => currentStamina <= 0f;

    public bool CanStartAttack =>
        currentStamina >= minimumStaminaToAttack;

    public event Action<float, float> StaminaChanged;

    private void Awake()
    {
        currentStamina = Mathf.Clamp(
            currentStamina,
            0f,
            maxStamina
        );
    }

    private void Update()
    {
        RecoverStamina();
    }

    public bool TrySpend(float amount)
    {
        if (amount <= 0f)
            return true;

        if (currentStamina <= 0f)
            return false;

        currentStamina = Mathf.Max(
            0f,
            currentStamina - amount
        );

        recoveryTimer = recoveryDelay;
        NotifyChanged();

        return currentStamina > 0f;
    }

    public void Restore(float amount)
    {
        if (amount <= 0f)
            return;

        currentStamina = Mathf.Min(
            maxStamina,
            currentStamina + amount
        );

        NotifyChanged();
    }

    private void RecoverStamina()
    {
        if (currentStamina >= maxStamina)
            return;

        if (recoveryTimer > 0f)
        {
            recoveryTimer -= Time.deltaTime;
            return;
        }

        currentStamina = Mathf.MoveTowards(
            currentStamina,
            maxStamina,
            recoveryPerSecond * Time.deltaTime
        );

        NotifyChanged();
    }

    private void NotifyChanged()
    {
        StaminaChanged?.Invoke(
            currentStamina,
            maxStamina
        );
    }
}
