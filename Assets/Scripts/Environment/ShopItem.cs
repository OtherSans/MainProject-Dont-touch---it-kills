using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [Header("Price")]
    [SerializeField] private int price;
    [SerializeField] private CurrencyCollector wallet;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Selection")]
    [SerializeField] private float minSwordExtension;

    private bool isPurchased;

    private void Start()
    {
        priceText.text = price.ToString();
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

        SwordController touchingSword = detector.Sword;
        if (touchingSword.ExtensionNormalized < minSwordExtension)
            return;

        if (!wallet.TrySpendCurrency(price))
        {
            Debug.Log("Out of Money!");
            return;
        }
            
        detector.RegisterPurchase();
        ApplyPurchase(touchingSword);

    }

    private void ApplyPurchase(SwordController touchingSword)
    {
        isPurchased = true;
        Debug.Log($"Товар куплен за {price} монет");
        priceText.text = "Куплено";
        Destroy(gameObject);
    }
}
