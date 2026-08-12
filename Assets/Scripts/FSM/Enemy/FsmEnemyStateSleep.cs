using UnityEngine;

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
        Debug.Log(
        $"{enemy.name}: Sleep [ENTER]"
    );

        enemy.IsSleeping = true;

        if (enemy.Agent != null &&
            enemy.Agent.isActiveAndEnabled &&
            enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.ResetPath();
            enemy.Agent.isStopped = true;
        }

        if (enemy.Rigidbody != null)
        {
            enemy.Rigidbody.linearVelocity =
                Vector2.zero;
        }
    }

    public override void Update()
    {
        // Ничего не делает.
    }

    public override void Exit()
    {
        enemy.IsSleeping = false;

        if (enemy.Agent != null &&
            enemy.Agent.isActiveAndEnabled &&
            enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.isStopped = false;
        }
    }
}