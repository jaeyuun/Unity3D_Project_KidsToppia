using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{ // Cavase Audio Setting에 넣어주기
    public Slider bgmSlider, sfxSlider;

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
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }
}
