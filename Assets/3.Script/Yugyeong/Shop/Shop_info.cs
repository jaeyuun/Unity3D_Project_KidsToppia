using UnityEngine;

public abstract class Shop_info : MonoBehaviour
{
    GameObject shop_pannel;
    Shop_slot[] slots;

    public void Open()
    {
        shop_pannel.SetActive(true);
    }

    public void Close()
    {
        shop_pannel.SetActive(false);
    }
}
