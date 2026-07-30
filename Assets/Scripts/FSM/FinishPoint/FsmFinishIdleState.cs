using UnityEngine;

public class FsmFinishIdleState : FsmState
{
    protected readonly FsmFinishController finish;
    public FsmFinishIdleState(Fsm fsm, FsmFinishController finish) : base(fsm)
    {
        this.finish = finish;
    }

    public override void Enter(FsmContext ctx)
    {
        Debug.Log("FinishIdle State [ENTER]");
        
    }
    public override void Exit()
    {
        Debug.Log("FinishIdle State [EXIT]");
    }
    public override void Update()
    {

        if (!finish.CanCapture)
            return;

        Fsm.SetState<FsmFinishCapturingState>();

    }
}
