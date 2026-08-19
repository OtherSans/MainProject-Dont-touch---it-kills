using UnityEngine;

public class BlockerController : EnemyController
{
    protected override void SetInitialState()
    {
        Fsm.SetState<FsmEnemyStateSleep>();
    }

    public override void OnRoomActivated()
    {
        WakeUp();

        // Никуда не идём.
        Fsm.SetState<FsmBlockerStateIdle>();
    }

    public override void OnKnockbackFinished()
    {
        Fsm.SetState<FsmBlockerStateIdle>();
    }

    public override void OnStunFinished()
    {
        Fsm.SetState<FsmBlockerStateIdle>();
    }

    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(new FsmBlockerStateIdle(Fsm, this));
    }
}
