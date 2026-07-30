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
        float captureSpeed = GetCaptureSpeed();

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

        finish.CurrentCaptureProgress = Mathf.Clamp(
            finish.CurrentCaptureProgress,
            0f,
            finish.CaptureRequired
        );

        finish.UpdateCaptureBar();

        if (finish.CurrentCaptureProgress >=
            finish.CaptureRequired)
        {
            Fsm.SetState<FsmFinishCapturedState>();
            return;
        }

        if (finish.CurrentCaptureProgress <= 0f &&
            !finish.CanCapture)
        {
            Fsm.SetState<FsmFinishIdleState>();
        }
    }

    private float GetCaptureSpeed()
    {
        // После зачистки комнаты игрок получает быструю скорость.
        // Это условие имеет приоритет, даже если меч всё ещё в слоте.
        if (finish.CanPlayerCapture)
            return finish.PlayerCaptureSpeed;

        // При живых врагах меч выполняет медленный захват.
        if (finish.CanWeaponCapture)
            return finish.WeaponCaptureSpeed;

        return 0f;
    }
}
