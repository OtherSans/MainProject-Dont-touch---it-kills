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

    public InvincibilityController InvincibilityController =>
        invincibilityController;

    public PlayerVisualController VisualController => visualController;


    [SerializeField] private Transform target;
    [SerializeField] private float deltaVelocityThreshold;

    public HealthController PlayerHealth => playerHealth;

    public event Action OnMoveEvent;

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

    }
    private void OnDisable()
    {

        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled -= OnAttackCanceled;

        playerInput.Player.Interact.performed -= OnInteract;

        playerInput.Disable();
    }
    private void Update()
    {
        if (draggingCheck)
            SwordSwing();
        else
            ResetSwordSwingTracking();
    }
    private void FixedUpdate()
    {
        if (draggingCheck)
        {
            rb.linearVelocity = Vector2.zero;
            return;
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
    }
    private void SwordSwing()
    {

        if (Time.deltaTime <= Mathf.Epsilon)
        {
            ResetSwordSwingTracking();
            return;
        }

        Vector3 velocity =
        (transform.position - lastPosition) / Time.deltaTime;

        swordController.AddImpulse(velocity.x);

        lastVelocity = velocity;
        lastPosition = transform.position;
    }
}
