using System.Collections;
using UnityEngine;

public class FsmEnemyStateSkewered : FsmState
{
    private readonly EnemyController enemy;
    private SwordController sword;
    private PlayerController player;
    private SkeweredEnemyInteractable interactable;
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

        player = sword.GetComponentInParent<PlayerController>();

        interactable =
            enemy.GetComponent<SkeweredEnemyInteractable>();

        //enemy.Chase.enabled = false;
        if(enemy.Agent != null)
            enemy.Agent.enabled = false;

        enemy.Rigidbody.simulated = false;
        enemy.Rigidbody.linearVelocity = Vector2.zero;
        enemy.Collider.enabled = false;
        
        enemy.transform.SetParent(skewer.SkewerPoint);
        enemy.transform.localPosition = new Vector3(0, -0.2f, 0);
        //enemy.transform.position = skewer.SkewerPoint.position;
        //enemy.Animator.Play("Skewered");
        if (player == null)
        {
            Debug.LogError(
                "PlayerController не найден среди родителей SwordController.",
                sword
            );

            return;
        }

        if (interactable == null)
        {
            Debug.LogError(
                "На враге отсутствует SkeweredEnemyInteractable.",
                enemy
            );

            return;
        }

        interactable.EnableInteraction(player, sword);

    }
    public override void Exit()
    {
        if (interactable != null)
            interactable.DisableInteraction();

        enemy.transform.SetParent(null);
        enemy.Collider.enabled = true;
        enemy.Rigidbody.simulated = true;

        if (enemy.Agent != null)
            enemy.Agent.enabled = true;

        interactable = null;
        player = null;
        sword = null;

        Debug.Log("Skewer State [EXIT]");
    }
    public override void Update()
    {

    }
    
}
