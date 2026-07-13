using UnityEngine;
using UnityEngine.Rendering;

public class FsmStartingEnemyState : MonoBehaviour
{
    [SerializeField] private ChaseController chaseContr;
    private Fsm fsm;

    private void Start()
    {
        fsm = new Fsm();

        fsm.AddState(new FsmEnemyStateIdle(fsm, chaseContr));
        fsm.AddState(new FsmEnemyStateWalk(fsm, chaseContr));

        fsm.SetState<FsmEnemyStateIdle>();
    }
    private void Update()
    {
        fsm.Update();
    }
}
