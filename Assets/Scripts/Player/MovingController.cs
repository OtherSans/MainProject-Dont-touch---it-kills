using UnityEngine;

public class MovingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerContr;

    private bool isMoving;

    private void OnEnable()
    {
        if (playerContr != null)
            playerContr.OnMoveEvent += MovePerform;
    }
    private void OnDisable()
    {
        if (playerContr != null)
            playerContr.OnMoveEvent -= MovePerform;
    }
    private void Update()
    {
        //MovingChange();
    }
    private void MovePerform(bool moveCheck)
    {
        isMoving = moveCheck;
    }
    private void MovingChange()
    {
        if (isMoving)
        {
            playerContr.NavMove();
        }
        else if (!isMoving)
        {
            playerContr.NavStop();
        }
    }

}
