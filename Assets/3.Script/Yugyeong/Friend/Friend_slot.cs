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
    [SerializeField] private GameObject friend_obj;
    [SerializeField] private GameObject my_obj;

    private void OnEnable()
    {
        data = SQLManager.instance.Friend(SQLManager.instance.info.User_Id);
        friend_x = transform.GetChild(0).gameObject;
        friend_o = transform.GetChild(1).gameObject;
        Setting();
    }

    private void Setting()
    {
        data = SQLManager.instance.Friend(SQLManager.instance.info.User_Id);

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

    public void Chase(int index) //0116 merge후 각 따라가기 버튼에 달고 인덱스 0,1,2 
    {
        //친구 플레이어, 본인 플레이어 찾기
        var players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerCreate>().info.User_NickName == SQLManager.instance.info.User_NickName)
            {
                my_obj = players[i];
                //Debug.Log($"내 닉네임 : {players[i].GetComponent<PlayerCreate>().info.User_NickName}");
            }

            //친구 닉네임 = 내 데이터에 저장된 닉네임
            if (players[i].GetComponent<PlayerCreate>().info.User_NickName == SQLManager.instance.Friend(SQLManager.instance.info.User_Id).friends[index])
            {
                friend_obj = players[i];
                //Debug.Log($"친구 닉네임 : {players[i].GetComponent<PlayerCreate>().info.User_NickName}");
            }
        }

        //위치 이동
        my_obj.transform.position = friend_obj.transform.position + Vector3.up;

    }
}
