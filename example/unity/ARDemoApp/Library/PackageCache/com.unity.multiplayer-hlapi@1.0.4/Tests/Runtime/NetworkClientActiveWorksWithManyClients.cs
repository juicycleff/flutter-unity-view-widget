using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkClientActiveWorksWithManyClients
{
    int kListenPort = 7073;
    bool isTestDone;
    int m_ClientConnectionCount = 0;

    [UnityTest]
    public IEnumerator NetworkClientActiveWorksWithManyClientsTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        for (int i = 0; i < 3; ++i)
        {
            NetworkClient myClient = new NetworkClient();
            if (!myClient.Configure(config, 10))
            {
                Assert.Fail("Client configure failed");
            }
            myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        }

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        Assert.IsFalse(NetworkClient.active, "NetworkClient.active should be false as there is no clients yet");
        NetworkClient.allClients[0].Connect("127.0.0.1", kListenPort);
        Assert.IsTrue(NetworkClient.active, "NetworkClient.active should be true as there is one client");
        NetworkClient.allClients[1].Connect("127.0.0.1", kListenPort);
        NetworkClient.allClients[2].Connect("127.0.0.1", kListenPort);

        while (!isTestDone)
        {
            yield return null;
        }
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        if (++m_ClientConnectionCount == 3)
        {
            NetworkClient.allClients[1].Shutdown();
            Assert.IsTrue(NetworkClient.active, "NetworkClient.active should be true as there are two clients");
            NetworkClient.allClients[0].Shutdown();
            Assert.IsTrue(NetworkClient.active, "NetworkClient.active should be true as there is one client");
            // The 2nd basic client instance is now 0, since they are removed from the list on shut down...
            NetworkClient.allClients[0].Shutdown();
            Assert.IsFalse(NetworkClient.active, "NetworkClient.active should be false as all clients were shut down");
            isTestDone = true;
        }
    }
}
#pragma warning restore 618
