using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public void SceneLoadButton(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
