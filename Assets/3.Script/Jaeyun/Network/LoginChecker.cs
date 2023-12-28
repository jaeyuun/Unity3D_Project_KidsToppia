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

public class LoginChecker : MonoBehaviour
{
    public User_info info;

    private SQLManager sqlManager;
    private MySqlConnection connection;
    private MySqlDataReader reader;

    private void Start()
    {
        TryGetComponent(out sqlManager);
    }

    public bool SignIn(string id, string password, string nick)
    { // 로그인
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
            if (!sqlManager.ConnectionCheck(connection))
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
                    if (!name.Equals(string.Empty) || !pw.Equals(string.Empty) || !nickName.Equals(string.Empty))
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

    public bool SignUp(string id, string password, string nick)
    { // 회원가입
        try
        {
            if (!sqlManager.ConnectionCheck(connection))
            {
                return false;
            }
            string sqlCommand = string.Format(@"SELECT id, pw, nickname FROM user_info WHERE id = '{0}' OR nickname = '{1}';", id, nick); // @: 줄바꿈이 있어도 한줄로 인식한다는 의미
            MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows) // reader 읽은 데이터 1개 이상 존재하는지?
            { // 회원가입 실패
                return false;
            }
            else
            {
                // 읽은 데이터를 하나씩 나열
                while (reader.Read())
                {
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["id"].ToString();
                    string pw = (reader.IsDBNull(0)) ? string.Empty : (string)reader["pw"].ToString();
                    string nickName = (reader.IsDBNull(0)) ? string.Empty : (string)reader["nickname"].ToString();
                    if (!name.Equals(string.Empty))
                    { // 정상적으로 Data를 불러온 상황
                        info = new User_info(name, pw, nickName);
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                        return true;
                    }
                    else
                    { // 회원가입 실패
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
