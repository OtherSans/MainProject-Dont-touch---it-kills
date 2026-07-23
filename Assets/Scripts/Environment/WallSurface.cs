using UnityEngine;

public class WallSurface : MonoBehaviour
{
    [SerializeField]
    private WallMaterial material;

    public WallMaterial Material => material;
}
