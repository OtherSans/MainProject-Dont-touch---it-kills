using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnEnable()
    {
        if(playerController != null)
        {
            playerController.OnAttackEvent += AttackPerform;
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnAttackEvent -= AttackPerform;
        }
    }
    private void AttackPerform()
    {
        Debug.Log("attack");
    }

}
