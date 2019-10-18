using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class AuthorityOnSpawnedObjectsIsCorrect : IPrebuildSetup, IPostBuildCleanup
{
    public static bool isTestDone = false;

    public void Setup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();

        var playerWithAuthPrefab = new GameObject("PlayerWithAuthPrefab");
        playerWithAuthPrefab.AddComponent<PlayerWithAuthority>();
        playerWithAuthPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(playerWithAuthPrefab, "Assets/" + playerWithAuthPrefab.name + ".prefab").GetComponent<PlayerWithAuthority>();
        GameObject.DestroyImmediate(playerWithAuthPrefab);

        var noAuthObjPrefab = new GameObject("NoAuthObjPrefab");
        noAuthObjPrefab.AddComponent<NoAuthSpawnableObject>();
        noAuthObjPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(noAuthObjPrefab, "Assets/" + noAuthObjPrefab.name + ".prefab");
        GameObject.DestroyImmediate(noAuthObjPrefab);

        var authObjPrefab = new GameObject("AuthObjPrefab");
        authObjPrefab.AddComponent<AuthSpawnableObject>();
        authObjPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        PrefabUtility.SaveAsPrefabAsset(authObjPrefab, "Assets/" + authObjPrefab.name + ".prefab");
        GameObject.DestroyImmediate(authObjPrefab);

        var bridgeScriptRef = new GameObject(AuthorityOnSpawnedObjectsIsCorrect_BridgeScript.bridgeGameObjectName).AddComponent<AuthorityOnSpawnedObjectsIsCorrect_BridgeScript>();
        bridgeScriptRef.playerWithAuthPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PlayerWithAuthPrefab.prefab");
        bridgeScriptRef.authObjPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AuthObjPrefab.prefab");
        bridgeScriptRef.noAuthObjPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/NoAuthObjPrefab.prefab");
#endif
    }

    [SetUp]
    public void SetUp()
    {
        var bridgeRef = GameObject.Find(AuthorityOnSpawnedObjectsIsCorrect_BridgeScript.bridgeGameObjectName).GetComponent<AuthorityOnSpawnedObjectsIsCorrect_BridgeScript>();
        var playerWithAuthorityRef = bridgeRef.playerWithAuthPrefab.GetComponent<PlayerWithAuthority>();
        playerWithAuthorityRef.objAuthPrefab = bridgeRef.authObjPrefab;
        playerWithAuthorityRef.objNoAuthPrefab = bridgeRef.noAuthObjPrefab;

        var nmObject = new GameObject("NetworkManager");
        var nmanager = nmObject.AddComponent<NetworkManager>();

        nmanager.playerPrefab = bridgeRef.playerWithAuthPrefab;
        nmanager.spawnPrefabs.Add(bridgeRef.authObjPrefab);
        nmanager.spawnPrefabs.Add(bridgeRef.noAuthObjPrefab);
    }

    [UnityTest]
    public IEnumerator AuthorityOnSpawnedObjectsIsCorrectTest()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();

        Assert.IsNotNull(NetworkManager.singleton.playerPrefab, "Player prefab field is not set on NetworkManager");
        NetworkManager.singleton.StartHost();
        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not active after StartHost");
        Assert.IsTrue(NetworkClient.active, "Client is not active after StartHost");

        while (!isTestDone)
        {
            yield return null;
        }

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
        if (File.Exists("Assets/PlayerWithAuthPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/PlayerWithAuthPrefab.prefab");

        if (File.Exists("Assets/NoAuthObjPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/NoAuthObjPrefab.prefab");

        if (File.Exists("Assets/authObjPrefab.prefab"))
            AssetDatabase.DeleteAsset("Assets/AuthObjPrefab.prefab");
    }
#endif

    public void Cleanup()
    {
#if UNITY_EDITOR
        DeleteAssetsIfExist();
        GameObject.DestroyImmediate(GameObject.Find(AuthorityOnSpawnedObjectsIsCorrect_BridgeScript.bridgeGameObjectName));
#endif
    }
}
#pragma warning restore 618
