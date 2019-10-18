using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class SettingNetworkStartPositionWorks : IPrebuildSetup, IPostBuildCleanup
{
    public static Vector3 startpos = new Vector3(1.4f, 6.3f, 6.23f);    

    public class TestNetworkManagerStartPos : NetworkManager
    {
        public bool isDone = false;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);
            StringAssert.IsMatch(conn.playerControllers[0].gameObject.transform.position.ToString(), startpos.ToString());
            isDone = true;
        }
    }

    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var cleanPlayerPrefab = new GameObject("CleanPlayerPrefab_SettingNetworkStartPositionWorks");
        cleanPlayerPrefab.AddComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(cleanPlayerPrefab, "Assets/" + cleanPlayerPrefab.name + ".prefab");
        GameObject.DestroyImmediate(cleanPlayerPrefab);

        var bridgeScriptRef = new GameObject(SettingNetworkStartPositionWorks_BridgeScript.bridgeObjectName).AddComponent<SettingNetworkStartPositionWorks_BridgeScript>();
        bridgeScriptRef.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CleanPlayerPrefab_SettingNetworkStartPositionWorks.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(SettingNetworkStartPositionWorks_BridgeScript.bridgeObjectName).GetComponent<SettingNetworkStartPositionWorks_BridgeScript>();
        var nmObject = new GameObject("NetworkManager");
        var nmanager = nmObject.AddComponent<TestNetworkManagerStartPos>();
        nmanager.playerPrefab = bridgeRef.playerPrefab;
        nmanager.networkAddress = "localhost";

        var start = new GameObject();
        start.transform.position = startpos;
        start.AddComponent<NetworkStartPosition>();
    }

    [UnityTest]
    public IEnumerator SettingNetworkStartPositionWorksTest()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();
        yield return null;
        var testNetworkManagerStartPos = NetworkManager.singleton.gameObject.GetComponent<TestNetworkManagerStartPos>();
        testNetworkManagerStartPos.StartServer();
        testNetworkManagerStartPos.StartClient();

        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not started");
        Assert.IsTrue(NetworkClient.active, "Client is not started");
        yield return null;

        while (!testNetworkManagerStartPos.isDone)
        {
            yield return null;
        }

        NetworkManager.singleton.StopServer();
        NetworkManager.singleton.StopClient();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(NetworkManager.singleton.gameObject);
    }

    void DeleteAssetsIfExist()
    {
#if UNITY_EDITOR
        if(File.Exists("Assets/CleanPlayerPrefab_SettingNetworkStartPositionWorks.prefab"))
            AssetDatabase.DeleteAsset("Assets/CleanPlayerPrefab_SettingNetworkStartPositionWorks.prefab");
#endif
    }

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(SettingNetworkStartPositionWorks_BridgeScript.bridgeObjectName));
#endif
    }
}
#pragma warning restore 618
