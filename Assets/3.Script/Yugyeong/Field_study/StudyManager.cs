using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Study_data
{
    public string name { get; private set; }
    public string study_info { get; private set; }
}

public class StudyManager : MonoBehaviour
{
    public static StudyManager instance = null;

    [Header("UI")]
    [SerializeField] GameObject study_pannel;
    [SerializeField] TextMeshProUGUI animal_name;
    [SerializeField] TextMeshProUGUI animal_info;
    [SerializeField] Image animal_image;
    [SerializeField] GameObject eatstate;
    [SerializeField] TextMeshProUGUI eatstate_text;

    [Header("Data")]
    [SerializeField] Study_YG animal_data;
    [SerializeField] int food_num;
    [SerializeField] private LayerMask layer;
    [SerializeField] private AudioSource audio_source;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audio_source = GetComponent<AudioSource>();
        Close();
    }

    private void Open()
    {
        study_pannel.SetActive(true);
    }

    public void Close()
    {
        study_pannel.SetActive(false);
    }

    public void Try_raycast(Vector3 pos)
    {
        Debug.Log("Try_raycast");
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Debug.Log("레이 쏨");
            if (hit.collider.CompareTag("Animal"))
            {
                animal_data = hit.collider.gameObject.GetComponent<Nonplayer_YG>().data;
                Interactive_Nonplayer();
            }
        }
    }

    private void Interactive_Nonplayer()
    {
        if (!study_pannel.activeSelf)
        {
            Open();
        }

        Data_update();
    }

    private void Data_update()
    {
        animal_name.text = animal_data.animal_name;
        animal_info.text = animal_data.info;
        animal_image.sprite = animal_data.sprite;
    }

    public void Eat_btn()
    {
        if (food_num <= 0)
        {
            eatstate_text.text = "가진 먹이가 없어요.";
        }

        if (food_num > 0)
        {
            //동물 먹이주기
            //호감도 올리기
            eatstate_text.text = "<color = black>동물에게 먹이를 주었어요.";
        }

        Turn_eatstate();
        Invoke("Turn_eatstate", 3f);
    }

    private void Turn_eatstate()
    {
        eatstate_text.enabled = !eatstate_text.enabled;
    }

    public void Read_info()
    {
        //TTS 넣기
        Debug.Log("Read_info");
        Debug.Log(animal_data.animal_name);
    }


}
