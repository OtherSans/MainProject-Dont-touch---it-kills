using System;
using UnityEngine;

public abstract class RoomController : MonoBehaviour
{
    [Header("Room")]
    [SerializeField] private string roomID;

    [Header("Optional content")]
    [SerializeField] private GameObject roomContent;

    [Header("Transition")]
    [SerializeField] private Transform entryPoint;
    [SerializeField] private Collider2D cameraBounds;

    public Transform EntryPoint => entryPoint;
    public Collider2D CameraBounds => cameraBounds;

    private bool isActive;
    private bool isCompleted;

    public string RoomID => roomID;
    public bool IsActive => isActive;
    public bool IsCompleted => isCompleted;

    public event Action<RoomController> Completed;

    public void EnterRoom()
    {
        if (isActive)
            return;
        isActive = true;
        
        if(roomContent != null)
            roomContent.SetActive(true);
        OnRoomEntered();
    }
    public void ExitRoom()
    {
        if (!isActive)
            return;
        isActive = false;
        OnRoomExited();
    }
    protected void CompleteRoom()
    {
        if (isCompleted)
            return;
        isCompleted = true;
        OnRoomCompleted();
        Completed?.Invoke(this);
    }
    protected virtual void OnRoomEntered()
    {

    }
    protected virtual void OnRoomExited()
    {

    }
    protected virtual void OnRoomCompleted()
    {

    }
}
