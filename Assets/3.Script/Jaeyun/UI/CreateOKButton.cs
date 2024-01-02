using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOKButton : MonoBehaviour
{
    public void OKButtonClick()
    {
        User_info info = SQLManager.instance.info;
        SQLManager.instance.UpdateUserInfo("firstConnect", 'F', info.User_Id);
        SQLManager.instance.UpdateUserInfo("connecting", 'T', info.User_Id);
        ServerChecker.instance.StartClient();
    }
}
