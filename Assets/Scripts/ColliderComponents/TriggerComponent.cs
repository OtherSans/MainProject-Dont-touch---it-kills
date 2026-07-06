using UnityEngine;
using UnityEngine.Events;

public class TriggerComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEnter;
    [SerializeField] private UnityEvent triggerStay;
    [SerializeField] private UnityEvent triggerExit;

    [SerializeField] private string objectTag;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(objectTag))
        {
            triggerEnter?.Invoke();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(objectTag))
        {
            triggerStay?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(objectTag))
        {
            triggerExit?.Invoke();
        }
    }
}
