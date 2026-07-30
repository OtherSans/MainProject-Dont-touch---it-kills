using UnityEngine;

public class SkeweredEnemyInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private EnemyController enemy;
    [SerializeField] private HealthController enemyHealth;

    [Header("Absorption Effect")]
    [SerializeField]private ConsumeEffect[] consumeEffects;

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

        /*
         * Если эффекты не назначены вручную,
         * находим их на объекте врага автоматически.
         */
        if (consumeEffects == null || consumeEffects.Length == 0)
            consumeEffects = GetComponents<ConsumeEffect>();
    }
    public void EnableInteraction(PlayerController playerController, SwordController swordController)
    {
        if (isAbsorbed)
            return;

        player = playerController;
        sword = swordController;

        isAvailable = true;

        if (player != null)
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
        if (isAbsorbed)
            return;

        isAbsorbed = true;
        isAvailable = false;

        interactingPlayer.ClearInteractable(this);

        ApplyConsumeEffects(interactingPlayer);

        /*
         * Убираем поглощённого врага из меча.
         */
        if (sword != null)
            sword.ConsumeSkeweredEnemy(enemy);

        /*
         * Убиваем через HealthController,
         * чтобы сработали OnDied и логика комнаты.
         */
        if (enemyHealth != null)
        {
            enemyHealth.Kill();
            return;
        }

        /*
         * Запасной вариант на случай,
         * если HealthController отсутствует.
         */
        if (enemy != null)
            Destroy(enemy.gameObject);
        else
            Destroy(gameObject);
    }
    private void ApplyConsumeEffects(PlayerController interactingPlayer)
    {
        if (consumeEffects == null || consumeEffects.Length == 0)
        {
            Debug.LogWarning(
                $"{name}: не назначен ни один эффект поглощения.",
                this
            );

            return;
        }

        foreach (ConsumeEffect effect in consumeEffects)
        {
            if (effect == null)
                continue;

            effect.Apply(interactingPlayer);
        }
    }
    private void OnDisable()
    {
        /*
         * При уничтожении врага или выключении комнаты
         * не оставляем его в PlayerController.
         */
        if (player != null)
            player.ClearInteractable(this);

        player = null;
        sword = null;
        isAvailable = false;
    }
}
