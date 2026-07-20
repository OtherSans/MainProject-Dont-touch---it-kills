using UnityEngine;

public class FsmFinishCapturingState : FsmState
{
    protected readonly FsmFinishController finish;
    public FsmFinishCapturingState(Fsm fsm, FsmFinishController finish) : base(fsm)
    {
        this.finish = finish;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("FinishCapturing State [ENTER]");
        //finish.CurrentCaptureTime = 0;
    }
    public override void Exit()
    {
        Debug.Log("FinishCapturing State [EXIT]");
        
    }
    public override void Update()
    {
        if (!finish.PlayerInside)
        {
            Fsm.SetState<FsmFinishIdleState>();
            return;
        }

        finish.CurrentCaptureTime += Time.deltaTime;

        finish.CaptureBar.fillAmount =
            finish.CurrentCaptureTime /
            finish.CaptureTime;

        if (finish.CurrentCaptureTime >= finish.CaptureTime)
        {
            Fsm.SetState<FsmFinishCapturedState>();
        }
    }
}
