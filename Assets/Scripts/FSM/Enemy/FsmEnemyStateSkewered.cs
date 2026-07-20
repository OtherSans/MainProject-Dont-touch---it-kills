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
        enemy.Rigidbody.simulated = false;
        enemy.Collider.enabled = false;

        enemy.transform.SetParent(skewer.SkewerPoint);
        enemy.transform.localPosition = Vector3.zero;

        //enemy.Animator.Play("Skewered");
    }
    public override void Exit()
    {
        enemy.transform.SetParent(null);
        enemy.Collider.enabled = true;
        enemy.Rigidbody.simulated = true;

        sword = null;

        Debug.Log("Skewer State [EXIT]");
    }
    public override void Update()
    {

    }
}
