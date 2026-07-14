using UnityEngine;

public class ScalingChains : MonoBehaviour
{
    [SerializeField] private Vector3 swordTargetScale;
    [SerializeField] private float scaleTime;

    private Rigidbody2D rb;
    private AttackController atackContr;

    private Vector3 chainCurrentScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        atackContr = GetComponentInParent<AttackController>();
        chainCurrentScale = transform.localScale;
    }

    private void Update()
    {
        if(atackContr.isAttacking)
        {
            ChainScaleUp();
        }
        else if(!atackContr.isAttacking)
        {
            ChainScaleDown();
        }
    }

    public void ChainScaleUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, swordTargetScale, Time.deltaTime * scaleTime);
    }
    public void ChainScaleDown()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, chainCurrentScale, Time.deltaTime * scaleTime);
    }
}
