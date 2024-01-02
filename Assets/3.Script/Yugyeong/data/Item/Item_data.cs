using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_data", menuName = "YG/Quest_YG/quest_data", order = 0)]
public class Item_data : ScriptableObject
{
    [SerializeField] private string item_Name;
    public string itemName => item_Name;

    [SerializeField] private int num_;
    public int num => num_;

    [SerializeField] private int maxnum;
    public int max_num => maxnum;

    [SerializeField] private Sprite sprite_;
    public Sprite sprite => sprite_;
}
