using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class FsmEnemyStateThrown : FsmState
{
    private readonly EnemyController enemy;
    private ThrownEnemyDamage thrownDamage;
    private float timer;

    public FsmEnemyStateThrown(
        Fsm fsm,
        EnemyController enemy)
        : base(fsm)
    {
        this.enemy = enemy;
    }

    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Thrown State [ENTER]");

        if (ctx is not FsmThrownContext thrown)
        {
            Debug.LogError("ThrownContext expected.");
            return;
        }

        enemy.Rigidbody.bodyType =
            RigidbodyType2D.Dynamic;

        enemy.transform.SetParent(null);
        enemy.Rigidbody.simulated = true;
        enemy.Collider.enabled = true;

        Vector2 velocity =
            thrown.swordCntr.TipVelocity;

        velocity = Vector2.ClampMagnitude(
            velocity,
            thrown.swordCntr.maxThrowSpeed
        );

        enemy.Rigidbody.linearVelocity =
            velocity;

        thrownDamage =
            enemy.GetComponent<ThrownEnemyDamage>();

        if (thrownDamage != null)
            thrownDamage.EnableDamage();

        timer = 0f;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer <= 0.8f)
            return;

        enemy.OnThrownFinished();
    }

    public override void Exit()
    {
        Debug.Log("Thrown State [EXIT]");

        enemy.Rigidbody.bodyType =
            RigidbodyType2D.Kinematic;

        if (thrownDamage != null)
            thrownDamage.DisableDamage();

        thrownDamage = null;
    }
}
