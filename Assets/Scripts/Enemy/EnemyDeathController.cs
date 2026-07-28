using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    [SerializeField] private float deathTime;
    [SerializeField] private GameObject currencyPrefab;
    private PetrifiedController petrifiedContr;


    private void Start()
    {
        petrifiedContr = GetComponent<PetrifiedController>();
    }
    public void DiePerform()
    {
        Destroy(gameObject, deathTime);
        if(!petrifiedContr.IsPetrified)
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
    }
}
