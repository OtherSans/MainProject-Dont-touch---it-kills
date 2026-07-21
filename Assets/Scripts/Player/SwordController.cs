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
    [SerializeField] private float impulseThreshold;

    [Header("Throw")]
    [SerializeField] private float throwVelocity = 180f;
    [SerializeField] private float minSkewerSpeed = 5f;
    [SerializeField, Range(0,1)] private float maxExtendLength = 1f;
    [SerializeField] private float minThrowSpeed = 6f;
    public float maxThrowSpeed = 20f;

    private Vector3 lastTipPosition;

    private float currentLength;
    private float targetLength;

    private float angle;
    private float angularVelocity;

    public float AngularVelocity => angularVelocity;

    public EnemyController SkeweredEnemy { get; private set; }
    public Vector2 TipVelocity { get; private set; }

    //проверки на нанизывание
    public float TipSpeed => TipVelocity.magnitude;

    public bool IsPlaced { get; private set; }



    private void Awake()
    {
        currentLength = minLength;
        targetLength = minLength;
    }
    private void Start()
    {
        lastTipPosition = skewerPoint.position;
    }

    private void Update()
    {
        if (IsPlaced)
            return;

        UpdateLength();
        UpdateSwing();
        CheckSwordVelocity();
        CheckThrow();

    }

    public void SetPlaced(bool value)
    {
        IsPlaced = value;
        enabled = !value;
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
        if (Mathf.Abs(impulse) < impulseThreshold)
            return;

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

    public bool TrySkewer(EnemyController enemy)
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
        if (TipVelocity.magnitude < minThrowSpeed)
            return;

        SkeweredEnemy.Fsm.SetState<FsmEnemyStateThrown>(
    new FsmThrownContext()
    {
        swordCntr = this
    });
        
        SkeweredEnemy = null;
    }
    private void CheckSwordVelocity()
    {
        TipVelocity = (skewerPoint.position - lastTipPosition) / Time.deltaTime;

        lastTipPosition = skewerPoint.position;
    }
    public bool IsExtended()
    {
        return currentLength > maxLength * maxExtendLength;
    }
    public bool IsMovingForward()
    {
        if (TipVelocity.sqrMagnitude < 0.01f)
            return false;

        Vector2 tipDirection = TipVelocity.normalized;
        Vector2 swordForward = transform.up;

        return Vector2.Dot(tipDirection, swordForward) > 0.6f;
    }
    public bool CanSkewer()
    {
        if (!IsExtended())
            return false;

        if (TipVelocity.magnitude < minSkewerSpeed)
            return false;

        //if (!IsMovingForward())
        //    return false;

        return true;
    }
    #endregion
}
