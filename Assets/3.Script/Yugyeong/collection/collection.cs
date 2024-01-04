using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class collection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI non_text;
    [SerializeField] Sprite non_sprite;
    [SerializeField] TextMeshProUGUI heart_num;
    [SerializeField] TextMeshProUGUI star_num;

    [Header("Data")]
    public Study_YG study;

    private void Start()
    {
        StudyManager.Event_colupdate += Set_image;
    }
    private void OnEnable()
    {
        Set_image();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Open_study();
        }
    }

    public void Set_image()
    {
        Debug.Log("Set_image()");
        if (study.data.is_open == 'T')
        {
            non_text.enabled = false;
            image.sprite = study.sprite;
            Set_number(study.data.is_solved, star_num);
            Set_number(study.data.give_food, heart_num);
        }

        else
        {
            Debug.Log(study.data.is_open == 'F');
            non_text.enabled = true;
            image.sprite = non_sprite;
            star_num.text = "?";
            heart_num.text = "?";
        }
    }

    public void Open_study()
    {
        if ( study.data.is_open == 'T' && EventSystem.current.IsPointerOverGameObject())
        {
            StudyManager.instance.animal_data = study;
            StudyManager.instance.Interactive_Nonplayer();
        }
    }

    private void Set_number(char state, TextMeshProUGUI ui)
    {
        if (state == 'F')
        {
            ui.text = "0";
        }
        else
        {
            ui.text = "5";
        }
    }
}
