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
        float captureSpeed = 0f;

        if (finish.PlayerInside)
        {
            captureSpeed = finish.PlayerCaptureSpeed;
        }
        else if (finish.WeaponPlaced)
        {
            captureSpeed = finish.WeaponCaptureSpeed;
        }

        if (captureSpeed > 0f)
        {
            finish.CurrentCaptureProgress +=
                captureSpeed * Time.deltaTime;
        }
        else
        {
            finish.CurrentCaptureProgress -=
            finish.DecaySpeed * Time.deltaTime;
        }


        finish.CurrentCaptureProgress =
        Mathf.Clamp(
            finish.CurrentCaptureProgress,
            0,
            finish.CaptureRequired);

        finish.CaptureBar.fillAmount =
            finish.CurrentCaptureProgress /
            finish.CaptureRequired;

        if (finish.CurrentCaptureProgress <= 0)
        {
            Fsm.SetState<FsmFinishIdleState>();
            return;
        }

        if (finish.CurrentCaptureProgress >= finish.CaptureRequired)
        {
            Fsm.SetState<FsmFinishCapturedState>();
        }
    }
}
