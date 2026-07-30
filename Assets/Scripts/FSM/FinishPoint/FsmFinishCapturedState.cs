using UnityEngine;

public class FsmFinishCapturedState : FsmState
{
    protected readonly FsmFinishController finish;
    public FsmFinishCapturedState(Fsm fsm, FsmFinishController finish) : base(fsm)
    {
        this.finish = finish;
    }
    public override void Enter(FsmContext ctx)
    {
        Debug.Log("FinishCaptured State [ENTER]");
        Debug.Log("Room Complete");

        finish.CaptureController.CapturePerform();

        foreach (
            EnemyController enemy
            in finish.CaptureController.Enemies
        )
        {
            if (enemy == null)
                continue;

            enemy.PetrifiedController.Petrify();
        }

        finish.CompleteCapture();
    }
    public override void Exit()
    {
        Debug.Log("FinishCaptured State [EXIT]");
    }
    public override void Update()
    {

    }
}
