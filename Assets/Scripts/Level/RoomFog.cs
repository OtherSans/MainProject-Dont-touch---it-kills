using UnityEngine;

public class RoomFog : MonoBehaviour
{
    [SerializeField]
    private GameObject fogObject;

    private bool revealed;

    public void Reveal()
    {
        if (revealed)
            return;

        revealed = true;

        if (fogObject != null)
            fogObject.SetActive(false);
    }
}
