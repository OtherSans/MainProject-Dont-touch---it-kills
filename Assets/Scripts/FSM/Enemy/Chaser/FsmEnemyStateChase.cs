using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class FsmEnemyStateChase : FsmState
{
    private ChaserController enemy;
    private ChaseController chaseCont;

    public FsmEnemyStateChase(Fsm fsm, ChaserController enemy) : base(fsm)
    {
        this.enemy = enemy;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Chase State [ENTER]");
        if (ctx is not FsmChaseContext chase)
        {
            Debug.LogError("SkewerContext expected.");
            return;
        }

        chaseCont = chase.chaseContr;
        
    }
    public override void Update()
    {
        base.Update();
        chaseCont.ChasePlayer();
        if (!enemy.playerInRange.IsChasing)
        {
            enemy.Fsm.SetState<FsmChaserStateIdle>();
        }
    }
    public override void Exit()
    {
        chaseCont.StopChasing();
        Debug.Log("Chase State [EXIT]");
    }
}
