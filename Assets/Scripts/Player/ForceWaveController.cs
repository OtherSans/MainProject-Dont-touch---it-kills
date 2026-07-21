using UnityEngine;

public class ForceWaveController : MonoBehaviour
{
    [SerializeField] private PlayerController playerContr;
    
    public float waveRadius = 5f;
    public float knockbackStrength = 10f;
    public LayerMask affectedLayer;

    private void OnEnable()
    {
        if(playerContr != null)
        {
            playerContr.OnForcePerformedEvent += TriggerWave;
        }
    }
    private void OnDisable()
    {
        if (playerContr != null)
        {
            playerContr.OnForcePerformedEvent -= TriggerWave;
        }
    }

    public void TriggerWave()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, waveRadius, affectedLayer);

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 forceDirection = (hit.transform.position - transform.position).normalized;

                rb.AddForce(forceDirection * knockbackStrength, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}
