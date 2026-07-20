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
        enemy.Agent.isStopped = true;
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
                Fsm.SetState<FsmChaserStateIdle>();
            }
            
        }
    }
    public override void Exit()
    {
        enemy.Agent.isStopped = false;
        Debug.Log("Knockback State [EXIT]");
    }
}
