using OpenApiFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatGPT : MonoBehaviour
{
    string npcPrompt = string.Empty; // npc info
    private string playerRequest = string.Empty; // player audio

    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;
    public string npcResponse = string.Empty;

    private OpenAIRequest api;
    private ChatRequest chatRequest;

    public void FirstResponse()
    { // Goppi Button click 할 때마다 Method 불러오기
        api = new OpenAIRequest();
        api.openAi_key = "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";
        api.Init();

        // npc info
        npcPrompt = $"{TalkManager.instance.npcInfoSet.npcInfo.prompt}";
        Debug.Log(npcPrompt);
        chatRequest = new ChatRequest(); // message list 저장
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.system}", content = $"{npcPrompt}" }); // npc prompt
    }

    public async void NpcResponse(string message)
    { // response
        // npc info
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.user}", content = $"{playerRequest}" }); // player stt
        List<ChatChoice> data = ((await (api.ClientResponseChat(chatRequest))).choices);
        foreach (ChatChoice choice in data)
        {
            npcResponse = $"{choice.message.content}";
        }
        TalkManager.instance.responseText = npcResponse;
        nameText.text = TalkManager.instance.npcInfoSet.npcInfo.npcName;
        dialogText.text = npcResponse;

        chatRequest.messages.Add(new ChatMessage() { role = $"{role.assistant}", content = $"{npcResponse}" }); // npc response
    }
}
