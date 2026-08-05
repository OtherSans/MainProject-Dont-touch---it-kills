using UnityEngine;

public class FsmPatrolerStatePatrol : FsmState
{
    private readonly PatrolerController patroler;

    private int currentPointIndex;
    private float waitTimer;

    public FsmPatrolerStatePatrol(
        Fsm fsm,
        PatrolerController patroler)
        : base(fsm)
    {
        this.patroler = patroler;
    }

    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Patroler Patrol State [ENTER]");

        waitTimer = 0f;

        if (patroler.Rigidbody != null)
        {
            patroler.Rigidbody.bodyType =
                RigidbodyType2D.Kinematic;

            patroler.Rigidbody.linearVelocity =
                Vector2.zero;
        }

        if (patroler.Agent == null)
            return;

        patroler.Agent.isStopped = false;
        patroler.Agent.speed = patroler.PatrolSpeed;

        MoveToCurrentPoint();
    }

    public override void Update()
    {
        if (patroler.Agent == null)
            return;

        if (!patroler.Agent.isActiveAndEnabled)
            return;

        if (!patroler.Agent.isOnNavMesh)
            return;

        patroler.UpdateViewDirection();

        Transform[] points = patroler.PatrolPoints;

        if (points == null || points.Length == 0)
        {
            patroler.Agent.ResetPath();
            return;
        }

        if (patroler.Agent.pathPending)
            return;

        if (patroler.Agent.remainingDistance >
            patroler.PointReachDistance)
        {
            return;
        }

        waitTimer += Time.deltaTime;

        if (waitTimer < patroler.WaitAtPoint)
            return;

        waitTimer = 0f;

        currentPointIndex =
            (currentPointIndex + 1) % points.Length;

        MoveToCurrentPoint();
    }

    public override void Exit()
    {
        Debug.Log("Patroler Patrol State [EXIT]");

        waitTimer = 0f;

        if (patroler.Agent != null &&
            patroler.Agent.isActiveAndEnabled)
        {
            patroler.Agent.ResetPath();
        }
    }

    private void MoveToCurrentPoint()
    {
        Transform[] points = patroler.PatrolPoints;

        if (points == null || points.Length == 0)
            return;

        if (currentPointIndex >= points.Length)
            currentPointIndex = 0;

        Transform targetPoint =
            points[currentPointIndex];

        if (targetPoint == null)
            return;

        if (!patroler.Agent.isOnNavMesh)
            return;

        patroler.Agent.SetDestination(
            targetPoint.position
        );
    }
}