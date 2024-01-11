using OpenApiFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatGPT : MonoBehaviour
{
    // npc info
    private NPCInfo_Data info;
    string npcPrompt = string.Empty;
    // player audio
    private string playerRequest = string.Empty;

    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button audioButton;

    private OpenAIRequest api;

    public async void NpcResponse()
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
            dialogText.text = $"{choice.message.content}";
        }
    }

    public void DialogButtonClick()
    {
        ChatGPTRequest(SpeechToText()); // STT한 string message
    }

    private string SpeechToText()
    { // 음성 인식 버튼
        // 음성 인식이 일정 크기보다 클 때
        string message = string.Empty;

        return message;
    }

    private void ChatGPTRequest(string message)
    {
        ChatRequest request = new ChatRequest();
        request.messages = new List<ChatMessage>
        {
            new ChatMessage() { role = role.user.ToString(), content = $"{message}" },
        };

    }
}
