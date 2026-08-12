using UnityEngine;

public class RandomRoomGenerator : MonoBehaviour
{
    [Header("Room Variants")]
    [SerializeField]
    private GameObject[] roomPrefabs;

    [Header("Spawn")]
    [SerializeField]
    private Transform roomSpawnPoint;

    private bool hasGenerated;

    public void GenerateRoom()
    {
        if (hasGenerated)
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

        if (roomSpawnPoint == null)
        {
            Debug.LogWarning(
                $"{name}: Room Spawn Point не назначен.",
                this
            );

            return;
        }

        int randomIndex =
            Random.Range(0, roomPrefabs.Length);

        GameObject selectedRoom =
            roomPrefabs[randomIndex];

        Instantiate(
            selectedRoom,
            roomSpawnPoint.position,
            roomSpawnPoint.rotation
        );

        hasGenerated = true;
    }
}
