using System.Collections;
using UnityEngine;

public class ForceWaveController : MonoBehaviour
{
    [SerializeField] private PlayerController playerContr;
    public float forceDelay = 0.2f;
    public float waveRadius;
    public float knockbackStrength = 10f;
    public LayerMask affectedLayer;
    public Collider2D[] hits;


    private void Update()
    {
        hits = Physics2D.OverlapCircleAll(transform.position, waveRadius, affectedLayer);
    }
    public void TriggerWave()
    {
        
        foreach (Collider2D hit in hits)
        { 
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                Vector2 forceDirection = (hit.transform.position - transform.position).normalized;

                //rb.AddForce(forceDirection * knockbackStrength, ForceMode2D.Impulse);
                rb.linearVelocity = forceDirection * knockbackStrength;
                
                //enemy.Agent.enabled = true;
            }
        }
    }

  

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}
