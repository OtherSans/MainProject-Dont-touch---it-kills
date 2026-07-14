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
<<<<<<< HEAD
<<<<<<< HEAD

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;
=======
    public event Action OnAttackEvent;
>>>>>>> parent of 58e45cd (Local changes and merge attack)

    public event Action OnAttackEvent;
    public event Action<bool> OnMoveEvent;

=======
    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;
>>>>>>> parent of aca6323 (Local changes and merge FSM)

    private bool draggingCheck;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
<<<<<<< HEAD

=======
>>>>>>> parent of 58e45cd (Local changes and merge attack)
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

<<<<<<< HEAD

    private void Update()
    {
        PendulumSwing();
    }

=======
>>>>>>> parent of 58e45cd (Local changes and merge attack)
    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Drag.started += OnDragStarted;
        playerInput.Player.Drag.canceled += OnDragCancelled;

<<<<<<< HEAD
<<<<<<< HEAD

        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Player.Move.performed += OnMove;
=======
        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;
>>>>>>> parent of aca6323 (Local changes and merge FSM)
=======
        playerInput.Player.Attack.performed += OnAttack;
>>>>>>> parent of 58e45cd (Local changes and merge attack)
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.performed -= OnAttack;

        playerInput.Disable();
    }
<<<<<<< HEAD
=======
    private void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
        }
    }
>>>>>>> parent of aca6323 (Local changes and merge FSM)
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
}
