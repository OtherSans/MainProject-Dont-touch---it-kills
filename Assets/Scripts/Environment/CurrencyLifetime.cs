using UnityEngine;

public class CurrencyLifetime : MonoBehaviour
{
    private bool isDisappearing;

    public void StartLifetime(float lifetime)
    {
        if (isDisappearing)
            return;

        if (lifetime <= 0f)
            return;

        isDisappearing = true;

        Destroy(gameObject, lifetime);
    }
}
