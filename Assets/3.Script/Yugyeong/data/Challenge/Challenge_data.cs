using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Challenge_data
{
    public string player_id;
    public DateTime cur_time;
    public DateTime last_time;
    public int dailycount;
    public int trash;//쓰레기 얼마나 주웠는지
    public int box;//박스 얼마나 깠는지
    public int open_count
    {
        get
        {
            return Get_count("is_open");
        }
    }
    public int solved_count
    {
        get
        {
            return Get_count("is_solved");
        }
    }
    public int food_count
    {
        get
        {
            return Get_count("give_food");
        }
    }
    public Challenge_data(string playerid, DateTime curtime, DateTime lasttime, int daily_count,int Trash, int Box)
    {
        player_id = playerid;
        cur_time = curtime;
        last_time = lasttime;
        dailycount = daily_count;
        trash = Trash;
        box = Box;
    }

    public int Get_count(string type)
    {
        int count = 0;
        List<Nonplayer_data> tmp = SQLManager.instance.nonplayer_Datas;
        for (int i = 0; i < tmp.Count; i++)
        {
            switch (type)
            {
                case "is_open":
                    if (tmp[i].is_open == 'T')
                        count += 1;
                    break;
                case "is_solved":
                    if (tmp[i].give_food == 'T')
                        count += 1;
                    break;
                case "give_food":
                    if (tmp[i].give_food == 'T')
                        count += 1;
                    break;
                default:
                    Debug.Log($"매개변수 잘못 넣었음");
                    break;
            }
        }
        return count;
    }
}  
