using UnityEngine;

[CreateAssetMenu(fileName = "Study_YG", menuName = "YG/Study_YG/study_yg", order = 0)]
public class Study_YG : ScriptableObject
{
    [SerializeField] public string animal_name;
    [SerializeField] public string table_name;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Nonplayer_data data;
    [SerializeField] public string info;

}
