using UnityEngine;
using UnityEngine.UI;

public class FsmFinishController : MonoBehaviour
{
    public Fsm Fsm { get; private set; }
    [SerializeField] private float playerCaptureSpeed = 1f;
    [SerializeField] private float weaponCaptureSpeed = 0.4f;
    public GameObject BlockWall;
    public CaptureController captureContr;

    public float PlayerCaptureSpeed => playerCaptureSpeed;
    public float WeaponCaptureSpeed => weaponCaptureSpeed;
    public float CaptureRequired = 5f;
    public float CurrentCaptureProgress;

    public float CaptureSpeed = 1f;
    public float DecaySpeed = 1f;

    public bool PlayerInside;
    public bool WeaponPlaced;

    public Image CaptureBar;
    private void Awake()
    {
        Fsm = new Fsm();

        Fsm.AddState(new FsmFinishIdleState(Fsm,this));
        Fsm.AddState(new FsmFinishCapturingState(Fsm, this));
        Fsm.AddState(new FsmFinishCapturedState(Fsm, this));

        Fsm.SetState<FsmFinishIdleState>();
    }
    private void Update()
    {
        Fsm.Update();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerInside = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerInside = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerInside = false;
    }
}
