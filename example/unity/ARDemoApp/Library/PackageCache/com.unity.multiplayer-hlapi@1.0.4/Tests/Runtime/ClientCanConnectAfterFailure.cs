using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class ClientCanConnectAfterFailure
{
    int kListenPort = 7073;
    NetworkClient client1;
    NetworkClient client2;
    bool isClientConnected = false;
    bool serverRecievedConnection = false;
    ConnectionConfig config;

    bool isTestDone;

    [UnityTest]
    public IEnumerator ClientCanConnectAfterFailureTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();
        
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);

        config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableFragmented);
        if (!NetworkServer.Configure(config, 10))
        {
            Assert.Fail("Server configure failed");
        }

        // Mismatched channels between client 1 and server, so connect will fail with CRCMismatch error
        ConnectionConfig customConfig = new ConnectionConfig();
        customConfig.AddChannel(QosType.UnreliableFragmented);

        client1 = new NetworkClient();
        if (!client1.Configure(customConfig, 10))
        {
            Assert.Fail("Client1 configure failed");
        }

        client1.RegisterHandler(MsgType.Connect, OnClient1Connected);
        client1.RegisterHandler(MsgType.Disconnect, OnClient1Disconnected);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        LogAssert.Expect(LogType.Error, "UNet Client Disconnect Error: CRCMismatch");
        client1.Connect("127.0.0.1", kListenPort);

        while (!serverRecievedConnection || !isClientConnected)
        {
            yield return null;
        }

        NetworkServer.DisconnectAll();

        while (!isTestDone)
        {
            yield return null;
        }
    }

    public void OnServerConnected(NetworkMessage netMsg)
    {
        serverRecievedConnection = true;
    }

    public void OnClient1Connected(NetworkMessage netMsg)
    {
        Assert.Fail("Client1 connection should not happen");
    }

    public void OnClient1Disconnected(NetworkMessage netMsg)
    {
        client2 = new NetworkClient();
        if (!client2.Configure(config, 10))
        {
            Assert.Fail("Client2 configure failed");
        }

        client2.RegisterHandler(MsgType.Connect, OnClient2Connected);
        client2.RegisterHandler(MsgType.Disconnect, OnClient2Disconnected);
        client2.Connect("127.0.0.1", kListenPort);
    }

    public void OnClient2Connected(NetworkMessage netMsg)
    {
        isClientConnected = true;
    }

    public void OnClient2Disconnected(NetworkMessage netMsg)
    {
        Assert.IsTrue(serverRecievedConnection);
        Assert.IsTrue(isClientConnected);
        isTestDone = true;
    }
}
#pragma warning restore 618
