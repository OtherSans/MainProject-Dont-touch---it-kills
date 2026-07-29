using UnityEngine;

public class SkeweredEnemyInteractable : MonoBehaviour, IInteractable
{
    [Header("Absorption")]
    [SerializeField, Min(0f)] private float restoredHealth = 2f;
    [Header("References")]
    [SerializeField] private EnemyController enemy;
    [SerializeField] private HealthController enemyHealth;

    private SwordController sword;
    private PlayerController player;

    private bool isAvailable;
    private bool isAbsorbed;

    public bool IsAvailable => isAvailable && !isAbsorbed;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponent<EnemyController>();
        if (enemyHealth == null)
            enemyHealth = GetComponent<HealthController>();
    }
    public void EnableInteraction(PlayerController playerController, SwordController swordController)
    {
        player = playerController;
        sword = swordController;

        isAvailable = true;
        isAbsorbed = false;

        player.SetInteractable(this);
    }
    public void DisableInteraction()
    {
        isAvailable = false;
        if (player != null)
            player.ClearInteractable(this);

        player = null;
        sword = null;
    }
    public void Interact(PlayerController interactingPlayer)
    {
        if (!isAvailable)
            return;
        if (interactingPlayer == null)
            return;
        Absorb(interactingPlayer);
    }
    private void Absorb(PlayerController interactingPlayer)
    {
        isAbsorbed = true;
        isAvailable = false;

        interactingPlayer.ClearInteractable(this);
        interactingPlayer.PlayerHealth.AddHealth(restoredHealth);

        // Очищаем ссылку SkeweredEnemy у меча.
        if (sword != null)
            sword.ConsumeSkeweredEnemy(enemy);

        /*
         * Лучше завершить врага через HealthController,
         * чтобы сработали OnDied и логика боевой комнаты.
         */
        if (enemyHealth != null)
        {
            enemyHealth.Kill();
            return;
        }

        // Запасной вариант, если HealthController не назначен.
        Destroy(enemy.gameObject);
    }
    private void OnDisable()
    {
        /*
         * При уничтожении врага или выключении комнаты
         * не оставляем его в PlayerController.
         */
        if (player != null)
            player.ClearInteractable(this);
    }
}
