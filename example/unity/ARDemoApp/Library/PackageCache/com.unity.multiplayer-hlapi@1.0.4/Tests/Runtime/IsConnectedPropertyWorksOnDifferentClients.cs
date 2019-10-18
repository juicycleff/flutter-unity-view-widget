using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class IsConnectedPropertyWorksOnDifferentClients
{
    int kListenPort = 7073;
    NetworkClient remoteClient;
    NetworkClient localClient = null;
    bool isTestDone;

    [UnityTest]
    public IEnumerator IsConnectedPropertyWorksOnDifferentClientsTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        remoteClient = new NetworkClient();
        if (!remoteClient.Configure(config, 10))
        {
            Assert.Fail("Client configure failed");
        }

        remoteClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        Assert.IsFalse(remoteClient.isConnected);


        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        remoteClient.Connect("127.0.0.1", kListenPort);

        while (!isTestDone)
        {
            yield return null;
        }
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        Assert.IsTrue(remoteClient.isConnected);

        if (localClient == null)
        {
            localClient = ClientScene.ConnectLocalServer();
            Assert.IsTrue(localClient.isConnected);

            remoteClient.Disconnect();
            localClient.Disconnect();

            Assert.IsFalse(remoteClient.isConnected);
            Assert.IsFalse(localClient.isConnected);

            isTestDone = true;
        }
    }
}
#pragma warning restore 618
