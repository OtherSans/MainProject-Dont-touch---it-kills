using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class EnemyController : MonoBehaviour
{

    [Header("Skewered Visual")]
    [SerializeField]
    private Transform visual;

    [SerializeField]
    private Vector3 skeweredLocalPosition =
        new Vector3(0f, -0.2f, 0f);

    [SerializeField]
    private float skeweredVisualRotation = 90f;

    public Transform Visual => visual;

    public Vector3 SkeweredLocalPosition =>
        skeweredLocalPosition;

    public float SkeweredVisualRotation =>
        skeweredVisualRotation;

    [SerializeField, Min(0f)]
    private float skewerHealthThreshold = 20f;
    private RoomController roomController;

    public RoomController _RoomController =>
    roomController;

    public Rigidbody2D Rigidbody { get; private set; }

    public Animator Animator { get; private set; }
    public Collider2D Collider { get; private set; }
    public Canvas EnemyUI { get; private set; }
    public PlayerController player { get; private set; }
    public EnemyAttack EnemyAttack { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public PetrifiedController PetrifiedController { get; private set; }
    public EnemyKnockback Knockback { get; private set; }
    public Fsm Fsm { get; private set; }

    public bool IsSkewered =>
    Fsm.CurrentState is FsmEnemyStateSkewered;

    public bool IsSleeping { get;  set; }

    protected virtual void Awake()
    {

        Rigidbody = GetComponent<Rigidbody2D>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyUI = GetComponentInChildren<Canvas>();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
        Knockback = GetComponent<EnemyKnockback>();
        PetrifiedController = GetComponent<PetrifiedController>();
        player = FindAnyObjectByType<PlayerController>();
        Fsm = new Fsm();

        RegisterCommonStates();
        RegisterSpecificStates();

        SetInitialState();
    }
    protected virtual void Update()
    {
        Fsm.Update();
    }
    protected virtual void SetInitialState()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }

    public virtual void OnRoomActivated()
    {
        WakeUp();
    }
    protected virtual void RegisterCommonStates()
    {
        Fsm.AddState(new FsmEnemyStateIdle(Fsm, this));
        Fsm.AddState(new FsmEnemyStateSleep(Fsm, this));
        Fsm.AddState(new FsmEnemyStateWalk(Fsm, this));
        Fsm.AddState(new FsmEnemyStateKnockback(Fsm, this));
        Fsm.AddState(new FsmEnemyStateSkewered(Fsm, this));
        Fsm.AddState(new FsmEnemyStateDropped(Fsm, this));
        Fsm.AddState(new FsmEnemyStateStun(Fsm, this));
        Fsm.AddState(new FsmEnemyStatePetrified(Fsm, this));
        //Fsm.AddState(new FsmEnemyStateDead(Fsm, this));
    }
    public void Initialize(
    PlayerController playerController,
    RoomController room)
    {
        player = playerController;
        roomController = room;
    }
    public virtual void OnDroppedFromSword()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }
    public virtual void OnKnockbackFinished()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }

    public virtual void OnSkewered()
    {
    }

    public virtual void OnUnskewered()
    {
    }
    public virtual void Stun(float duration)
    {
        if (duration <= 0f)
            return;

        /*
         * Окаменевших и уже нанизанных врагов
         * повторно оглушать не нужно.
         *
         * Если у твоего Fsm нет CurrentState,
         * этот блок пока можно не добавлять.
         */

        Fsm.SetState<FsmEnemyStateStun>(
            new FsmStunContext
            {
                Duration = duration
            }
        );
    }

    public virtual void OnStunFinished()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }

    public bool CanReceiveSwordHit()
    {
        if (Fsm.CurrentState is FsmEnemyStateSkewered)
            return false;

        return true;
    }
    public bool CanBeSkewered()
    {
        // Спящего врага можно нанизать независимо от HP.
        if (IsSleeping)
            return true;

        HealthController health =
            GetComponent<HealthController>();

        if (health == null)
            return false;

        // Бодрствующего можно нанизать только при низком здоровье.
        return health.CurHealth <=
               skewerHealthThreshold;
    }
    

    public virtual void OnThrownFinished()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }

    public virtual void EnterSleepState()
    {
        IsSleeping = true;

        if (Agent != null)
        {
            Agent.ResetPath();
            Agent.isStopped = true;
        }

        Fsm.SetState<FsmEnemyStateSleep>();
    }

    public virtual void WakeUp()
    {
        if (!IsSleeping)
            return;

        IsSleeping = false;

        if (Agent != null)
            Agent.isStopped = false;
    }
    protected abstract void RegisterSpecificStates();

}
