using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] private RoomAlarmController roomAlarmController;

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

    public RoomAlarmController _RoomAlarmController =>
    roomAlarmController;

    public bool IsSleeping { get; private set; }

    protected virtual void Awake()
    {

        if (roomAlarmController == null)
        {
            roomAlarmController =
                GetComponentInParent<RoomAlarmController>();
        }

        if (roomAlarmController == null)
        {
            Debug.LogError(
                $"{name}: RoomAlarmController не найден среди родителей.",
                this
            );
        }

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

    public virtual void OnDroppedFromSword()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }
    public virtual void OnKnockbackFinished()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
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

    public void RaiseRoomAlarm()
    {
        if (roomAlarmController == null)
        {
            Debug.LogError(
                $"{name}: невозможно поднять тревогу — RoomAlarmController отсутствует.",
                this
            );

            return;
        }

        Debug.Log($"{name}: поднимает тревогу комнаты", this);

        roomAlarmController.RaiseAlarm();
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
