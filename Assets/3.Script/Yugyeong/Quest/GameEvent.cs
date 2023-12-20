using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public string event_description;
}

public class BuildingGameEvent : GameEvent
{
    public string building_name;
    public BuildingGameEvent(string name)
    {
        building_name = name;
    }
}
