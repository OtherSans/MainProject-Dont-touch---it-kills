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
    public override void OnDroppedFromSword()
    {
        if (IsPetrified)
            return;

        Fsm.SetState<FsmChaserStateIdle>();
    }
    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(new FsmEnemyStateChase(Fsm, this));
        Fsm.AddState(new FsmChaserStateIdle(Fsm, this));
    }
    protected override void SetInitialState()
    {

        Fsm.SetState<FsmEnemyStateSleep>();
    }
    public override void OnKnockbackFinished()
    {
        if (IsPetrified)
            return;

        Fsm.SetState<FsmChaserStateIdle>();
    }
    //public override void WakeUp()
    //{
    //    if (!IsSleeping)
    //        return;

    //    base.WakeUp();

    //    Fsm.SetState<FsmChaserStateIdle>();
    //}

    public override void OnRoomActivated()
    {
        if (IsPetrified)
            return;

        Debug.Log(
        $"{name}: CHASER ACTIVATED"
    );

        WakeUp();

        Fsm.SetState<FsmChaserStateIdle>();
    }
    public override void OnThrownFinished()
    {
        Fsm.SetState<FsmChaserStateIdle>();
    }
    public override void OnStunFinished()
    {
        if (IsPetrified)
            return;

        Fsm.SetState<FsmChaserStateIdle>();
    }
}
