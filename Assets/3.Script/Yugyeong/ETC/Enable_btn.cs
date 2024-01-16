using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enable_btn : MonoBehaviour
{
    [SerializeField] Shopname shop;
    [SerializeField] int index;
    [SerializeField] Button btn;
    // Start is called before the first frame update

    private void OnEnable()
    {
        btn = GetComponent<Button>();
        var data = SQLManager.instance.Shop();

        switch (shop)
        {
            case Shopname.hair:
                if (index == 0)
                {
                    Change_click(data.hair1);
                }
                else
                {
                    Change_click(data.hair2);
                }
                break;
            case Shopname.riding:
                if (index == 0)
                {
                    Change_click(data.riding1);
                }
                else
                {
                    Change_click(data.riding2);
                }
                break;
            case Shopname.clothes:
                if (index == 0)
                {
                    Change_click(data.clothes1);
                }
                else
                {
                    Change_click(data.clothes2);
                }
                break;
            case Shopname.acc:
                if (index == 0)
                {
                    Change_click(data.acc1);
                }
                else
                {
                    Change_click(data.acc2);
                }
                break;
            default:
                break;
        }
    }

    private void Change_click(char a)
    {
        if (a == 'T')
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }
    }
}
