using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 startPos;

    private void Awake()
    {
        Instance = this;
        startPos = transform.localPosition;
    }

    public void Shake(float duration, float strength)
    {
        StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float timer = 0;

        while (timer < duration)
        {
            transform.localPosition =
                startPos +
                (Vector3)Random.insideUnitCircle * strength;

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localPosition = startPos;
    }
}
