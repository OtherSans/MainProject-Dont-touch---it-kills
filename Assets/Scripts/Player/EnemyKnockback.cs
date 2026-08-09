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
    public void KnockbackPerform()
    {
        if (enemyContr.PetrifiedController.IsPetrified)
            return;
        timer = knockbackTimer;
        rb.linearVelocity = Vector2.zero;
        dir = transform.position - target.position;
        rb.linearVelocity = dir.normalized * force;
    }

}
