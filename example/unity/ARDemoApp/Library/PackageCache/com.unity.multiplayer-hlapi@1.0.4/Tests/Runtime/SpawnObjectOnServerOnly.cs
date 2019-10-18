using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class SpawnObjectOnServerOnly : SpawningTestBase
{
    GameObject obj;

    [UnityTest]
    public IEnumerator SpawnObjectOnServerOnlyTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();
        TestSetup();
        StartServer();

        obj = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(obj);
        yield return null;

        // 1 is rock, there is no player
        Assert.AreEqual(1, numStartServer);

        NetworkServer.Destroy(obj);
    }
}
#pragma warning restore 618
