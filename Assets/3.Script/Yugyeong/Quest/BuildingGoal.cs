//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "Building_goal", menuName = "Questgoal_data", order = 1)]
//public class BuildingGoal : QuestGoal_YG
//{
//    public string building;

//    public override void Initalize()
//    {
//        base.Initalize();
//        EventManager.Instance.AddListener<BuildingGameEvent>(On_building);
//    }
//    private void On_building(BuildingGameEvent eventInfo)
//    {
//        if (eventInfo.building_name == building)
//        {
//            current_amount++;
//            Evaluate();
//        }
//    }    
//}
