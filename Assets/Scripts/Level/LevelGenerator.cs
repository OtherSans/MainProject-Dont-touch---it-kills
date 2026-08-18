using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private RandomRoomSlot[] roomSlots;

    [SerializeField]
    private NavMeshSurface navMeshSurface;

    [Header("Shops")]
    [SerializeField] private GameObject smallShopPrefab;
    [SerializeField] private GameObject mediumShopPrefab;
    [SerializeField, Range(0f, 1f)] private float mediumShopChance = 0.2f;

    [Header("Key Room")]
    [SerializeField]
    private GameObject[] keyRoomPrefabs;

    [Header("Level Exit")]
    [SerializeField]
    private GameObject levelExitPrefab;

    [SerializeField]
    private Transform[] exitSpawnPoints;

    private GameObject spawnedExit;


    private void Start()
    {
        GenerateLevel();
    }
    private void GenerateLevel()
    {
        // 1. Магазин
        RandomRoomSlot shopSlot =
            SelectRequiredShopSlot();

        if (shopSlot != null)
            SpawnShop(shopSlot);

        // 2. Комната с ключом
        SpawnKeyRoom();

        // 3. Остальные комнаты
        foreach (RandomRoomSlot slot in roomSlots)
        {
            if (slot == null)
                continue;

            if (slot.HasRoom)
                continue;

            slot.GenerateRoom();
        }

        // 4. Выход
        SpawnLevelExit();

        // 5. NavMesh
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();

        ValidateLevel();
    }
    private void SpawnLevelExit()
    {
        if (levelExitPrefab == null)
        {
            Debug.LogError(
                "Level Exit Prefab не назначен.",
                this
            );

            return;
        }

        if (exitSpawnPoints == null ||
            exitSpawnPoints.Length == 0)
        {
            Debug.LogError(
                "Нет Exit Spawn Points.",
                this
            );

            return;
        }

        Transform selectedPoint =
            exitSpawnPoints[
                Random.Range(
                    0,
                    exitSpawnPoints.Length
                )
            ];

        if (selectedPoint == null)
            return;

        spawnedExit =
            Instantiate(
                levelExitPrefab,
                selectedPoint.position,
                selectedPoint.rotation
            );

        Debug.Log(
            $"EXIT SPAWNED AT: {selectedPoint.name}"
        );
    }
    private RandomRoomSlot SelectRequiredShopSlot()
    {
        System.Collections.Generic.List<RandomRoomSlot>
            possibleSlots = new();

        foreach (RandomRoomSlot slot in roomSlots)
        {
            if (slot == null)
                continue;

            if (!slot.CanBeShop)
                continue;

            possibleSlots.Add(slot);
        }

        if (possibleSlots.Count == 0)
            return null;

        return possibleSlots[
            Random.Range(
                0,
                possibleSlots.Count
            )
        ];
    }
    private void SpawnShop(
    RandomRoomSlot shopSlot)
    {
        bool allowsSmall =
            shopSlot.AllowsSize(RoomSize.Small);

        bool allowsMedium =
            shopSlot.AllowsSize(RoomSize.Medium);

        GameObject selectedShop = null;

        if (allowsSmall && allowsMedium)
        {
            bool spawnMedium =
                Random.value < mediumShopChance;

            selectedShop =
                spawnMedium
                    ? mediumShopPrefab
                    : smallShopPrefab;
        }
        else if (allowsMedium)
        {
            selectedShop =
                mediumShopPrefab;
        }
        else if (allowsSmall)
        {
            selectedShop =
                smallShopPrefab;
        }

        if (selectedShop == null)
        {
            Debug.LogError(
                $"{shopSlot.name}: слот отмечен CanBeShop, " +
                "но не разрешает ни Small, ни Medium.",
                shopSlot
            );

            return;
        }

        shopSlot.SpawnSpecificRoom(
            selectedShop
        );

        Debug.Log(
            $"SHOP SPAWNED: {selectedShop.name} " +
            $"в {shopSlot.name}"
        );
    }

    private RandomRoomSlot SelectKeyRoomSlot()
    {
        System.Collections.Generic.List<RandomRoomSlot>
            possibleSlots = new();

        foreach (RandomRoomSlot slot in roomSlots)
        {
            if (slot == null)
                continue;

            if (slot.HasRoom)
                continue;

            if (!slot.CanBeKeyRoom)
                continue;

            possibleSlots.Add(slot);
        }

        if (possibleSlots.Count == 0)
        {
            Debug.LogError(
                "НЕТ СВОБОДНОГО СЛОТА ДЛЯ KEY ROOM!"
            );

            return null;
        }

        return possibleSlots[
            Random.Range(
                0,
                possibleSlots.Count
            )
        ];
    }

    private void SpawnKeyRoom()
    {
        if (keyRoomPrefabs == null ||
            keyRoomPrefabs.Length == 0)
        {
            Debug.LogError(
                "Key Room Prefabs не назначены!"
            );

            return;
        }

        RandomRoomSlot slot =
            SelectKeyRoomSlot();

        if (slot == null)
            return;

        GameObject prefab =
            keyRoomPrefabs[
                Random.Range(
                    0,
                    keyRoomPrefabs.Length
                )
            ];

        slot.SpawnSpecificRoom(prefab);

        Debug.Log(
            $"KEY ROOM SPAWNED: " +
            $"{prefab.name} в {slot.name}"
        );
    }
    private void ValidateLevel()
    {
        LevelKey[] keys =
        FindObjectsByType<LevelKey>(
            FindObjectsInactive.Include
        );

        if (keys.Length == 0)
        {
            Debug.LogError(
                "КРИТИЧЕСКАЯ ОШИБКА: уровень создан без ключа!"
            );

            return;
        }

        if (keys.Length > 1)
        {
            Debug.LogError(
                $"КРИТИЧЕСКАЯ ОШИБКА: " +
                $"на уровне создано ключей: {keys.Length}"
            );

            return;
        }

        Debug.Log(
            "LEVEL VALIDATION: ключ создан корректно."
        );
    }
}
