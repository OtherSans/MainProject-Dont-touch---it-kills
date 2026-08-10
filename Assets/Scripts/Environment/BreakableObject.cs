using System;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Header("Durability")]
    [SerializeField, Min(1)]
    private int hitsToBreak = 1;

    [Header("Sword")]
    [SerializeField, Min(0f)]
    private float minSwordSpeed = 2.5f;

    [SerializeField, Min(0f)]
    private float hitCooldown = 0.2f;

    private float nextHitTime;

    public event Action<PlayerController> OnBroken;

    private ConsumeChargePickup chargePickup;

    private int currentHits;
    private bool isBroken;

    public int HitsRemaining =>
        Mathf.Max(0, hitsToBreak - currentHits);
    private void Awake()
    {
        chargePickup =
            GetComponent<ConsumeChargePickup>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBroken)
            return;

        SwordController sword =
            other.GetComponentInParent<SwordController>();

        if (sword == null)
            return;

        if (sword.TipSpeed < minSwordSpeed)
            return;

        if (Time.time < nextHitTime)
            return;

        nextHitTime =
            Time.time + hitCooldown;

        PlayerController player =
            sword.GetComponentInParent<PlayerController>();

        // Импакт происходит ВСЕГДА.
        sword.PlayImpact();

        /*
         * Если это объект восстановления зарядов,
         * а заряды уже полные — durability не уменьшаем.
         */
        if (chargePickup != null &&
            !chargePickup.CanBeDamaged(player))
        {
            return;
        }

        TakeHit(player);
    }

    public void TakeHit(PlayerController player)
    {
        if (isBroken)
            return;

        currentHits++;

        if (currentHits < hitsToBreak)
            return;

        Break(player);
    }

    private void Break(PlayerController player)
    {
        if (isBroken)
            return;

        isBroken = true;

        OnBroken?.Invoke(player);

        Destroy(gameObject);
    }
}
