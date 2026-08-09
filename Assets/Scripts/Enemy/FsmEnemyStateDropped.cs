using UnityEngine;

public class FsmEnemyStateDropped : FsmState
{
    private readonly EnemyController enemy;

    public FsmEnemyStateDropped(
        Fsm fsm,
        EnemyController enemy)
        : base(fsm)
    {
        this.enemy = enemy;
    }

    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Dropped State [ENTER]");

        enemy.transform.SetParent(null);

        if (enemy.Rigidbody != null)
        {
            enemy.Rigidbody.simulated = true;

            enemy.Rigidbody.linearVelocity =
                Vector2.zero;

            enemy.Rigidbody.angularVelocity = 0f;

            enemy.Rigidbody.bodyType =
                RigidbodyType2D.Kinematic;
        }

        if (enemy.Collider != null)
            enemy.Collider.enabled = true;

        /*
         * Чуть отодвигаем врага от игрока,
         * чтобы он не оказался внутри него.
         */
        if (enemy.player != null)
        {
            Vector2 direction =
                enemy.transform.position -
                enemy.player.transform.position;

            if (direction.sqrMagnitude >
                Mathf.Epsilon)
            {
                direction.Normalize();

                enemy.transform.position +=
                    (Vector3)(direction * 0.3f);
            }
        }

        enemy.OnDroppedFromSword();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}