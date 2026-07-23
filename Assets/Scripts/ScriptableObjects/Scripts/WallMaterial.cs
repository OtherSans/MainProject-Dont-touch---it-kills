using UnityEngine;

[CreateAssetMenu(
    fileName = "WallMaterial",
    menuName = "Game/Wall Material")]
public class WallMaterial : ScriptableObject
{
    [Header("Sword resistance")]

    [Tooltip("Мгновенная потеря скорости при ударе")]
    [Range(0f, 1f)]
    public float velocityMultiplier = 0.3f;

    [Tooltip("Сопротивление во время движения меча внутри стены")]
    [Min(0f)]
    public float resistance = 10f;

    [Tooltip("Максимальное угловое проникновение меча")]
    [Min(0f)]
    public float maxPenetrationAngle = 15f;
}
