using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private Rigidbody2D characterRB;

    [SerializeField] private PendulumController pendController;

    private Vector3 lastPosition;


    public event Action<bool> OnDragEvent;
    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;

    private bool draggingCheck;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void Update()
    {
        PendulumSwing();
    }
    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Drag.started += OnDragStarted;
        playerInput.Player.Drag.canceled += OnDragCancelled;

        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Disable();
    }

    private void OnDragStarted(InputAction.CallbackContext ctx)
    {
        // ПКМ нажата — начинаем перетаскивание
        draggingCheck = true;

        OnDragEvent?.Invoke(draggingCheck);
        
    }
    private void OnDragCancelled(InputAction.CallbackContext ctx)
    {
        // ПКМ отпущена — заканчиваем перетаскивание
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

    private void PendulumSwing()
    {
        Vector3 velocity =
(transform.position - lastPosition) / Time.deltaTime;

        lastPosition = transform.position;

        pendController.AddImpulse(velocity.x);
    }
}
