using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Friend_slot : MonoBehaviour
{
    [SerializeField] Friend_data data;
    [SerializeField] int index;

    [SerializeField] GameObject friend_o;
    [SerializeField] GameObject friend_x;

    [SerializeField] private TMP_Text name;
    [SerializeField] private Text onoff_text;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites; //O,X
    [SerializeField] private Button chase_btn;

    private void OnEnable()
    {
        data = SQLManager.instance.Friend();
        friend_x = transform.GetChild(0).gameObject;
        friend_o = transform.GetChild(1).gameObject;
        Setting();
    }

    private void Setting()
    {
        data = SQLManager.instance.Friend();

        if (data.friends[index] == string.Empty)
        {
            friend_x.SetActive(true);
            friend_o.SetActive(false);
        }

        else
        {
            friend_o.SetActive(true);
            friend_x.SetActive(false);
            O_setting();
        }
    }
    private void O_setting()
    {
        name.text = data.friends[index];

        if (SQLManager.instance.PlayerInfo(data.friends[index]).Connecting == 'T')
        {
            onoff_text.text = "온라인";
            image.sprite = sprites[0];
            chase_btn.interactable = true;
        }

        else
        {
            onoff_text.text = "오프라인";
            image.sprite = sprites[1];
            chase_btn.interactable = false;
        }
    }

    public void DeleteFriend()
    {
        SQLManager.instance.DeleteFriend(index);
        Setting();
    }

    public void Chase()
    {
        if (chase_btn.interactable)
        {
            //친구 위치로 이동
        }
        else
        {
            return;
        }
    }
}
