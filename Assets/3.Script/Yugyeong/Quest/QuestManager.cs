using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject quest_prefab;
    [SerializeField] private Transform quest_content;
    [SerializeField] private GameObject quest_holder;

    public List<Quest_YG> current_quests;

    private void Awake()
    {
        foreach (var quest in current_quests)
        {
            quest.Initalize();
            quest.quest_completed.AddListener(On_quest_completed);

            GameObject quest_obj = Instantiate(quest_prefab, quest_content);
            //quest_obj.transform.Find("Icon").GetComponent<Image>().sprite = quest.information.icon;

            quest_obj.GetComponent<Button>().onClick.AddListener(delegate {
                quest_holder.GetComponent<QuestWindow>().Initialize(quest);
                quest_holder.SetActive(true); 
            });
        }
    }

    public void Build(string building_name)
    {
        EventManager.Instance.QueueEvent(new BuildingGameEvent(building_name));
    }

    private void On_quest_completed(Quest_YG quest)
    {
        quest_content.GetChild(current_quests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}
