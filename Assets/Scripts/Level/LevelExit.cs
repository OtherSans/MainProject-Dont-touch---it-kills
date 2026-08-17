using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(
        Collider2D other)
    {
        PlayerLevelKeyController keyController =
            other.GetComponent<PlayerLevelKeyController>();

        if (keyController == null)
            return;

        if (!keyController.HasKey)
        {
            Debug.Log(
                "Для прохода нужен ключ!"
            );

            return;
        }

        OpenExit(keyController);
    }

    private void OpenExit(
        PlayerLevelKeyController keyController)
    {
        keyController.RemoveKey();

        Debug.Log(
            "Проход на следующий уровень открыт!"
        );

        // Позже здесь:
        // переход на следующий уровень
    }
}
