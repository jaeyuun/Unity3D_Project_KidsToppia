using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OpenApiFormat
{
    interface URL
    {
        public string Get_API_URL();
    }

    public enum role
    {
        system = 0,
        user,
        assistant
    }

    [Serializable]
    public class ChatMessage
    {
        public string role = "";
        public string content = "";
    }

    [Serializable]
    public class ChatRequest : URL
    { // Chat GPT 요청, Only POST
        public const string api_url = "https://api.openai.com/v1/chat/completions"; // chat post url
        public string model = "gpt-3.5-turbo"; // Required
        public List<ChatMessage> messages = new List<ChatMessage>(); // Required

        public string Get_API_URL()
        {
            return api_url;
        }
    }

    [Serializable]
    public class ChatResponse
    { // Chat GPT 결과
        public string id { get; set; }
        public string object_ { get; set; }  // "object"를 "object_"로 변경
        public int created { get; set; }
        public string model { get; set; }
        public List<ChatChoice> choices { get; set; }
        public ChatUsage usage { get; set; }
        public string system_fingerprint { get; set; }
    }

    [Serializable]
    public class ChatChoice
    { // choices list
        public int index { get; set; }
        public ChatMessage message { get; set; }
        public object logprobs { get; set; }
        public string finish_reason { get; set; }
    }

    [Serializable]
    public class ChatUsage
    { // usage list
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}
