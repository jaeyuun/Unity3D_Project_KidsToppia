using OpenApiFormat;
using System;
using System.Text; // encoding
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class OpenAIRequest
{
    public string openAi_key = "Secret Key"; // api secret key

    private static HttpClient client;

    private string api_URL = "";
    private const string authoirzationHeader = "Bearer";
    private const string userAgentHeader = "User-Agent";

    public void Init()
    {
        CreateHttpClient();
    }

    private void CreateHttpClient()
    {
        client = new HttpClient();
        // HttpRequestHeaders Setting
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authoirzationHeader, openAi_key);
        client.DefaultRequestHeaders.Add(userAgentHeader, "okgodoit/dotnet_openai_api"); // gpt-3.5-turbo sdk, https://github.com/OkGoDoIt/OpenAI-API-dotnet
    }

    public async Task<string> ClientResponse<SendData>(SendData request) // ChatRequest or STTRequest
    {
        if (client == null)
        {
            CreateHttpClient();
        }

        api_URL = ((URL)request).Get_API_URL(); // URL 가져오기       

        string jsonContent = JsonUtility.ToJson(request); // json 직렬화
        StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        Debug.Log(api_URL);
        Debug.Log(stringContent);

        // 데이터 전송 후 대기
        using (HttpResponseMessage response = await client.PostAsync(api_URL, stringContent)) // only post
        {
            if (response.IsSuccessStatusCode)
            { // 응답 성공
                return await response.Content.ReadAsStringAsync(); // string으로 return
            }
            else
            { // 응답 실패
                throw new HttpRequestException($"Error calling OpenAI API to get completion. HTTP status code: {response.StatusCode}. Request body: {jsonContent}");
            }
        }
    }

    public string ResponseJson(string player_audio)
    {
        ChatMessage chatMessage = new ChatMessage();
        chatMessage.role = role.user.ToString();
        chatMessage.content = player_audio;

        string json = JsonUtility.ToJson(chatMessage);

        return json;
    }

    public async Task<ChatResponse> ClientResponseChat(ChatRequest r)
    {
        return JsonMapper.ToObject<ChatResponse>(await ClientResponse(r));
    }
}
