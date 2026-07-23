using System.Drawing;
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
    [SerializeField] private float moveForwardFloat = 0.05f;
    public float maxThrowSpeed = 20f;

    [Header("Impact")]
    [SerializeField] private float maxSwordSpeed = 20f;
    [SerializeField] private float minHitStopDur = 0.01f;
    [SerializeField] private float maxHitStopDur = 0.02f;
    [SerializeField] private float minShakeDur = 0.03f;
    [SerializeField] private float maxShakeDur = 0.08f;
    [SerializeField] private float minShakeStr = 0.03f;
    [SerializeField] private float maxShakeStr = 0.08f;

    [Header("Wall collision")]
    [SerializeField, Range(0f, 1f)]
    private float wallVelocityMultiplier = 0.7f;
    [SerializeField]
    private float wallResistance = 20f;
    [SerializeField]
    private float maxWallPenetrationAngle = 6f;
    [SerializeField]
    private float minWallSpeed = 0.2f;

    private bool blocked;
    private bool wallStuck;
    private float blockedAngle;
    private float wallStuckAngle;
    private float wallMoveDirection;
    
    private float nextWallHitTime;
    private Vector3 lastTipPosition;
    private float currentLength;
    private float targetLength;
    private float angle;
    private float angularVelocity;

    private WallMaterial currentWallMaterial;

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

    public void OnWallHit(WallMaterial wallMaterial)
    {
        Debug.Log($"WALL ENTER | angle: {angle} | speed: {angularVelocity}");

        if (blocked)
            return;

        blocked = true;
        currentWallMaterial = wallMaterial;

        blockedAngle = angle;
        wallMoveDirection = Mathf.Sign(angularVelocity);

        angularVelocity *= wallMaterial.velocityMultiplier;
    }
    public void OnWallHitEnd()
    {
        Debug.Log("WALL EXIT");

        blocked = false;
        currentWallMaterial = null;

    }
    public void SetPlaced(bool value)
    {
        IsPlaced = value;
        enabled = !value;
    }

    public void PlayImpact()
    {
        float impact = Mathf.Clamp01(TipVelocity.magnitude / maxSwordSpeed);

        HitStop.Instance.StopHit(
            Mathf.Lerp(minHitStopDur, maxHitStopDur, impact));

        CameraShake.Instance.Shake(
            Mathf.Lerp(minShakeDur, maxShakeDur, impact),
            Mathf.Lerp(minShakeStr, maxShakeStr, impact));
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
        if (float.IsNaN(impulse) || float.IsInfinity(impulse))
            return;

        if (Mathf.Abs(impulse) < impulseThreshold)
            return;

        angularVelocity += impulse * impulseMultiplier;
    }

    private void UpdateSwing()
    {
        if (Time.deltaTime <= Mathf.Epsilon)
            return;

        if (float.IsNaN(angle) || float.IsInfinity(angle))
            angle = 0f;

        if (float.IsNaN(angularVelocity) ||
            float.IsInfinity(angularVelocity))
        {
            angularVelocity = 0f;
        }
        if (blocked)
        {
            Debug.Log(
                $"BLOCKED | resistance: {wallResistance} | " +
                $"speed: {angularVelocity} | angle: {angle}");
        }

        //if (blocked && wallStuck)
        //{
        //    angularVelocity = 0f;
        //    angle = wallStuckAngle;

        //    transform.localRotation =
        //        Quaternion.Euler(0f, 0f, angle);

        //    return;
        //}

        angularVelocity += -angle * spring * Time.deltaTime;
        angularVelocity *= damping;

        if (blocked && currentWallMaterial != null)
        {
            bool movingIntoWall =
                Mathf.Sign(angularVelocity) == wallMoveDirection;

            if (movingIntoWall)
            {
                //angularVelocity = Mathf.MoveTowards(
                //    angularVelocity,
                //    0f,
                //    wallResistance * Time.deltaTime);
                angularVelocity *= Mathf.Exp(
    -currentWallMaterial.resistance * Time.deltaTime);
            }
        }

        float nextAngle =
    angle + angularVelocity * Time.deltaTime;

        if (blocked && currentWallMaterial != null)
        {
            float limitAngle =
                blockedAngle +
                wallMoveDirection * currentWallMaterial.maxPenetrationAngle;

            bool passedLimit =
                wallMoveDirection > 0f
                    ? nextAngle > limitAngle
                    : nextAngle < limitAngle;

            if (passedLimit)
            {
                nextAngle = limitAngle;

                // Убираем только скорость, направленную внутрь стены.
                if (Mathf.Sign(angularVelocity) == wallMoveDirection)
                    angularVelocity = 0f;
            }
        }

        angle = nextAngle;

        transform.localRotation =
            Quaternion.Euler(0f, 0f, angle);
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
        if (Time.deltaTime <= Mathf.Epsilon)
            return;

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
        Debug.Log(Vector2.Dot(tipDirection, swordForward));
        return Vector2.Dot(tipDirection, swordForward) > moveForwardFloat;
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
