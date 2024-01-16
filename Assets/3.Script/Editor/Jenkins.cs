// Assets/Editor/Jenkins.cs
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Jenkins
{
    [MenuItem("Build/Standalone Windows")]
    public static void PerformBuild_Windows()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        // 씬 추가
        List<string> scenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            scenes.Add(scene.path);
        }
        options.scenes = scenes.ToArray();
        // 타겟 경로(빌드 결과물이 여기 생성됨)
        options.locationPathName = "Build/BuildTesting.exe";
        // 빌드 타겟
        options.target = BuildTarget.StandaloneWindows;

        // 빌드
        BuildPipeline.BuildPlayer(options);
    }
    [MenuItem("Build/Android")]
    public static void PerformBuild_Android()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        // 씬 추가
        List<string> scenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            scenes.Add(scene.path);
        }
        options.scenes = scenes.ToArray();
        // 타겟 경로(빌드 결과물이 여기 생성됨)
        options.locationPathName = "Build/BuildTesting.apk";
        // 빌드 타겟
        options.target = BuildTarget.Android;

        //Publishing Settings
        PlayerSettings.keyaliasPass = GetArg("-keyaliasPass");
        PlayerSettings.keystorePass = GetArg("-keystorePass");

        Debug.LogFormat("**** keyaliasPass : {0}", PlayerSettings.keyaliasPass);
        Debug.LogFormat("**** keystorePass : {0}", PlayerSettings.keystorePass);

        // 빌드
        BuildPipeline.BuildPlayer(options);
    }

    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
