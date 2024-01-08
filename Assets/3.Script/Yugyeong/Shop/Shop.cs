using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Shopname
{
    hair = 0,
    riding,
    clothes,
    acc
}

public class Shop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text name_text;
    [SerializeField] Shop_info shop_Info;

    [Header("Player")]
    [SerializeField] private int money = 1000;
}
