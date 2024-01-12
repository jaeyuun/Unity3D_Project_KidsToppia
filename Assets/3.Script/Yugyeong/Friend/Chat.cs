using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Chat : NetworkBehaviour
{
    [SerializeField] private Text chatText;
    [SerializeField] private InputField inputfield;
    [SerializeField] private GameObject canvas;

    private static event Action<string> onMessage;

    //client가 server에 connect 되었을 때 콜백함수
    public override void OnStartAuthority()
    {
        Debug.Log("OnStartAuthority");
        if(isLocalPlayer)
        {
            canvas.SetActive(true);
        }
        onMessage += newMessage;
    }
    private void newMessage(string mess)
    {
        Debug.Log("newMessage");
        Debug.Log($"추가된 메세지 {mess}");
        chatText.text += mess;
    }

    //클라이언트가 Server를 나갔을 때 
    [ClientCallback]
    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }
    //RPC는 결국 ClientRpc 명령어 < Command(server)명령어 < Client 명령어?
    
    [Client]
    public void Send()
    {
        Debug.Log("Send");
        //if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (string.IsNullOrWhiteSpace(inputfield.text)) return;
        cmdSendMessage(SQLManager.instance.info.User_NickName,inputfield.text);
        inputfield.text = string.Empty;
    }
    
    [Command(requiresAuthority = false)]
    private void cmdSendMessage(string nickname, string message)
    {
        Debug.Log($"cmdSendMessage : {message}");
        RPCHandleMessage(nickname,message);
    }

    [ClientRpc]
    private void RPCHandleMessage(string nickname, string message)
    {
        Debug.Log("RPCHandleMessage");
        onMessage?.Invoke($"[{nickname}] : {message}\n");
    }
}
