using UnityEngine;
using UnityEngine.InputSystem;

public class DraggingController : MonoBehaviour
{
    [Tooltip("Множитель скорости перемещения. 1 = персонаж двигается точно так же, как курсор")]
    [SerializeField, Range(0,1)] private float dragSensitivity = 1f;

    private PlayerInput playerInput;
    
    private Camera mainCamera;
    private Vector3 lastMouseWorldPos;
    private bool isDragging;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Drag.started += OnDrugStarted;
        playerInput.Player.Drag.canceled += OnDrugCancelled;
    }
    private void OnDisable()
    {
        playerInput.Player.Drag.started -= OnDrugStarted;
        playerInput.Player.Drag.canceled -= OnDrugCancelled;

        playerInput.Disable();       
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        DraggingControl();
    }

    private void OnDrugStarted(InputAction.CallbackContext ctx)
    {
        // ПКМ нажата — начинаем перетаскивание
        isDragging = true;
        lastMouseWorldPos = GetMouseWorldPosition();
    }
    private void OnDrugCancelled(InputAction.CallbackContext ctx)
    {
        // ПКМ отпущена — заканчиваем перетаскивание
        isDragging = false;
    }

    private void DraggingControl()
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
        worldPos.z = transform.position.z; // сохраняем исходную глубину персонажа (для 2D обычно 0)
        return worldPos;
    }
}
