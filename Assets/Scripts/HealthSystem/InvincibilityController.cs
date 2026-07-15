using System.Collections;
using UnityEngine;

public class InvincibilityController : MonoBehaviour
{
    private HealthController healthCont;

    private void Awake()
    {
        healthCont = GetComponent<HealthController>();
    }

    public void StartInvincibility(float invincibilityDuration)
    {
        StartCoroutine(InvincibleCoroutine(invincibilityDuration));
    }
    private IEnumerator InvincibleCoroutine(float invincibilityDuration)
    {
        healthCont.IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        healthCont.IsInvincible = false;
    }
}
