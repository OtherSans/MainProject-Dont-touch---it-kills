using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private Rigidbody2D characterRB;


    public event Action<bool> OnDragEvent;
    public event Action OnAttackEvent;

    private bool draggingCheck;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Drag.started += OnDragStarted;
        playerInput.Player.Drag.canceled += OnDragCancelled;

        playerInput.Player.Attack.performed += OnAttack;
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.performed -= OnAttack;

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

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        OnAttackEvent?.Invoke();
    }
}
