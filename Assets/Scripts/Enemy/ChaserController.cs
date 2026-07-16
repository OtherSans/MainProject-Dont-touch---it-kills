using UnityEngine;

public class ChaserController : EnemyController
{
    public ChaseController Chase { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        Chase = GetComponent<ChaseController>();
    }
    protected override void RegisterSpecificStates()
    {
        Fsm.AddState(new FsmEnemyStateChase(Fsm, this));
    }

}
