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
    protected override void SetInitialState()
    {
        Fsm.SetState<FsmChaserStateIdle>();
    }

}
