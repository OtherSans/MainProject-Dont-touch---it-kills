using UnityEngine;

public class StatUpgradeItemPickup : MonoBehaviour
{
    [SerializeField, Min(1)] private int amount = 1;

    private bool isPickedUp;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();

        col.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp)
            return;
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        StatUpgradeController upgradeController = player.GetComponent<StatUpgradeController>();

        if (upgradeController == null)
        {
            Debug.LogWarning(
                $"{name}: у игрока нет StatUpgradeController.",
                player
            );

            return;
        }

        isPickedUp = true;

        upgradeController.AddUpgradeItems(amount);

        Debug.Log(
            $"Подобран предмет улучшения. " +
            $"Теперь предметов: {upgradeController.UpgradeItems}",
            player
        );

        Destroy(gameObject);
    }
}
