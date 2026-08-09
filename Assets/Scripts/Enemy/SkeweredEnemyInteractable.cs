using UnityEngine;

public class SkeweredEnemyInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField]
    private EnemyController enemy;

    [SerializeField]
    private HealthController enemyHealth;

    [Header("Absorption Effect")]
    [SerializeField]
    private ConsumeEffect[] consumeEffects;

    private SwordController sword;
    private PlayerController player;

    private ConsumeChargeController chargeController;

    private bool isAvailable;
    private bool isAbsorbed;

    public bool IsAvailable =>
        isAvailable && !isAbsorbed;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponent<EnemyController>();

        if (enemyHealth == null)
            enemyHealth =
                GetComponent<HealthController>();

        if (consumeEffects == null ||
            consumeEffects.Length == 0)
        {
            consumeEffects =
                GetComponents<ConsumeEffect>();
        }
    }

    public void EnableInteraction(
        PlayerController playerController,
        SwordController swordController)
    {
        if (isAbsorbed)
            return;

        player = playerController;
        sword = swordController;

        if (player != null)
        {
            chargeController =
                player.GetComponent<ConsumeChargeController>();

            player.SetInteractable(this);
        }

        isAvailable = true;
    }

    public void DisableInteraction()
    {
        isAvailable = false;

        if (player != null)
            player.ClearInteractable(this);

        player = null;
        sword = null;
        chargeController = null;
    }

    public void Interact(
        PlayerController interactingPlayer)
    {
        if (!isAvailable)
            return;

        if (interactingPlayer == null)
            return;

        /*
         * Есть заряд — поглощаем.
         */
        if (chargeController != null &&
            chargeController.TrySpendCharge())
        {
            Absorb(interactingPlayer);
            return;
        }

        /*
         * Заряда нет — просто снимаем врага с меча.
         */
        DropEnemy();
    }

    private void Absorb(
        PlayerController interactingPlayer)
    {
        if (isAbsorbed)
            return;

        isAbsorbed = true;
        isAvailable = false;

        interactingPlayer.ClearInteractable(this);

        ApplyConsumeEffects(interactingPlayer);

        /*
         * Убираем поглощённого врага
         * из SwordController.
         */
        if (sword != null)
            sword.ConsumeSkeweredEnemy(enemy);

        /*
         * Убиваем через HealthController,
         * чтобы сохранилась логика смерти комнаты.
         */
        if (enemyHealth != null)
        {
            enemyHealth.Kill();
            return;
        }

        if (enemy != null)
            Destroy(enemy.gameObject);
        else
            Destroy(gameObject);
    }

    private void DropEnemy()
    {
        if (enemy == null)
            return;

        isAvailable = false;

        if (player != null)
            player.ClearInteractable(this);

        /*
         * Освобождаем ссылку в SwordController.
         */
        if (sword != null)
            sword.ConsumeSkeweredEnemy(enemy);

        /*
         * Выходим из состояния Skewered.
         */
        enemy.Fsm.SetState<FsmEnemyStateDropped>();

        player = null;
        sword = null;
        chargeController = null;
    }

    private void ApplyConsumeEffects(
        PlayerController interactingPlayer)
    {
        if (consumeEffects == null ||
            consumeEffects.Length == 0)
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
        if (player != null)
            player.ClearInteractable(this);

        player = null;
        sword = null;
        chargeController = null;
        isAvailable = false;
    }
}
