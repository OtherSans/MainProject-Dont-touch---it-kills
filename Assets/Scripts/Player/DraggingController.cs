using System;
using UnityEngine;

public class DraggingController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;


    [Tooltip("Множитель скорости перемещения. 1 = персонаж двигается точно так же, как курсор")]
    [SerializeField, Range(0, 1)] private float dragSensitivity = 1f;

    private Camera mainCamera;
    private Vector3 lastMouseWorldPos;

    private bool isDragging;

    private void Start()
    {
        mainCamera = Camera.main;

        
    }
    private void OnEnable()
    {
        if(playerController != null)
        {
            
            playerController.OnDragEvent += DragCheck;
            
        }
    }
    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnDragEvent -= DragCheck;
        }
    }
    private void Update()
    {
        DragControl();
    }
    private void DragCheck(bool dragCheck)
    {
        isDragging = dragCheck;
        if(isDragging)
            lastMouseWorldPos = GetMouseWorldPosition();
    }
    private void DragControl()
    {
        
        if (isDragging)
        {
            
            Vector3 currentMouseWorldPos = GetMouseWorldPosition();
            Vector3 delta = currentMouseWorldPos - lastMouseWorldPos;

            transform.position += delta * dragSensitivity;

            lastMouseWorldPos = currentMouseWorldPos;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = transform.position.z; // сохраняем исходную глубину персонажа
        return worldPos;
    }
}
