using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class SpawnObjectThenDeleteItBeforeConnect : SpawningTestBase
{
    bool isDone = false;

    [UnityTest]
    public IEnumerator SpawnObjectThenDeleteItBeforeConnectTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();
        TestSetup();
        StartServer();
        NetworkServer.SpawnObjects();

        GameObject obj = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(obj);
        yield return new WaitForSeconds(2);
        NetworkServer.Destroy(obj);

        StartClientAndConnect();

        while (!isDone)
        {
            yield return null;
        }

        ClientScene.DestroyAllClientObjects();
        yield return null;
        NetworkServer.Destroy(playerObj);
    }

    public override void OnClientReady(short playerId)
    {
        Assert.AreEqual(2, numStartServer, "StartServer should be called 2 times - for player and SpawnableObject");
        Assert.AreEqual(1, numStartClient, "StartClient should be called 1 time - for player only"); // 1 for player
        Assert.AreEqual(0, numDestroyClient, "numDestroyClient should be 0, as there was no SpawnableObject on the Client"); //no rock on client
        isDone = true;
    }
}
#pragma warning restore 618
