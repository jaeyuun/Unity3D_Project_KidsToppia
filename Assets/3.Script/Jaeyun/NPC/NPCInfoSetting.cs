using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfo")]
public class NPCInfo_Data : ScriptableObject
{
    public string npcName;
    public string prompt;
}

public class NPCInfoSetting : MonoBehaviour
{
    public NPCInfo_Data info;
}
