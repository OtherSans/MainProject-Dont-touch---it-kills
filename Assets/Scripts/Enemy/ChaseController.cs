using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChaseController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;

    private void Awake()
    {
        
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void ChasePlayer()
    {
        agent.isStopped = false;
        if(target != null)
            agent.SetDestination(target.position);
    }
    public void StopChasing()
    {
        agent.isStopped = true;
    }
}
