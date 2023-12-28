using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* SQL namespace */
using MySql.Data;
using MySql.Data.MySqlClient;
using LitJson;
using System;
using System.IO;


public class ConfigItem
{ // Database config.json
    public string IP;
    public string TableName;
    public string ID;
    public string PW;
    public string PORT;

    public ConfigItem(string ipValue, string tableName, string userId, string userPw, string port)
    {
        IP = ipValue;
        this.TableName = tableName;
        this.ID = userId;
        this.PW = userPw;
        this.PORT = port;
    }
}

public class SQLManager : MonoBehaviour
{
    public MySqlConnection connection;
    public MySqlDataReader reader;

    private string dbPath = string.Empty;

    public static SQLManager instance = null;

    [Header("Input My IP")]
    public string serverIp = string.Empty;
    public string tableName = string.Empty;

    public bool isLogin = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Application.dataPath는 Asset 폴더까지를 뜻함

        string serverInfo = ServerSet();
        try
        {
            if (serverInfo.Equals(string.Empty))
            {
                Debug.Log("SQL Server Json Error");
                return;
            }
            connection = new MySqlConnection(serverInfo); // Connection create
            connection.Open(); // Connection open
            Debug.Log("SQL Server Open Complete");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string ServerSet()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (dbPath.Equals(string.Empty))
            {
                dbPath = Application.persistentDataPath + "/Database"; // 경로를 string에 저장
            }
            if (!File.Exists(dbPath)) // 해당 경로에 파일이 없다면
            { // folder 검사
                Directory.CreateDirectory(dbPath); // Directory 생성
            }
            if (!File.Exists(dbPath + "/config.json"))
            { // file 검사
                DefaultData(dbPath);
            }
        }
        else
        { // window
            if (dbPath.Equals(string.Empty))
            {
                dbPath = Application.dataPath + "/Database"; // 경로를 string에 저장
            }
            if (!File.Exists(dbPath)) // 해당 경로에 파일이 없다면
            { // folder 검사
                Directory.CreateDirectory(dbPath); // Directory 생성
            }
            if (!File.Exists(dbPath + "/config.json"))
            { // file 검사
                DefaultData(dbPath);
            }
        }

        string jsonString = File.ReadAllText(dbPath + "/config.json"); // json file을 string으로 받아옴
        JsonData ItemData = JsonMapper.ToObject(jsonString); // string 형태를 json 형태로 바꿔줌

        if (serverIp == string.Empty)
        {
            serverIp = $"{ItemData[0]["IP"]}";
        }
        if (tableName == string.Empty)
        {
            tableName = $"{ItemData[0]["TableName"]}";
        }

        string serverInfo = $"Server = {serverIp}; Database = {tableName}; Uid = {ItemData[0]["ID"]}; Pwd = {ItemData[0]["PW"]}; Port = {ItemData[0]["PORT"]}; CharSet = utf8;";
        return serverInfo;
    }

    private void DefaultData(string path)
    {
        List<ConfigItem> items = new List<ConfigItem>();
        items.Add(new ConfigItem("13.124.181.154", "kidstopia_player", "root", "1234", "3306")); // serverIP, tableName, id, pw, port
        JsonData data = JsonMapper.ToJson(items); // 반드시 자료구조로 넣어야 함
        File.WriteAllText(path + "/config.json", data.ToString());
    }

    public bool ConnectionCheck(MySqlConnection con)
    {
        // 현재 MySqlConnection이 Open 상태가 아니라면
        if (con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }
}