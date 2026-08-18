using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeChoiceView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private UpgradeDefinition upgrade;

    public UpgradeDefinition Upgrade => upgrade;

    public void SetUpgrade(UpgradeDefinition newUpgrade)
    {
        upgrade = newUpgrade;

        if (nameText != null)
            nameText.text = upgrade.upgradeName;
        if (descriptionText != null)
            descriptionText.text = upgrade.description;
        if(icon != null)
        {
            icon.sprite = upgrade.icon;
            icon.enabled = 
                upgrade.icon != null;
        }
    }
    public Button Button => button;
}
