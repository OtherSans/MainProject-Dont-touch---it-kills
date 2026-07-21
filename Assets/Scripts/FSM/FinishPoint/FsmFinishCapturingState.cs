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
        


        if (finish.PlayerInside)
        {
            finish.CurrentCaptureTime += Time.deltaTime;

            finish.CaptureBar.fillAmount =
                finish.CurrentCaptureTime /
                finish.CaptureTime;
        }
        else if(finish.WeaponPlaced)
        {
            Debug.Log("DONE");
            //pause capture
        }
        else
        {
            finish.CurrentCaptureTime -=
            finish.DecaySpeed * Time.deltaTime;
        }


        finish.CurrentCaptureTime =
        Mathf.Clamp(
            finish.CurrentCaptureTime,
            0,
            finish.CaptureTime);

        finish.CaptureBar.fillAmount =
            finish.CurrentCaptureTime /
            finish.CaptureTime;

        if (finish.CurrentCaptureTime <= 0)
        {
            Fsm.SetState<FsmFinishIdleState>();
            return;
        }

        if (finish.CurrentCaptureTime >= finish.CaptureTime)
        {
            Fsm.SetState<FsmFinishCapturedState>();
        }
    }
}
