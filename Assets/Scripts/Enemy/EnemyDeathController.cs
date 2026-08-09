using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    [SerializeField] private float deathTime;
    [SerializeField] private GameObject currencyPrefab;
    [SerializeField] private CaptureController captureController;
    [Header("XP")]
    [SerializeField] private XPManager experienceManager;
    [SerializeField] private int experienceReward = 10;

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

        if(captureController != null)
            captureController.NotifyEnemyDied(GetComponent<EnemyController>());

        experienceManager.AddExperience(experienceReward);

        Destroy(gameObject, deathTime);
        if(!petrifiedContr.IsPetrified)
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
    }
}
