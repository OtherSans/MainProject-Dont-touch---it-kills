using System.Collections;
using UnityEngine;

public class SkewerableDoorWallSensor : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SkewerableDoor door;

    [Header("Collision")]
    [SerializeField]
    private LayerMask wallMask;

    [Tooltip(
        "Небольшая задержка после нанизывания, " +
        "чтобы дверь не разбилась об исходную стену сразу"
    )]
    [SerializeField, Min(0f)]
    private float activationDelay = 0.15f;

    private Collider2D sensorCollider;
    private bool isArmed;
    private bool isBreaking;

    private void Awake()
    {
        sensorCollider = GetComponent<Collider2D>();
        sensorCollider.isTrigger = true;

        if (door == null)
            door = GetComponentInParent<SkewerableDoor>();

        sensorCollider.enabled = false;
    }

    public void EnableSensor()
    {
        if (isBreaking)
            return;

        StopAllCoroutines();
        StartCoroutine(EnableAfterDelay());
    }

    public void DisableSensor()
    {
        StopAllCoroutines();

        isArmed = false;
        isBreaking = false;

        if (sensorCollider != null)
            sensorCollider.enabled = false;
    }

    private IEnumerator EnableAfterDelay()
    {
        isArmed = false;
        sensorCollider.enabled = false;

        yield return new WaitForSeconds(
            activationDelay
        );

        if (door == null || !door.IsSkewered)
            yield break;

        sensorCollider.enabled = true;
        isArmed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryBreakDoor(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryBreakDoor(other);
    }

    private void TryBreakDoor(Collider2D other)
    {
        if (!isArmed || isBreaking)
            return;

        if (!IsWall(other))
            return;

        if (door == null || !door.IsSkewered)
            return;

        isBreaking = true;
        isArmed = false;
        sensorCollider.enabled = false;

        door.BreakFromWallImpact();
    }

    private bool IsWall(Collider2D other)
    {
        int layerBit = 1 << other.gameObject.layer;

        return (wallMask.value & layerBit) != 0;
    }

    private void OnDisable()
    {
        isArmed = false;
        isBreaking = false;
    }
}
