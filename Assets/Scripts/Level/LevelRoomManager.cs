using System.Collections.Generic;
using UnityEngine;

public class LevelRoomManager : MonoBehaviour
{
    private readonly List<RoomController> rooms =
        new List<RoomController>();

    private bool keyPhaseActivated;

    public void RegisterRoom(
        RoomController room)
    {
        if (room == null)
            return;

        if (rooms.Contains(room))
            return;

        rooms.Add(room);
    }

    public void UnregisterRoom(
        RoomController room)
    {
        if (room == null)
            return;

        rooms.Remove(room);
    }

    public void ActivateKeyPhase()
    {
        if (keyPhaseActivated)
            return;

        keyPhaseActivated = true;

        foreach (RoomController room in rooms)
        {
            if (room == null)
                continue;

            room.ForceOpenRoom();
        }

        Debug.Log(
            "KEY PHASE ACTIVATED"
        );
    }
}
