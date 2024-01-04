using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_data
{
    public string player_id;
    public int key_num;
    public int have_fishingrod;
    public int food_num;

    public item_data(string id, int keynum, int havefishingrod, int foodnum)
    {
        player_id = id;
        key_num = keynum;
        have_fishingrod = havefishingrod;
        food_num = foodnum;
    }
}

public class Itemchecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Get_itemdata()
    {
    
    }
}
