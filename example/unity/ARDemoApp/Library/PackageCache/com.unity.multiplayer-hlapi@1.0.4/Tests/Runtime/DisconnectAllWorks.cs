using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class DisconnectAllWorks
{
    int kListenPort = 7073;
    int maxConnections = 10;
    int numClients = 5;
    int clientsConnected = 0;
    int serverConnections = 0;
    int clientsDisccnnected = 0;

    [UnityTest]
    public IEnumerator DisconnectAllWorksTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        NetworkServer.Configure(config, maxConnections);
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        for (int i = 0; i < numClients; ++i)
        {
            NetworkClient myClient = new NetworkClient();
            if (!myClient.Configure(config, maxConnections))
            {
                Assert.Fail("Client configure failed");
            }
            myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
            myClient.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);
            myClient.Connect("127.0.0.1", kListenPort);
        }

        while (serverConnections != numClients || clientsConnected != numClients)
        {
            yield return null;
        }
        NetworkServer.DisconnectAll();

        while (clientsDisccnnected != numClients)
        {
            yield return null;
        }

        Assert.IsTrue(NetworkServer.active, "NetworkServer should still be active after DisconnectAll()");
    }

    public void OnServerConnected(NetworkMessage netMsg)
    {
        serverConnections += 1;
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        clientsConnected += 1;
    }

    public void OnClientDisconnected(NetworkMessage netMsg)
    {
        clientsDisccnnected += 1;
    }
}
#pragma warning restore 618
