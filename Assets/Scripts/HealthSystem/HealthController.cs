using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    [SerializeField] private float curHealth;

    public float RemainingHealthPercents
    {
        get
        {
            return curHealth / maxHealth;
        }
    }
    public float MaxHealth => maxHealth;
    public float CurHealth => curHealth;
    public bool IsDead => curHealth <= 0f;
    public bool IsFull => curHealth >= maxHealth;

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;

    public UnityEvent OnHeal;
    public bool IsInvincible { get; set; }

    public void TakeDamage(float damageAmount)
    {
        if (IsDead)
            return;

        if (IsInvincible)
            return;

        if (damageAmount <= 0f)
            return;

        curHealth -= damageAmount;
        curHealth = Mathf.Max(curHealth, 0f);

        if (IsDead)
            OnDied.Invoke();
        else
            OnDamaged.Invoke();
    }
    public void AddHealth(float amountToAdd)
    {
        if (IsDead)
            return;

        if (amountToAdd <= 0f)
            return;

        if (IsFull)
            return;

        curHealth = Mathf.Min(
            curHealth + amountToAdd,
            maxHealth
        );

        OnHeal.Invoke();
    }

    public void IncreaseMaxHealth(int amount, bool healToFull = true)
    {
        if (amount <= 0)
            return;

        maxHealth += amount;

        if (healToFull)
            curHealth = maxHealth;

        OnHeal?.Invoke();
    }
    public void Kill()
    {
        if (IsDead)
            return;

        curHealth = 0f;
        OnDied.Invoke();
    }

}
