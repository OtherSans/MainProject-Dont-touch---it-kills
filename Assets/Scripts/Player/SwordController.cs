using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform sword;
    [SerializeField] private Transform skewerPoint;
    [SerializeField] private AttackController attackBool;

    [Header("Length")]
    [SerializeField] private float minLength = 1f;
    [SerializeField] private float maxLength = 4f;
    [SerializeField] private float extendSpeed = 5f;
    [SerializeField] private float retractSpeed = 4f;

    [Header("Swing")]
    [SerializeField] private float spring = 35f;
    [SerializeField] private float damping = 0.95f;
    [SerializeField] private float impulseMultiplier = 0.12f;

    [Header("Throw")]
    [SerializeField] private float throwVelocity = 180f;

    private float currentLength;
    private float targetLength;

    private float angle;
    private float angularVelocity;

    public float AngularVelocity => angularVelocity;

    public FsmStartingEnemyState SkeweredEnemy { get; private set; }

    private void Awake()
    {
        currentLength = minLength;
        targetLength = minLength;
    }

    private void Update()
    {
        UpdateLength();
        UpdateSwing();
        CheckThrow();
    }

    #region Length

    private void UpdateLength()
    {
        targetLength = attackBool.isAttacking
            ? maxLength
            : minLength;

        float speed = targetLength > currentLength
            ? extendSpeed
            : retractSpeed;

        currentLength = Mathf.MoveTowards(
            currentLength,
            targetLength,
            speed * Time.deltaTime);

        sword.localScale = new Vector3(
            sword.localScale.x,
            currentLength,
            sword.localScale.z);
    }

    #endregion

    #region Swing

    public void AddImpulse(float impulse)
    {
        angularVelocity += impulse * impulseMultiplier;
    }

    private void UpdateSwing()
    {
        angularVelocity += -angle * spring * Time.deltaTime;

        angularVelocity *= damping;

        angle += angularVelocity * Time.deltaTime;

        transform.localRotation =
            Quaternion.Euler(0, 0, angle);
    }

    #endregion

    #region Skewer

    public bool TrySkewer(FsmStartingEnemyState enemy)
    {
        if (SkeweredEnemy != null)
            return false;

        SkeweredEnemy = enemy;

        enemy.Fsm.SetState<FsmEnemyStateSkewered>(
            new FsmSkewerContext()
            {
                Sword = this,
                SkewerPoint = skewerPoint
            });

        return true;
    }

    #endregion

    #region Throw

    private void CheckThrow()
    {
        if (SkeweredEnemy == null)
            return;

        if (Mathf.Abs(angularVelocity) < throwVelocity)
            return;

        SkeweredEnemy.Fsm.SetState<FsmEnemyStateThrown>(
    new FsmThrownContext()
    {
        Direction = transform.right,
        Force = Mathf.Abs(angularVelocity)
    });

        SkeweredEnemy = null;
    }

    #endregion
}
