using System;
using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Rigidbody2D rb;

    [Header("Camera")]
    [SerializeField] private RoomCameraController cameraController;

    [Header("Transition")]
    [SerializeField, Min(0.01f)] private float autoMoveSpeed = 5f;
    [SerializeField, Min(0.001f)] private float arrivalDistance = 0.05f;

    [Header("Start room")]
    [SerializeField] private RoomController startRoom;
    public RoomController CurrentRoom { get; private set; }
    private bool isTransitioning;
    public bool IsTransitioning => isTransitioning;
    private void Start()
    {
        CurrentRoom = startRoom;
        CurrentRoom.EnterRoom();

        if (cameraController != null)
            cameraController.SetRoomBounds(startRoom.CameraBounds);
    }
    public void SetStartRoom(RoomController newRoom)
    {
        if (newRoom == null)
        {
            Debug.LogError("Start Room не назначена.", this);
            return;
        }

        CurrentRoom = newRoom;
        if(cameraController != null)
        {
            cameraController.SetRoomBounds(newRoom.CameraBounds);
            cameraController.SnapToPlayer();
        }

        CurrentRoom.EnterRoom();
    }
    public void StartTransition(
        RoomController targetRoom,
        Transform exitMovePoint,
        Transform targetOutsidePoint,
        Transform targetEntryPoint,
        Action onFinished = null)
    {
        if (isTransitioning)
            return;

        if (targetRoom == null ||
            exitMovePoint == null ||
            targetOutsidePoint == null ||
            targetEntryPoint == null)
        {
            Debug.LogError("В переход переданы пустые ссылки.", this);
            return;
        }

        StartCoroutine(
            TransitionRoutine(targetRoom,
                exitMovePoint,
                targetOutsidePoint,
                targetEntryPoint,
                onFinished)
        );
    }

    private IEnumerator TransitionRoutine(
        RoomController targetRoom,
        Transform exitMovePoint,
        Transform targetOutsidePoint,
        Transform targetEntryPoint,
        Action onFinished)
    {
        isTransitioning = true;

        //player.SetInputEnabled(false);
        rb.linearVelocity = Vector2.zero;

        // 1. Игрок автоматически выходит из текущей камеры.
        yield return MovePlayerTo(exitMovePoint.position);

        CurrentRoom?.ExitRoom();

        // 2. Переносим игрока за границу следующей комнаты.

        rb.position =
            targetOutsidePoint.position;

        rb.linearVelocity = Vector2.zero;

        Physics2D.SyncTransforms();

        CurrentRoom = targetRoom;

        // 3. Переключаем ограничители камеры.
        if (cameraController != null)
        {
            cameraController.SetRoomBounds(targetRoom.CameraBounds);
            cameraController.SnapToPlayer();
        }

        

        CurrentRoom.EnterRoom();

        // Один кадр нужен, чтобы Cinemachine обновила позицию.
        yield return null;

        // 4. Игрок автоматически входит в новую комнату.
        yield return MovePlayerTo(targetEntryPoint.position);

        rb.linearVelocity = Vector2.zero;
        //player.SetInputEnabled(true);

        isTransitioning = false;
        onFinished?.Invoke();
    }

    private IEnumerator MovePlayerTo(Vector2 targetPosition)
    {
        while (Vector2.Distance(
                   rb.position,
                   targetPosition) > arrivalDistance)
        {
            Vector2 nextPosition = Vector2.MoveTowards(
                rb.position,
                targetPosition,
                autoMoveSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }

        rb.position = targetPosition;
        rb.linearVelocity = Vector2.zero;
    }
}
