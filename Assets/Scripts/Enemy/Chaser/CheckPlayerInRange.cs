using UnityEngine;

public class CheckPlayerInRange : MonoBehaviour
{
    public bool IsChasing { get; private set; }

    [SerializeField] private ChaserController enemy;
    public ChaseController chasingContr { get; private set; }
    private void Awake()
    {
        chasingContr = GetComponent<ChaseController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
            

        IsChasing = true;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;


        IsChasing = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        Debug.Log("EXIT TRIGGER");
        IsChasing = false;
    }
}
