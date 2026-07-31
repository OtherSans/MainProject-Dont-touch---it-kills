using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Transform spawnPoint;

    public void SpawnReward()
    {
        Instantiate(rewardPrefab, spawnPoint.position, Quaternion.identity);
    }
}
