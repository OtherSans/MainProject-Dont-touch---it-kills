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
        

        chaseCont.ChasePlayer();
    }
    public override void Update()
    {
        base.Update();
        
            

    }
    public override void Exit()
    {
        Debug.Log("Chase State [EXIT]");
        
    }
}
