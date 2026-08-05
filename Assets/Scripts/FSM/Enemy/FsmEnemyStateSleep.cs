public class FsmEnemyStateSleep : FsmState
{
    private readonly EnemyController enemy;
    public FsmEnemyStateSleep(Fsm fsm, EnemyController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext ctx)
    {
        //enemy.Animator.SetBool("IsWalking", false);

        if (enemy.Agent != null)
            enemy.Agent.ResetPath();
    }

    public override void Update()
    {
        // Ничего не делает.
    }

    public override void Exit()
    {
    }
}