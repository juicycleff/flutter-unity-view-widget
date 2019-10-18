using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerGivesErrorWhenNoPlayerPrefabSet
{
    public class CustomNetworkManagerGivesErrorWhenNoPlayerPrefabSet : NetworkManager
    {
        public bool isDone = false;
        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);
            isDone = true;
        }
    }

    [UnityTest]
    public IEnumerator NetworkManagerGivesErrorWhenNoPlayerPrefabSetTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        GameObject nmObject = new GameObject();
        CustomNetworkManagerGivesErrorWhenNoPlayerPrefabSet nmanager = nmObject.AddComponent<CustomNetworkManagerGivesErrorWhenNoPlayerPrefabSet>();
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
        Object.Destroy(nmObject);
    }
}
#pragma warning restore 618
