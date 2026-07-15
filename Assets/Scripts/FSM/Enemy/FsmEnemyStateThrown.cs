using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class FsmEnemyStateThrown : FsmState
{
    private readonly FsmStartingEnemyState enemy;
    private float timer;

    public FsmEnemyStateThrown(Fsm fsm, FsmStartingEnemyState enemy) : base(fsm)
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
        enemy.transform.SetParent(null);
        enemy.Rigidbody.simulated = true;
        enemy.Chase.enabled = false;
        enemy.Rigidbody.linearVelocity = Vector2.zero;
        enemy.Rigidbody.AddForce(thrown.Direction.normalized * thrown.Force, ForceMode2D.Impulse);
        timer = 0f;
        //enemy.Animator.Play("Thrown")
        
    }
    public override void Exit()
    {
        Debug.Log("Thrown State [EXIT]");

        enemy.Chase.enabled = true;

        //enemy.Animator.Play("Walk");
    }
    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.8f)
        {
            Fsm.SetState<FsmEnemyStateWalk>();
        }
    }
}
