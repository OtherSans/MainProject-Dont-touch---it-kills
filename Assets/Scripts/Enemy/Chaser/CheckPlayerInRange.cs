using UnityEngine;

public class CheckPlayerInRange : MonoBehaviour
{
    public bool IsChasing { get; private set; }

    [SerializeField] private ChaserController enemy;
    private ChaseController chasingContr;
    private void Awake()
    {
        chasingContr = GetComponent<ChaseController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        IsChasing = true;

        enemy.Fsm.SetState<FsmEnemyStateChase>(new FsmChaseContext
        {
            chaseContr = chasingContr
            
        });
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        IsChasing = false;

        enemy.Fsm.SetState<FsmEnemyStateIdle>();
    }
}
