using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* SQL namespace */
using MySql.Data;
using MySql.Data.MySqlClient;
using LitJson;
using System;
using System.IO;

public class User_info
{ // Database table info
    public string User_Id { get; private set; }
    public string User_Pw { get; private set; }
    public string User_Nickname { get; set; }

    public User_info(string userId, string password, string userName)
    { // 생성자 호출될 때만 설정됨
        User_Id = userId;
        User_Pw = password;
        User_Nickname = userName;
    }
}

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
    public User_info info;

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

    private bool ConnectionCheck(MySqlConnection con)
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

    public bool Login(string id, string password, string nick)
    {
        // 직접적으로 DB에서 데이터를 가지고 오는 메소드
        // 조회되는 데이터가 없다면 false, 조회가 되는 데이터가 있다면 true
        // 위에서 선언한 info에 담은 다음에 리턴함
        /*
            Data 갖고오기
            1. Connection Open인지 확인
            2. Reader 상태가 읽고 있는 상황인지 확인(1quary 1reader)
            3. Data를 다 읽었으면 Reader의 상태를 확인 후 Close
         */
        try
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT id, pw, nickname FROM user_info WHERE id = '{0}' AND pw = '{1}' AND nickname = '{2}';", id, password, nick); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
            {
                // 읽은 데이터를 하나씩 나열
                while (reader.Read())
                {
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["id"].ToString();
                    string pw = (reader.IsDBNull(0)) ? string.Empty : (string)reader["pw"].ToString();
                    string nickName = (reader.IsDBNull(0)) ? string.Empty : (string)reader["nickname"].ToString();
                    if (!name.Equals(string.Empty) || !pw.Equals(string.Empty))
                    { // 정상적으로 Data를 불러온 상황
                        info = new User_info(name, pw, nickName);
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                        return true;
                    }
                    else
                    { // 로그인 실패
                        break;
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return false;
        }
    }
}