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

        Debug.Log("Level Complete");

        // открыть победное окно
        // остановить игру
    }
    public override void Exit()
    {
        Debug.Log("FinishCaptured State [EXIT]");
    }
    public override void Update()
    {

    }
}
