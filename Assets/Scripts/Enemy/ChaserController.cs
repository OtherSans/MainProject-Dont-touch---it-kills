using UnityEngine;

public class ChaserController : EnemyController
{
    public ChaseController Chase { get; private set; }
    public CheckPlayerInRange playerInRange { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        Chase = GetComponent<ChaseController>();
        playerInRange = GetComponent<CheckPlayerInRange>();
    }

    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(new FsmEnemyStateChase(Fsm, this));
        Fsm.AddState(new FsmChaserStateIdle(Fsm, this));
    }

    public override void OnKnockbackFinished()
    {
        Fsm.SetState<FsmChaserStateIdle>();
    }
    protected override void SetInitialState()
    {
        Fsm.SetState<FsmChaserStateIdle>();
    }
    public override void WakeUp()
    {
        if (!IsSleeping)
            return;

        base.WakeUp();

        Fsm.SetState<FsmChaserStateIdle>();
    }
    public override void OnThrownFinished()
    {
        Fsm.SetState<FsmChaserStateIdle>();
    }
    public override void OnStunFinished()
    {
        Fsm.SetState<FsmChaserStateIdle>();
    }
}
