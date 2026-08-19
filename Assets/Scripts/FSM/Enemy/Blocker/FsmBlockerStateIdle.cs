using UnityEngine;

public class FsmBlockerStateIdle : FsmState
{
    private readonly BlockerController enemy;

    public FsmBlockerStateIdle(Fsm fsm, 
        BlockerController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }

    public override void Enter(
        FsmContext context)
    {
        if (enemy.Rigidbody != null)
        {
            enemy.Rigidbody.linearVelocity =
                Vector2.zero;

            enemy.Rigidbody.angularVelocity =
                0f;
        }
    }

    public override void Update()
    {
        // Ничего.
    }
}
