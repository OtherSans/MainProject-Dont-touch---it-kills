using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    [SerializeField] private float deathTime;
    public void DiePerform()
    {
        Destroy(gameObject, deathTime);
    }
}
