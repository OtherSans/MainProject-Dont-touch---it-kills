using UnityEngine;

public class SwordShopDetector : MonoBehaviour
{
    [field: SerializeField]
    public SwordController Sword { get; private set; }
    [SerializeField] private float purchaseCooldown;

    private float nextPurchaseTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            $"SWORD TIP TRIGGERED: {other.name}",
            this
        );
    }

    public bool CanPurchase()
    {
        return Time.time >= nextPurchaseTime;
    }
    public void RegisterPurchase()
    {
        nextPurchaseTime = Time.time + purchaseCooldown;
    }
}
