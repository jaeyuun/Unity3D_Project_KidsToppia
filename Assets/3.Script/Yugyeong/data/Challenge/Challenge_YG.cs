using UnityEngine;

[CreateAssetMenu(fileName = "Challenge_YG", menuName = "YG/Challenge_YG/Challenge_yg", order = 0)]
public class Challenge_YG : ScriptableObject //도전과제 내용
{
    public string info;
    public string need;
    public int clear_count;
    public int cur_count
    {
        get
        {
            return Get_curcount(need);
        }
    }
    public int reward_count;

    private int Get_curcount(string type)
    {
        var SQL = SQLManager.instance;
        switch (type)
        {
            case "dailycount":
                return SQL.Challenge().dailycount;
            case "trash":
                return SQL.Challenge().trash;
            case "box":
                return SQL.Challenge().box;
            case "open_count":
                return SQL.Challenge().open_count;
            case "solved_count":
                return SQL.Challenge().solved_count;
            case "food_count":
                return SQL.Challenge().food_count;
            default:
                Debug.Log("오타났다 유경아 정신차리렴");
                return 0;
        }
    }

}
