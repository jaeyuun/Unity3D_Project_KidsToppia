using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Challenge_data : MonoBehaviour
{
    public string player_id;
    public DateTime cur_time;
    public DateTime last_time;
    public int dailycount;
    public int trash;
    public int box;
   
    public Challenge_data(string playerid, DateTime curtime, DateTime lasttime, int daily_count,int Trash, int Box)
    {
        player_id = playerid;
        cur_time = curtime;
        last_time = lasttime;
        dailycount = daily_count;
        trash = Trash;
        box = Box;
    }
}  

public class Challenge_YG : ScriptableObject //도전과제 내용
{
    [SerializeField] private string info;
    [SerializeField] private int clear_count;
    [SerializeField] private int cur_count;
    [SerializeField] private int reward_count;
}
