using UnityEngine;

public class IgnoreParentScale : MonoBehaviour
{
    public Transform parentToIgnore;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = new Vector3(
            originalScale.x / parentToIgnore.localScale.x,
            originalScale.y / parentToIgnore.localScale.y,
            originalScale.z / parentToIgnore.localScale.z
            );
    }
}
