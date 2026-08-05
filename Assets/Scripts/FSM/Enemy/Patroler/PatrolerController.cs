using UnityEngine;

public class PatrolerController : EnemyController
{
    [Header("Room")]
    [SerializeField] private RoomAlarmController roomAlarmController;

    [Header("Patrol")]
    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private Transform viewDirection;

    public Transform ViewDirection => viewDirection;

    [SerializeField, Min(0f)]
    private float patrolSpeed = 2f;

    [SerializeField, Min(0f)]
    private float pointReachDistance = 0.25f;

    [SerializeField, Min(0f)]
    private float waitAtPoint = 0.5f;

    [Header("Flee")]
    [SerializeField, Min(0f)]
    private float fleeSpeed = 4f;

    [SerializeField, Min(0f)]
    private float fleeDistance = 5f;

    [SerializeField, Min(0.02f)]
    private float pathRefreshInterval = 0.15f;

    [SerializeField, Min(0.1f)]
    private float navMeshSearchRadius = 2f;

    public Transform[] PatrolPoints => patrolPoints;
    public float PatrolSpeed => patrolSpeed;
    public float PointReachDistance => pointReachDistance;
    public float WaitAtPoint => waitAtPoint;

    public float FleeSpeed => fleeSpeed;
    public float FleeDistance => fleeDistance;
    public float PathRefreshInterval => pathRefreshInterval;
    public float NavMeshSearchRadius => navMeshSearchRadius;

    protected override void Awake()
    {
        base.Awake();

        if (roomAlarmController == null)
        {
            roomAlarmController =
                GetComponentInParent<RoomAlarmController>();
        }

        /*
         * Для 2D NavMeshAgent обычно отключают
         * стандартное вращение и изменение оси Up.
         */
        if (Agent != null)
        {
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
        }
    }
    public void UpdateViewDirection()
    {
        if (viewDirection == null || Agent == null)
            return;

        Vector2 movementDirection =
            Agent.velocity;

        if (movementDirection.sqrMagnitude < 0.001f)
            return;

        float angle = Mathf.Atan2(
            movementDirection.y,
            movementDirection.x
        ) * Mathf.Rad2Deg;

        viewDirection.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }
    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(
            new FsmPatrolerStatePatrol(Fsm, this)
        );

        Fsm.AddState(
            new FsmPatrolerStateFlee(Fsm, this)
        );
    }
    public override void OnStunFinished()
    {
        if (roomAlarmController != null &&
        roomAlarmController.IsAlarmRaised)
        {
            StartFleeing();
        }
        else
        {
            BeginPatrol();
        }
    }
    protected override void SetInitialState()
    {
        Fsm.SetState<FsmPatrolerStatePatrol>();
    }

    public void BeginPatrol()
    {
        if (Fsm == null)
            return;

        Fsm.SetState<FsmPatrolerStatePatrol>();
    }

    public void DetectPlayer()
    {
        RaiseAlarm();
    }

    public void RaiseAlarm()
    {
        if (roomAlarmController != null)
        {
            roomAlarmController.RaiseAlarm();
            return;
        }

        /*
         * Запасной вариант, если Patroler пока
         * тестируется без RoomAlarmController.
         */
        StartFleeing();
    }

    public void StartFleeing()
    {
        if (Fsm == null)
            return;

        Fsm.SetState<FsmPatrolerStateFlee>();
    }

    /*
     * Patroler не должен переходить в сон вместе
     * с обычными врагами.
     */
    public override void EnterSleepState()
    {
        BeginPatrol();
    }

    public override void WakeUp()
    {
        StartFleeing();
    }
}
