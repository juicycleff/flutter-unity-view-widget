using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class SpawnObjectThenDeleteItAfterConnect : SpawningTestBase
{
    GameObject deleteMe;
    bool isDone = false;

    [UnityTest]
    public IEnumerator SpawnObjectThenDeleteItAfterConnectTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();
        TestSetup();
        StartServer();

        deleteMe = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(deleteMe);

        StartClientAndConnect();

        while (!isDone)
        {
            yield return null;
        }

        ClientScene.DestroyAllClientObjects();
        yield return null;
        NetworkServer.Destroy(playerObj);
    }

    public override void OnServerReady(GameObject player)
    {
        NetworkServer.Destroy(deleteMe);
        Assert.AreEqual(2, numStartServer, "StartServer should be called 2 times - for player and SpawnableObject");
    }

    public override void OnClientReady(short playerId)
    {
        Assert.AreEqual(2, numStartClient, "StartClient should be called 2 times - for player and SpawnableObject");
        isDone = true;
    }
}
#pragma warning restore 618
