using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerStopServerAndClientWorks
{
    public class TestNetworkManagerStop : NetworkManager
    {
        public bool isDone;

        public override void OnClientConnect(NetworkConnection conn)
        {
            StopServer();
            StopClient();

            Assert.IsFalse(NetworkServer.active, "Server should not be active at this point");
            Assert.IsFalse(NetworkClient.active, "Client should not be active at this point");
            isDone = true;
        }
    }

    [UnityTest]
    public IEnumerator NetworkManagerStopServerAndClientWorksTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        GameObject nmObject = new GameObject();
        TestNetworkManagerStop nmanager = nmObject.AddComponent<TestNetworkManagerStop>();

        nmanager.networkAddress = "localhost";
        nmanager.StartServer();
        nmanager.StartClient();
        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not started");
        Assert.IsTrue(NetworkClient.active, "Client is not started");
        yield return null;

        while (!nmanager.isDone)
        {
            yield return null;
        }

        NetworkManager.singleton.StopServer();
        NetworkManager.singleton.StopClient();

        Object.Destroy(nmObject);
    }
}
#pragma warning restore 618
