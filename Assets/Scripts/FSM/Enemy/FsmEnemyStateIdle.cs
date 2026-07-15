using UnityEngine;

public class FsmEnemyStateIdle : FsmState
{
    private readonly FsmStartingEnemyState enemy;
    public FsmEnemyStateIdle(Fsm fsm, FsmStartingEnemyState enemy) : base(fsm)
    {
        this.enemy = enemy;

    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Idle State [ENTER]");
        //enemy.Animator.Play("Idle");
        //enemy.Chase.enabled = false;
    }
    public override void Exit()
    {
        Debug.Log("Idle State [EXIT]");
    }
    public override void Update()
    {
        if (enemy.Chase.isChasing)
        {
            Fsm.SetState<FsmEnemyStateWalk>();
        }
    }
}
