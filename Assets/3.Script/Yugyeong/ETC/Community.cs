using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Community : NetworkBehaviour
{
    private GameObject pannel;
    private bool is_pannelopen;
    private Text notice_text;


    [Command]
    private void CmdOpen()
    {

    }

    [TargetRpc]
    private void TRPCOpen()
    {

    }

    [Client]
    private void Yes()
    {

    }
}

public class Friend_data
{
    public string player_id;
    public string friend1;
    public string friend2;
    public string friend3;

    public Friend_data(string playerid, string f1, string f2, string f3)
    {
        player_id = playerid;
        friend1 = f1;
        friend2 = f2;
        friend3 = f3;
    }
}
