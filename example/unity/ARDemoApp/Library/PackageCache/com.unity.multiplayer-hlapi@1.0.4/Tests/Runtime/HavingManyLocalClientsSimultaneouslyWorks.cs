using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking;

#pragma warning disable 618
public class HavingManyLocalClientsSimultaneouslyWorks
{
    int kListenPort = 7073;
    int maxConnections = 100;
    const int kMsgTest = 555;
    public class TestMessage : MessageBase
    {
        public int number;
        public string str;
    };
    private int numClients = 15; // Maximum hosts per process is 16 so 15 client + 1 server
    int clientsConnected = 0;
    int serverConnections = 0;
    int msgCountClientRecieved = 0;

    [UnityTest]
    public IEnumerator HavingManyLocalClientsSimultaneouslyWorksTest()
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
            myClient.RegisterHandler(MsgType.Error, OnError);
            myClient.RegisterHandler(kMsgTest, OnClientTest);
            myClient.Connect("127.0.0.1", kListenPort);
        }

        while (serverConnections != numClients || clientsConnected != numClients)
        {
            yield return null;
        }

        TestMessage testMsg = new TestMessage();
        testMsg.number = 98756;
        testMsg.str = "teststring";

        NetworkServer.SendToAll(kMsgTest, testMsg);

        while (msgCountClientRecieved != numClients)
        {
            yield return null;
        }
    }

    public void OnServerConnected(NetworkMessage netMsg)
    {
        serverConnections += 1;
    }

    public void OnClientTest(NetworkMessage netMsg)
    {
        msgCountClientRecieved += 1;
        var receivedMessage = netMsg.reader.ReadMessage<TestMessage>();
        StringAssert.IsMatch("teststring", receivedMessage.str, "Received message has invalid sting");
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        clientsConnected += 1;
    }

    public void OnError(NetworkMessage netMsg)
    {
        ErrorMessage msg = netMsg.ReadMessage<ErrorMessage>();
        Assert.Fail("Error: " + msg.errorCode);
    }
}
#pragma warning restore 618