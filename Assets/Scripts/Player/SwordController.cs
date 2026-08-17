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

    [Header("Skewer")]
    [SerializeField] private float minSkewerSpeed = 5f;
    [SerializeField] private float minExtensionSpeed = 1f;
    [SerializeField, Range(0f, 1f)]
    private float minSkewerExtension = 0.5f;
    

    [Header("Impact")]
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float maxSwordSpeed = 20f;
    [SerializeField] private float minHitStopDur = 0.01f;
    [SerializeField] private float maxHitStopDur = 0.02f;
    [SerializeField] private float minShakeStr = 0.25f;
    [SerializeField] private float maxShakeStr = 0.75f;

    [Header("Stamina")]
    [SerializeField]
    private StaminaController staminaController;
    [Tooltip("Расход стамины за секунду выдвижения меча")]
    [SerializeField, Min(0f)]
    private float staminaCostPerSecond = 35f;

    [Header("Wall collision")]
    [SerializeField]
    private float wallResistance = 20f;

    [Header("Swing Wall Collision")]
    [SerializeField]
    private LayerMask swingWallMask;

    [SerializeField, Min(0f)]
    private float swingWallOffset = 0.02f;

    [Header("Sword wall blocking")]
    [SerializeField]
    private LayerMask swordObstacleMask;

    [SerializeField, Min(0.01f)]
    private float swordCollisionRadius = 0.1f;

    [SerializeField, Min(0f)]
    private float wallOffset = 0.05f;

    [SerializeField, Min(0.01f)]
    private float swordLengthMultiplier = 1f;

    [SerializeField]
    private Transform swordBase;

    [Header("Door Break")]
    [SerializeField, Min(0f)]
    private float minDoorBreakExtensionSpeed = 3f;

    [SerializeField, Range(0f, 1f)]
    private float minDoorBreakExtension = 0.6f;

    private bool isExtensionBlocked;
    private float blockedLength;

    private bool isDoorExtensionBlocked;
    private float doorBlockedLength;

    [Header("Door skewer")]
    [SerializeField, Min(0f)]
    private float minDoorExtensionSpeed = 2f;

    [SerializeField, Range(0f, 1f)]
    private float minDoorExtension = 0.5f;


    private bool blocked;
    private float blockedAngle;
    private float wallMoveDirection;
    
    private Vector3 lastTipPosition;
    private float currentLength;
    private float targetLength;
    private float angle;
    private float angularVelocity;

    private WallMaterial currentWallMaterial;

    public float AngularVelocity => angularVelocity;
    public EnemyController SkeweredEnemy { get; private set; }
    public Vector2 TipVelocity { get; private set; }

    public float TipSpeed => TipVelocity.magnitude;

    public bool IsPlaced { get; private set; }

    private float previousLength;
    public float ExtensionSpeed { get; private set; }

    public SkewerableDoor SkeweredDoor { get; private set; }

    public bool HasSkeweredObject =>
        SkeweredEnemy != null ||
        SkeweredDoor != null;
    public bool IsExtending =>
    attackBool.isAttacking &&
    currentLength < maxLength;



    private void Awake()
    {

        currentLength = minLength;
        targetLength = minLength;
        previousLength = currentLength;

        if (staminaController == null)
            staminaController = GetComponentInParent<StaminaController>();

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
    public void SetExtensionBlocked(bool blocked)
    {
        if (blocked && !isExtensionBlocked)
        {
            // Запоминаем длину, на которой меч коснулся стены.
            blockedLength = currentLength;
        }

        isExtensionBlocked = blocked;
    }
    public bool CanBreakDoor()
    {
        float normalizedExtension =
            Mathf.InverseLerp(
                minLength,
                maxLength,
                currentLength
            );

        return ExtensionSpeed >= minDoorBreakExtensionSpeed &&
               normalizedExtension >= minDoorBreakExtension;
    }
    public void SetDoorExtensionBlocked(bool blocked)
    {
        if (blocked && !isDoorExtensionBlocked)
        {
            doorBlockedLength = currentLength;
        }

        isDoorExtensionBlocked = blocked;
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

        cameraShake.Shake(
            Mathf.Lerp(minShakeStr, maxShakeStr, impact));
    }

    #region Length

    private void UpdateLength()
    {
        bool wantsToAttack = attackBool.isAttacking;

        if (!wantsToAttack)
        {
            targetLength = minLength;
        }
        else if (isExtensionBlocked || isDoorExtensionBlocked)
        {
            float allowedLength = maxLength;

            if (isExtensionBlocked)
            {
                allowedLength = Mathf.Min(
                    allowedLength,
                    blockedLength
                );
            }

            if (isDoorExtensionBlocked)
            {
                allowedLength = Mathf.Min(
                    allowedLength,
                    doorBlockedLength
                );
            }

            targetLength = allowedLength;
        }
        else
        {
            bool canExtend =
                CanExtendSword(wantsToAttack);

            targetLength = canExtend
                ? maxLength
                : minLength;
        }

        /*
         * ВАЖНО:
         * Если меч сейчас должен быть вытянут,
         * проверяем, сколько места реально есть
         * перед ним до стены.
         *
         * Эта проверка работает не только во время
         * выдвижения, но и когда уже вытянутый меч
         * поворачивается в стену.
         */
        if (wantsToAttack &&
            targetLength > minLength)
        {
            float wallAllowedLength =
                GetAllowedSwordLength(
                    targetLength
                );

            targetLength =
                Mathf.Min(
                    targetLength,
                    wallAllowedLength
                );
        }

        float speed =
            targetLength > currentLength
                ? extendSpeed
                : retractSpeed;

        previousLength =
            currentLength;

        currentLength =
            Mathf.MoveTowards(
                currentLength,
                targetLength,
                speed * Time.deltaTime
            );

        if (Time.deltaTime >
            Mathf.Epsilon)
        {
            ExtensionSpeed =
                (currentLength -
                 previousLength) /
                Time.deltaTime;
        }

        sword.localScale =
            new Vector3(
                sword.localScale.x,
                currentLength,
                sword.localScale.z
            );
    }
    private bool CanExtendSword(bool wantsToAttack)
    {
        if (!wantsToAttack)
            return false;

        if (staminaController == null)
            return true;

        bool isStartingNewAttack =
            currentLength <= minLength + 0.01f;

        if (isStartingNewAttack &&
            !staminaController.CanStartAttack)
        {
            return false;
        }

        bool isStillExtending =
            currentLength < maxLength - 0.01f;

        if (!isStillExtending)
            return true;

        float cost =
            staminaCostPerSecond * Time.deltaTime;

        return staminaController.TrySpend(cost);
    }
    private float GetAllowedSwordLength(
     float desiredLength)
    {
        Vector2 origin =
        swordBase != null
            ? swordBase.position
            : transform.position;

        Vector2 direction =
            -transform.up;

        // Реальная текущая длина меча в world space.
        float currentWorldLength =
            Vector2.Distance(
                origin,
                skewerPoint.position
            );

        // Определяем, сколько world units
        // приходится на 1 единицу currentLength.
        float worldPerLengthUnit =
            currentLength > Mathf.Epsilon
                ? currentWorldLength / currentLength
                : 1f;

        float desiredWorldDistance =
            desiredLength *
            worldPerLengthUnit;

        RaycastHit2D hit =
            Physics2D.Raycast(
                origin,
                direction,
                desiredWorldDistance,
                swordObstacleMask
            );

        if (hit.collider == null)
            return desiredLength;

        float allowedWorldDistance =
            Mathf.Max(
                0f,
                hit.distance - wallOffset
            );

        float allowedLength =
            allowedWorldDistance /
            worldPerLengthUnit;

        return Mathf.Clamp(
            allowedLength,
            minLength,
            desiredLength
        );
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

        angularVelocity += -angle * spring * Time.deltaTime;
        angularVelocity *= damping;

        if (blocked && currentWallMaterial != null)
        {
            bool movingIntoWall =
                Mathf.Sign(angularVelocity) == wallMoveDirection;

            if (movingIntoWall)
            {
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

        nextAngle = ClampSwingAngleByWalls(
    angle,
    nextAngle
);

        angle = nextAngle;

        transform.localRotation =
            Quaternion.Euler(0f, 0f, angle);
    }

    private float ClampSwingAngleByWalls(
    float currentAngle,
    float desiredAngle)
    {
        if (skewerPoint == null)
            return desiredAngle;

        Vector2 pivot =
            swordBase != null
                ? swordBase.position
                : transform.position;

        Vector2 currentTip =
            skewerPoint.position;

        float angleDelta =
            desiredAngle - currentAngle;

        Quaternion rotation =
            Quaternion.Euler(
                0f,
                0f,
                angleDelta
            );

        Vector2 currentOffset =
            currentTip - pivot;

        Vector2 desiredOffset =
            rotation * currentOffset;

        Vector2 desiredTip =
            pivot + desiredOffset;

        Vector2 move =
            desiredTip - currentTip;

        float distance =
            move.magnitude;

        if (distance <= Mathf.Epsilon)
            return desiredAngle;

        RaycastHit2D hit =
            Physics2D.Raycast(
                currentTip,
                move.normalized,
                distance + swingWallOffset,
                swingWallMask
            );

        if (hit.collider == null)
            return desiredAngle;

        angularVelocity = 0f;

        return currentAngle;
    }

    #endregion

    #region Skewer
    public bool CanSkewerDoor()
    {
        // На мече уже что-то находится.
        if (SkeweredEnemy != null || SkeweredDoor != null)
            return false;

        if (!IsExtending)
            return false;

        // Меч должен быть вытянут хотя бы на заданную часть.
        if (ExtensionNormalized < minDoorExtension)
            return false;

        // Главное условие: меч должен прямо сейчас выдвигаться.
        if (ExtensionSpeed < minDoorExtensionSpeed)
            return false;

        return true;
    }
    public bool TrySkewerDoor(
    SkewerableDoor door,
    PlayerController player)
    {
        if (door == null || player == null)
            return false;

        // На мече уже что-то находится.
        if (SkeweredEnemy != null || SkeweredDoor != null)
            return false;

        bool success = door.TrySkewer(
            this,
            player,
            skewerPoint
        );

        if (!success)
            return false;

        SkeweredDoor = door;
        return true;
    }

    public void RemoveSkeweredDoor(SkewerableDoor door)
    {
        if (SkeweredDoor != door)
            return;

        SkeweredDoor = null;
    }
    public bool TrySkewer(EnemyController enemy)
    {
        if (SkeweredEnemy != null || SkeweredDoor != null)
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

    public void ConsumeSkeweredEnemy(EnemyController enemy)
    {
        if (SkeweredEnemy != enemy)
            return;

        SkeweredEnemy = null;
    }
    private void CheckSwordVelocity()
    {
        if (Time.deltaTime <= Mathf.Epsilon)
            return;

        TipVelocity = (skewerPoint.position - lastTipPosition) / Time.deltaTime;


        lastTipPosition = skewerPoint.position;
    }
    public float ExtensionNormalized =>
    Mathf.InverseLerp(minLength, maxLength, currentLength);


    public bool CanSkewer()
    {
        

        // Меч должен быть вытянут хотя бы на 50%.
        if (ExtensionNormalized < minSkewerExtension)
            return false;

        // Меч должен продолжать выдвигаться.
        if (ExtensionSpeed < minExtensionSpeed)
            return false;

        // Острие должно двигаться вперёд вдоль меча.
        Vector2 swordForward = -transform.up;

        float forwardSpeed = Vector2.Dot(
            TipVelocity,
            swordForward);
        Debug.Log(forwardSpeed + " Forward speed|| " + ExtensionNormalized + " ExtensionNormalized|| " + ExtensionSpeed + " ExtensionSpeed");
        if (forwardSpeed < minSkewerSpeed)
            return false;

        return true;
    }
    #endregion

}
