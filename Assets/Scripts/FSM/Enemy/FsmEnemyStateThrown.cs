using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class FsmEnemyStateThrown : FsmState
{
    private readonly EnemyController enemy;
    [SerializeField] private float throwPower = 20f;
    private float timer;

    public FsmEnemyStateThrown(Fsm fsm, EnemyController enemy) : base(fsm)
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
        enemy.Collider.enabled = true;

        enemy.player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        enemy.Rigidbody.linearVelocity = thrown.swordCntr.TipVelocity;


        //enemy.Rigidbody.linearVelocity = thrown.swordCntr.TipVelocity;
        timer = 0f;

        //enemy.Animator.Play("Thrown")


    }
    public override void Exit()
    {
        Debug.Log("Thrown State [EXIT]");

        //enemy.Chase.enabled = true;

        //enemy.Animator.Play("Walk");
    }
    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.8f)
        {
            Fsm.SetState<FsmChaserStateIdle>();
        }
    }
}
