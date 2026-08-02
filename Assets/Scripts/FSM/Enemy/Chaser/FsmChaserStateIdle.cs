using UnityEngine;

public class FsmChaserStateIdle : FsmState
{
    private ChaserController enemy;
    public FsmChaserStateIdle(Fsm fsm, ChaserController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Chaser Idle State [ENTER]");
        enemy.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public override void Update()
    {
        base.Update();

        if(enemy.playerInRange.IsChasing)
        {
            enemy.Fsm.SetState<FsmEnemyStateChase>(new FsmChaseContext
            {
                chaseContr = enemy.playerInRange.chasingContr

            });
        }
    }
    public override void Exit()
    {
        Debug.Log("Chaser Idle State [EXIT]");
    }
}
