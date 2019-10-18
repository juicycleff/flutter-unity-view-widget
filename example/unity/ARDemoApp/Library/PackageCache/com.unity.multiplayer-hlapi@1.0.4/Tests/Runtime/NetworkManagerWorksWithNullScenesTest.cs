using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerWorksWithNullScenesTest : IPrebuildSetup, IPostBuildCleanup
{
    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var cleanPlayerPrefab = new GameObject("CleanPlayerPrefab_NetworkManagerWorksWithNullScenesTest");
        cleanPlayerPrefab.AddComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(cleanPlayerPrefab, "Assets/" + cleanPlayerPrefab.name + ".prefab");
        GameObject.DestroyImmediate(cleanPlayerPrefab);

        var bridgeScriptRef = new GameObject(NetworkManagerWorksWithNullScenesTest_BridgeScript.bridgeGameObjectName).AddComponent<NetworkManagerWorksWithNullScenesTest_BridgeScript>();
        bridgeScriptRef.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CleanPlayerPrefab_NetworkManagerWorksWithNullScenesTest.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(NetworkManagerWorksWithNullScenesTest_BridgeScript.bridgeGameObjectName).GetComponent<NetworkManagerWorksWithNullScenesTest_BridgeScript>();
        var networkManagerObj = new GameObject("NetworkManager");
        var nmanager = networkManagerObj.AddComponent<NetworkManager>();
        nmanager.playerPrefab = bridgeRef.playerPrefab;

        nmanager.offlineScene = null;
        nmanager.onlineScene = null;
    }

    [UnityTest]
    public IEnumerator TestNetworkManageNullScenes()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        if (!NetworkManager.singleton.isNetworkActive)
        {
            NetworkManager.singleton.StartHost();
            yield return null;
        }

        Assert.IsTrue(NetworkManager.singleton.isNetworkActive,
            "Network is not active.");

        NetworkManager.singleton.StopHost();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(NetworkManager.singleton.gameObject);
    }

#if UNITY_EDITOR
    void DeleteAssetsIfExist()
    {
        if(File.Exists("Assets/CleanPlayerPrefab_NetworkManagerWorksWithNullScenesTest.prefab"))
            AssetDatabase.DeleteAsset("Assets/CleanPlayerPrefab_NetworkManagerWorksWithNullScenesTest.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(NetworkManagerWorksWithNullScenesTest_BridgeScript.bridgeGameObjectName));
#endif
    }
}
#pragma warning restore 618
