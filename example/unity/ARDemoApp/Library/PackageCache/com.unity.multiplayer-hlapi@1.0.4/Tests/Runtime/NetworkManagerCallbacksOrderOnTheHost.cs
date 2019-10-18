using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerCallbacksOrderOnTheHost : IPrebuildSetup, IPostBuildCleanup
{
    public static List<string> resultListOfCallbacks = new List<string>()
    {
        "OnStartHost",
        "OnStartServer",
        "OnServerConnect",
        "OnStartClient",
        "OnServerReady",
        "OnServerAddPlayer",
        "OnClientConnect",
        "OnStopHost",
        "OnStopServer",
        "OnStopClient"
    };

    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var cleanPlayerPrefab = new GameObject("CleanPlayerPrefab_NetworkManagerCallbacksOrderOnTheHost");
        cleanPlayerPrefab.AddComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(cleanPlayerPrefab, "Assets/" + cleanPlayerPrefab.name + ".prefab");
        GameObject.DestroyImmediate(cleanPlayerPrefab);

        var bridgeScriptRef = new GameObject(NetworkManagerCallbacksOrderOnTheHost_BridgeScript.bridgeGameObjectName).AddComponent<NetworkManagerCallbacksOrderOnTheHost_BridgeScript>();
        bridgeScriptRef.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CleanPlayerPrefab_NetworkManagerCallbacksOrderOnTheHost.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(NetworkManagerCallbacksOrderOnTheHost_BridgeScript.bridgeGameObjectName).GetComponent<NetworkManagerCallbacksOrderOnTheHost_BridgeScript>();
        var nmObject = new GameObject("NetworkManager");
        var nmanager = nmObject.AddComponent<CustomNetworkManagerWithCallbacks>();
        nmanager.playerPrefab = bridgeRef.playerPrefab;
    }

    [UnityTest]
    public IEnumerator CallbacksOrderInNetworkManagerOnTheHostIsCorrect()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();

        var customeNetworkManagerWithCallbacks =
            NetworkManager.singleton.gameObject.GetComponent<CustomNetworkManagerWithCallbacks>();
        yield return null;
        Assert.IsNotNull(customeNetworkManagerWithCallbacks.playerPrefab, "Player prefab field is not set on NetworkManager");

        customeNetworkManagerWithCallbacks.StartHost();
        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not active after StartHost");
        Assert.IsTrue(NetworkClient.active, "Client is not active after StartHost");
        yield return null;

        while (!customeNetworkManagerWithCallbacks.isStartHostPartDone)
        {
            yield return null;
        }

        customeNetworkManagerWithCallbacks.StopHost();
        while (!customeNetworkManagerWithCallbacks.isStopHostPartDone)
        {
            yield return null;
        }

        CollectionAssert.AreEqual(resultListOfCallbacks, customeNetworkManagerWithCallbacks.actualListOfCallbacks, "Wrong order of callbacks or some callback is missing");        
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(NetworkManager.singleton.gameObject);
    }

#if UNITY_EDITOR
    void DeleteAssetsIfExist()
    {
        if (File.Exists("Assets/CleanPlayerPrefab_NetworkManagerCallbacksOrderOnTheHost.prefab"))
            AssetDatabase.DeleteAsset("Assets/CleanPlayerPrefab_NetworkManagerCallbacksOrderOnTheHost.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(NetworkManagerCallbacksOrderOnTheHost_BridgeScript.bridgeGameObjectName));
#endif
    }
}
#pragma warning restore 618
