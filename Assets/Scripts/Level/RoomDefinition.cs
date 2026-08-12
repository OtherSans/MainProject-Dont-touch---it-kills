using UnityEngine;
public enum RoomSize
{
    Small,
    Medium,
    Large
}
public class RoomDefinition : MonoBehaviour
{
    [SerializeField]
    private RoomSize size;

    public RoomSize Size => size;
}
