using UnityEngine;

public class HealConsumeEffect : ConsumeEffect
{
    [SerializeField, Min(0f)]
    private float healAmount = 2f;

    public override void Apply(PlayerController player)
    {
        if (player == null)
            return;

        if (player.PlayerHealth == null)
        {
            Debug.LogError(
                $"{name}: у PlayerController отсутствует PlayerHealth.",
                this
            );

            return;
        }

        player.PlayerHealth.AddHealth(healAmount);
    }
}
