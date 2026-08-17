using System;
using UnityEngine;

public class PlayerLevelKeyController : MonoBehaviour
{
    public bool HasKey { get; private set; }

    public event Action KeyCollected;

    public void GiveKey()
    {
        if (HasKey)
            return;

        HasKey = true;

        KeyCollected?.Invoke();

        Debug.Log("Ключ от выхода получен!");
    }

    public void RemoveKey()
    {
        HasKey = false;
    }
}
