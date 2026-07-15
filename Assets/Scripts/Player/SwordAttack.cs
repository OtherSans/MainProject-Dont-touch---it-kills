using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    [SerializeField] private string targetTag;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            var healthContr = collision.gameObject.GetComponent<HealthController>();
            healthContr.TakeDamage(attackDamage);
        }
    }
}
