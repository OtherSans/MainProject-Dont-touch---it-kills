using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [Header("Price")]
    [SerializeField, Min(0)]
    private int price;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI priceText;

    [Header("Selection")]
    [SerializeField, Range(0f, 1f)]
    private float minSwordExtension;

    private bool isPurchased;

    private void Start()
    {
        if(priceText != null)
        { 
            priceText.text = price.ToString(); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryPurchaseItem(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        TryPurchaseItem(collision);
    }
    private void TryPurchaseItem(Collider2D collision)
    {
        if (isPurchased)
            return;

        SwordShopDetector detector = collision.GetComponent<SwordShopDetector>();

        if (detector == null)
            return;

        if (!detector.CanPurchase())
            return;

        SwordController touchingSword = detector.Sword;
        if (touchingSword == null)
            return;
        if (touchingSword.ExtensionNormalized < minSwordExtension)
            return;

        PlayerController player = touchingSword.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        CurrencyCollector wallet = player.GetComponent<CurrencyCollector>();

        if(wallet == null)
        {
            Debug.LogWarning(
               "На Player нет CurrencyCollector.",
               player
           );

            return;
        }

        if (!wallet.TrySpendCurrency(price))
        {
            Debug.Log("Out of Money!");
            return;
        }
            
        detector.RegisterPurchase();
        ApplyPurchase(player, touchingSword);

    }

    private void ApplyPurchase(PlayerController player, SwordController touchingSword)
    {
        isPurchased = true;
        Debug.Log($"Товар куплен за {price} монет");
        if (priceText != null)
            priceText.text = "Куплено";
        Destroy(gameObject);
    }
}
