using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    [SerializeField] List<Slot> slots = new List<Slot>();
    private item_data data;

    private void OnEnable()
    {
        Get_data();
    }

    private void Set_data(string name, int num)
    {
        Debug.Log("Set_data");
        SQLManager.instance.Updateitem(name, num);
        Get_data();
    }

    private void Get_data()
    {
        Debug.Log("Get_data");
        data = SQLManager.instance.Item(SQLManager.instance.info.User_Id);
        Debug.Log(data.key_num);
        Debug.Log(data.player_id);
        Debug.Log(data.food_num);
        Slot_update();
    }

    private void Slot_update()
    {
        Debug.Log("Slot_update");
        slots[0].Slot_update(data.key_num);
        slots[1].Slot_update(data.have_fishingrod);
        slots[2].Slot_update(data.food_num);
    }
}
