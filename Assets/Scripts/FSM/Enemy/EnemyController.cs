using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class EnemyController : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    
    public Animator Animator { get; private set; }
    public Collider2D Collider { get; private set; }
    public PlayerController player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public EnemyKnockback Knockback { get; private set; }
    public Fsm Fsm { get; private set; }

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
        Knockback = GetComponent<EnemyKnockback>();
        player = FindAnyObjectByType<PlayerController>();
        Fsm = new Fsm();

        RegisterCommonStates();
        RegisterSpecificStates();

        SetInitialState();
    }
    protected virtual void Update()
    {
        Fsm.Update();
    }
    protected virtual void SetInitialState()
    {
        Fsm.SetState<FsmEnemyStateIdle>();
    }
    protected virtual void RegisterCommonStates()
    {
        Fsm.AddState(new FsmEnemyStateIdle(Fsm, this));
        Fsm.AddState(new FsmEnemyStateWalk(Fsm, this));
        Fsm.AddState(new FsmEnemyStateKnockback(Fsm, this));
        Fsm.AddState(new FsmEnemyStateSkewered(Fsm, this));
        Fsm.AddState(new FsmEnemyStateThrown(Fsm, this));
        //Fsm.AddState(new FsmEnemyStateDead(Fsm, this));
    }

    protected abstract void RegisterSpecificStates();
}
