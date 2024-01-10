using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfo_Data", menuName = "NPCInfo_Data")]
public class NPCInfo_Data : ScriptableObject
{
    [Header("NPC Info Setting")]
    public string npcName;
    public string npcText;
    public string prompt; // gpt prompt

    // button setting
    [Header("TalkPanel Setting")]
    public bool micButton; // audio button
    public bool npcYesButton; // talk panel에 button 1 or 2, true = 2
    public string yesButtonText;
    public string noButtonText;

    [Header("Animal Info")] // 24.01.10 유경이와 animal info 대신 npc info에 animal 관련을 저장해두기로 상의완.
    public string animalName;
}
