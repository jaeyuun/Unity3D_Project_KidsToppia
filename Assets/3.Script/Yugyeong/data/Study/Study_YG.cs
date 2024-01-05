using UnityEngine;

[CreateAssetMenu(fileName = "Study_YG", menuName = "YG/Study_YG/study_yg", order = 0)]
public class Study_YG : ScriptableObject
{
    public string animal_name;
    public string table_name;
    public Sprite sprite;
    public Nonplayer_data data
    {
        set 
        {
        }
        get
        {
            return SQLManager.instance.Collection(SQLManager.instance.info.User_Id, table_name);
        }
    }
    public string info;
    public void set_data(string state, char val)
    {
        SQLManager.instance.Updatecollection(table_name, state, val);
        data = SQLManager.instance.Collection(SQLManager.instance.info.User_Id, table_name);
    }
}
