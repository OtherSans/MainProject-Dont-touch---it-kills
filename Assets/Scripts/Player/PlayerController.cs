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

    [SerializeField] private PendulumController pendController;

    private Vector3 lastPosition;


    public event Action<bool> OnDragEvent;
<<<<<<< HEAD

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;

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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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

<<<<<<< HEAD

        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Player.Move.performed += OnMove;
=======
        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;
>>>>>>> parent of aca6323 (Local changes and merge FSM)
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

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
