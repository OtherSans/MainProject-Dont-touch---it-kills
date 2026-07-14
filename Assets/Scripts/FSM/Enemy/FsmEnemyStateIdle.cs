using UnityEngine;

public class FsmEnemyStateIdle : FsmState
{
    protected readonly ChaseController chaseContr;
    public FsmEnemyStateIdle(Fsm fsm, ChaseController chaseContr) : base(fsm)
    {
        this.chaseContr = chaseContr;
    }
    public override void Enter()
    {
        Debug.Log("Idle State [ENTER]");
    }
    public override void Exit()
    {
        Debug.Log("Idle State [EXIT]");
    }
    public override void Update()
    {
        if (chaseContr.isChasing)
            Fsm.SetState<FsmEnemyStateWalk>();
    }
}
