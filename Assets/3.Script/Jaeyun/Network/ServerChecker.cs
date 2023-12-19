using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Network namespace */
using Mirror;
using kcp2k;
using LitJson;
using System;
using System.IO;

public enum ServerType
{
    Empty = 0,
    Server,
    Client
}

public class ServerItem
{ // license file class
    public string license;
    public string serverIp;
    public string port;

    public ServerItem(string license, string ipValue, string port)
    {
        this.license = license;
        serverIp = ipValue;
        this.port = port;
    }
}

public class ServerChecker : MonoBehaviour
{
    public static ServerChecker instance = null;

    public ServerType type;

    private NetworkManager manager;
    private KcpTransport kcp;

    private string path = string.Empty;
    public string serverIp { get; private set; }
    public string serverPort { get; private set; }

    [Header("Input My IP")]
    public string myIp = string.Empty;
    public string stringType = string.Empty;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        if (path.Equals(string.Empty))
        {
            path = Application.dataPath + "/License";
        }
        if (!File.Exists(path))
        { // folder �˻�
            Directory.CreateDirectory(path);
        }
        if (!File.Exists(path + "/License.json"))
        { // file �˻�
            DefaultData(path);
        }
        manager = GetComponent<NetworkManager>();
        kcp = (KcpTransport)manager.transport;
    }

    private void DefaultData(string path)
    {
        List<ServerItem> items = new List<ServerItem>();
        items.Add(new ServerItem("1", "127.0.0.1", "7777"));
        JsonData data = JsonMapper.ToJson(items); // �ݵ�� �ڷᱸ���� �־�� ��
        File.WriteAllText(path + "/License.json", data.ToString());
    }

    private void Start()
    {
        
    }

    private ServerType LicenseType()
    {
        ServerType type = ServerType.Empty;
        try
        {
            string jsonString = File.ReadAllText(path + "/License.json");
            JsonData itemData = JsonMapper.ToObject(jsonString);
            string stringType = this.stringType;
            // string stringType = itemData[0]["license"].ToString();
            string stringServerIp = itemData[0]["serverIp"].ToString();
            string stringPort = itemData[0]["port"].ToString();
            serverIp = myIp;
            // serverIp = stringServerIp;
            serverPort = stringPort;

            type = (ServerType)Enum.Parse(typeof(ServerType), stringType); // string to enum

            manager.networkAddress = serverIp;
            kcp.port = ushort.Parse(serverPort);
            return type;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return ServerType.Empty;
        }
    }

    public void StartServer()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.Log("WebGL can not be Server");
        }
        else
        {
            manager.StartServer();
            Debug.Log($"{manager.networkAddress} StartServer...");
            NetworkServer.OnConnectedEvent += (NetworkConnectionToClient) =>
            {
                Debug.Log($"New Client Connect : {NetworkConnectionToClient.address}");
            };
            NetworkServer.OnDisconnectedEvent += (NetworkDisconnectionToClient) =>
            {
                Debug.Log($"Client Disconnect : {NetworkDisconnectionToClient.address}");
            };
        }
    }

    public void StartClient()
    {
        Debug.Log($"{manager.networkAddress} : StartClient...");
        manager.StartClient();
    }

    private void OnApplicationQuit()
    {
        if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        if (NetworkServer.active)
        {
            manager.StopServer();
        }
    }

    public void StartButton()
    {
        type = LicenseType();
        if (type.Equals(ServerType.Server))
        {
            StartServer();
            Debug.Log("Server");
        }
        else
        {
            StartClient();
            Debug.Log("Client");
        }
    }
}