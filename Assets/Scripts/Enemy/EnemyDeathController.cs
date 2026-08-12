using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    [SerializeField] private float deathTime;
    [SerializeField] private GameObject currencyPrefab;
    [Header("XP")]
    [SerializeField] private XPManager experienceManager;
    [SerializeField] private int experienceReward = 10;

    private bool deathReported;
    private PetrifiedController petrifiedContr;

    private void Awake()
    {
        experienceManager = FindAnyObjectByType<XPManager>();

    }
    private void Start()
    {
        petrifiedContr = GetComponent<PetrifiedController>();
    }
    public void DiePerform()
    {
        if (deathReported)
            return;

        deathReported = true;

        experienceManager.AddExperience(experienceReward);

        Destroy(gameObject, deathTime);
        if(!petrifiedContr.IsPetrified)
            Instantiate(currencyPrefab, transform.position, Quaternion.identity);
    }
}
