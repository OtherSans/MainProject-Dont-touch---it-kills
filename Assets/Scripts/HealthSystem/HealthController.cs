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

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    public bool IsInvincible { get; set; }

    public void TakeDamage(float damageAmount)
    {
        if (curHealth == 0)
            return;

        if (IsInvincible)
            return;

        curHealth -= damageAmount;

        if (curHealth < 0)
            curHealth = 0;

        if (curHealth == 0)
            OnDied.Invoke();
        else
            OnDamaged.Invoke();
    }
    public void AddHealth(float amountToAdd)
    {
        if (curHealth == maxHealth)
            return;

        curHealth += amountToAdd;

        if (curHealth > maxHealth)
            curHealth = maxHealth;
    }

}
