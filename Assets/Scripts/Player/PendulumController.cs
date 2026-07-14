using Unity.Hierarchy;
using UnityEngine;

public class PendulumController : MonoBehaviour
{
    [Header("Length")]
    [SerializeField] private Transform sword;

    [SerializeField] private float minLength = 1f;
    [SerializeField] private float maxLength = 5f;

    [SerializeField] private float extendSpeed = 4f;
    [SerializeField] private float retractSpeed = 3f;

    [Header("Swing")]

    [SerializeField] private float spring = 40f;
    [SerializeField] private float damping = 0.94f;
    [SerializeField] private float impulseMultiplier = 0.12f;

    private float currentLength = 1f;
    private float targetLength = 1f;

    private float angle;
    private float angularVelocity;

    public void AddImpulse(float impulse)
    {
        angularVelocity += impulse * impulseMultiplier;
    }

    private void Update()
    {
        //UpdateLength();
        UpdateSwing();
    }

    private void UpdateLength()
    {
        if (Input.GetMouseButton(0))
            targetLength = maxLength;
        else
            targetLength = minLength;

        currentLength = Mathf.MoveTowards(
            currentLength,
            targetLength,
            (targetLength > currentLength ? extendSpeed : retractSpeed) * Time.deltaTime);

        sword.localScale = new Vector3(
            sword.localScale.x,
            currentLength,
            sword.localScale.z);
    }

    private void UpdateSwing()
    {
        angularVelocity += -angle * spring * Time.deltaTime;

        angularVelocity *= damping;

        angle += angularVelocity * Time.deltaTime;

        transform.localRotation =
            Quaternion.Euler(0, 0, angle);
    }
}
