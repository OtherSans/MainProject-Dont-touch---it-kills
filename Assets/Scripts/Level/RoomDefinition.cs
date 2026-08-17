using UnityEngine;
public enum RoomSize
{
    Small,
    Medium,
    Large
}
public enum RoomType
{
    Normal,
    Shop,
    Key
}
public class RoomDefinition : MonoBehaviour
{
    [SerializeField]
    private RoomSize size;
    [SerializeField]
    private RoomType type;
    [SerializeField]
    private BoxCollider2D generationBounds;

    public RoomSize Size => size;
    public RoomType Type => type;
    public BoxCollider2D GenerationBounds => generationBounds;
}
