using UnityEngine;

public class FsmEnemyStateIdle : FsmState
{
    private readonly EnemyController enemy;
    public FsmEnemyStateIdle(Fsm fsm, EnemyController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Idle State [ENTER]");
        enemy.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public override void Exit()
    {
        Debug.Log("Idle State [EXIT]");
    }
    public override void Update()
    {
    }
}
