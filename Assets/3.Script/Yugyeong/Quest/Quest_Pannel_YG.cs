using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest_Pannel_YG : MonoBehaviour
{
    [SerializeField] Quest_YG quest;
    [SerializeField] TextMeshProUGUI info_text;

    private void Start()
    {
        info_text.text = quest.info;
    }
}
