using UnityEngine;

public class FsmEnemyStateWalk : FsmState
{
    protected readonly ChaseController chaseContr;
    public FsmEnemyStateWalk(Fsm fsm, ChaseController chaseContr) : base(fsm)
    {
        this.chaseContr = chaseContr;
    }
    public override void Enter()
    {
        Debug.Log("Walk State [ENTER]");
    }
    public override void Exit()
    {
        Debug.Log("Walk State [EXIT]");
    }
    public override void Update()
    {
        if (!chaseContr.isChasing)
            Fsm.SetState<FsmEnemyStateIdle>();
        chaseContr.ChasePlayer();
    }
}
