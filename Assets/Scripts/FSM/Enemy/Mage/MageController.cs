using UnityEngine;

public class MageController : EnemyController
{
    [Header("Attack")]
    [SerializeField]
    private MageAreaAttack attackPrefab;

    [SerializeField]
    private Transform attackContainer;

    [SerializeField, Min(0.1f)]
    private float attackInterval = 3f;

    [SerializeField, Min(0f)]
    private float firstAttackDelay = 1f;

    [SerializeField, Min(0f)]
    private float positionRandomRadius = 1f;

    [Header("Room")]
    [SerializeField]
    private RoomAlarmController roomAlarmController;

    public MageAreaAttack AttackPrefab => attackPrefab;
    public Transform AttackContainer => attackContainer;
    public float AttackInterval => attackInterval;
    public float FirstAttackDelay => firstAttackDelay;
    public float PositionRandomRadius => positionRandomRadius;

    protected override void Awake()
    {
        base.Awake();

        if (roomAlarmController == null)
        {
            roomAlarmController =
                GetComponentInParent<RoomAlarmController>();
        }

        if (Agent != null)
        {
            Agent.ResetPath();
            Agent.isStopped = true;
            Agent.enabled = false;
        }
    }

    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(
            new FsmMageStateCast(Fsm, this)
        );
    }

    protected override void SetInitialState()
    {
        /*
         * При запуске маг спит вместе с остальными.
         * RoomAlarmController затем разбудит его.
         */
        Fsm.SetState<FsmEnemyStateSleep>();
    }

    public override void WakeUp()
    {
        if (!IsSleeping)
            return;

        base.WakeUp();

        Fsm.SetState<FsmMageStateCast>();
    }

    public override void OnStunFinished()
    {
        Fsm.SetState<FsmMageStateCast>();
    }

    public void CreateAttack()
    {
        if (AttackPrefab == null || player == null)
            return;

        Vector2 randomOffset =
            Random.insideUnitCircle *
            PositionRandomRadius;

        Vector2 attackPosition =
            (Vector2)player.transform.position +
            randomOffset;

        Instantiate(
            AttackPrefab,
            attackPosition,
            Quaternion.identity,
            AttackContainer
        );
    }
}
