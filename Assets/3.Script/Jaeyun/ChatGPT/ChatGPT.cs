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
    public string npcResponse = string.Empty;

    private OpenAIRequest api;

    public async void NpcResponse(string message)
    { // response
        api = new OpenAIRequest();
        api.openAi_key = "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";
        api.Init();

        // npc info
        npcPrompt = $"{TalkManager.instance.npcInfoSet.npcInfo.prompt}";
        ChatRequest chatRequest = new ChatRequest();
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.system}", content = $"{npcPrompt}" }); // npc prompt
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.user}", content = $"{playerRequest}" }); // player stt
        List<ChatChoice> data = ((await (api.ClientResponseChat(chatRequest))).choices);
        foreach (ChatChoice choice in data)
        {
            npcResponse = $"{choice.message.content}";
        }

        TalkManager.instance.responseText = npcResponse;
    }
}
