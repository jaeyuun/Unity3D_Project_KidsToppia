using OpenApiFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class ChatGPT : MonoBehaviour
{
    private NPCInfo_Data info;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Button audioButton;

    OpenAIRequest api;

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
        request.message = new List<ChatMessage>
        {
            new ChatMessage() { role = role.system, message = $""},
        };
    }

    private async void ChatGPTRequest(string message)
    {
        ChatRequest request = new ChatRequest();
        request.message = new List<ChatMessage>
        {
            
            new ChatMessage() { role = role.system, message = $"{message}" },
        };

    }
}
