using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    static public TalkManager instance = null;

    [SerializeField] GameObject talk_pannel;
    [SerializeField] TextMeshProUGUI dialog_text;
    [SerializeField] int dialog_index;
    [SerializeField] TextMeshProUGUI name_text;
    public delegate void del_talkend();
    public static event del_talkend event_talkend;

    [Header("data")]
    List<Dictionary<string, object>> data_Dialog;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        data_Dialog = CSVReader.Read("test");
        Close_dialog();
    }

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Print();
        }
    }

    public void Open_dialog()
    {
        talk_pannel.SetActive(true);
        if (event_talkend != null)
        {
            event_talkend();
        }
    }

    public void Close_dialog()
    {
        talk_pannel.SetActive(false);
        if (event_talkend != null)
        {
            event_talkend();
        }
    }

    public void Print()
    {
        if (!talk_pannel.activeSelf)
        {
            Open_dialog();
        }

        if (dialog_index < data_Dialog.Count)
        {
            name_text.text = data_Dialog[dialog_index]["Character_name"].ToString();
            dialog_text.text = data_Dialog[dialog_index]["Dialog"].ToString();
            dialog_index++;
        }

        else
        {
            dialog_index = 0;
            Close_dialog();
        }
    }
}

[System.Serializable]
public struct TalkData
{
    public string name;
    public string[] contexts;
}
