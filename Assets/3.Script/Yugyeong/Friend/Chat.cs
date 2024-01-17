using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Chat : NetworkBehaviour
{
    [SerializeField] private Text chatText;
    [SerializeField] private InputField inputfield;
    [SerializeField] private GameObject canvas;

    [SerializeField]private bool is_smallsize = true;
    [SerializeField] private Image image;
    [SerializeField] private GameObject xbutton;
    [SerializeField] private GameObject scrollview;

    private static event Action<string> onMessage;

    //client�� server�� connect �Ǿ��� �� �ݹ��Լ�
    public override void OnStartAuthority()
    {
        if(isLocalPlayer)
        {
            canvas.SetActive(true);
        }
        onMessage += newMessage;
        Set_size();
    }
    private void newMessage(string mess)
    {
        chatText.text += mess;
    }

    //Ŭ���̾�Ʈ�� Server�� ������ �� 
    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }
    //RPC�� �ᱹ ClientRpc ���ɾ� < Command(server)���ɾ� < Client ���ɾ�?

    [Client]
    public void Send()
    {
        //if (!Input.GetKeyDown(KeyCode.Return)) return;
        if(!is_smallsize)
        {
            Set_size();
            return;
        }

        if (string.IsNullOrWhiteSpace(inputfield.text)) return;
        cmdSendMessage(SQLManager.instance.info.User_NickName, inputfield.text);
        inputfield.text = string.Empty;
    }

    [Command(requiresAuthority = false)]
    private void cmdSendMessage(string nickname, string message)
    {
        RPCHandleMessage(nickname,message);
    }

    [ClientRpc]
    private void RPCHandleMessage(string nickname, string message)
    {
        onMessage?.Invoke($"[{nickname}] : {message}\n");
    }

    public void Set_size()
    {
        if (is_smallsize)
        {
            is_smallsize = false;
            image.enabled = false;
            xbutton.SetActive(false);
            scrollview.SetActive(false);
        }

        else
        {
            is_smallsize = true;
            image.enabled = true;
            xbutton.SetActive(true);
            scrollview.SetActive(true);
        }
    }
}
