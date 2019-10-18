using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkDiscoveryWorks
{
    const string testData = "TESTING";
    GameObject clientObj;
    GameObject serverObj;

    [UnityTest]
    public IEnumerator NetworkDiscoveryWorksTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        bool result;

        serverObj = new GameObject();
        var serverDiscovery = serverObj.AddComponent<NetworkDiscovery>();
        serverDiscovery.useNetworkManager = false;
        serverDiscovery.broadcastData = testData;
        result = serverDiscovery.Initialize();
        Assert.IsTrue(result, "serverDiscovery.Initialize() returned false");

        result = serverDiscovery.StartAsServer();
        Assert.IsTrue(result, "serverDiscovery.StartAsServer() returned false");

        clientObj = new GameObject();
        var clientDiscovery = clientObj.AddComponent<NetworkDiscovery>();

        result = clientDiscovery.Initialize();
        Assert.IsTrue(result, "clientDiscovery.Initialize() returned false");

        result = clientDiscovery.StartAsClient();
        Assert.IsTrue(result, "clientDiscovery.StartAsClient() returned false");


        while (clientDiscovery.broadcastsReceived.Count <= 0)
        {
            yield return null;
        }

        foreach (var dis in clientDiscovery.broadcastsReceived.Values)
        {
            char[] chars = new char[dis.broadcastData.Length / sizeof(char)];
            System.Buffer.BlockCopy(dis.broadcastData, 0, chars, 0, dis.broadcastData.Length);
            var str = new string(chars);

            Assert.AreEqual(testData, str, "Sent and received data are different");
        }
        serverDiscovery.StopBroadcast();
        clientDiscovery.StopBroadcast();
        Object.Destroy(serverDiscovery);
        Object.Destroy(clientDiscovery);
    }
}
#pragma warning restore 618
