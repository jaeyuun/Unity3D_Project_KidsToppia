using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettingData
{
    public int bgmValue;
    public int sfxValue;
    public int qualitySet;
    public UserSettingData(int bgm, int sfx, int quality)
    {
        bgmValue = bgm;
        sfxValue = sfx;
        qualitySet = quality;
    }
}

public class SettingController : MonoBehaviour
{ // Cavase Audio Setting에 넣어주기
    public Slider bgmSlider, sfxSlider;
    public int quality;

    private void Start()
    {
        StartSetting();
    }

    private void StartSetting()
    {
        // bgm, sfx setting
        if (AudioManager.instance == null) return;
        bgmSlider.value = AudioManager.instance.bgmValue;
        sfxSlider.value = AudioManager.instance.sfxValue;
        quality = AudioManager.instance.qualitySet;

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
        AudioManager.instance.UpdateData((int)bgmSlider.value, (int)sfxSlider.value, quality);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
        AudioManager.instance.UpdateData((int)bgmSlider.value, (int)sfxSlider.value, quality);
    }

    public void LowQuality()
    {
        QualitySettings.SetQualityLevel(0);
        quality = 0;
        AudioManager.instance.UpdateData((int)bgmSlider.value, (int)sfxSlider.value, quality);
    }

    public void MiddleQuality()
    {
        QualitySettings.SetQualityLevel(1);
        quality = 1;
        AudioManager.instance.UpdateData((int)bgmSlider.value, (int)sfxSlider.value, quality);
    }

    public void HighQuality()
    {
        QualitySettings.SetQualityLevel(2);
        quality = 2;
        AudioManager.instance.UpdateData((int)bgmSlider.value, (int)sfxSlider.value, quality);
    }
}
