using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float flashDur = 0.06f;

    private Color baseColor;
    private Coroutine flashCoroutine;
    private void Awake()
    {
        baseColor = sprite.color;
    }
    public void SetBaseColor(Color color)
    {
        baseColor = color;
        if (flashCoroutine == null)
            sprite.color = baseColor;

    }
    public void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sprite.color = Color.white;

        yield return new WaitForSeconds(flashDur);

        sprite.color = baseColor;
        flashCoroutine = null;
    }
}
