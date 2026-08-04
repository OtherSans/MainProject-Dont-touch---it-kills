using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SwordTipWallBlocker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SwordController swordController;

    [Header("Collision")]
    [Tooltip("Слои стен, которые должны останавливать вытягивание меча")]
    [SerializeField] private LayerMask wallMask;

    private readonly HashSet<Collider2D> touchingWalls = new();

    private void Awake()
    {
        if (swordController == null)
        {
            swordController =
                GetComponentInParent<SwordController>();
        }

        Collider2D tipCollider = GetComponent<Collider2D>();

        if (!tipCollider.isTrigger)
        {
            Debug.LogWarning(
                $"{name}: Collider2D у SwordTip должен быть Trigger.",
                this
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryAddWall(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        /*
         * Нужен на случай, если стена уже пересекалась с кончиком
         * во время включения объекта или комнаты.
         */
        TryAddWall(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsWall(other))
            return;

        touchingWalls.Remove(other);

        if (touchingWalls.Count == 0)
        {
            swordController.SetExtensionBlocked(false);
        }
    }

    private void TryAddWall(Collider2D other)
    {
        if (!IsWall(other))
            return;

        touchingWalls.Add(other);
        swordController.SetExtensionBlocked(true);
    }

    private bool IsWall(Collider2D other)
    {
        int otherLayerMask = 1 << other.gameObject.layer;

        return (wallMask.value & otherLayerMask) != 0;
    }

    private void OnDisable()
    {
        touchingWalls.Clear();

        if (swordController != null)
        {
            swordController.SetExtensionBlocked(false);
        }
    }
}
