using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordCheck : MonoBehaviour
{
    public InputField pwInputField;
    private void Update()
    {
        if (pwInputField.isFocused)
        { // pw inputfield에 포커스 되어있을 때
            Input.imeCompositionMode = IMECompositionMode.Off; // 한글 비활성화
        }
        else
        {
            Input.imeCompositionMode = IMECompositionMode.Auto;
        }
    }
}
