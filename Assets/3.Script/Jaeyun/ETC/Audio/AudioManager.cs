using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.IO;
using System.Globalization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public Sound[] bgm, sfx;
    public AudioSource bgmSource, sfxSource;
    public UserSettingData userSetting;
    public string settingPath = string.Empty;

    public int bgmValue = 100;
    public int sfxValue = 100;
    public int qualitySet = 0;

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

        UserSet();
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

    public void PlaySFX(string name)
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

    private void UserSet()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (settingPath.Equals(string.Empty))
            {
                settingPath = Application.persistentDataPath + "/Database";
            }
            if (!File.Exists(settingPath)) // 해당 경로에 파일이 없다면
            {
                Directory.CreateDirectory(settingPath); // Directory 생성
            }
            if (!File.Exists(settingPath + "/UserSetting.json"))
            {
                DefaultData(settingPath);
            }
        }
        else
        { // window
            if (settingPath.Equals(string.Empty))
            {
                settingPath = Application.dataPath + "/Database";
            }
            if (!File.Exists(settingPath)) // 해당 경로에 파일이 없다면
            {
                Directory.CreateDirectory(settingPath); // Directory 생성
            }
            if (!File.Exists(settingPath + "/UserSetting.json"))
            {
                DefaultData(settingPath);
            }
        }

        string jsonString = File.ReadAllText(settingPath + "/UserSetting.json"); // json file을 string으로 받아옴
        JsonData jsonData = JsonMapper.ToObject(jsonString); // string 형태를 json 형태로 바꿔줌
        float bVal = float.Parse(jsonData[0]["bgmValue"].ToString(), CultureInfo.InvariantCulture);
        float sVal = float.Parse(jsonData[0]["sfxValue"].ToString(), CultureInfo.InvariantCulture);
        
        bgmValue = (int)bVal;
        sfxValue = (int)sVal;
        qualitySet = int.Parse(jsonData[0]["qualitySet"].ToString(), CultureInfo.InvariantCulture);

        // intro setting
        BGMVolume(bgmValue);
        SFXVolume(sfxValue);
        QualitySettings.SetQualityLevel(qualitySet);
    }

    private void DefaultData(string path)
    {
        List<UserSettingData> items = new List<UserSettingData>();
        items.Add(new UserSettingData(100, 100, 0)); // bgm, sfx, qaulity
        JsonData data = JsonMapper.ToJson(items);
        File.WriteAllText(path + "/UserSetting.json", data.ToString());
    }

    public void UpdateData(int bgm, int sfx, int quality)
    {
        List<UserSettingData> items = new List<UserSettingData>();
        items.Add(new UserSettingData(bgm, sfx, quality)); // bgm, sfx, qaulity
        JsonData data = JsonMapper.ToJson(items);
        File.WriteAllText(settingPath + "/UserSetting.json", data.ToString());
    }
}
