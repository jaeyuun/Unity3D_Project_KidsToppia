using LitJson;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class SpeechToText : MonoBehaviour
{
    public Button micButton;
    public Image micButtonImage;

    private string microphoneId = null;
    private AudioClip recording = null;
    private int recordingLength = 5; // recording 최대 초
    private int recordingHZ = 44100;
    private string filePath = string.Empty;

    private void Start()
    {
        microphoneId = Microphone.devices[0]; // 마이크 디바이스 설정
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Application.persistentDataPath + "/Clova";
        }
        else
        { // window

            filePath = Application.dataPath + "/Clova";
        }
        if (!File.Exists(filePath)) // 해당 경로에 파일이 없다면
        { // folder 검사
            Directory.CreateDirectory(filePath); // Directory 생성
        }
    }

    private void MicrophonePermission()
    { // Android 권한 요청
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }
    }

    public void AudioButtonClick()
    { // mic button click method
        MicrophonePermission();
        StartRecording();
    }

    #region MicrophoneRecoring
    private void StartRecording()
    {
        recording = Microphone.Start(microphoneId, false, recordingLength, recordingHZ);
        StartCoroutine(StopRecording_Co());
        return;
    }

    public IEnumerator StopRecording_Co()
    {
        // color
        Color color;
        ColorUtility.TryParseHtmlString("#00CD9D", out color);
        micButtonImage.color = color;
        micButton.enabled = false;

        yield return new WaitForSeconds(5f);

        // color
        ColorUtility.TryParseHtmlString("#FF727D", out color);
        micButtonImage.color = color;
        micButton.enabled = true;

        if (Microphone.IsRecording(microphoneId))
        {
            Microphone.End(microphoneId);
            SavWav.Save($"{filePath}/audio", recording);
            if (recording == null)
            {
                Debug.LogError("Nothing Recorded");
            }
            yield return null;
            // byte[] byteData = GetByteFromAudioClip(recording); // audio clip to byte array
            StartCoroutine(PostVoice_Co(filePath));
        }
    }
    #endregion
    /*private byte[] GetByteFromAudioClip(AudioClip audioClip)
    { // audio clip을 저장하지 않고 바로 byte로 변환하기
        float[] samples = new float[audioClip.samples];
        audioClip.GetData(samples, 0);

        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);

        int length = samples.Length;
        writer.Write(length);

        foreach (float sample in samples)
        {
            writer.Write(sample);
        }

        byte[] bytes = stream.ToArray();
        return bytes;
    }*/
    [SerializeField]
    public class ClovaResponse
    { // 받아온 값에 간편하게 접근하기 위한 json 선언
        public string text;
    }

    #region API STT
    private string client_id = "tzzjets5nx";
    private string client_api_key = "ovtg4WKrK1R1nw6FExiinMK3Nafk4IEWuTmdw9fw";

    private IEnumerator PostVoice_Co(string path)
    {
        string FilePath = $"{path}/audio.wav";
        string lang = TalkManager.instance.npcInfoSet.npcInfo.language;
        FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        byte[] fileData = new byte[fs.Length];
        fs.Read(fileData, 0, fileData.Length);
        fs.Close();

        string url = $"https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=";
        url += lang; // 언어 코드 ( Kor, Jpn, Eng, Chn )
        // Request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("X-NCP-APIGW-API-KEY-ID", $"{client_id}");
        request.Headers.Add("X-NCP-APIGW-API-KEY", $"{client_api_key}");
        request.Method = "POST";
        request.ContentType = "application/octet-stream";
        request.ContentLength = fileData.Length;
        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(fileData, 0, fileData.Length);
            requestStream.Close();
        }

        // Response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        yield return request.GetResponse(); // request 후 response 받을 때까지 대기

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        string text = reader.ReadToEnd();
        stream.Close();
        response.Close();
        reader.Close();

        string jsonText = JsonUtility.ToJson(text);
        ClovaResponse clovaText = new ClovaResponse(); // json to text
        clovaText = JsonMapper.ToObject<ClovaResponse>(text);
        TalkManager.instance.PlayerRequestText(clovaText.text);
    }
    #endregion
}
