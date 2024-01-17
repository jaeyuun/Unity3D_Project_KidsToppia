using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserSettingData
{
    public string bgmValue;
    public string sfxValue;
    public int qualitySet;
    public UserSettingData(string bgm, string sfx, int quality)
    {
        bgmValue = bgm;
        sfxValue = sfx;
        qualitySet = quality;
    }
}

public class SettingController : MonoBehaviour
{ // Cavase Audio Setting¿¡ ³Ö¾îÁÖ±â
    public Slider bgmSlider, sfxSlider;
    public AudioSource ttsSource;
    public int quality;
    public TMP_Text userNameText;

    private void Start()
    {
        StartSetting();
    }

    private void StartSetting()
    {
        // bgm, sfx setting
        if (AudioManager.instance == null) return;
        if (SQLManager.instance.info == null) return;
        bgmSlider.value = AudioManager.instance.bgmValue;
        sfxSlider.value = AudioManager.instance.sfxValue;
        ttsSource.volume = sfxSlider.value;
        quality = AudioManager.instance.qualitySet;
        userNameText.text = SQLManager.instance.info.User_NickName;

        // quality setting
        switch (quality)
        {
            case 0:
                LowQuality();
                break;
            case 1:
                MiddleQuality();
                break;
            case 2:
                HighQuality();
                break;
            default:
                LowQuality();
                break;
        }
    }

    public void MuteBGM()
    {
        AudioManager.instance.MuteBGM();
    }

    public void MuteSFX()
    {
        AudioManager.instance.MuteSFX();
    }

    public void BGMVolume()
    {
        AudioManager.instance.BGMVolume(bgmSlider.value);
        AudioManager.instance.UpdateData(bgmSlider.value.ToString(), sfxSlider.value.ToString(), quality);
    }

    public void SFXVolume()
    {
        ttsSource.volume = sfxSlider.value;
        AudioManager.instance.SFXVolume(sfxSlider.value);
        AudioManager.instance.UpdateData(bgmSlider.value.ToString(), sfxSlider.value.ToString(), quality);
    }

    public void LowQuality()
    {
        QualitySettings.SetQualityLevel(0);
        quality = 0;
        AudioManager.instance.UpdateData(bgmSlider.value.ToString(), sfxSlider.value.ToString(), quality);
    }

    public void MiddleQuality()
    {
        QualitySettings.SetQualityLevel(1);
        quality = 1;
        AudioManager.instance.UpdateData(bgmSlider.value.ToString(), sfxSlider.value.ToString(), quality);
    }

    public void HighQuality()
    {
        QualitySettings.SetQualityLevel(2);
        quality = 2;
        AudioManager.instance.UpdateData(bgmSlider.value.ToString(), sfxSlider.value.ToString(), quality);
    }

    public void SignOut()
    { // È¸¿øÅ»Åð method
        SQLManager.instance.SignOut(SQLManager.instance.info.User_Id);
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        { // widow
            Application.Quit();
        }
    }
}
