using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class LocalClientSpawnsObjectBeforeConnect : SpawningTestBase
{
    bool isDone;
    GameObject obj;

    [UnityTest]
    public IEnumerator LocalClientSpawnsObjectBeforeConnectTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();
        TestSetup();
        StartServer();
        obj = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(obj);

        StartLocalClient();

        while (!isDone)
        {
            yield return null;
        }

        ClientScene.DestroyAllClientObjects();
        yield return null;
        NetworkServer.Destroy(obj);
        NetworkServer.Destroy(playerObj);
    }

    public override void OnServerReady(GameObject player)
    {
        // 2 is player and rock
        Assert.AreEqual(2, numStartServer);
        Assert.AreEqual(2, numStartClient);
        isDone = true;
    }
}
#pragma warning restore 618
