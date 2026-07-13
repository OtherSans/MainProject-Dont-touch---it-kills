using System;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [NonSerialized] public bool isMoving;

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.OnMoveEvent += MovingPerform;
        }
    }
    private void OnDisable()
    {
        if(playerController != null)
        {
            playerController.OnMoveEvent -= MovingPerform;
        } 
    }
    private void Update()
    {
        MoveCheck();
    }
    private void MovingPerform(bool moveCheck)
    {
        isMoving = moveCheck;
    }
    private void MoveCheck()
    {
        if (isMoving)
            playerController.NavMoving();
        else
            playerController.NavStop();
    }
}
