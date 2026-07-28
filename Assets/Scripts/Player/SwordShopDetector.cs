using UnityEngine;

public class SwordShopDetector : MonoBehaviour
{
    [field: SerializeField]
    public SwordController Sword { get; private set; }
    [SerializeField] private float purchaseCooldown;

    private float nextPurchaseTime;

    public bool CanPurchase()
    {
        return Time.time >= nextPurchaseTime;
    }
    public void RegisterPurchase()
    {
        nextPurchaseTime = Time.time + purchaseCooldown;
    }
}
