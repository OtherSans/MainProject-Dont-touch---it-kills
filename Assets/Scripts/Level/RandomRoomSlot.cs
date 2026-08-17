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

    [SerializeField] private bool canBeShop;

    [SerializeField]
    private bool canBeKeyRoom;

    [SerializeField]
    private LayerMask roomGenerationMask;

    public bool CanBeKeyRoom =>
        canBeKeyRoom;

    public bool CanBeShop => canBeShop;

    private GameObject spawnedRoom;

    public bool HasRoom =>
    spawnedRoom != null;


    public void GenerateRoom()
    {
        if (spawnedRoom != null)
            return;

        List<GameObject> validRooms =
            new List<GameObject>();

        foreach (GameObject prefab in roomPrefabs)
        {
            if (prefab == null)
                continue;

            RoomDefinition definition =
                prefab.GetComponent<RoomDefinition>();

            if (definition == null)
                continue;

            if (!IsSizeAllowed(definition.Size))
                continue;

            validRooms.Add(prefab);
        }

        while (validRooms.Count > 0)
        {
            int index =
                Random.Range(
                    0,
                    validRooms.Count
                );

            GameObject candidate =
                validRooms[index];

            if (CanSpawnRoom(candidate))
            {
                spawnedRoom = Instantiate(
                    candidate,
                    transform.position,
                    transform.rotation,
                    transform
                );

                spawnedRoom.transform.localPosition =
                    Vector3.zero;

                spawnedRoom.transform.localRotation =
                    Quaternion.identity;

                return;
            }

            // Этот prefab сюда не помещается.
            validRooms.RemoveAt(index);
        }

        Debug.LogWarning(
            $"{name}: ни одна комната не помещается.",
            this
        );
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
    public void SpawnSpecificRoom(GameObject roomPrefab)
    {
        if (spawnedRoom != null)
            return;
        if (roomPrefab == null)
            return;

        spawnedRoom = Instantiate(roomPrefab, transform.position, transform.rotation, transform);

        spawnedRoom.transform.localPosition = Vector3.zero;

        spawnedRoom.transform.localRotation = Quaternion.identity;
    }
    public bool AllowsSize(RoomSize size)
    {
        if (allowedSizes == null)
            return false;

        foreach (RoomSize allowed in allowedSizes)
        {
            if (allowed == size)
                return true;
        }

        return false;
    }

    private bool CanSpawnRoom(
    GameObject roomPrefab)
    {
        RoomDefinition definition =
            roomPrefab.GetComponent<RoomDefinition>();

        if (definition == null ||
            definition.GenerationBounds == null)
        {
            return false;
        }

        BoxCollider2D bounds =
            definition.GenerationBounds;

        Vector2 worldCenter =
            (Vector2)transform.position +
            bounds.offset;

        Vector2 worldSize =
            Vector2.Scale(
                bounds.size,
                roomPrefab.transform.localScale
            );

        Collider2D hit =
            Physics2D.OverlapBox(
                worldCenter,
                worldSize,
                transform.eulerAngles.z,
                roomGenerationMask
            );

        return hit == null;
    }
}
