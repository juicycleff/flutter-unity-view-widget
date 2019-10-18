using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

#pragma warning disable 618
public class SpawningTestBase : IPrebuildSetup, IPostBuildCleanup
{
    // CreateNamedPrefab - simply use prefabs in Resources folder

    int listenPort = 7073;
    private const short kReadyMsgId = 55;
    public const short kStringMsgId = 56;

    // One player per client by default
    int m_PlayerCount = 1;
    protected NetworkClient myClient;
    public GameObject playerObj;

    public static int numStartServer = 0;
    public static int numStartClient = 0;
    public static int numDestroyClient = 0;

    BridgeScript m_BridgeScript;

    protected BridgeScript GetBridgeScript
    {
        get
        {
            if (m_BridgeScript == null)
            {
                m_BridgeScript = GameObject.Find(BridgeScript.bridgeGameObjectName).GetComponent<BridgeScript>();
            }

            return m_BridgeScript;
        }
    }


    public static void IncrementStartServer()
    {
        numStartServer += 1;
    }

    public static void IncrementStartClient()
    {
        numStartClient += 1;
    }

    public static void IncrementDestroyClient()
    {
        numDestroyClient += 1;
    }

    public void Setup()
    {
#if UNITY_EDITOR
        var bridgeGO = GameObject.Find(BridgeScript.bridgeGameObjectName); 
        if (bridgeGO == null)
        {
            bridgeGO = new GameObject(BridgeScript.bridgeGameObjectName, typeof(BridgeScript));

            DeleteAssetsIfExist();

            var spawningBase_PlayerPrefab = new GameObject("SpawningBase_PlayerPrefab");
            spawningBase_PlayerPrefab.AddComponent<SpawningBase_PlayerScript>();
            PrefabUtility.SaveAsPrefabAsset(spawningBase_PlayerPrefab, "Assets/" + spawningBase_PlayerPrefab.name + ".prefab");
            GameObject.DestroyImmediate(spawningBase_PlayerPrefab);

            var spawningBase_SpawnableObjectPrefab = new GameObject("SpawningBase_SpawnableObjectPrefab");
            spawningBase_SpawnableObjectPrefab.AddComponent<SpawningBase_SpawnableObjectScript>();
            PrefabUtility.SaveAsPrefabAsset(spawningBase_SpawnableObjectPrefab, "Assets/" + spawningBase_SpawnableObjectPrefab.name + ".prefab");
            GameObject.DestroyImmediate(spawningBase_SpawnableObjectPrefab);

            m_BridgeScript = bridgeGO.GetComponent<BridgeScript>();
            m_BridgeScript.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SpawningBase_PlayerPrefab.prefab");
            m_BridgeScript.rocketPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SpawningBase_SpawnableObjectPrefab.prefab");
        }
#endif
    }

    public void TestSetup()
    {
        numStartServer = 0;
        numStartClient = 0;
        numDestroyClient = 0;
    }

    public void StartServer()
    {
        NetworkServer.Reset();
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnected);
        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        NetworkServer.RegisterHandler(kStringMsgId, OnServerStringMessage);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++listenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }
    }

    internal void RegisterClientData()
    {
        // Just use default handler
        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        myClient.RegisterHandler(kReadyMsgId, OnClientReadyInternal);
        myClient.RegisterHandler(kStringMsgId, OnClientStringMessage);

        ClientScene.RegisterPrefab(GetBridgeScript.playerPrefab);
        ClientScene.RegisterPrefab(GetBridgeScript.rocketPrefab);
    }

    public void StartClientAndConnect()
    {
        myClient = new NetworkClient();
        // not sure if we need custom config
        //  if (!myClient.Configure(config, maxConnections))
        //  {
        //      Assert.Fail("Client configure failed");
        //  }

        RegisterClientData();
        myClient.Connect("127.0.0.1", listenPort);
    }

    public void StartLocalClient()
    {
        StartLocalClient(1);
    }

    public void StartLocalClient(int playerCount)
    {
        m_PlayerCount = playerCount;
        myClient = ClientScene.ConnectLocalServer();
        RegisterClientData();
    }

    public void OnServerDisconnected(NetworkMessage netMsg)
    {
        NetworkServer.DestroyPlayersForConnection(netMsg.conn);
    }

    public void OnAddPlayer(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<UnityEngine.Networking.NetworkSystem.AddPlayerMessage>();
        playerObj = GameObject.Instantiate(GetBridgeScript.playerPrefab, Vector3.zero, Quaternion.identity);

        SpawningBase_PlayerScript script = playerObj.GetComponent<SpawningBase_PlayerScript>(); // need to add this script ot prefab in Resources
        script.intValue = 999;
        script.floatValue = 55.5f;

        NetworkServer.AddPlayerForConnection(netMsg.conn, playerObj, msg.playerControllerId);

        OnServerReady(playerObj);

        NetworkServer.SendToClientOfPlayer(playerObj, kReadyMsgId, msg);
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        ClientScene.AddPlayer(netMsg.conn, 0);

        for (int i = 1; i < m_PlayerCount; i++)
        {
            ClientScene.AddPlayer((short)(i + 1));
        }
    }

    public void OnClientReadyInternal(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<UnityEngine.Networking.NetworkSystem.AddPlayerMessage>();
        OnClientReady(msg.playerControllerId);
    }

    public virtual void OnServerReady(GameObject player)
    {
    }

    public virtual void OnClientReady(short playerId)
    {
    }

    public virtual void OnClientStringMessage(NetworkMessage netMsg)
    {
    }

    public virtual void OnServerStringMessage(NetworkMessage netMsg)
    {
    }

#if UNITY_EDITOR
    void DeleteAssetsIfExist()
    {
        if (File.Exists("Assets/SpawningBase_PlayerPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/SpawningBase_PlayerPrefab.prefab");
        if (File.Exists("Assets/SpawningBase_SpawnableObjectPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/SpawningBase_SpawnableObjectPrefab.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
#endif
        GameObject.DestroyImmediate(GameObject.Find(BridgeScript.bridgeGameObjectName));
    }
}
#pragma warning restore 618
