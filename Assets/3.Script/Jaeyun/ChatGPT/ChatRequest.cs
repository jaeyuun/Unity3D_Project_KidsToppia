using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.Runtime.Serialization;

namespace OpenApiFormat
{
    interface URL
    {
        public string Get_API_URL();
    }

    public enum role
    {
        [EnumMember(Value = "system")]
        system,
        [EnumMember(Value = "user")]
        user,
        [EnumMember(Value = "assistant")]
        assistant
    }

    public class ChatMessage
    {
        public role role;
        public string message = "";
    }


    public class ChatRequest : URL
    { // Chat GPT 요청, Only POST
        public const string api_url = "https://api.openai.com/v1/chat/completions"; // chat post url
        public string model = "gpt-3.5-turbo"; // Required
        public List<ChatMessage> message { get; set; } // Required

        public string Get_API_URL()
        {
            return api_url;
        }
    }

    public class ChatResponse
    { // Chat GPT 결과

        public string id { get; set; }
        public string object_ { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public string system_fingerprint { get; set; }
        public List<ChatChoice> choices { get; set; }
        public ChatUsage usage { get; set; }
    }

    public class ChatChoice
    { // choices list
        public int index { get; set; }

        public ChatMessage message { get; set; }

        public string finish_reason { get; set; }
    }

    public class ChatUsage
    { // usage list
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}
