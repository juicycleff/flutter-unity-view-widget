using System.Collections;
using System.IO;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
[UnityPlatform(RuntimePlatform.WindowsPlayer)]
[UnityPlatform(RuntimePlatform.LinuxPlayer)]
[UnityPlatform(RuntimePlatform.OSXPlayer)]
public class GetCurrentRTTCallDoesntCrashWhenUseWebSockets : IPrebuildSetup, IPostBuildCleanup
{
    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab = new GameObject("GetCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab");
        getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.AddComponent<UnetPlayerWithGetCurrentRTTCallScript>();
        getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.AddComponent<NetworkIdentity>();
        getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab, "Assets/" + getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.name + ".prefab");
        GameObject.DestroyImmediate(getCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab);

        var bridgeScriptRef = new GameObject(GetCurrentRTTCallDoesntCrashWhenUseWebSockets_BridgeScript.bridgeGameObjectName).AddComponent<GetCurrentRTTCallDoesntCrashWhenUseWebSockets_BridgeScript>();
        bridgeScriptRef.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GetCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(GetCurrentRTTCallDoesntCrashWhenUseWebSockets_BridgeScript.bridgeGameObjectName).GetComponent<GetCurrentRTTCallDoesntCrashWhenUseWebSockets_BridgeScript>();
        var nmObject = new GameObject("NetworkManager");
        var nmanager = nmObject.AddComponent<NetworkManager>();
        nmanager.playerPrefab = bridgeRef.playerPrefab;
        nmanager.networkAddress = "localhost";
        nmanager.useWebSockets = true;
    }

    [UnityTest]
    public IEnumerator GetCurrentRTTCallDoesntCrashWhenUseWebSocketsTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        LogAssert.Expect(LogType.Error, "the function called has not been supported for web sockets communication");
        NetworkManager.singleton.StartHost();
        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not active after StartHost");
        Assert.IsTrue(NetworkClient.active, "Client is not active after StartHost");

        yield return null;
        GameObject player = GameObject.Find("GetCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab(Clone)");

        while (!player.GetComponent<UnetPlayerWithGetCurrentRTTCallScript>().isDone)
        {
            yield return null;
        }
        NetworkManager.singleton.StopHost();
        yield return null;
        Assert.IsNull(GameObject.Find("GetCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab(Clone)"), "PlayerPrefab(Clone) object should be destroyed after calling StopHost");        
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(NetworkManager.singleton.gameObject);
    }

#if UNITY_EDITOR
    void DeleteAssetsIfExist()
    {
        if (File.Exists("Assets/GetCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/GetCurrentRTTCallDoesntCrashWhenUseWebSockets_PlayerPrefab.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(GetCurrentRTTCallDoesntCrashWhenUseWebSockets_BridgeScript.bridgeGameObjectName));
#endif
    }
}
#pragma warning restore 618
