using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop_slot : MonoBehaviour
{
    [SerializeField] Goods goods;
    [SerializeField] Button btn;
    [SerializeField] bool is_purchase;
    [SerializeField] Text purchase_text;

    private void UI_update()
    {
        if (goods.is_purchase)
        {
            purchase_text.text = "구매 완료";
            btn.interactable = false;
        }
        else
        {
            purchase_text.text = "구매하기";
            btn.interactable = true;
        }
    }
}
