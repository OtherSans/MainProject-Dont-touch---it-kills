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
=======
<<<<<<< HEAD
>>>>>>> parent of aa5758a (Cancel attack)

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;

    public event Action OnAttackEvent;


    public event Action<bool> OnMoveEvent;




    private bool draggingCheck;
    private void Awake()
    {
        playerInput = new PlayerInput();
    }
<<<<<<< HEAD
=======

>>>>>>> parent of aa5758a (Cancel attack)
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

<<<<<<< HEAD

        playerInput.Player.Attack.performed += OnAttack;

=======
<<<<<<< HEAD

        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Player.Move.performed += OnMove;
=======
        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;
>>>>>>> parent of aca6323 (Local changes and merge FSM)
>>>>>>> parent of aa5758a (Cancel attack)
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Disable();
    }
    private void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
        }
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

    private void PendulumSwing()
    {
        Vector3 velocity =
(transform.position - lastPosition) / Time.deltaTime;

        lastPosition = transform.position;

        pendController.AddImpulse(velocity.x);
    }
}
