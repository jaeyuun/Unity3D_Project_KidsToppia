using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // TMP

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;

    public void IntroLoading(string scene)
    {
        StartCoroutine(IntroLoading_Co(scene));
    }

    private IEnumerator IntroLoading_Co(string scene)
    {
        loadingScreen.SetActive(true);
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(scene);
        while (!asyncOper.isDone)
        {
            yield return null;
            loadingText.text = $"{Mathf.RoundToInt(asyncOper.progress * 100f)}%"; // loading 진행 정도 백분율
        }
    }
}
