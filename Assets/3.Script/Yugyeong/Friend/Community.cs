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
        target_player = gameObject.transform.root.GetComponent<PlayerName>().target_info;
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
        string[] tmp = SQLManager.instance.Friend().friends;
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i] == target_player.info.User_NickName)
            {
                return;
            }
        }
        SQLManager.instance.UpdateFriend(target_player.info.User_NickName);
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
