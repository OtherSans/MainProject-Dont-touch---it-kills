using UnityEngine;

public class RoomBarrier : MonoBehaviour
{
    [SerializeField] private BoxCollider2D barrierCollider;
    [SerializeField] private GameObject barrierVisual;

    private bool isOpen;
    public bool IsOpen => isOpen;

    public void Open()
    {
        if (isOpen)
            return;
        isOpen = true;
        if (barrierCollider != null)
            barrierCollider.enabled = false;

        if (barrierVisual != null)
            barrierVisual.SetActive(false);
    }
    public void Close()
    {
        if (!isOpen)
            return;
        isOpen = false;

        if (barrierCollider != null)
            barrierCollider.enabled = true;

        if (barrierVisual != null)
            barrierVisual.SetActive(true);
    }
}
