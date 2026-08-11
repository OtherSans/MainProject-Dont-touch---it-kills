using System;
using UnityEngine;

public class RoomCurrencySpawner : MonoBehaviour
{
    [Serializable] private class CurrencySpawn
    {
        public Transform spawnPoint;
        public GameObject currencyPrefab;
    }

    [Header("Currency")]
    [SerializeField] private CurrencySpawn[] currencies;
    [SerializeField] private float currencyLifetime = 1f;

    private bool hasSpawned;

    public void SpawnCurrency()
    {
        if (hasSpawned)
            return;

        hasSpawned = true;

        foreach(CurrencySpawn currency in currencies)
        {
            if (currency.spawnPoint == null || currency.currencyPrefab == null)
                continue;

            GameObject currencyPref = Instantiate(currency.currencyPrefab, currency.spawnPoint.position, Quaternion.identity);

            CurrencyLifetime lifetime = currencyPref.GetComponent<CurrencyLifetime>();

            if(lifetime != null)
            {
                lifetime.StartLifetime(currencyLifetime);
            }
        }
    }
}
