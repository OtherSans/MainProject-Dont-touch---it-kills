using System;
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
    [SerializeField] private ForceWaveController forceContr;

    private NavMeshAgent agent;


    public event Action<bool> OnDragEvent;

    public event Action OnAttackStartedEvent;
    public event Action OnAttackCanceledEvent;

    public event Action OnForcePerformedEvent;

    public event Action<bool> OnMoveEvent;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;

    public WeaponSlot CurrentWeaponSlot { get; set; }

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

        playerInput.Player.Drag.performed += OnForcePerformed;


        playerInput.Player.Attack.started += OnAttackStarted;
        playerInput.Player.Attack.canceled += OnAttackCanceled;

        playerInput.Player.Interact.performed += OnInteract;

        //playerInput.Player.Move.performed += OnMove;

    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDragStarted;
        playerInput.Player.Drag.canceled -= OnDragCancelled;

        playerInput.Player.Drag.performed -= OnForcePerformed;

        playerInput.Player.Attack.started -= OnAttackStarted;
        playerInput.Player.Attack.canceled -= OnAttackCanceled;

        playerInput.Player.Interact.performed -= OnInteract;

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
    private void OnForcePerformed(InputAction.CallbackContext ctx)
    {
        OnForcePerformedEvent?.Invoke();
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

    //private void OnMove(InputAction.CallbackContext ctx)
    //{
    //    movingCheck = !movingCheck;
    //    OnMoveEvent?.Invoke(movingCheck);
    //}
    private void SwordSwing()
    {
        Vector3 velocity =
        (transform.position - lastPosition) / Time.deltaTime;

        //Vector3 deltaVelocity = velocity - lastVelocity;
        swordController.AddImpulse(velocity.x);
        lastVelocity = velocity;
        lastPosition = transform.position;
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
