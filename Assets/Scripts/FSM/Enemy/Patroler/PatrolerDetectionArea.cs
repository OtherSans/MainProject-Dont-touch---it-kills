using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolerDetectionArea : MonoBehaviour
{
    [SerializeField]
    private PatrolerController patroler;

    [SerializeField]
    private Transform viewDirection;

    [SerializeField, Range(0f, 360f)]
    private float viewAngle = 90f;

    [SerializeField]
    private LayerMask obstacleMask;

    [SerializeField]
    private bool requireLineOfSight = true;

    private bool playerDetected;

    private void Awake()
    {
        if (patroler == null)
        {
            patroler =
                GetComponentInParent<PatrolerController>();
        }

        Collider2D detectionCollider =
            GetComponent<Collider2D>();

        detectionCollider.isTrigger = true;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Detection trigger sees: {other.name}");

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        Debug.Log("Player found in detection area");

        if (!IsInsideViewAngle(player.transform))
            return;

        if (requireLineOfSight &&
            !HasLineOfSight(player.transform))
        {
            return;
        }

        playerDetected = true;
        patroler.DetectPlayer();
    }

    private bool IsInsideViewAngle(
        Transform player)
    {
        Vector2 toPlayer =
            player.position -
            patroler.transform.position;

        if (toPlayer.sqrMagnitude <= Mathf.Epsilon)
            return true;

        Vector2 forward =
     viewDirection != null
         ? viewDirection.right
         : patroler.transform.right;

        float angle = Vector2.Angle(
            forward,
            toPlayer.normalized
        );

        return angle <= viewAngle * 0.5f;
    }

    private bool HasLineOfSight(
        Transform player)
    {
        Vector2 origin =
            patroler.transform.position;

        Vector2 target =
            player.position;

        Vector2 direction =
            target - origin;

        float distance =
            direction.magnitude;

        if (distance <= Mathf.Epsilon)
            return true;

        RaycastHit2D hit =
            Physics2D.Raycast(
                origin,
                direction.normalized,
                distance,
                obstacleMask
            );

        return hit.collider == null;
    }

    private void OnDrawGizmosSelected()
    {
        if (patroler == null)
            return;

        Vector2 forward =
            patroler.ViewDirection != null
                ? patroler.ViewDirection.right
                : patroler.transform.right;

        Gizmos.DrawLine(
            patroler.transform.position,
            (Vector2)patroler.transform.position +
            forward * 3f
        );
    }
}
