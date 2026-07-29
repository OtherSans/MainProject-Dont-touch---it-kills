using UnityEngine;

public class WeaponSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform slotPoint;
    public FsmFinishController finishPoint;

    public SwordController CurrentSword { get; private set; }

    public bool HasSword => CurrentSword != null;

    public bool PlaceSword(SwordController sword)
    {
        if (HasSword)
            return false;

        CurrentSword = sword;

        sword.transform.SetParent(slotPoint);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        sword.SetPlaced(true);

        finishPoint.WeaponPlaced = true;

        return true;
    }

    public SwordController TakeSword(Transform parent)
    {
        if (!HasSword)
            return null;

        SwordController sword = CurrentSword;

        sword.transform.SetParent(parent);

        sword.SetPlaced(false);

        CurrentSword = null;

        finishPoint.WeaponPlaced = false;

        return sword;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player =
        other.GetComponent<PlayerController>();

        if (player == null)
            return;

        player.SetInteractable(this);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player =
        other.GetComponent<PlayerController>();

        if (player == null)
            return;

        player.SetInteractable(this);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player =
        other.GetComponent<PlayerController>();

        if (player == null)
            return;

        player.ClearInteractable(this);
    }

    public void Interact(PlayerController player)
    {
        if (HasSword)
            TakeSword(player.transform);
        else
            PlaceSword(player.swordController);
    }
}
