using UnityEngine;

public class CaptureController : MonoBehaviour
{
    private bool isCaptured;
    public EnemyController[] enemies { get; private set; }

    public void CapturePerform()
    {
        if(isCaptured)
          return;

        isCaptured = true;

        enemies = FindObjectsByType<EnemyController>();
    }
}
