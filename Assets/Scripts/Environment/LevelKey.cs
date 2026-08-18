using UnityEngine;

public class LevelKey : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float minSwordExtension = 0.6f;

    private bool isCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
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

        SwordShopDetector detector =
            other.GetComponent<SwordShopDetector>();

        if (detector == null)
            return;

        SwordController sword =
            detector.Sword;

        if (sword == null)
            return;

        if (sword.ExtensionNormalized <
            minSwordExtension)
        {
            return;
        }

        PlayerController player =
            sword.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        PlayerLevelKeyController keyController =
            player.GetComponent<PlayerLevelKeyController>();

        if (keyController == null)
        {
            Debug.LogError(
                "PlayerLevelKeyController не найден.",
                player
            );

            return;
        }

        LevelRoomManager roomManager =
            FindAnyObjectByType<LevelRoomManager>();

        if (roomManager == null)
        {
            Debug.LogError(
                "LevelRoomManager не найден.",
                this
            );

            return;
        }

        isCollected = true;

        keyController.GiveKey();

        roomManager.ActivateKeyPhase();

        Debug.Log("KEY COLLECTED");

        Destroy(gameObject);
    }
}
