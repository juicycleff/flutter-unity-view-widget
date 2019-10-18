using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkObserversObjectsWork : SpawningTestBase
{
    private GameObject observerFar, observerClose;
    bool isDone = false;

    [UnityTest]
    public IEnumerator NetworkObserversObjectsCheck()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();
        TestSetup();
        StartServer();
        StartLocalClient();

        observerClose = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        observerClose.AddComponent<NetworkProximityChecker>();
        observerClose.gameObject.name = "RockClose";

        observerFar = GameObject.Instantiate(GetBridgeScript.rocketPrefab, new Vector3(100, 100, 100), Quaternion.identity);
        observerFar.AddComponent<NetworkProximityChecker>();
        observerFar.gameObject.name = "RockFar";

        NetworkServer.Spawn(observerClose);
        NetworkServer.Spawn(observerFar);

        while (!isDone)
        {
            yield return null;
        }

        ClientScene.DestroyAllClientObjects();
        yield return null;
        NetworkServer.Destroy(observerClose);
        NetworkServer.Destroy(observerFar);
    }

    public override void OnServerReady(GameObject player)
    {
        // add physics collider to player so proximity check will find it
        player.AddComponent<Rigidbody>();
        var box = player.AddComponent<BoxCollider>();
        box.bounds.SetMinMax(Vector3.zero, new Vector3(1, 1, 1));

        // rebuild observer lists
        observerClose.GetComponent<NetworkIdentity>().RebuildObservers(false);
        observerFar.GetComponent<NetworkIdentity>().RebuildObservers(false);
    }

    public override void OnClientReady(short playerId)
    {
        Assert.AreEqual(1, observerClose.GetComponent<NetworkIdentity>().observers.Count, "Player sees observerClose object as it is close");
        Assert.AreEqual(0, observerFar.GetComponent<NetworkIdentity>().observers.Count, "Player doesn't see observerFar object as it is far away");

        observerFar.transform.position = Vector3.zero;
        observerFar.GetComponent<NetworkIdentity>().RebuildObservers(false);
        Assert.AreEqual(1, observerFar.GetComponent<NetworkIdentity>().observers.Count, "Player sees observerFar object as it is close now");

        isDone = true;
    }
}
#pragma warning restore 618
