using System;
using UnityEngine;
using UnityEngine.UI;

public class FsmFinishController : MonoBehaviour
{
    public event Action Captured;

    [Header("Capture settings")]
    [SerializeField, Min(0f)]
    private float playerCaptureSpeed = 2f;

    [SerializeField, Min(0f)]
    private float weaponCaptureSpeed = 0.5f;

    [SerializeField, Min(0.1f)]
    private float captureRequired = 5f;

    [SerializeField, Min(0f)]
    private float decaySpeed = 1f;

    [Header("References")]
    [SerializeField]
    private RoomBarrier exitBlock;

    [SerializeField]
    private CaptureController captureController;

    [SerializeField]
    private Image captureBar;

    public bool IsCaptured { get; private set; }
    public Fsm Fsm { get; private set; }

    public float PlayerCaptureSpeed => playerCaptureSpeed;
    public float WeaponCaptureSpeed => weaponCaptureSpeed;
    public float CaptureRequired => captureRequired;
    public float DecaySpeed => decaySpeed;

    public float CurrentCaptureProgress { get; set; }

    public bool PlayerInside { get; private set; }
    public bool WeaponPlaced { get; private set; }

    public Image CaptureBar => captureBar;
    public RoomBarrier ExitBlock => exitBlock;
    public CaptureController CaptureController => captureController;

    /// <summary>
    /// Игрок может захватывать точку только после убийства всех врагов.
    /// </summary>
    public bool CanPlayerCapture =>
        PlayerInside &&
        captureController != null &&
        captureController.AreAllEnemiesDefeated;

    /// <summary>
    /// Меч может захватывать точку независимо от оставшихся врагов.
    /// </summary>
    public bool CanWeaponCapture => WeaponPlaced;

    public bool CanCapture =>
        CanPlayerCapture ||
        CanWeaponCapture;

    private void Awake()
    {
        Fsm = new Fsm();

        Fsm.AddState(new FsmFinishIdleState(Fsm, this));
        Fsm.AddState(new FsmFinishCapturingState(Fsm, this));
        Fsm.AddState(new FsmFinishCapturedState(Fsm, this));

        Fsm.SetState<FsmFinishIdleState>();

        UpdateCaptureBar();
    }

    private void Update()
    {
        Fsm.Update();
    }

    public void SetWeaponPlaced(bool isPlaced)
    {
        WeaponPlaced = isPlaced;
    }

    public void UpdateCaptureBar()
    {
        if (captureBar == null)
            return;

        captureBar.fillAmount =
            CurrentCaptureProgress / captureRequired;
    }

    public void CompleteCapture()
    {
        if (IsCaptured)
            return;

        IsCaptured = true;
        Captured?.Invoke();
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
