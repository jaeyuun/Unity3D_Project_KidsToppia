using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum shop_info
{
    hair = 0,
    riding,
    clothes,
    acc
}

public class Shop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private string name_text;
    [SerializeField] private GameObject[] shop_pannels;

    [SerializeField] private shop_info info;

    [Header("Player")]
    [SerializeField] private int money = 1000;

    //DB바꾸기 - 학원가서 하기
    private void OnEnable()
    {
        for (int i = 0; i < shop_pannels.Length; i++)
        {
            switch (info)
            {
                case shop_info.hair:
                    Open(0);
                    break;
                case shop_info.riding:
                    Open(1);
                    break;
                case shop_info.clothes:
                    Open(2);
                    break;
                case shop_info.acc:
                    Open(3);
                    break;
                default:
                    break;
            }
        }
    }

    private void Open(int index)
    {
        for (int i = 0; i < shop_pannels.Length; i++)
        {
            if (i == index)
            {
                shop_pannels[i].SetActive(true);
            }
            else
            {
                shop_pannels[i].SetActive(false);
            }
        }
    }
}
