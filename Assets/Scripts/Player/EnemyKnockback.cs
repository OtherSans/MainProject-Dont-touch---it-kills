using System.Collections;
using Unity.AppUI.Core;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private EnemyController enemyContr;
    [SerializeField] private Transform target;
    private Vector3 dir;
    [SerializeField] private float force;
    public float knockbackTimer;
    public float timer;
    public bool knockbackIsRunning = false;

    private bool useCustomKnockback;
    private Vector2 customDirection;
    private float customForce;
    private float customDuration;
    public void SetState()
    {
        if (enemyContr == null || enemyContr.Fsm == null)
            return;

        if (enemyContr.Fsm.CurrentState is FsmEnemyStateSkewered)
            return;

        if (enemyContr.Fsm.CurrentState is FsmEnemyStatePetrified)
            return;

        if (enemyContr.Fsm.CurrentState is FsmEnemyStateKnockback)
            return;

        // Любой удар будит всю комнату.
        enemyContr.RaiseRoomAlarm();

        // Только атакованный враг получает отбрасывание.
        enemyContr.Fsm.SetState<FsmEnemyStateKnockback>();
    }

    public void KnockbackFrom(
    Vector2 sourcePosition,
    float force,
    float duration)
    {
        Vector2 direction =
            (Vector2)transform.position -
            sourcePosition;

        if (direction.sqrMagnitude <
            0.001f)
        {
            direction =
                Random.insideUnitCircle.normalized;
        }

        customDirection =
            direction.normalized;

        customForce = force;
        customDuration = duration;

        useCustomKnockback = true;

        SetState();
    }
    public void KnockbackPerform()
    {
        if (enemyContr.PetrifiedController.IsPetrified)
            return;

        rb.linearVelocity =
            Vector2.zero;

        if (useCustomKnockback)
        {
            timer =
                customDuration;

            rb.linearVelocity =
                customDirection *
                customForce;

            useCustomKnockback = false;

            return;
        }

        timer =
            knockbackTimer;

        dir =
            transform.position -
            target.position;

        rb.linearVelocity =
            dir.normalized *
            force;
    }

}
