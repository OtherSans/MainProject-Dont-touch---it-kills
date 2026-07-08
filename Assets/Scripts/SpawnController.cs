using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] public GameObject spawnObject;
    [SerializeField] private Vector3 spawnAreaSize;
    [SerializeField] private float spawnTimer;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void SpawnObject()
    {
        Vector3 randomSpawnPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

        Instantiate(spawnObject, randomSpawnPos, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnTimer);
            
        }
        
    }
}
