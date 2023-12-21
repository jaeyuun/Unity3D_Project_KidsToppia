using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Text loadingText;

    // Fake Time
    private float loadingTime = 3f;
    private float currentTime = 0;

    private void Awake()
    {
        StartCoroutine(IntroLoading_Co());
    }

    private IEnumerator IntroLoading_Co()
    {
        loadingScreen.SetActive(true);
        while (currentTime < loadingTime)
        {
            if (Mathf.RoundToInt(currentTime / loadingTime * 100f) > 100) break;
            currentTime += Time.deltaTime;
            loadingText.text = $"{Mathf.RoundToInt(currentTime / loadingTime * 100f)}%";
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        loadingScreen.SetActive(false);
    }
}
