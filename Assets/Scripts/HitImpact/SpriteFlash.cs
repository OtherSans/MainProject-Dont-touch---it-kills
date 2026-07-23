using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;

    private Color defaultColor;

    private void Awake()
    {
        defaultColor = sprite.color;
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sprite.color = Color.white;

        yield return new WaitForSeconds(0.06f);

        sprite.color = defaultColor;
    }
}
