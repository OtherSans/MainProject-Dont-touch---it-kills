using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public RunData Run { get; private set; } =
    new RunData();

    [Header("Systems")]
    [SerializeField] private XPManager experience;

    public XPManager Experience => experience;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (experience == null)
            experience = GetComponentInChildren<XPManager>();

        if (experience == null)
        {
            Debug.LogError(
                "ExperienceManager не назначен в GameManager.",
                this
            );
        }

        DontDestroyOnLoad(gameObject);
    }
}
