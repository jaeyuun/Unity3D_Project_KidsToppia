using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettingData
{
    public double bgmValue;
    public double sfxValue;
    public int qualitySet;
    public UserSettingData(double bgm, double sfx, int quality)
    {
        bgmValue = bgm;
        sfxValue = sfx;
        qualitySet = quality;
    }
}

public class SettingController : MonoBehaviour
{ // Cavase Audio Setting에 넣어주기
    public Slider bgmSlider, sfxSlider;
    private int quality = AudioManager.instance.qualitySet;

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
        AudioManager.instance.UpdateData(AudioManager.instance.settingPath, bgmSlider.value, sfxSlider.value, quality);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
        AudioManager.instance.UpdateData(AudioManager.instance.settingPath, bgmSlider.value, sfxSlider.value, quality);
    }

    public void LowQuality()
    {
        QualitySettings.SetQualityLevel(0);
        AudioManager.instance.UpdateData(AudioManager.instance.settingPath, bgmSlider.value, sfxSlider.value, 0);
    }

    public void MiddleQuality()
    {
        QualitySettings.SetQualityLevel(1);
        AudioManager.instance.UpdateData(AudioManager.instance.settingPath, bgmSlider.value, sfxSlider.value, 1);
    }

    public void HighQuality()
    {
        QualitySettings.SetQualityLevel(2);
        AudioManager.instance.UpdateData(AudioManager.instance.settingPath, bgmSlider.value, sfxSlider.value, 2);
    }
}
