using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChaseController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private EnemyController enemy;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemy = GetComponent<EnemyController>();
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        transform.rotation = Quaternion.identity;

    }

    public void ChasePlayer()
    {
        agent.isStopped = false;
        if(enemy.player != null)
            agent.SetDestination(enemy.player.transform.position);
    }
    public void StopChasing()
    {
        agent.isStopped = true;
    }
}
