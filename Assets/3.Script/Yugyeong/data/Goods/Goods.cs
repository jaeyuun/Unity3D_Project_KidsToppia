using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Goods", menuName = "YG/Goods", order = 0)]
public class Goods : ScriptableObject
{
    [SerializeField] public Shopname shop;
    [SerializeField] public int index;
    [SerializeField] public int price;
    [SerializeField] public char is_purchase
    {
        get
        {
            var data = SQLManager.instance.Shop();
            char a = 'a';
            switch (shop)
            {
                case Shopname.hair:
                    a = index == 0 ? data.hair1 : data.hair2;
                    break;
                case Shopname.riding:
                    a = index == 0 ? data.riding1 : data.riding2;
                    break;
                case Shopname.clothes:
                    a = index == 0 ? data.clothes1 : data.clothes2;
                    break;
                case Shopname.acc:
                    a = index == 0 ? data.acc1 : data.acc2;
                    break;
                default:
                    break;
            }
            return a;
        }
    }
}

