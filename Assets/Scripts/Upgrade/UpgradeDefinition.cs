using UnityEngine;
[CreateAssetMenu (
    fileName = "Upgrade_",
    menuName = "Game/Upgrade Definition")]
public class UpgradeDefinition : ScriptableObject
{
    [Header("Info")]
    public string upgradeName;

    [TextArea] public string description;

    public Sprite icon;

    [Header("Category")]
    public UpgradeCategory category;

    [Header("ID")]
    public string id;

    [Header("Effect")]
    public float effectValue;

    public float duration;
}
