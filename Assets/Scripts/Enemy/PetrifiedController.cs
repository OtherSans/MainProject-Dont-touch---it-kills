using UnityEngine;

public class PetrifiedController : MonoBehaviour
{
    [SerializeField] private SpriteFlash sprite;
    [SerializeField] private Color stoneColor = Color.gray;
    private Rigidbody2D rb;
    private EnemyController enemyContr;

    private bool isPetrified;
    private bool isDead;
    private Color originalColor;

    public bool IsPetrified => isPetrified;

    private void Awake()
    {
        enemyContr = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Petrify()
    {
        if (isPetrified || isDead)
            return;

        isPetrified = true; 

        sprite.SetBaseColor(stoneColor);

        enemyContr.Fsm.SetState<FsmEnemyStatePetrified>();
    }
}
