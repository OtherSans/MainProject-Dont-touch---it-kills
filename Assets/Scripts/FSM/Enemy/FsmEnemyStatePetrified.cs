using UnityEngine;

public class FsmEnemyStatePetrified : FsmState
{
    private readonly EnemyController enemy;
    public FsmEnemyStatePetrified(Fsm fsm, EnemyController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext context)
    {
        Debug.Log("Petrified State [ENTER]");
        enemy.Rigidbody.linearVelocity = Vector2.zero;
        enemy.Rigidbody.angularVelocity = 0f;
        enemy.EnemyUI.enabled = false;
        enemy.Agent.isStopped = true;
    }
    public override void Exit()
    { 
        Debug.Log("Petrified State [EXIT]");
    }
    public override void Update()
    {

    }
}
