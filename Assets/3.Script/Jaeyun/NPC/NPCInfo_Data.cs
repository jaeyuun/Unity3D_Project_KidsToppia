using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfo_Data", menuName = "NPCInfo_Data")]
public class NPCInfo_Data : ScriptableObject
{
    public string npcName;
    public string npcText;
    public string prompt; // gpt prompt
}
