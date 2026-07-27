using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    public IInteractable CurrentInteractable;

    public SwordController swordController;
    [SerializeField] private Transform target;
    [SerializeField] private float deltaVelocityThreshold;


    public event Action<bool> OnDragEvent;

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;


    private Vector3 lastPosition;
    private Vector3 lastVelocity;

    public WeaponSlot CurrentWeaponSlot { get; set; }

    private bool draggingCheck;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void Start()
    {
    }



    private void OnEnable()
    {
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
        SwordSwing();
    }
    private void OnDragStarted(InputAction.CallbackContext ctx)
    {
        draggingCheck = true;

        OnDragEvent?.Invoke(draggingCheck);
        
    }
    private void OnDragCancelled(InputAction.CallbackContext ctx)
    {
        draggingCheck = false;

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

    private void SwordSwing()
    {
        if (Time.deltaTime <= Mathf.Epsilon)
        {
            lastPosition = transform.position;
            lastVelocity = Vector3.zero;
            return;
        }

        Vector3 velocity =
        (transform.position - lastPosition) / Time.deltaTime;


        swordController.AddImpulse(velocity.x); 
        lastVelocity = velocity;
        lastPosition = transform.position;
    }
}
