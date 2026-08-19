using UnityEngine;
using UnityEngine.InputSystem;

public class GameQuitController : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Debug.Log("QUIT GAME");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
