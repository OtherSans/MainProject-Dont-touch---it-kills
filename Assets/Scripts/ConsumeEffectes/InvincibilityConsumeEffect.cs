using System.Collections;
using UnityEngine;

public class InvincibilityConsumeEffect : ConsumeEffect
{
    [SerializeField, Min(0f)]
    private float duration = 5f;

    public override void Apply(PlayerController player)
    {
        if (player == null)
            return;

        if (player.InvincibilityController == null)
        {
            Debug.LogError(
                $"{player.name}: InvincibilityController не назначен.",
                player
            );

            return;
        }

        player.InvincibilityController.StartInvincibility(duration);
    }
}
