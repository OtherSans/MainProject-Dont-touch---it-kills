using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class RoomEntranceTrigger : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private RoomController targetRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if (player == null) return;

        roomManager.EnterRoom(targetRoom);
    }
}
