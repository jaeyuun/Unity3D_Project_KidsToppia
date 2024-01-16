using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Goods", menuName = "YG/Goods", order = 0)]
public class Goods : ScriptableObject
{
    [SerializeField] public ShopInfo shop;
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
                case ShopInfo.Hair:
                    a = index == 0 ? data.hair1 : data.hair2;
                    break;
                case ShopInfo.Riding:
                    a = index == 0 ? data.riding1 : data.riding2;
                    break;
                case ShopInfo.Clothes:
                    a = index == 0 ? data.clothes1 : data.clothes2;
                    break;
                case ShopInfo.Acc:
                    a = index == 0 ? data.acc1 : data.acc2;
                    break;
                default:
                    break;
            }
            return a;
        }
    }
}

