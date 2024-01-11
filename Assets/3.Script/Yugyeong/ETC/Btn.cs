using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    [Header("pannel")]
    [SerializeField] private GameObject bag_pannel;
    [SerializeField] private GameObject collection_pannel;
    [SerializeField] private GameObject challange_pannel;
    [SerializeField] private GameObject setting_pannel;
    [SerializeField] private GameObject talk_pannel;
    [SerializeField] private GameObject quest_pannel;

    public void OpenandCloseUI(string str)
    {
        GameObject pannel = null;
        switch (str)
        {
            case "bag":
                pannel = bag_pannel;
                break;
            case "collection":
                pannel = collection_pannel;
                break;
            case "challange":
                pannel = challange_pannel;
                break;
            case "setting":
                pannel = setting_pannel;
                break;
            case "gobbi":
                pannel = talk_pannel;
                break;
            case "quest":
                pannel = quest_pannel;
                break;
            default:
                Debug.Log("뭔가 이상하다");
                break;
        }

        if (pannel.activeSelf)
        {
            pannel.SetActive(false);
        }
        else
        {
            pannel.SetActive(true);
        }
    }
}
