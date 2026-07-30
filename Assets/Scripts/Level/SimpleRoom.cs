using UnityEngine;

public class SimpleRoom : RoomController
{
    protected override void OnRoomEntered()
    {
        Debug.Log($"Entered room {RoomID}");
    }

    protected override void OnRoomExited()
    {
    }

    protected override void OnRoomCompleted()
    {
    }
}
