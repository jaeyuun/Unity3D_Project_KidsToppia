using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text; // encoding
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInItem
{ // Database login.json
    public string ID;
    public string PW;
    public string NickName;

    public SignInItem(string id, string pw, string nickName)
    {
        ID = id;
        PW = pw;
        NickName = nickName;
    }
}

public class LoginChecker : MonoBehaviour
{
    [SerializeField] private GameObject signInPanel;
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private GameObject deleteaccountPannel;
    [SerializeField] private GameObject checkButton;
    [SerializeField] private TMP_Text log;

    [SerializeField] private GameObject playerLogButton;
    [SerializeField] private TMP_Text id_userInfo;
    [SerializeField] private TMP_Text nickname_userInfo;

    [Header("Sign In")]
    public InputField idInput_in;
    public InputField pwInput_in;
    [Header("Sign Up")]
    public InputField idInput_up;
    public InputField pwInput_up;
    public InputField nickName_up;
    [Header("Delete")]
    public InputField idInput_delete;
    public InputField pwInput_delete;
    public InputField nickName_delete;

    private bool isLogin = false;
    private string dbPath = string.Empty;

    private void Start()
    {
        InfoButtonActive();
    }

    public void InfoButtonActive()
    { // 플레이어 로그인 시 유저 정보 버튼 활성화 및 아이디 닉네임 띄우기
        if (Application.platform == RuntimePlatform.Android)
        {
            dbPath = Application.persistentDataPath + "/Database"; // 경로를 string에 저장
            if (!File.Exists(dbPath + "/UserInfo.json"))
            { // file 검사
                isLogin = false;
                return;
            }
            else
            {
                playerLogButton.SetActive(true);
                isLogin = true;
            }
        }
        else
        { // window
            dbPath = Application.dataPath + "/Database"; // 경로를 string에 저장
            if (!File.Exists(dbPath + "/UserInfo.json"))
            { // file 검사
                isLogin = false;
                return;
            }
            else
            {
                playerLogButton.SetActive(true);
                isLogin = true;
            }
        }

        // user info text
        string jsonString = File.ReadAllText(dbPath + "/UserInfo.json", Encoding.UTF8);
        JsonData ItemData = JsonMapper.ToObject(jsonString);
        id_userInfo.text = $"{ItemData[0]["ID"]}";
        nickname_userInfo.text = $"{ItemData[0]["NickName"]}";
    }

    private void DefaultData(string path, User_info info)
    {
        List<SignInItem> items = new List<SignInItem>();
        items.Add(new SignInItem($"{info.User_Id}", $"{info.User_Pw}", $"{info.User_NickName}")); // id, pw, nickName
        JsonData data = JsonMapper.ToJson(items);
        File.WriteAllText(path + "/UserInfo.json", data.ToString(), Encoding.UTF8);
    }

    public void SignInButton()
    { // 로그인 버튼
        if (idInput_in.text.Equals(string.Empty) || pwInput_in.text.Equals(string.Empty))
        {
            checkButton.SetActive(true);
            log.text = "빈칸을 입력해 주세요.";
            return;
        }
        if (SQLManager.instance.SignIn(idInput_in.text, pwInput_in.text))
        {
            // 로그인 성공
            User_info info = SQLManager.instance.info;
            SQLManager.instance.isLogin = true;
            DefaultData(dbPath, info);
            // 처음 접속이 아닌 경우 CreateScene이 아니라 Start로 넘어감
            if (info.FirstConnect.Equals('F'))
            {
                if (info.Connecting.Equals('F'))
                {
                    SQLManager.instance.UpdateUserInfo("connecting", 'T', info.User_Id);
                    ServerChecker.instance.StartClient();
                }
                else
                { // 접속 중인 경우
                    checkButton.SetActive(true);
                    log.text = "다른 기기에서 접속 중입니다.";
                    return;
                }
            }
            else
            { // 처음 접속인 경우
                SceneManager.LoadScene("CreateScene");
            }
        }
        else
        {
            // 로그인 실패
            checkButton.SetActive(true);
            log.text = "아이디와 비밀번호를 확인해 주세요.";
            return;
        }
    }

    public void SignUpButton()
    { // 회원가입 완료 버튼
        if (idInput_up.text.Equals(string.Empty) || pwInput_up.text.Equals(string.Empty) || nickName_up.text.Equals(string.Empty))
        {
            checkButton.SetActive(true);
            log.text = "빈칸을 입력해 주세요.";
            return;
        }
        if (SQLManager.instance.SignUp(idInput_up.text, pwInput_up.text, nickName_up.text))
        {
            // 회원가입 성공
            signUpPanel.SetActive(false);
        }
        else
        {
            // 회원가입 실패
            checkButton.SetActive(true);
            log.text = "중복된 아이디가 있습니다.";
            return;
        }
    }

    public void SignOutButton()
    { // 로그아웃 버튼
        if (Application.platform == RuntimePlatform.Android)
        {
            dbPath = Application.persistentDataPath + "/Database";
        }
        else
        { // window
            dbPath = Application.dataPath + "/Database";
        }

        File.Delete(dbPath + "/UserInfo.json");
        isLogin = false;
    }

    public void StartButton()
    { // 이미 로그인 되어있는 경우
        if (isLogin)
        {
            string jsonString = File.ReadAllText(dbPath + "/UserInfo.json", Encoding.UTF8);
            JsonData ItemData = JsonMapper.ToObject(jsonString);
            if (SQLManager.instance.SignIn(ItemData[0]["ID"].ToString(), ItemData[0]["PW"].ToString()))
            {
                User_info info = SQLManager.instance.info;
                SQLManager.instance.isLogin = true;
                SQLManager.instance.UpdateUserInfo("connecting", 'T', info.User_Id);
                ServerChecker.instance.StartClient();
            }
        }
        else
        {
            signInPanel.SetActive(true);
        }
    }

    public void DeleteAccount()
    {
        if (idInput_delete.text.Equals(string.Empty) || pwInput_delete.text.Equals(string.Empty) || nickName_delete.text.Equals(string.Empty))
        {
            checkButton.SetActive(true);
            log.text = "빈칸을 입력해 주세요.";
            return;
        }

        if (SQLManager.instance.SignOut(idInput_delete.text, pwInput_delete.text))
        {
            // 회원탈퇴 성공
            deleteaccountPannel.SetActive(false);
        }
        else
        {
            // 회원탈퇴 실패
            checkButton.SetActive(true);
            log.text = "해당하는 아이디가 없습니다.";
            return;
        }
    }
}
