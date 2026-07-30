using System.Collections;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Invincibility")]
    [SerializeField] private float blinkInterval = 0.12f;
    [SerializeField] private Color invincibleColor = Color.cyan;

    private Coroutine invincibleRoutine;
    private Color defaultColor;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        defaultColor = spriteRenderer.color;
    }

    public void ShowInvincibility()
    {
        if (invincibleRoutine != null)
            StopCoroutine(invincibleRoutine);

        invincibleRoutine = StartCoroutine(InvincibleRoutine());
    }

    public void HideInvincibility()
    {
        if (invincibleRoutine != null)
            StopCoroutine(invincibleRoutine);

        invincibleRoutine = null;

        spriteRenderer.enabled = true;
        spriteRenderer.color = defaultColor;
    }

    private IEnumerator InvincibleRoutine()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            if (spriteRenderer.enabled)
                spriteRenderer.color = invincibleColor;

            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
