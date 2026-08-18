using System;
using UnityEngine;

public class PlayerLevelKeyController : MonoBehaviour
{
    public bool HasKey { get; private set; }

    public event Action KeyCollected;
    public event Action KeyUsed;

    public void GiveKey()
    {
        if (HasKey)
            return;

        HasKey = true;

        KeyCollected?.Invoke();
    }

    public bool TryUseKey()
    {
        if (!HasKey)
            return false;

        HasKey = false;

        KeyUsed?.Invoke();

        Debug.Log("Ключ использован.");

        return true;
    }
}
