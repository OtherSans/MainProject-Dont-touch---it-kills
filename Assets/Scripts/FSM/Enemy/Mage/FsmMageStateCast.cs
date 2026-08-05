using UnityEngine;

public class FsmMageStateCast : FsmState
{
    private readonly MageController mage;

    private float attackTimer;
    private bool firstAttack;

    public FsmMageStateCast(
        Fsm fsm,
        MageController mage)
        : base(fsm)
    {
        this.mage = mage;
    }

    public override void Enter(FsmContext context)
    {
        Debug.Log("Mage Cast State [ENTER]");

        firstAttack = true;
        attackTimer = mage.FirstAttackDelay;

        if (mage.Rigidbody != null)
        {
            mage.Rigidbody.linearVelocity =
                Vector2.zero;

            mage.Rigidbody.angularVelocity = 0f;

            mage.Rigidbody.bodyType =
                RigidbodyType2D.Kinematic;
        }

        if (mage.EnemyAttack != null)
            mage.EnemyAttack.enabled = false;

        // mage.Animator.Play("Cast");
    }

    public override void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        mage.CreateAttack();

        firstAttack = false;
        attackTimer = mage.AttackInterval;
    }

    public override void Exit()
    {
        attackTimer = 0f;

        Debug.Log("Mage Cast State [EXIT]");
    }
}
