using UnityEngine;

public class SpawningEnemyController : MonoBehaviour
{
    [SerializeField] private float destroyTimer;

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SwingAttack();
    }
    private void SwingAttack()
    {
        
        Destroy(gameObject, destroyTimer);
    }

}
