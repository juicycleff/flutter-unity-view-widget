using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class LocalClientSpawnsObjectAfterConnect : SpawningTestBase
{
    bool isDone;
    GameObject obj;

    [UnityTest]
    public IEnumerator LocalClientSpawnsObjectAfterConnectTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        TestSetup();
        StartServer();
        StartLocalClient();

        while (!isDone)
        {
            yield return null;
        }

        // 2 is player and rock
        Assert.AreEqual(2, numStartServer);
        Assert.AreEqual(2, numStartClient);

        ClientScene.DestroyAllClientObjects();
        yield return null;
        NetworkServer.Destroy(obj);
        NetworkServer.Destroy(playerObj);
    }

    public override void OnServerReady(GameObject player)
    {
        obj = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(obj);
        isDone = true;
    }
}
#pragma warning restore 618
