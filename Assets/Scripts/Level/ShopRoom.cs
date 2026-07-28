using UnityEngine;

public class ShopRoom : RoomController
{
    [SerializeField] private GameObject shopInterface;

    protected override void OnRoomEntered()
    {
        if(shopInterface != null)
            shopInterface.SetActive(true);

        Debug.Log("Игрок вошел в магазин");
    }
    protected override void OnRoomExited()
    {
        if (shopInterface != null)
            shopInterface.SetActive(false);

        Debug.Log("Игрок вышел из магазина");
    }
}
