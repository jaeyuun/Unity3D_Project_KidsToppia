using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginChecker : MonoBehaviour
{
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private GameObject checkButton;
    [SerializeField] private TMP_Text log;

    [Header("Sign In")]
    public InputField idInput_in;
    public InputField pwInput_in;
    [Header("Sign Up")]
    public InputField idInput_up;
    public InputField pwInput_up;
    public InputField nickName_up;

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
            Debug.Log(info.User_Id + " | " + info.User_Pw);
            // 처음 접속이 아닌 경우 CreateScene이 아니라 Start로 넘어감
            if (info.FirstConnect.Equals('T'))
            {
                info.Connecting = 'T';
                SQLManager.instance.UpdateUserInfo("firstConnect", 'T', info.User_Id);
                ServerChecker.instance.StartClient();
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
}
