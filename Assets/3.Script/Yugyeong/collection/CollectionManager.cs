using System.Collections;
using UnityEngine;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] collection[] collection_arr;
    [SerializeField] Study_YG[] animal_arr;
    [SerializeField] Study_YG[] fish_arr;

    [SerializeField] GameObject info_pannel;
    [SerializeField] TMP_Text btn_text;

    [SerializeField] bool is_animaldata = true;
    //playerid = SQLManager.instance.info.User_Id;

    private void Start()
    {
        info_pannel.SetActive(false);
        Set_studydata();
    }
    private void Set_studydata()
    {
        for (int i = 0; i < collection_arr.Length; i++)
        {
            if (is_animaldata)
            {
                collection_arr[i].study = animal_arr[i];
            }
            else
            {
                collection_arr[i].study = fish_arr[i];
            }
            collection_arr[i].gameObject.SetActive(true);
            collection_arr[i].Set_image();
        }

        if (is_animaldata)
        {
            btn_text.text = "동물 도감";
        }
        else
        {
            btn_text.text = "물고기 도감";
        }
    }

    public void change_btn()
    {
        is_animaldata = !is_animaldata;
        Set_studydata();
    }

    //info
    private void Set_info()
    {
        if (info_pannel.activeSelf)
        {
            info_pannel.SetActive(false);
        }
        else
        {
            info_pannel.SetActive(true);
        }
    }

    public void info_btn()
    {
        Set_info();
        StartCoroutine(Wait_input());
    }

    IEnumerator Wait_input()
    {
        while (true)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    break;
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    break;
                }
            }
            yield return null;
        }
        Set_info();
    }

}
