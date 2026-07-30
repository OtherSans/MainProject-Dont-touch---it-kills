using UnityEngine;

public class CaptureController : MonoBehaviour
{
    [SerializeField]
    private EnemyController[] enemies;

    private int aliveEnemies;
    private bool isCaptured;

    public bool IsCaptured => isCaptured;
    public bool AreAllEnemiesDefeated => aliveEnemies <= 0;
    public EnemyController[] Enemies => enemies;

    private void Awake()
    {
        aliveEnemies = 0;

        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
                aliveEnemies++;
        }
    }

    public void NotifyEnemyDied(EnemyController enemy)
    {
        if (isCaptured || enemy == null)
            return;

        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);

        Debug.Log(
            $"В комнате осталось врагов: {aliveEnemies}"
        );
    }

    public void CapturePerform()
    {
        if (isCaptured)
            return;

        isCaptured = true;
    }
}
