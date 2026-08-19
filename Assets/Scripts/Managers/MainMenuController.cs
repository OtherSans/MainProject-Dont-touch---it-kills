using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private string firstLocationScene = "Location_01";

    public void PlayGame()
    {
        SceneManager.LoadScene(firstLocationScene);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("QUIT GAME");
#endif
    }
}
