using Cinemachine;
using System.ComponentModel;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineConfiner2D confiner;



    public void SetRoomBounds(Collider2D bounds)
    {
        if (bounds == null)
            return;

        confiner.m_BoundingShape2D = bounds;
        confiner.InvalidateCache();
    }
}
