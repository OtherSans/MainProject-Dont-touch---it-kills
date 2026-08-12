using System.Collections.Generic;
using UnityEngine;

public class RandomRoomSlot : MonoBehaviour
{
    [Header("Room Pool")]
    [SerializeField]
    private GameObject[] roomPrefabs;

    [Header("Allowed Sizes")]
    [SerializeField]
    private RoomSize[] allowedSizes;

    [Header("Settings")]
    [SerializeField]
    private bool generateOnStart = true;

    private GameObject spawnedRoom;

    public void GenerateRoom()
    {
        if (spawnedRoom != null)
            return;

        if (roomPrefabs == null ||
            roomPrefabs.Length == 0)
        {
            Debug.LogWarning(
                $"{name}: Room Prefabs пуст.",
                this
            );

            return;
        }

        List<GameObject> validRooms =
            new List<GameObject>();

        foreach (GameObject prefab in roomPrefabs)
        {
            if (prefab == null)
                continue;

            RoomDefinition definition =
                prefab.GetComponent<RoomDefinition>();

            if (definition == null)
            {
                Debug.LogWarning(
                    $"{prefab.name}: нет RoomDefinition.",
                    prefab
                );

                continue;
            }

            if (!IsSizeAllowed(definition.Size))
                continue;

            validRooms.Add(prefab);
        }

        if (validRooms.Count == 0)
        {
            Debug.LogWarning(
                $"{name}: нет подходящих комнат.",
                this
            );

            return;
        }

        GameObject selectedRoom =
            validRooms[
                Random.Range(
                    0,
                    validRooms.Count
                )
            ];

        spawnedRoom =
            Instantiate(
                selectedRoom,
                transform.position,
                transform.rotation,
                transform
            );

        spawnedRoom.transform.localPosition =
            Vector3.zero;

        spawnedRoom.transform.localRotation =
            Quaternion.identity;
    }

    private bool IsSizeAllowed(
        RoomSize roomSize)
    {
        if (allowedSizes == null ||
            allowedSizes.Length == 0)
        {
            return false;
        }

        foreach (RoomSize allowedSize in allowedSizes)
        {
            if (allowedSize == roomSize)
                return true;
        }

        return false;
    }
}
