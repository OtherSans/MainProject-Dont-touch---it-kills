using System.Collections;
using UnityEngine;

public class InvincibilityController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private PlayerVisualController visualController;

    private Coroutine invincibilityCoroutine;

    public bool IsActive =>
        healthController != null &&
        healthController.IsInvincible;

    private void Awake()
    {
        if (healthController == null)
            healthController = GetComponent<HealthController>();

        if (visualController == null)
            visualController = GetComponent<PlayerVisualController>();

        if (healthController == null)
        {
            Debug.LogError(
                $"{name}: HealthController не найден.",
                this
            );
        }

        if (visualController == null)
        {
            Debug.LogError(
                $"{name}: PlayerVisualController не найден.",
                this
            );
        }
    }

    public void StartInvincibility(float duration)
    {
        if (duration <= 0f)
            return;

        /*
         * Если старая неуязвимость ещё работает,
         * останавливаем её и запускаем новый полный таймер.
         */
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);

        invincibilityCoroutine =
            StartCoroutine(InvincibilityRoutine(duration));
    }

    public void StopInvincibility()
    {
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
            invincibilityCoroutine = null;
        }

        if (healthController != null)
            healthController.IsInvincible = false;

        if (visualController != null)
            visualController.HideInvincibility();
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        healthController.IsInvincible = true;

        if (visualController != null)
            visualController.ShowInvincibility();

        yield return new WaitForSeconds(duration);

        healthController.IsInvincible = false;

        if (visualController != null)
            visualController.HideInvincibility();

        invincibilityCoroutine = null;
    }

    private void OnDisable()
    {
        /*
         * Не оставляем игрока неуязвимым и мигающим,
         * если объект выключили.
         */
        StopInvincibility();
    }
}
