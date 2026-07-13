using System;
using UnityEngine;
using UnityEngine.Events;

public class CollisionComponent : MonoBehaviour
{
    [SerializeField] private EnterEvent actionEnter;
    [SerializeField] private EnterEvent actionStay;
    [SerializeField] private EnterEvent actionExit;

    public string actionTag;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(actionTag))
        {
            actionEnter?.Invoke(collision.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(actionTag))
        {
            actionStay?.Invoke(collision.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(actionTag))
        {
            actionExit?.Invoke(collision.gameObject);
        }
    }
    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    { }
}
