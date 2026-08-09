using UnityEngine;

public class FsmEnemyStateKnockback : FsmState
{
    protected readonly EnemyController enemy;
    public FsmEnemyStateKnockback(Fsm fsm, EnemyController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Knockback State [ENTER]");

        if (enemy.Agent != null &&
            enemy.Agent.isActiveAndEnabled &&
            enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.ResetPath();
            enemy.Agent.isStopped = true;
        }

        enemy.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        enemy.Rigidbody.simulated = true;

        enemy.Knockback.knockbackIsRunning = true;
        enemy.Knockback.KnockbackPerform();
    }
    public override void Update()
    {
        base.Update();


        enemy.Rigidbody.linearVelocity = Vector2.Lerp(
    enemy.Rigidbody.linearVelocity,
    Vector2.zero,
    8f * Time.deltaTime);

        if (enemy.Knockback.knockbackIsRunning)
        {
            
            if (enemy.Knockback.timer > 0)
            {
                enemy.Knockback.timer -= Time.deltaTime;
            }
            else
            {
                enemy.Knockback.timer = 0;
                enemy.Knockback.knockbackIsRunning = false;
                Debug.Log("Timer is up!!");
                enemy.Rigidbody.linearVelocity = Vector2.zero;
                enemy.OnKnockbackFinished();
            }
            
        }
    }
    public override void Exit()
    {
        enemy.Rigidbody.linearVelocity = Vector2.zero;
        enemy.Rigidbody.bodyType = RigidbodyType2D.Kinematic;

        if (enemy.Agent != null &&
            enemy.Agent.isActiveAndEnabled &&
            enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.isStopped = false;
        }

        Debug.Log("Knockback State [EXIT]");
    }
}
