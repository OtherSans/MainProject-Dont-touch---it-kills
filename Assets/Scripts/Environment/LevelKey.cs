using UnityEngine;

public class LevelKey : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float minSwordExtension = 0.6f;

    private bool isCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"KEY touched by: {other.name}");

        TryCollect(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider2D other)
    {
        if (isCollected)
            return;

        Debug.Log($"KEY touched by: {other.name}");

        SwordShopDetector detector =
            other.GetComponent<SwordShopDetector>();

        if (detector == null)
        {
            Debug.Log("Нет SwordShopDetector");
            return;
        }

        SwordController sword =
            detector.Sword;

        if (sword == null)
        {
            Debug.Log("У detector нет Sword");
            return;
        }

        Debug.Log(
            $"Sword extension = {sword.ExtensionNormalized}"
        );

        if (sword.ExtensionNormalized < minSwordExtension)
        {
            Debug.Log("Меч недостаточно вытянут");
            return;
        }

        PlayerController player =
            sword.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            Debug.Log("PlayerController не найден");
            return;
        }

        PlayerLevelKeyController keyController =
            player.GetComponent<PlayerLevelKeyController>();

        if (keyController == null)
        {
            Debug.Log("PlayerLevelKeyController не найден");
            return;
        }

        LevelRoomManager roomManager =
            FindAnyObjectByType<LevelRoomManager>();

        if (roomManager == null)
        {
            Debug.Log("LevelRoomManager НЕ НАЙДЕН");
            return;
        }

        Debug.Log("KEY COLLECTED");

        isCollected = true;

        keyController.GiveKey();

        roomManager.ActivateKeyPhase();

        Destroy(gameObject);
    }
}
