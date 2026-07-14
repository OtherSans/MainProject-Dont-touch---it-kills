using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [NonSerialized] public bool isAttacking;
    private void Start()
    {

    }

    private void OnEnable()
    {
        if(playerController != null)
        {
            playerController.OnAttackStartedEvent += AttackStart;
            playerController.OnAttackCanceledEvent += AttackCancel;
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnAttackStartedEvent -= AttackStart;
            playerController.OnAttackCanceledEvent -= AttackCancel;
        }
    }
    private void AttackStart()
    {
        Debug.Log("attackStart");
        isAttacking = true;
    }
    private void AttackCancel()
    {
        Debug.Log("attackCancel");
        isAttacking = false;
    }
}
