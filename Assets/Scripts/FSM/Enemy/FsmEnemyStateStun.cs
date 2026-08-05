using UnityEngine;

public class FsmEnemyStateStun : FsmState
{
    private readonly EnemyController enemy;

    private float stunTimer;

    public FsmEnemyStateStun(
        Fsm fsm,
        EnemyController enemy)
        : base(fsm)
    {
        this.enemy = enemy;
    }

    public override void Enter(FsmContext context)
    {
        if (context is not FsmStunContext stunContext)
        {
            Debug.LogError(
                "FsmStunContext expected.",
                enemy
            );

            return;
        }

        stunTimer = stunContext.Duration;

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

            enemy.Rigidbody.angularVelocity = 0f;
        }

        if (enemy.EnemyAttack != null)
            enemy.EnemyAttack.enabled = false;

        // Здесь позже можно включить анимацию оглушения.
        // enemy.Animator.SetBool("IsStunned", true);

        Debug.Log(
            $"{enemy.name} stunned for {stunTimer} seconds",
            enemy
        );
    }

    public override void Update()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer > 0f)
            return;

        /*
         * Пока переводим врага в Idle.
         * Для Chaser ниже сделаем отдельное возвращение.
         */
        enemy.OnStunFinished();
    }

    public override void Exit()
    {
        if (enemy.Agent != null &&
            enemy.Agent.isActiveAndEnabled &&
            enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.isStopped = false;
        }

        if (enemy.EnemyAttack != null)
            enemy.EnemyAttack.enabled = true;

        // enemy.Animator.SetBool("IsStunned", false);

        stunTimer = 0f;
    }
}
