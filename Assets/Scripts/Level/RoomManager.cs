using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private RoomController startRoom;
    //[SerializeField] private RoomCameraController cameraController;
    public RoomController CurrentRoom { get; private set; }
    private void Start()
    {
        CurrentRoom = startRoom;
        CurrentRoom.EnterRoom();

        //if (cameraController != null)
        //    cameraController.SetRoomBounds(startRoom.CameraBounds);
    }
    public void SetStartRoom(RoomController newRoom)
    {
        CurrentRoom = newRoom;

        //cameraController.SetRoomBounds(room.CameraBounds);

        CurrentRoom.EnterRoom();
    }
    public void EnterRoom(RoomController newRoom)
    {
        CurrentRoom?.ExitRoom();

        CurrentRoom = newRoom;

        player.transform.position = newRoom.EntryPoint.position;

        //cameraController.SetRoomBounds(room.CameraBounds);

        CurrentRoom.EnterRoom();
    }
}
