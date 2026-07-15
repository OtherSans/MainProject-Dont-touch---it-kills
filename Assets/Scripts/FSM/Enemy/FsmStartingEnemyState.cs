using UnityEngine;
using UnityEngine.Rendering;

public class FsmStartingEnemyState : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public ChaseController Chase { get; private set; }
    public Animator Animator { get; private set; }
    public Collider2D Collider { get; private set; }

    public Fsm Fsm { get; private set; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Chase = GetComponent<ChaseController>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();

        Fsm = new Fsm();

        Fsm.AddState(new FsmEnemyStateIdle(Fsm, this));
        Fsm.AddState(new FsmEnemyStateWalk(Fsm, this));
        Fsm.AddState(new FsmEnemyStateSkewered(Fsm, this));
        Fsm.AddState(new FsmEnemyStateThrown(Fsm, this));

        Fsm.SetState<FsmEnemyStateIdle>();
    }
    private void Update()
    {
        Fsm.Update();
    }
}
