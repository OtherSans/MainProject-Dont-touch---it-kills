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

        DontDestroyOnLoad(gameObject);

        experience =
            GetComponentInChildren<XPManager>();

        Debug.Log(
            $"GameManager Awake | {gameObject.name}"
        );
    }
}
