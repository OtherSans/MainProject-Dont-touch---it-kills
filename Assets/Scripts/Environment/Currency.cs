using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] private int CurrencyValue;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out CurrencyCollector collector))
            return;

        collector.AddCurrency(CurrencyValue);

        Destroy(gameObject);
    }
}
