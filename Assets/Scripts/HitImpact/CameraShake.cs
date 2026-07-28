using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    public void Shake(float strength)
    {
        if (impulseSource == null)
            return;
        impulseSource.GenerateImpulse(strength);
    }
}
