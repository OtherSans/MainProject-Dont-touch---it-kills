using UnityEngine;

public class InvincibleStart : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float invincibilityDuration = 1f;

    [SerializeField]
    private InvincibilityController invincibilityController;

    private void Awake()
    {
        if (invincibilityController == null)
        {
            invincibilityController =
                GetComponent<InvincibilityController>();
        }

        if (invincibilityController == null)
        {
            Debug.LogError(
                $"{name}: InvincibilityController не найден.",
                this
            );
        }
    }

    public void InvincibilityActivate()
    {
        if (invincibilityController == null)
            return;

        invincibilityController.StartInvincibility(
            invincibilityDuration
        );
    }
}
