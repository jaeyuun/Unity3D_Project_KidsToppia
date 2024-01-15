using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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
    public Study_YG animal_data;
    [SerializeField] int food_num;
    [SerializeField] private LayerMask layer;

    [Header("TTS")]
    [SerializeField] private AudioSource audio_source;
    private string url;

    public delegate void del_col();
    public static event del_col Event_colupdate;


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
        url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=";
        Close();
    }

    private void Open()
    {
        study_pannel.SetActive(true);
    }

    public void Close()
    {
        study_pannel.SetActive(false);

        if (audio_source.isPlaying)
        {
            audio_source.Stop();
        }
    }

    public void Interactive_Nonplayer()
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
        //StartCoroutine(Play_tts());
    }

    public void Eat_btn()
    {
        if (food_num <= 0)
        {
            eatstate_text.text = "가진 먹이가 없어요.";
            eatstate.SetActive(true);
        }

        else if (animal_data.data.give_food == 'F')
        {
            //동물 먹이주기
            animal_data.set_data("is_open", 'T');
            animal_data.set_data("givefood", 'T');
            eatstate_text.text = "동물에게 먹이를 주었어요.";
            eatstate.SetActive(true);
            Event_colupdate();
        }
        else
        {
            eatstate_text.text = "이미 먹이를 주었어요.";
            eatstate.SetActive(true);
        }
        Invoke("Turn_eatstate", 3f);
    }

    private void Turn_eatstate()
    {
        eatstate.SetActive(false);
    }

    IEnumerator Play_tts(string tts_info)
    {
        //개행 문자 제거
        tts_info = Regex.Replace(tts_info, "<br>", "");
        tts_info = Regex.Replace(tts_info, ",", "");

        //string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=SampleText&tl=Ko-gb";
        url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + $"{tts_info}" + "&tl=Ko-gb";
        
        WWW www = new WWW(url);
        yield return www;

        audio_source.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        audio_source.Play();
    }

    public void Playtts_btn()
    {
        StartCoroutine(Play_tts(animal_data.info));
    }
}
