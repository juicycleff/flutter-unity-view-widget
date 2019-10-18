using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerStillWorksWhenUserUseStartAndAwake
{
    public class CustomNetworkManagerWithAwakeAndStart : NetworkManager
    {
        public bool isDone = false;
        public  int counter;

        public void Awake()
        {
            counter++;
        }

        public void Start()
        {
            counter++;
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            counter++;
            isDone = true;
        }
    }

    [UnityTest]
    public IEnumerator NetworkManagerStillWorksWhenUserUseStartAndAwakeTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        GameObject nmObject = new GameObject();
        CustomNetworkManagerWithAwakeAndStart nmanager = nmObject.AddComponent<CustomNetworkManagerWithAwakeAndStart>();
        nmanager.networkAddress = "localhost";

        yield return null;
        Assert.IsNull(nmanager.playerPrefab, "Player prefab field is set on NetworkManager, but shouldn't be");

        nmanager.StartHost();
        yield return null;

        LogAssert.Expect(LogType.Error, "The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
        Assert.IsTrue(NetworkServer.active, "Server is not active after StartHost");
        Assert.IsTrue(NetworkClient.active, "Client is not active after StartHost");
        yield return null;

        while (!nmanager.isDone)
        {
            yield return null;
        }

        nmanager.StopHost();
        Assert.AreEqual(3, nmanager.counter, "Start or Awake was not called on NetwotkManager");
        Object.Destroy(nmObject);
    }
}
#pragma warning restore 618
