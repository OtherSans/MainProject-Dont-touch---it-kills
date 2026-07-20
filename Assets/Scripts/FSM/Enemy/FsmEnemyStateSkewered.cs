using UnityEngine;

public class FsmEnemyStateSkewered : FsmState
{
    private readonly EnemyController enemy;
    private SwordController sword;
    public FsmEnemyStateSkewered(Fsm fsm, EnemyController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext context)
    {
        Debug.Log("Skewer State [ENTER]");

        if (context is not FsmSkewerContext skewer)
        {
            Debug.LogError("SkewerContext expected.");
            return;
        }

        sword = skewer.Sword;

        //enemy.Chase.enabled = false;
        enemy.Agent.enabled = false;
        enemy.Rigidbody.simulated = false;
        enemy.Rigidbody.linearVelocity = Vector3.zero;
        enemy.Collider.enabled = false;

        enemy.transform.SetParent(skewer.SkewerPoint);
        enemy.transform.localPosition = new Vector3(0, -0.2f, 0);
        //enemy.transform.position = skewer.SkewerPoint.position;

        //enemy.Animator.Play("Skewered");
    }
    public override void Exit()
    {
        enemy.transform.SetParent(null);
        enemy.Collider.enabled = true;
        enemy.Rigidbody.simulated = true;
        enemy.Agent.enabled = true;
        sword = null;

        Debug.Log("Skewer State [EXIT]");
    }
    public override void Update()
    {

    }
}
