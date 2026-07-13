using Newtonsoft.Json.Bson;
using UnityEngine;

public class InvincibleStart : MonoBehaviour
{
    [SerializeField] private float invDuration;

    private InvincibilityController invContr;
    private void Awake()
    {
        invContr = GetComponent<InvincibilityController>();
    }
    public void InvincibilityActivate()
    {
        invContr.StartInvincibility(invDuration);
    }
}
