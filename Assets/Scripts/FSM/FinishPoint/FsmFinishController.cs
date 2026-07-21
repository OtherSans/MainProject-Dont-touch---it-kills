using UnityEngine;
using UnityEngine.UI;

public class FsmFinishController : MonoBehaviour
{
    public Fsm Fsm { get; private set; }
    public float CaptureTime = 5f;
    public float CurrentCaptureTime;

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
