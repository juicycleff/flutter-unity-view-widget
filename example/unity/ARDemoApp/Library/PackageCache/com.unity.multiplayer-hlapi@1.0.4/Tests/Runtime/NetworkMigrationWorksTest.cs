using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

#pragma warning disable 618
public class NetworkMigrationWorksTest : IPrebuildSetup, IPostBuildCleanup
{
    NetworkMigrationManager networkMigrManager;
    NetworkClient client;
    int _port = 8888;
    string _ip = "127.0.0.1";

    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var playerGameObject = new GameObject("PlayerGameObject");
        playerGameObject.AddComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(playerGameObject, "Assets/" + playerGameObject.name + ".prefab");
        GameObject.DestroyImmediate(playerGameObject);

        var bridgeScriptRef = new GameObject(NetworkMigrationWorksTest_BridgeScript.bridgeGameObjectName).AddComponent<NetworkMigrationWorksTest_BridgeScript>();
        bridgeScriptRef.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PlayerGameObject.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(NetworkMigrationWorksTest_BridgeScript.bridgeGameObjectName).GetComponent<NetworkMigrationWorksTest_BridgeScript>();
        var netManagerObj = new GameObject("NetworkManager");
        var networkManager = netManagerObj.AddComponent<NetworkManager>();
        networkManager.playerPrefab = bridgeRef.playerPrefab;
    }

    [UnityTest]
    public IEnumerator NetworkMigrationWorksCheck()
    {
        NetworkServer.Reset();

        SetupNetwork();

        yield return new WaitUntil(() => networkMigrManager.peers != null);

        NetworkManager.singleton.StopServer();

        PeerInfoMessage newHostInfo;
        bool youAreNewHost;
        Assert.IsTrue(
            networkMigrManager.FindNewHost(out newHostInfo, out youAreNewHost),
            "New host was not found.");

        Assert.IsTrue(
            client.ReconnectToNewHost(newHostInfo.address, newHostInfo.port),
            "Old client did not reconnect to new host.");

        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(NetworkManager.singleton.gameObject);
    }

    public void SetupNetwork()
    {
        Assert.IsNotNull(NetworkManager.singleton.playerPrefab);

        NetworkManager.singleton.customConfig = true;
        NetworkManager.singleton.networkAddress = _ip;
        NetworkManager.singleton.networkPort = _port;
        NetworkManager.singleton.autoCreatePlayer = false;

        networkMigrManager = NetworkManager.singleton.gameObject.AddComponent<NetworkMigrationManager>();
        Assert.IsTrue(NetworkManager.singleton.StartServer(), "Server was not started!");
        NetworkManager.singleton.SetupMigrationManager(networkMigrManager);

        client = NetworkManager.singleton.StartClient();
        client.Connect(_ip, _port);
        Assert.IsNull(client.connection, "Client is not connected");

        networkMigrManager.Initialize(client, NetworkManager.singleton.matchInfo);
        networkMigrManager.SendPeerInfo();
    }

#if UNITY_EDITOR
    void DeleteAssetsIfExist()
    {
        if(File.Exists("Assets/PlayerGameObject.prefab"))
            AssetDatabase.DeleteAsset("Assets/PlayerGameObject.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(NetworkMigrationWorksTest_BridgeScript.bridgeGameObjectName));
#endif
    }
}
#pragma warning restore 618
