using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkBehaviourCallbacksOrderOnTheHost : IPrebuildSetup, IPostBuildCleanup
{
    public static List<string> expectedListOfCallbacks = new List<string>()
    {
        "OnStartServer",
        "OnStartClient",
        "OnRebuildObservers",
        "OnStartAuthority",
        "OnStartLocalPlayer",
        "Start",
        "OnSetLocalVisibility",
        "OnSetLocalVisibility"
    };

    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var playerCallbacksOrderOnTheHost_PlayerPrefab = new GameObject("PlayerCallbacksOrderOnTheHost_PlayerPrefab");
        playerCallbacksOrderOnTheHost_PlayerPrefab.AddComponent<PlayerCallbacksOrderOnTheHostScript>();
        playerCallbacksOrderOnTheHost_PlayerPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(playerCallbacksOrderOnTheHost_PlayerPrefab, "Assets/" + playerCallbacksOrderOnTheHost_PlayerPrefab.name + ".prefab");
        GameObject.DestroyImmediate(playerCallbacksOrderOnTheHost_PlayerPrefab);

        var bridgeScriptRef = new GameObject(NetworkBehaviourCallbacksOrderOnTheHost_BridgeScript.bridgeGameObjectName).AddComponent<NetworkBehaviourCallbacksOrderOnTheHost_BridgeScript>();
        bridgeScriptRef.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PlayerCallbacksOrderOnTheHost_PlayerPrefab.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(NetworkBehaviourCallbacksOrderOnTheHost_BridgeScript.bridgeGameObjectName).GetComponent<NetworkBehaviourCallbacksOrderOnTheHost_BridgeScript>();
        var nmObject = new GameObject("NetworkManager");
        var nmanager = nmObject.AddComponent<NetworkManager>();
        nmanager.playerPrefab = bridgeRef.playerPrefab;
    }

    //[KnownFailure(855941, "OnSetLocalVisibility callback should appear only once ")]
    [UnityTest]
    public IEnumerator CallbacksOrderInNetworkBehaviourOnTheHostIsCorrect()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        Assert.IsNotNull(NetworkManager.singleton.playerPrefab, "Player prefab field is not set on NetworkManager");

        NetworkManager.singleton.StartHost();
        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not active after StartHost");
        Assert.IsTrue(NetworkClient.active, "Client is not active after StartHost");
        yield return null;
        GameObject player = GameObject.Find("PlayerCallbacksOrderOnTheHost_PlayerPrefab(Clone)");
        yield return null;

        while (!player.GetComponent<PlayerCallbacksOrderOnTheHostScript>().isDone)
        {
            yield return null;
        }
        NetworkManager.singleton.StopHost();
        CollectionAssert.AreEqual(expectedListOfCallbacks, player.GetComponent<PlayerCallbacksOrderOnTheHostScript>().actualListOfCallbacks, "Wrong order of callbacks or some callback is missing");       
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(NetworkManager.singleton.gameObject);
    }

#if UNITY_EDITOR
    void DeleteAssetsIfExist()
    {
        if(File.Exists("Assets/PlayerCallbacksOrderOnTheHost_PlayerPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/PlayerCallbacksOrderOnTheHost_PlayerPrefab.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(NetworkBehaviourCallbacksOrderOnTheHost_BridgeScript.bridgeGameObjectName));
#endif
    }
}
#pragma warning restore 618
