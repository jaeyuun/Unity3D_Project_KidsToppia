using Mirror;
using TMPro;
using UnityEngine;

public class Community : NetworkBehaviour
{
    [SerializeField] private GameObject pannel;
    [SerializeField] private PlayerCreate target_player;
    [SerializeField] private PlayerName target_name;
    [SerializeField] private TMP_Text notice_text;


    private void Start()
    {
        pannel.SetActive(false);
    }

    public void community_btn(bool is_firend)
    {
        if (is_firend)
        {
            notice_text.text = $"{target_player.info.User_NickName}를 친구로 추가할까요?";
        }
        else
        {
            notice_text.text = $"{target_player.info.User_NickName}에게 파티를 신청할까요?";
        }
        pannel.SetActive(true);
    }

    public void Friend_Yes()

    {
        User_info my_info = SQLManager.instance.info;
        //Debug.Log($"내 닉네임 :{my_info.User_NickName} / 친구 닉네임 {target_player.info.User_NickName}");
        Add_friend(my_info, target_player.info);
        Add_friend(target_player.info, my_info);
    }

    public void Add_friend(User_info my_info, User_info friend_info)
    {
        string[] tmp = SQLManager.instance.Friend(my_info.User_Id).friends;
        //중복체크
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i] == friend_info.User_NickName)
            {
                return;
            }
        }
        SQLManager.instance.UpdateFriend(my_info, friend_info.User_NickName);
    }
}

public class Friend_data
{
    public string player_id;
    public string friend1;
    public string friend2;
    public string friend3;
    public string[] friends;


    public Friend_data(string playerid, string f1, string f2, string f3)
    {
        player_id = playerid;
        friend1 = f1;
        friend2 = f2;
        friend3 = f3;

        friends = new string[3];
        friends[0] = f1;
        friends[1] = f2;
        friends[2] = f3;
    }
}
