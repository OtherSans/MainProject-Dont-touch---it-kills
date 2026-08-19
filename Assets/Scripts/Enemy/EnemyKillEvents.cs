using System;
using UnityEngine;

public static class EnemyKillEvents
{
    public static event Action EnemyKilled;

    public static void ReportKill()
    {
        EnemyKilled?.Invoke();
    }
}
