using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    public void DeathPlayerPerformed()
    {
        Destroy(gameObject);
        Debug.Log("YOU FAILED");
    }
}
