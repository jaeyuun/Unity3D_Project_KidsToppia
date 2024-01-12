using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [Header("Sign In")]
    public InputField idInput_in;
    public InputField pwInput_in;
    public Study_YG[] study_YGs = new Study_YG[7];

    public void scene_btn()
    {
        SceneManager.LoadScene("NPCScene_YG");
    }

    public void TestSignInButton()
    { // 로그인 버튼
        if (SQLManager.instance.SignIn(idInput_in.text, pwInput_in.text))
        {
            // 로그인 성공
            User_info info = SQLManager.instance.info;
            for (int i = 0; i < study_YGs.Length; i++)
            {
                Set_testdata(study_YGs[i]);
            }
            //Nonplayer_data nonData = SQLManager.instance.Collection(info.User_Id, "yellow_sheep");
            SQLManager.instance.isLogin = true;
            Debug.Log(info.User_Id + " | " + info.User_Pw);
            SceneManager.LoadScene("NPCScene_YG");
        }
    }
    public void TestSignInButton2()
    { // 로그인 버튼
        if (SQLManager.instance.SignIn(idInput_in.text, pwInput_in.text))
        {
            // 로그인 성공
            User_info info = SQLManager.instance.info;
            for (int i = 0; i < study_YGs.Length; i++)
            {
                Set_testdata(study_YGs[i]);
            }
            //Nonplayer_data nonData = SQLManager.instance.Collection(info.User_Id, "yellow_sheep");
            SQLManager.instance.isLogin = true;
            Debug.Log(info.User_Id + " | " + info.User_Pw);
            SceneManager.LoadScene("MainGame_J 1");
        }
    }

    public void Set_testdata(Study_YG study)
    {
        Nonplayer_data data = SQLManager.instance.Collection(SQLManager.instance.info.User_Id, study.table_name);
    }
}
