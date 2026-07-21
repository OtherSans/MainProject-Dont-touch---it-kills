using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    [SerializeField] private string targetTag;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            var healthContr = collision.gameObject.GetComponent<HealthController>();
            healthContr.TakeDamage(attackDamage);
        }
    }
}
