using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private PendulumController pendController;
    [SerializeField] private SwordController swordController;
    [SerializeField] private Transform target;

    private NavMeshAgent agent;


    public event Action<bool> OnDragEvent;

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;

    public event Action<bool> OnMoveEvent;

    private Vector3 lastPosition;


    private bool draggingCheck;
    private bool movingCheck;
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


        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        //playerInput.Player.Move.performed += OnMove;

    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        //playerInput.Player.Move.performed -= OnMove;

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

    //private void OnMove(InputAction.CallbackContext ctx)
    //{
    //    movingCheck = !movingCheck;
    //    OnMoveEvent?.Invoke(movingCheck);
    //}
    private void PendulumSwing()
    {
        Vector3 velocity =
(transform.position - lastPosition) / Time.deltaTime;

        lastPosition = transform.position;

        pendController.AddImpulse(velocity.x);
    }
    private void SwordSwing()
    {
        Vector3 velocity =
(transform.position - lastPosition) / Time.deltaTime;

        lastPosition = transform.position;

        swordController.AddImpulse(velocity.x);
    }
    public void NavMove()
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
