using UnityEngine;

public abstract class FsmState
{
    protected readonly Fsm Fsm;
    public FsmState(Fsm fsm)
    {
        Fsm = fsm;
    }
    public virtual void Enter(FsmContext ctx) { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual bool CanExit()
    {
        return true;
    }
}
