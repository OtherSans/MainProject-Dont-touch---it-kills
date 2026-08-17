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

    private bool canCast = true;

    public MageAreaAttack AttackPrefab => attackPrefab;
    public Transform AttackContainer => attackContainer;
    public float AttackInterval => attackInterval;
    public float FirstAttackDelay => firstAttackDelay;
    public float PositionRandomRadius => positionRandomRadius;

    protected override void Awake()
    {
        base.Awake();

        if (Agent != null)
        {
            Agent.ResetPath();
            Agent.isStopped = true;
            Agent.enabled = false;
        }
    }

    public override void OnSkewered()
    {
        canCast = false;

        CancelInvoke();

        StopAllCoroutines();

        if (EnemyAttack != null)
            EnemyAttack.enabled = false;
    }

    public override void OnUnskewered()
    {
        canCast = true;

        if (EnemyAttack != null)
            EnemyAttack.enabled = true;
    }
    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(
            new FsmMageStateCast(Fsm, this)
        );
    }
    public override void OnKnockbackFinished()
    {
        Fsm.SetState<FsmMageStateCast>();
    }
    protected override void SetInitialState()
    {
        /*
         * При запуске маг спит вместе с остальными.
         * RoomAlarmController затем разбудит его.
         */
        Fsm.SetState<FsmEnemyStateSleep>();
    }

    //public override void WakeUp()
    //{
    //    if (!IsSleeping)
    //        return;

    //    base.WakeUp();

    //    Fsm.SetState<FsmMageStateCast>();
    //}

    public override void OnStunFinished()
    {
        Fsm.SetState<FsmMageStateCast>();
    }
    public override void OnRoomActivated()
    {
        Debug.Log("MageWakeUp");
        WakeUp();

        Fsm.SetState<FsmMageStateCast>();
    }
    public void CreateAttack()
    {
        Debug.Log(
        $"Mage CreateAttack | " +
        $"canCast={canCast} | " +
        $"player={(player == null ? "NULL" : player.name)} | " +
        $"prefab={(AttackPrefab == null ? "NULL" : AttackPrefab.name)} | " +
        $"state={Fsm.CurrentState.GetType().Name}"
    );

        if (!canCast)
            return;

        if (Fsm.CurrentState is FsmEnemyStateSkewered)
            return;

        if (AttackPrefab == null)
        {
            Debug.LogError(
                $"{name}: AttackPrefab не назначен.",
                this
            );

            return;
        }

        if (player == null)
        {
            Debug.LogError(
                $"{name}: PlayerController не был передан через Initialize().",
                this
            );

            return;
        }

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
