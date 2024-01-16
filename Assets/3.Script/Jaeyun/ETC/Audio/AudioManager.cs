using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public Sound[] bgm, sfx;
    public AudioSource bgmSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayBGM("Main");
    }

    public void PlayBGM(string name)
    {
        Sound sound = Array.Find(bgm, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            bgmSource.clip = sound.clip;
            bgmSource.Play();
        }
    }

    public void PlayerSFX(string name)
    {
        Sound sound = Array.Find(sfx, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void MuteBGM()
    {
        bgmSource.mute = !bgmSource.mute;
    }

    public void MuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void BGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
