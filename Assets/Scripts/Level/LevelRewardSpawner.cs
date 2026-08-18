using UnityEngine;

public class LevelRewardSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject rewardPrefab;

    [SerializeField]
    private Transform[] spawnPoints;

    private bool hasSpawned;

    public void SpawnReward()
    {
        if (hasSpawned)
            return;

        if (rewardPrefab == null)
        {
            Debug.LogError(
                "Reward Prefab не назначен.",
                this
            );

            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "Нет Reward Spawn Points.",
                this
            );

            return;
        }

        Transform point =
            spawnPoints[
                Random.Range(
                    0,
                    spawnPoints.Length
                )
            ];

        if (point == null)
            return;

        hasSpawned = true;

        Instantiate(
            rewardPrefab,
            point.position,
            point.rotation
        );

        Debug.Log(
            $"REWARD SPAWNED: {point.name}"
        );
    }
}
