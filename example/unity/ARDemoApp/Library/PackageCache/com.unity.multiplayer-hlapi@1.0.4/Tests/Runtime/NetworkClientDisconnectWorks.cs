using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkClientDisconnectWorks
{
    int kListenPort = 7073;
    private const int totalConnects = 2;
    private int numConnects = 0;
    bool isClientConnected = false;
    bool isServerRecivedConnection = false;
    ConnectionConfig config;
    NetworkClient myClient;

    [UnityTest]
    public IEnumerator NetworkClientDisconnectWorksTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);

        config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        myClient = new NetworkClient();
        if (!myClient.Configure(config, 20))
        {
            Assert.Fail("Client configure failed");
        }

        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }
        myClient.Connect("127.0.0.1", kListenPort);

        while (numConnects != totalConnects)
        {
            if (isServerRecivedConnection && isClientConnected)
            {
                myClient.Disconnect();
                isClientConnected = false;
            }
            yield return null;
        }
    }

    public void OnServerConnected(NetworkMessage netMsg)
    {
        isServerRecivedConnection = true;
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        isClientConnected = true;
    }

    public void OnClientDisconnected(NetworkMessage netMsg)
    {
        numConnects++;
        Assert.IsTrue(isServerRecivedConnection);
        isServerRecivedConnection = false;
        isClientConnected = false;
        myClient = new NetworkClient();
        if (!myClient.Configure(config, 20))
        {
            Assert.Fail("Client configure failed");
        }
        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        myClient.Connect("127.0.0.1", kListenPort);
    }
}
#pragma warning restore 618
