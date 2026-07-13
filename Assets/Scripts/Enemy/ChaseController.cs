using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChaseController : MonoBehaviour
{
    [SerializeField] private Transform target;
    public bool isChasing = false;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }
    public void PatrolAction()
    {
        
    }
    public void ChaseTrue()
    {
        isChasing = true;
    }
    public void ChaseFalse()
    {
        isChasing = false;
    }
}
