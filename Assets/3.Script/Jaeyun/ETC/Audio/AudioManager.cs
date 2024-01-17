using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.IO;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public Sound[] bgm, sfx;
    public AudioSource bgmSource, sfxSource;

    private UserSettingData userSetting;
    public string settingPath = string.Empty;

    public float bgmValue = 1;
    public float sfxValue = 1;
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

    private void UserSet()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (settingPath.Equals(string.Empty))
            {
                settingPath = Application.persistentDataPath + "/Database";
            }
            if (!File.Exists(settingPath)) // 해당 경로에 파일이 없다면
            { // folder 검사
                Directory.CreateDirectory(settingPath); // Directory 생성
            }
            if (!File.Exists(settingPath + "/UserSetting.json"))
            { // file 검사
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
            { // folder 검사
                Directory.CreateDirectory(settingPath); // Directory 생성
            }
            if (!File.Exists(settingPath + "/UserSetting.json"))
            { // file 검사
                DefaultData(settingPath);
            }
        }

        string jsonString = File.ReadAllText(settingPath + "/UserSetting.json"); // json file을 string으로 받아옴
        JsonData ItemData = JsonMapper.ToObject(jsonString); // string 형태를 json 형태로 바꿔줌

        Debug.Log(float.Parse(ItemData[0]["bgmValue"].ToString()));
        bgmValue = float.Parse(ItemData[0]["bgmValue"].ToString());
        sfxValue = float.Parse(ItemData[0]["sfxValue"].ToString());
        qualitySet = ItemData[0]["qualitySet"].ToString()[0] - '0';

        BGMVolume(bgmValue);
        SFXVolume(sfxValue);
        QualitySettings.SetQualityLevel(qualitySet);
    }

    private void DefaultData(string path)
    {
        List<UserSettingData> items = new List<UserSettingData>();
        items.Add(new UserSettingData(1, 1, 0)); // bgm, sfx, qaulity
        JsonData data = JsonMapper.ToJson(items);
        File.WriteAllText(path + "/UserSetting.json", data.ToString());
    }

    public void UpdateData(string path, double bgm, double sfx, int quality)
    {
        List<UserSettingData> items = new List<UserSettingData>();
        items.Add(new UserSettingData(bgm, sfx, quality)); // bgm, sfx, qaulity
        JsonData data = JsonMapper.ToJson(items);
        File.WriteAllText(path + "/UserSetting.json", data.ToString());
    }
}
