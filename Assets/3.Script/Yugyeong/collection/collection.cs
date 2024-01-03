using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class collection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI non_text;
    [SerializeField] Sprite non_sprite;
    [SerializeField] TextMeshProUGUI heart_num;
    [SerializeField] TextMeshProUGUI star_num;

    [Header("Data")]
    public Study_YG study; // study_data.Get_data() = DBdata

    private void OnEnable()
    {
        Set_image();
    }

    private void Set_image()
    {
        if (study.data.is_open == 'T')
        {
            non_text.enabled = false;
            image.sprite = study.sprite;
            Set_number(study.data.is_solved, star_num);
            Set_number(study.data.give_food, heart_num);
        }

        else
        {
            non_text.enabled = true;
            image.sprite = non_sprite;
            star_num.text = "?";
            heart_num.text = "?";
        }
    }

    private void Set_number(char state, TextMeshProUGUI ui)
    {
        if (state == 'T')
        {
            ui.text = "5";
        }
        else
        {
            ui.text = "0";
        }
    }
}
