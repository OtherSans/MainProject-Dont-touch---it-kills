using UnityEngine;

public class FsmEnemyStateWalk : FsmState
{
    private readonly FsmStartingEnemyState enemy;

    public FsmEnemyStateWalk(Fsm fsm, FsmStartingEnemyState enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Walk State [ENTER]");
        enemy.Chase.ChasePlayer();
        //enemy.Animator.Play("Walk");
        //enemy.Chase.enabled = true;
    }
    public override void Exit()
    {
        Debug.Log("Walk State [EXIT]");
    }
    public override void Update()
    {
        if(!enemy.Chase.isChasing)
        {
            Fsm.SetState<FsmEnemyStateIdle>();
        }
        //enemy.Chase.enabled = false;
    }
}
