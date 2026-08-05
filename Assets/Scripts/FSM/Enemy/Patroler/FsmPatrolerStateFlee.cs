using UnityEngine;
using UnityEngine.AI;

public class FsmPatrolerStateFlee : FsmState
{
    private readonly PatrolerController patroler;

    private float refreshTimer;

    private static readonly float[] AlternativeAngles =
    {
        0f,
        35f,
        -35f,
        70f,
        -70f,
        110f,
        -110f,
        180f
    };

    public FsmPatrolerStateFlee(
        Fsm fsm,
        PatrolerController patroler)
        : base(fsm)
    {
        this.patroler = patroler;
    }

    public override void Enter(FsmContext ctx)
    {
        Debug.Log("Patroler Flee State [ENTER]");

        refreshTimer = 0f;

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
        patroler.Agent.speed = patroler.FleeSpeed;

        UpdateFleeDestination();
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

        refreshTimer -= Time.deltaTime;

        if (refreshTimer > 0f)
            return;

        refreshTimer =
            patroler.PathRefreshInterval;

        UpdateFleeDestination();
    }

    public override void Exit()
    {
        Debug.Log("Patroler Flee State [EXIT]");

        refreshTimer = 0f;

        if (patroler.Agent != null &&
            patroler.Agent.isActiveAndEnabled &&
            patroler.Agent.isOnNavMesh)
        {
            patroler.Agent.ResetPath();
        }
    }

    private void UpdateFleeDestination()
    {
        if (patroler.player == null)
            return;

        Vector2 patrolerPosition =
            patroler.transform.position;

        Vector2 playerPosition =
            patroler.player.transform.position;

        Vector2 awayDirection =
            patrolerPosition - playerPosition;

        if (awayDirection.sqrMagnitude <=
            Mathf.Epsilon)
        {
            awayDirection = Vector2.right;
        }

        awayDirection.Normalize();

        foreach (float angle in AlternativeAngles)
        {
            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) *
                awayDirection;

            Vector2 desiredPosition =
                patrolerPosition +
                direction * patroler.FleeDistance;

            bool foundPoint =
                NavMesh.SamplePosition(
                    desiredPosition,
                    out NavMeshHit hit,
                    patroler.NavMeshSearchRadius,
                    patroler.Agent.areaMask
                );

            if (!foundPoint)
                continue;

            NavMeshPath path = new NavMeshPath();

            bool hasPath =
                patroler.Agent.CalculatePath(
                    hit.position,
                    path
                );

            if (!hasPath)
                continue;

            if (path.status !=
                NavMeshPathStatus.PathComplete)
            {
                continue;
            }

            patroler.Agent.SetDestination(
                hit.position
            );

            return;
        }

        /*
         * Не нашли подходящую точку:
         * не оставляем старый путь, который мог вести
         * в сторону игрока.
         */
        patroler.Agent.ResetPath();
    }
}