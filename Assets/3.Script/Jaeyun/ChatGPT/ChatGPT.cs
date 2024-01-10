using OpenApiFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatGPT : MonoBehaviour
{
    private NPCInfo_Data info;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button audioButton;

    private OpenAIRequest api;

    public async void TestResponseButton()
    {
        api = new OpenAIRequest();
        api.openAi_key = "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";
        api.Init();

        ChatRequest chatRequest = new ChatRequest();
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.system}", content = $"한식, 중식, 일식의 음식 한 가지만 말해야 한다. 15자 이내로" }); // npc prompt
        chatRequest.messages.Add(new ChatMessage() { role = $"{role.user}", content = $"내일 밥 뭐먹을까?" }); // audio
        List<ChatChoice> data = ((await (api.ClientResponseChat(chatRequest))).choices);
        Debug.Log(data);
        foreach (ChatChoice choice in data)
        {
            Debug.Log(choice.message.content);
        }
    }

    public void DialogButtonClick()
    {
        ChatGPTRequest(SpeechToText()); // STT한 string message
    }

    private void NpcClick()
    {
        NpcDialogStart();
    }

    private string SpeechToText()
    { // 음성 인식 버튼
        // 음성 인식이 일정 크기보다 클 때
        string message = string.Empty;

        return message;
    }

    private async void NpcDialogStart()
    { // Npc 클릭했을 때 한 번 실행
        ChatRequest request = new ChatRequest();
        request.messages = new List<ChatMessage>
        {
            new ChatMessage() { role = role.system.ToString(), content = $""},
        };
    }

    private async void ChatGPTRequest(string message)
    {
        ChatRequest request = new ChatRequest();
        request.messages = new List<ChatMessage>
        {
            new ChatMessage() { role = role.user.ToString(), content = $"{message}" },
        };

    }
}
