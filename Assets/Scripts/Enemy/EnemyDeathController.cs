using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    [SerializeField] private float deathTime;
    [SerializeField] private GameObject currencyPrefab;
    [SerializeField] private CaptureController captureController;

    private bool deathReported;
    private PetrifiedController petrifiedContr;


    private void Start()
    {
        petrifiedContr = GetComponent<PetrifiedController>();
    }
    public void DiePerform()
    {
        if (deathReported)
            return;

        deathReported = true;

        captureController.NotifyEnemyDied(GetComponent<EnemyController>());

        Destroy(gameObject, deathTime);
        if(!petrifiedContr.IsPetrified)
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
    }
}
