using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController enemyContr;
    [SerializeField] private float attackDamage;
    [SerializeField] private string targetTag;

    private void Awake()
    {
        enemyContr = GetComponent<EnemyController>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (enemyContr.PetrifiedController.IsPetrified)
            return;
        if (collision.gameObject.CompareTag(targetTag))
        {
            var healthContr = collision.gameObject.GetComponent<HealthController>();
            healthContr.TakeDamage(attackDamage);
        }
    }

}
