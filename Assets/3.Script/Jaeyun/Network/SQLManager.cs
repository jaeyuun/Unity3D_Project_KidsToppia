using LitJson;
/* SQL namespace */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class User_info
{ // Database table info
    public string User_Id { get; private set; }
    public string User_Pw { get; private set; }
    public string User_NickName { get; set; }
    public char FirstConnect { get; set; }
    public char Connecting { get; set; }

    public User_info(string userId, string password, string nickname, char firstConnect, char conn)
    {
        User_Id = userId;
        User_Pw = password;
        User_NickName = nickname;
        FirstConnect = firstConnect;
        Connecting = conn;
    }
}

public class User_Character
{
    public string User_Id { get; private set; }
    public int User_Eyes { get; set; }
    public int User_Jummper { get; set; }
    public int User_Runners { get; set; }
    public int User_TShirt { get; set; }
    public int User_Trunk { get; set; }
    public int User_Skin { get; set; }
    public int User_Hat { get; set; }
    public int User_HairStyle { get; set; }
    public int User_Ride { get; set; }
    public char User_Riding { get; set; }
    public User_Character(string userId, int eyes, int jummper, int runners, int tshirt, int trunk, int skin, int hat, int hair, int ride, char riding)
    {
        User_Id = userId;
        User_Eyes = eyes;
        User_Jummper = jummper;
        User_Runners = runners;
        User_TShirt = tshirt;
        User_Trunk = trunk;
        User_Skin = skin;
        User_Hat = hat;
        User_HairStyle = hair;
        User_Ride = ride;
        User_Riding = riding;
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
    public static SQLManager instance = null;

    public User_info info;
    public User_Character character_info;

    public item_data item;
    public Challenge_data challenge_data;
    public Shop_data shopdata;
    public Friend_data friendsdata;

    public Nonplayer_data black_goat;
    public Nonplayer_data black_sheep;
    public Nonplayer_data chicken;
    public Nonplayer_data whale;
    public Nonplayer_data white_goat;
    public Nonplayer_data white_sheep;
    public Nonplayer_data yellow_sheep;
    public Nonplayer_data none;
    public List<Nonplayer_data> nonplayer_Datas = new List<Nonplayer_data>();

    public MySqlConnection connection;
    public MySqlDataReader reader;

    private string dbPath = string.Empty;

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
    #region SQL Server Connect
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
    #endregion
    #region Player Info Alter (UPDATE)
    public void UpdateUserInfo(string column, char content, string userId)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE user_info SET {0} = '{1}' WHERE id = '{2}';", column, content, userId);
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void UpdateData(string selectMenu, int select)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE user_character_info SET {0} = '{1}' WHERE user_id = '{2}';", selectMenu, select, info.User_Id);
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void UpdateRiding(char riding, int select)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE user_character_info SET Ride = {0}, Riding = {1} WHERE user_id = '{2}';", select, riding, info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void Updateitem(string name, int num)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE item SET {0} = {1} WHERE player_id = '{2}';", name, num, info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void Updatecollection(string col_name, string state, char val) //col_name = 열 이름, state = is_open,givefood,issolved, val = "T" or "F"
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }

            Nonplayer_data tmpdata = Collection(info.User_Id, col_name);
            string selectCommand = string.Empty;

            if (state == "is_open")
            {
                selectCommand = string.Format(@"UPDATE animal SET {0} = json_object(
                                                  'is_open', '{1}',
                                                  'givefood', '{3}',
                                                  'issolved', '{4}')
                                                   WHERE  `player_id`='{2}';",
                                                   col_name, val, info.User_Id, tmpdata.give_food, tmpdata.is_solved); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            }
            else if (state == "givefood")
            {
                selectCommand = string.Format(@"UPDATE animal SET {0} = json_object(
                                                  'is_open', '{3}',
                                                  'givefood', '{1}',
                                                  'issolved', '{4}')
                                                   WHERE  `player_id`='{2}';",
                                                   col_name, val, info.User_Id, tmpdata.is_open, tmpdata.is_solved);
            }
            else
            {
                selectCommand = string.Format(@"UPDATE animal SET {0} = json_object(
                                                  'is_open', '{3}',
                                                  'givefood', '{4}',
                                                  'issolved', '{1}')
                                                   WHERE  `player_id`='{2}';",
                                                   col_name, val, info.User_Id, tmpdata.is_open, tmpdata.give_food);
            }

            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void Updatechal_boxtrash(string name, int num)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE item SET {0} = {1} WHERE player_id = '{2}';", name, num, info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void Updatechal_curtime()
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }
            string selectCommand = string.Format(@"UPDATE challenge SET cur_time = now() WHERE player_id = '{0}';", info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }

    public void Updatechal_dailycount(DateTime cur, DateTime last, int dailycount)
    {
        int dayrange = (last.Date - cur.Date).Days;
        int tmp = 0;

        if (dayrange == 1)
        {
            tmp = dailycount + 1;
        }
        else if (dayrange >= 2)
        {
            tmp = 0;
        }
        else
        {
            tmp = dailycount;
        }

        if ((last.Date - cur.Date).Days == 1)
        {
            try
            {
                if (!ConnectionCheck(connection))
                {
                    return;
                }
                string selectCommand = string.Format(@"UPDATE challenge SET dailycount = {1} WHERE player_id = '{0}';", info.User_Id, tmp); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (!reader.IsClosed)
                {
                    reader.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                if (!reader.IsClosed)
                {
                    reader.Close();
                    return;
                }
            }
        }
    }

    public void Updateshop(Shopname shopname, int index, char val) //col_name = 열 이름, state = is_open,givefood,issolved, val = "T" or "F"
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }

            Shop_data tmpdata = Shop();
            char tmp1 = 'a';
            char tmp2 = 'a';
            string tmpstr = null;
            string col1 = null;
            string col2 = null;
            switch (shopname)
            {
                case Shopname.hair:
                    tmp1 = tmpdata.hair1;
                    tmp2 = tmpdata.hair2;
                    tmpstr = "hair_shop";
                    col1 = "hair1";
                    col2 = "hair2";
                    break;
                case Shopname.riding:
                    tmp1 = tmpdata.riding1;
                    tmp2 = tmpdata.riding2;
                    tmpstr = "riding_shop";
                    col1 = "riding1";
                    col2 = "riding2";
                    break;
                case Shopname.clothes:
                    tmp1 = tmpdata.clothes1;
                    tmp2 = tmpdata.clothes2;
                    tmpstr = "clothes_shop";
                    col1 = "clothes1";
                    col2 = "clothes2";
                    break;
                case Shopname.acc:
                    tmp1 = tmpdata.acc1;
                    tmp2 = tmpdata.acc2;
                    tmpstr = "acc_shop";
                    col1 = "acc1";
                    col2 = "acc2";
                    break;
                default:
                    break;
            }
            _ = index == 0 ? tmp1 = val : tmp2 = val;

            string selectCommand = string.Format(@"UPDATE shop SET {0} = json_object(
                                                  '{1}', '{2}',
                                                  '{3}', '{4}')
                                                   WHERE  `player_id`='{5}';",
                                                   tmpstr, col1, tmp1, col2, tmp2, info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미

            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }
    public void UpdateFriend(User_info info, string friend_nickname)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }

            Friend_data tmp = Friend(info.User_Id);
            string str = string.Empty;

            if (tmp.friend1 == string.Empty)
            {
                str = "friend1";
            }
            else if (tmp.friend2 == string.Empty)
            {
                str = "friend2";
            }
            else if (tmp.friend3 == string.Empty)
            {
                str = "friend3";
            }
            else
            {
                Debug.Log("친구창이 가득 찼어요!");
                return;
            }

            string selectCommand = string.Format(@"UPDATE friend SET {0} = '{1}' WHERE player_id = '{2}';",str, friend_nickname, info.User_Id);
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }
    public void DeleteFriend(int index)
    {
        try
        {
            if (!ConnectionCheck(connection))
            {
                return;
            }

            Friend_data tmp = Friend(SQLManager.instance.info.User_Id);
            string str = string.Empty;

            switch (index)
            {
                case 0:
                    str = "friend1";
                    break;
                case 1:
                    str = "friend2";
                    break;
                case 2:
                    str = "friend3";
                    break;
                default:
                    Debug.Log("무언가 이상하다");
                    break;
            }

            string selectCommand = string.Format(@"UPDATE friend SET {0} = '' WHERE player_id = '{1}';", str,info.User_Id);
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
                return;
            }
        }
    }
    #endregion
    #region Table Search (SELECT)
    public User_info PlayerInfo(string user_id)
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN user_character_info B ON A.id = B.user_id WHERE A.id = '{0}';", user_id);
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        // user_info
                        string id = (reader.IsDBNull(0)) ? string.Empty : reader["id"].ToString();
                        string pw = (reader.IsDBNull(0)) ? string.Empty : reader["pw"].ToString();
                        string nickName = reader["nickname"].ToString();
                        char firstConnect = reader["firstConnect"].ToString()[0];
                        char connecting = reader["connecting"].ToString()[0];
                        if (!id.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            User_info user_Info = new User_info(id, pw, nickName, firstConnect, connecting);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return user_Info;
                        }
                        else
                        { // 로그인 실패
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }
    public User_Character CharacterInfo(string user_id)
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN user_character_info B ON A.id = B.user_id WHERE A.id = '{0}';", user_id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        string id_c = reader["user_id"].ToString();
                        int eyes = reader["Eyes"].ToString()[0] - '0';
                        int jummper = reader["Jummper"].ToString()[0] - '0';
                        int runners = reader["Runners"].ToString()[0] - '0';
                        int tshirt = reader["TShirts"].ToString()[0] - '0';
                        int trunk = reader["Trunks"].ToString()[0] - '0';
                        int skin = reader["Skin"].ToString()[0] - '0';
                        int hat = reader["Hat"].ToString()[0] - '0';
                        int ride = reader["Riding"].ToString()[0] - '0';
                        int hair = reader["HairStyle"].ToString()[0] - '0';
                        char riding = reader["Ride"].ToString()[0];
                        if (!id_c.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            User_Character character_info = new User_Character(id_c, eyes, jummper, runners, tshirt, trunk, skin, hat, hair, ride, riding);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return character_info;
                        }
                        else
                        { // 로그인 실패
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }
    public item_data Item()
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN item B ON A.id = B.player_id WHERE A.id = '{0}';", info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        string player_id = reader["player_id"].ToString();
                        int key_num = reader["key_num"].ToString()[0] - '0';
                        int have_fisingrod = reader["have_fisingrod"].ToString()[0] - '0';
                        int food_num = reader["food_num"].ToString()[0] - '0';
                        int money = int.Parse(reader["money"].ToString());
                        if (!player_id.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            item_data itemdata = new item_data(player_id, key_num, have_fisingrod, food_num, money);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return itemdata;
                        }
                        else
                        { // 로그인 실패
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }
    public Nonplayer_data Collection(string user_id, string col_name)
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN animal B ON A.id = B.player_id WHERE A.id = '{0}';", user_id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        string jsonData = reader[$"{col_name}"].ToString();
                        JsonData G = JsonMapper.ToObject(jsonData);
                        string player_id = reader["player_id"].ToString();
                        char is_open = G["is_open"].ToString()[0];
                        char givefood = G["givefood"].ToString()[0];
                        char issolved = G["issolved"].ToString()[0];
                        if (givefood == 'T' || issolved == 'T' && is_open == 'F')
                        {
                            Updatecollection(col_name, "is_open", 'T');
                            is_open = 'T';
                        }

                        if (!player_id.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return new Nonplayer_data(player_id, is_open, givefood, issolved);
                        }
                        else
                        { // 로그인 실패
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }
    public Challenge_data Challenge()
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN challenge B ON A.id = B.player_id WHERE A.id = '{0}';", info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        string player_id = reader["player_id"].ToString();
                        DateTime cur_time = (DateTime)reader["cur_time"];
                        DateTime last_time = (DateTime)reader["cur_time"];
                        int dailycount = reader["dailycount"].ToString()[0] - '0';
                        int trash = reader["trash"].ToString()[0] - '0';
                        int box = reader["box"].ToString()[0] - '0';
                        if (!player_id.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            Challenge_data challenge_Data = new Challenge_data(player_id, cur_time, last_time, dailycount, trash, box);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return challenge_Data;
                        }
                        else
                        { // 로그인 실패
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }
    public Shop_data Shop()
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN shop B ON A.id = B.player_id WHERE A.id = '{0}';", info.User_Id); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        // shop
                        string jsonDData = reader["riding_shop"].ToString();
                        JsonData L = JsonMapper.ToObject(jsonDData);
                        string player_id = reader["player_id"].ToString();
                        char riding1 = L["riding1"].ToString()[0];
                        char riding2 = L["riding2"].ToString()[0];

                        jsonDData = reader["clothes_shop"].ToString();
                        JsonData M = JsonMapper.ToObject(jsonDData);
                        char clothes1 = M["clothes1"].ToString()[0];
                        char clothes2 = M["clothes2"].ToString()[0];

                        jsonDData = reader["hair_shop"].ToString();
                        JsonData N = JsonMapper.ToObject(jsonDData);
                        char hair1 = N["hair1"].ToString()[0];
                        char hair2 = N["hair2"].ToString()[0];

                        jsonDData = reader["acc_shop"].ToString();
                        JsonData O = JsonMapper.ToObject(jsonDData);
                        char acc1 = O["acc1"].ToString()[0];
                        char acc2 = O["acc2"].ToString()[0];

                        if (!player_id.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            shopdata = new Shop_data(player_id, riding1, riding2, clothes1, clothes2, hair1, hair2, acc1, acc2);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return shopdata;
                        }
                        else
                        { // 로그인 실패
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }
    public Friend_data Friend(string user_id)
    {
        try
        {
            if (ConnectionCheck(connection))
            {
                string selectCommand = string.Format(@"SELECT * FROM user_info A JOIN friend B ON A.id = B.player_id WHERE A.id = '{0}';", user_id);
                MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
                reader = cmd.ExecuteReader();
                if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
                {
                    // 읽은 데이터를 하나씩 나열
                    while (reader.Read())
                    {
                        // user_info
                        string player_id = reader["player_id"].ToString();
                        string f1 = reader["friend1"].ToString();
                        string f2 = reader["friend2"].ToString();
                        string f3 = reader["friend3"].ToString();
                        if (!player_id.Equals(string.Empty))
                        { // 정상적으로 Data를 불러온 상황
                            Friend_data friend_Data = new Friend_data(player_id, f1, f2, f3);
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            return friend_Data;
                        }
                        else
                        { 
                            break;
                        }
                    }
                }
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return null;
        }
    }


    #endregion
    #region Sign In, Up, Out
    public bool SignIn(string user_id, string user_pw)
    { // 로그인
        // 조회되는 데이터가 없다면 false, 조회가 되는 데이터가 있다면 true
        // 위에서 선언한 info에 담은 다음에 리턴함
        /*
            Data 갖고오기
            1. Connection Open인지 확인
            2. Reader 상태가 읽고 있는 상황인지 확인(1quary 1reader)
            3. Data를 다 읽었으면 Reader의 상태를 확인 후 Close
         */
        /*try*/
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string selectCommand = string.Format(@"SELECT * FROM user_info A
                                                   JOIN user_character_info B ON A.id = B.user_id
                                                   JOIN item C ON A.id = C.player_id
                                                   JOIN animal D ON A.id = D.player_id
                                                   JOIN challenge E ON A.id = E.player_id
                                                   JOIN shop F ON A.id = F.player_id
                                                   JOIN friend G ON A.id = G.player_id
                                                   WHERE A.id = '{0}' AND A.pw = '{1}';", user_id, user_pw); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(selectCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
            {
                // 읽은 데이터를 하나씩 나열
                while (reader.Read())
                {
                    // user_info
                    string id = (reader.IsDBNull(0)) ? string.Empty : reader["id"].ToString();
                    string pw = (reader.IsDBNull(0)) ? string.Empty : reader["pw"].ToString();
                    string nickName = reader["nickname"].ToString();
                    char firstConnect = reader["firstConnect"].ToString()[0];
                    char connecting = reader["connecting"].ToString()[0];

                    // user_character_info
                    string id_c = reader["user_id"].ToString();
                    int eyes = reader["Eyes"].ToString()[0] - '0';
                    int jummper = reader["Jummper"].ToString()[0] - '0';
                    int runners = reader["Runners"].ToString()[0] - '0';
                    int tshirt = reader["TShirts"].ToString()[0] - '0';
                    int trunk = reader["Trunks"].ToString()[0] - '0';
                    int skin = reader["Skin"].ToString()[0] - '0';
                    int hat = reader["Hat"].ToString()[0] - '0';
                    int ride = reader["Riding"].ToString()[0] - '0';
                    int hair = reader["HairStyle"].ToString()[0] - '0';
                    char riding = reader["Ride"].ToString()[0];

                    // item_data
                    string player_id = reader["player_id"].ToString();
                    int key_num = reader["key_num"].ToString()[0] - '0';
                    int have_fisingrod = reader["have_fisingrod"].ToString()[0] - '0';
                    int food_num = reader["food_num"].ToString()[0] - '0';
                    int money = reader["money"].ToString()[0] - '0';

                    // collection_data
                    string jsonDData = reader["black_goat"].ToString();
                    JsonData D = JsonMapper.ToObject(jsonDData);
                    string bg_player_id = reader["player_id"].ToString();
                    char bg_is_open = D["is_open"].ToString()[0];
                    char bg_givefood = D["givefood"].ToString()[0];
                    char bg_issolved = D["issolved"].ToString()[0];

                    jsonDData = reader["black_sheep"].ToString();
                    JsonData E = JsonMapper.ToObject(jsonDData);
                    string bs_player_id = reader["player_id"].ToString();
                    char bs_is_open = E["is_open"].ToString()[0];
                    char bs_givefood = E["givefood"].ToString()[0];
                    char bs_issolved = E["issolved"].ToString()[0];

                    jsonDData = reader["chicken"].ToString();
                    JsonData F = JsonMapper.ToObject(jsonDData);
                    string ch_player_id = reader["player_id"].ToString();
                    char ch_is_open = F["is_open"].ToString()[0];
                    char ch_givefood = F["givefood"].ToString()[0];
                    char ch_issolved = F["issolved"].ToString()[0];

                    jsonDData = reader["whale"].ToString();
                    JsonData G = JsonMapper.ToObject(jsonDData);
                    string wh_player_id = reader["player_id"].ToString();
                    char wh_is_open = G["is_open"].ToString()[0];
                    char wh_givefood = G["givefood"].ToString()[0];
                    char wh_issolved = G["issolved"].ToString()[0];

                    jsonDData = reader["white_goat"].ToString();
                    JsonData H = JsonMapper.ToObject(jsonDData);
                    string wg_player_id = reader["player_id"].ToString();
                    char wg_is_open = H["is_open"].ToString()[0];
                    char wg_givefood = H["givefood"].ToString()[0];
                    char wg_issolved = H["issolved"].ToString()[0];

                    jsonDData = reader["white_sheep"].ToString();
                    JsonData I = JsonMapper.ToObject(jsonDData);
                    string ws_player_id = reader["player_id"].ToString();
                    char ws_is_open = I["is_open"].ToString()[0];
                    char ws_givefood = I["givefood"].ToString()[0];
                    char ws_issolved = I["issolved"].ToString()[0];

                    jsonDData = reader["yellow_sheep"].ToString();
                    JsonData J = JsonMapper.ToObject(jsonDData);
                    string ys_player_id = reader["player_id"].ToString();
                    char ys_is_open = J["is_open"].ToString()[0];
                    char ys_givefood = J["givefood"].ToString()[0];
                    char ys_issolved = J["issolved"].ToString()[0];

                    jsonDData = reader["none"].ToString();
                    JsonData K = JsonMapper.ToObject(jsonDData);
                    string no_player_id = reader["player_id"].ToString();
                    char no_is_open = K["is_open"].ToString()[0];
                    char no_givefood = K["givefood"].ToString()[0];
                    char no_issolved = K["issolved"].ToString()[0];

                    // challenge
                    string chal_player_id = reader["player_id"].ToString();
                    DateTime cur_time = (DateTime)reader["cur_time"];
                    DateTime last_time = (DateTime)reader["cur_time"];
                    int dailycount = reader["dailycount"].ToString()[0] - '0';
                    Updatechal_dailycount(cur_time, last_time, dailycount);
                    dailycount = reader["dailycount"].ToString()[0] - '0';
                    int trash = reader["trash"].ToString()[0] - '0';
                    int box = reader["box"].ToString()[0] - '0';

                    // shop
                    jsonDData = reader["riding_shop"].ToString();
                    JsonData L = JsonMapper.ToObject(jsonDData);
                    string shop_player_id = reader["player_id"].ToString();
                    char riding1 = L["riding1"].ToString()[0];
                    char riding2 = L["riding2"].ToString()[0];

                    jsonDData = reader["clothes_shop"].ToString();
                    JsonData M = JsonMapper.ToObject(jsonDData);
                    char clothes1 = M["clothes1"].ToString()[0];
                    char clothes2 = M["clothes2"].ToString()[0];

                    jsonDData = reader["hair_shop"].ToString();
                    JsonData N = JsonMapper.ToObject(jsonDData);
                    char hair1 = N["hair1"].ToString()[0];
                    char hair2 = N["hair2"].ToString()[0];

                    jsonDData = reader["acc_shop"].ToString();
                    JsonData O = JsonMapper.ToObject(jsonDData);
                    char acc1 = O["acc1"].ToString()[0];
                    char acc2 = O["acc2"].ToString()[0];

                    //firend
                    string f_player_id = reader["player_id"].ToString();
                    string friend1 = reader["friend1"].ToString();
                    string friend2 = reader["friend2"].ToString();
                    string friend3 = reader["friend3"].ToString();


                    if (!id.Equals(string.Empty) || !pw.Equals(string.Empty) || !nickName.Equals(string.Empty))
                    {
                        // 정상적으로 Data를 불러온 상황
                        info = new User_info(id, pw, nickName, firstConnect, connecting);

                        character_info = new User_Character(id_c, eyes, jummper, runners, tshirt, trunk, skin, hat, hair, ride, riding);

                        item = new item_data(player_id, key_num, have_fisingrod, food_num, money);

                        black_goat = new Nonplayer_data(bg_player_id, bg_is_open, bg_givefood, bg_issolved);
                        nonplayer_Datas.Add(black_goat);
                        black_sheep = new Nonplayer_data(bs_player_id, bs_is_open, bs_givefood, bs_issolved);
                        nonplayer_Datas.Add(black_sheep);
                        chicken = new Nonplayer_data(ch_player_id, ch_is_open, ch_givefood, ch_issolved);
                        nonplayer_Datas.Add(chicken);
                        whale = new Nonplayer_data(wh_player_id, wh_is_open, wh_givefood, wh_issolved);
                        nonplayer_Datas.Add(whale);
                        white_goat = new Nonplayer_data(wg_player_id, wg_is_open, wg_givefood, wg_issolved);
                        nonplayer_Datas.Add(white_goat);
                        white_sheep = new Nonplayer_data(ws_player_id, ws_is_open, ws_givefood, ws_issolved);
                        nonplayer_Datas.Add(white_sheep);
                        yellow_sheep = new Nonplayer_data(ys_player_id, ys_is_open, ys_givefood, ys_issolved);
                        nonplayer_Datas.Add(yellow_sheep);
                        none = new Nonplayer_data(no_player_id, no_is_open, no_givefood, no_issolved);

                        challenge_data = new Challenge_data(chal_player_id, cur_time, last_time, dailycount, trash, box);
                        shopdata = new Shop_data(shop_player_id, riding1, riding2, clothes1, clothes2, hair1, hair2, acc1, acc2);
                        friendsdata = new Friend_data(f_player_id, friend1, friend2, friend3);

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
       /* catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return false;
        }*/
    }

    public bool SignUp(string user_id, string user_pw, string user_nick)
    { // 회원가입
        try
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT id, nickname FROM user_info WHERE id = '{0}' OR nickname = '{1}';", user_id, user_nick);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
            { // 회원가입 실패
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return false;
            }
            else
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                string insertCommand = string.Format(@"INSERT INTO user_info (id, pw, nickname) VALUES ('{0}', '{1}', '{2}');
                                                       INSERT INTO user_character_info (user_id) VALUES ('{0}');
                                                       INSERT INTO item (player_id) VALUES('{0}');
                                                       INSERT INTO friend (player_id) VALUES('{0}');
                                                       INSERT INTO challenge( player_id,cur_time,last_time) VALUES ( '{0}',now(),now() );
                                                       INSERT INTO shop(player_id,riding_shop,clothes_shop,hair_shop,acc_shop) VALUES ('{0}', json_object(
                                                            'riding1', 'F', 
                                                            'riding2', 'F'
                                                        ),json_object(
                                                            'clothes1', 'F', 
                                                            'clothes2', 'F' 
                                                        ),json_object(
                                                            'hair1', 'F', 
                                                            'hair2', 'F'
                                                        ),json_object(
                                                            'acc1', 'F', 
                                                            'acc2', 'F'
                                                        ));
                                                       INSERT INTO animal(player_id, black_goat,black_sheep,chicken,whale,white_goat,white_sheep,yellow_sheep,none) values('{0}', json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F',
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ),json_object(
                                                            'is_open', 'F', 
                                                            'givefood', 'F', 
                                                            'issolved', 'F'
                                                        ));",
                                                       user_id, user_pw, user_nick);
                MySqlCommand insertCmd = new MySqlCommand(insertCommand, connection);
                reader = insertCmd.ExecuteReader();
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return true;
            }
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

    public bool SignOut(string user_id, string user_pw)
    { // 회원탈퇴
        try
        {
            if (!ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT * FROM user_info A
                                                   JOIN user_character_info B ON A.id = B.user_id
                                                   JOIN item C ON A.id = C.player_id
                                                   JOIN animal D ON A.id = D.player_id
                                                   JOIN challenge E ON A.id = E.player_id
                                                   JOIN shop F ON A.id = F.player_id
                                                   JOIN friend G ON A.id = G.player_id
                                                   WHERE A.id = '{0}' AND A.pw = '{1}';", user_id, user_pw);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            Debug.Log(reader.HasRows);

            if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
            { // 회원탈퇴 실패
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                string insertCommand = string.Format(@"DELETE FROM user_info WHERE id = '{0}';
                                                       DELETE FROM user_character_info WHERE user_id = '{0}';
                                                       DELETE FROM item WHERE player_id = '{0}';
                                                       DELETE FROM animal WHERE player_id = '{0}';
                                                       DELETE FROM challenge WHERE player_id = '{0}';
                                                       DELETE FROM friend WHERE player_id = '{0}';
                                                       DELETE FROM shop WHERE player_id = '{0}';", user_id);
                MySqlCommand insertCmd = new MySqlCommand(insertCommand, connection);
                reader = insertCmd.ExecuteReader();
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return true;
            }

            else
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return false;
            }

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
    #endregion
}