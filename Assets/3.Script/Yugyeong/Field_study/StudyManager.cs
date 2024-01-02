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
    [SerializeField] Study_YG animal_data;
    [SerializeField] int food_num;
    [SerializeField] private LayerMask layer;

    [SerializeField] private AudioSource audio_source;
    private string url;


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

    public void Try_raycast(Vector3 pos)
    {
        Debug.Log("Try_raycast");
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Debug.Log("���� ��");
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
        StartCoroutine(Play_tts());
    }

    public void Eat_btn()
    {
        if (food_num <= 0)
        {
            eatstate_text.text = "���� ���̰� �����.";
        }

        if (food_num > 0)
        {
            //���� �����ֱ�
            //ȣ���� �ø���
            eatstate_text.text = "<color = black>�������� ���̸� �־����.";
        }

        Turn_eatstate();
        Invoke("Turn_eatstate", 3f);
    }

    private void Turn_eatstate()
    {
        eatstate_text.enabled = !eatstate_text.enabled;
    }

    IEnumerator Play_tts()
    {
        string tts_info = animal_data.info;

        //���� ���� ����
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
        StartCoroutine(Play_tts());
    }
}