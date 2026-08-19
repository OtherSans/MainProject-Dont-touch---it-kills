using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private Rigidbody2D rb;

    [Header("Interaction")]
    public IInteractable CurrentInteractable;

    [Header("Health")]
    public HealthController playerHealth;
    [SerializeField] private int levelUpHealth;

    [Header("Sword")]
    public SwordController swordController;

    [SerializeField]
    private PlayerVisualController visualController;
    [SerializeField]
    private InvincibilityController invincibilityController;
    [SerializeField]
    private StatUpgradeController statUpgradeController;

    [SerializeField]
    private StatUpgradePanel statUpgradePanel;

    private bool isKnockedBack;

    public bool IsKnockedBack => isKnockedBack;

    public InvincibilityController InvincibilityController =>
        invincibilityController;

    public PlayerVisualController VisualController => visualController;


    [SerializeField] private Transform target;
    [SerializeField] private float deltaVelocityThreshold;

    public HealthController PlayerHealth => playerHealth;

    public event Action<bool> OnDragEvent;

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;

    private Vector2 movePosition;
    public Vector2 MovePosition => movePosition;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;

    public WeaponSlot CurrentWeaponSlot { get; set; }

    private bool draggingCheck;
    private bool swingInitialized;

    private void Awake()
    {
        if (statUpgradeController == null)
        {
            statUpgradeController =
                GetComponent<StatUpgradeController>();
        }

        playerInput = new PlayerInput();

        rb = GetComponent<Rigidbody2D>();

        lastPosition = transform.position;
        lastVelocity = Vector3.zero;
    }
    private void Start()
    {
        GameManager.Instance.Experience.LevelIncreased += HandleLevelUp;
    }



    private void OnEnable()
    {
        swingInitialized = false;

        playerInput.Enable();

        playerInput.Player.Drag.started += OnDragStarted;
        playerInput.Player.Drag.canceled += OnDragCancelled;


        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Player.Interact.performed += OnInteract;

        playerInput.Player.StatPanel.performed += OnStatOpen;

    }
    private void OnDisable()
    {

        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled -= OnAttackCanceled;

        playerInput.Player.Interact.performed -= OnInteract;

        playerInput.Player.StatPanel.performed -= OnStatOpen;

        playerInput.Disable();
    }
    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            ResetSwordSwingTracking();
            return;
        }

        if (draggingCheck)
        {
            rb.linearVelocity =
                Vector2.zero;

            SwordSwing();
        }
        else
        {
            ResetSwordSwingTracking();
        }
    }
    public void SetInteractable(IInteractable interactable)
    {
        CurrentInteractable = interactable;
    }
    public void ClearInteractable(IInteractable interactable)
    {
        // Не очищаем ссылку, если другой объект уже занял её.
        if (CurrentInteractable == interactable)
            CurrentInteractable = null;
    }
    private void ResetSwordSwingTracking()
    {
        lastPosition = transform.position;
        lastVelocity = Vector3.zero;
    }
    private void OnDragStarted(InputAction.CallbackContext ctx)
    {
        

        draggingCheck = true;
        rb.linearVelocity = Vector2.zero;

        ResetSwordSwingTracking();

        OnDragEvent?.Invoke(draggingCheck);
        
    }
    private void OnDragCancelled(InputAction.CallbackContext ctx)
    {
        draggingCheck = false;

        ResetSwordSwingTracking();

        OnDragEvent?.Invoke(draggingCheck);
    }
    private void OnStatOpen(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        TryOpenStatUpgrade();
    }

    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        OnAttackStartedEvent?.Invoke();
    }
    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        OnAttackCanceledEvent?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        CurrentInteractable?.Interact(this);
    }
    private void HandleLevelUp(int level)
    {
        playerHealth.IncreaseMaxHealth(levelUpHealth);

        if (statUpgradeController != null)
        {
            statUpgradeController.AddUpgradePoints();
        }
    }
    private void SwordSwing()
    {
        if (Time.fixedDeltaTime <=
               Mathf.Epsilon)
        {
            ResetSwordSwingTracking();
            return;
        }

        Vector3 currentPosition =
            transform.position;

        Vector3 velocity =
            (currentPosition - lastPosition) /
            Time.fixedDeltaTime;

        swordController.AddImpulse(
            velocity.x
        );

        lastVelocity = velocity;
        lastPosition = currentPosition;
    }
    public void TryOpenStatUpgrade()
    {
        if (statUpgradeController == null)
        {
            Debug.LogWarning(
                "StatUpgradeController не найден.",
                this
            );

            return;
        }

        if (statUpgradePanel == null)
        {
            Debug.LogWarning(
                "StatUpgradePanel не назначен.",
                this
            );

            return;
        }

        if (!statUpgradeController.CanUseUpgradeItem)
        {
            Debug.Log(
                "Нельзя открыть улучшение: " +
                "нет предмета или очка улучшения."
            );

            return;
        }

        statUpgradePanel.Open();
    }
    public void ApplyKnockback(
    Vector2 direction,
    float force,
    float duration)
    {
        if (!gameObject.activeInHierarchy)
            return;

        StartCoroutine(
            KnockbackRoutine(
                direction,
                force,
                duration
            )
        );
    }

    private IEnumerator KnockbackRoutine(
        Vector2 direction,
        float force,
        float duration)
    {
        isKnockedBack = true;

        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;

        rb.linearVelocity =
            direction.normalized * force;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;

        isKnockedBack = false;
    }
}
