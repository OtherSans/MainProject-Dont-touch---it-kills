using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private SwordController swordContr;

    [SerializeField, Min(0f)]
    private float minDamageSpeed = 3f;

    [SerializeField] private RoomAlarmController roomAlarmController;
    [SerializeField] private float maxAttackDamage;
    [SerializeField] private float minAttackDamage;
    [SerializeField] private float maxSwordSpeed;
    [SerializeField] private string targetTag;

    //-------------------
    //Combo Impact
    //-------------------
    private bool comboImpactUnlocked;
    private bool comboReady;
    private float comboDamageMultiplier = 1.5f;
    private float comboDuration = 2f;
    private float comboTimer;
    private void Update()
    {
        if (!comboImpactUnlocked)
            return;
        if (!comboReady)
            return;

        comboTimer -= Time.deltaTime;
        if(comboTimer <= 0f)
        {
            comboReady = false;
            comboTimer = 0f;

            Debug.Log("COMBO IMPACT EXPIRED");
        }
    }
    private void OnTriggerEnter2D(
        Collider2D collision)
    {
        if (swordContr.SkeweredEnemy != null)
            return;

        if (swordContr.TipSpeed < minDamageSpeed)
            return;

        if (!collision.gameObject.CompareTag(targetTag))
            return;

        EnemyController enemy =
            collision.gameObject.GetComponent<EnemyController>();

        if (enemy == null)
            return;

        if (!enemy.CanReceiveSwordHit())
            return;

        roomAlarmController?.RaiseAlarm();

        HealthController healthContr =
            collision.gameObject.GetComponent<HealthController>();

        SpriteFlash spriteFlashContr =
            collision.gameObject.GetComponent<SpriteFlash>();

        if (healthContr == null)
            return;

        float speed =
            swordContr.TipVelocity.magnitude;

        float damage =
            Mathf.Lerp(
                minAttackDamage,
                maxAttackDamage,
                Mathf.Clamp01(
                    speed / maxSwordSpeed
                )
            );

        // Если Combo уже подготовлен,
        // этот удар получает бонус.
        if (comboImpactUnlocked &&
            comboReady)
        {
            damage *= comboDamageMultiplier;

            Debug.Log(
                $"COMBO IMPACT! Damage: {damage}"
            );
        }

        healthContr.TakeDamage(damage);

        if (spriteFlashContr != null)
            spriteFlashContr.Flash();

        swordContr.PlayImpact();

        // Любое успешное попадание
        // запускает новое окно Combo.
        if (comboImpactUnlocked)
        {
            comboReady = true;
            comboTimer = comboDuration;
        }
    }

    // Постоянное повышение Attack Stat
    public void IncreaseDamage(float amount)
    {
        if (amount <= 0f)
            return;

        minAttackDamage += amount;
        maxAttackDamage += amount;

        Debug.Log(
            $"Attack Damage +{amount}. " +
            $"New damage: " +
            $"{minAttackDamage} - {maxAttackDamage}"
        );
    }

    // Включение Combo Impact
    public void UnlockComboImpact(
        float damageMultiplier,
        float duration)
    {
        comboImpactUnlocked = true;

        comboDamageMultiplier =
            Mathf.Max(1f, damageMultiplier);

        comboDuration =
            Mathf.Max(0f, duration);

        comboReady = false;

        Debug.Log(
            $"COMBO IMPACT UNLOCKED | " +
            $"Damage x{comboDamageMultiplier} | " +
            $"Window {comboDuration}s"
        );
    }
}
