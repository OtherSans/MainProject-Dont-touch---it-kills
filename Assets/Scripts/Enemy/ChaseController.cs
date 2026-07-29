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
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
        if(target != null)
            agent.SetDestination(target.position);
    }
    public void StopChasing()
    {
        agent.isStopped = true;
    }
}
