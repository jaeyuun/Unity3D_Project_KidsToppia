using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] Text none_text;
    [SerializeField] Text text;

    public void Slot_update(int num)
    {
        Debug.Log("Slot_update");
        if (num <= 0)
        {
            Debug.Log("num <= 0");
            none_text.enabled = true;
            text.enabled = false;
        }
        else
        {
            Debug.Log("num > 0");
            none_text.enabled = false;
            text.enabled = true;
            text.text = $"{num}";
        }
    }
}
