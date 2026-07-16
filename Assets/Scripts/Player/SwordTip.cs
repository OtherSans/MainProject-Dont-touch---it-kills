using UnityEngine;

public class SwordTip : MonoBehaviour
{
    [SerializeField] private SwordController sword;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!sword.CanSkewer())
            return;

        if (!other.TryGetComponent(out FsmStartingEnemyState enemy))
            return;

        sword.TrySkewer(enemy);
    }
}
