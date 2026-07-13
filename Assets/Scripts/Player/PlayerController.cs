using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private Rigidbody2D characterRB;
    [SerializeField] private Transform target;

    private NavMeshAgent agent;


    public event Action<bool> OnDragEvent;
    public event Action OnAttackEvent;
    public event Action<bool> OnMoveEvent;

    private bool draggingCheck;
    private bool movingCheck = false;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Drag.started += OnDragStarted;
        playerInput.Player.Drag.canceled += OnDragCancelled;

        playerInput.Player.Attack.performed += OnAttack;

        playerInput.Player.Move.performed += OnMove;
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.performed -= OnAttack;

        playerInput.Player.Move.performed -= OnMove;

        playerInput.Disable();
    }
    private void Update()
    {
        
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

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        OnAttackEvent?.Invoke();
    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        movingCheck = !movingCheck;
        OnMoveEvent?.Invoke(movingCheck);
    }

    public void NavMoving()
    {
        if (target != null)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
    }

    public void NavStop()
    {
        if (target != null)
        {
            agent.isStopped = true;
        }
    }
}
