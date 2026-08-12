using NavMeshPlus.Components;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private RandomRoomSlot[] roomSlots;

    [SerializeField]
    private NavMeshSurface navMeshSurface;

    private void Start()
    {
        foreach (RandomRoomSlot slot in roomSlots)
        {
            slot.GenerateRoom();
        }

        navMeshSurface.BuildNavMesh();
    }
}
