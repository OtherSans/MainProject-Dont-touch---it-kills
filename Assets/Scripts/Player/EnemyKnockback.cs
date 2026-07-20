using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform target;
    private Vector3 dir;
    [SerializeField] private float force;
    public void KnockbackPerform()
    {
        dir = (transform.position - target.position).normalized;
        rb.AddForce(dir * force, ForceMode2D.Force);
    }
}
