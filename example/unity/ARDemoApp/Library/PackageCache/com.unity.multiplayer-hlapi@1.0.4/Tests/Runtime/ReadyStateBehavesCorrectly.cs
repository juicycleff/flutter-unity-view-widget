using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking;

#pragma warning disable 618
public class ReadyStateBehavesCorrectly
{
    int kListenPort = 7073;
    bool isDone = false;
    ConnectionConfig config;
    NetworkClient myClient;
    NetworkClient localClient;

    private int numClientConnects = 0;
    private bool doDisconnect = false;

    [UnityTest]
    public IEnumerator ReadyStateBehavesCorrectlyTest()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();

        NetworkServer.RegisterHandler(MsgType.Ready, OnServerReady);

        config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        myClient = new NetworkClient();
        if (!myClient.Configure(config, 10))
        {
            Assert.Fail("Client configure failed");
        }

        myClient.RegisterHandler(MsgType.Connect, OnClient1Connected);
        myClient.RegisterHandler(MsgType.Disconnect, OnClient1Disconnected);
        myClient.Connect("127.0.0.1", kListenPort);

        while (!isDone)
        {
            yield return null;
            if (doDisconnect)
            {
                ClientDisconnect();
                doDisconnect = false;
            }
        }
    }

    public void OnServerReady(NetworkMessage netMsg)
    {
        if (numClientConnects == 1)
        {
            // server disconnects client
            netMsg.conn.Disconnect();
        }
        else if (numClientConnects <= 3)
        {
            // client will disconnect from server
            doDisconnect = true;
        }
    }

    public void OnClient1Connected(NetworkMessage netMsg)
    {
        numClientConnects += 1;
        ClientScene.Ready(netMsg.conn);
    }

    public void OnClient1Disconnected(NetworkMessage netMsg)
    {
        //is called only for clients 1 and 3
        if (numClientConnects == 1)
        {
            myClient.Connect("127.0.0.1", kListenPort);
        }

        if (numClientConnects == 3)
        {
            isDone = true;
        }
    }

    private void ClientDisconnect()
    {
        if (numClientConnects == 2)
        {
            myClient.Disconnect();
            localClient = ClientScene.ConnectLocalServer();
            localClient.RegisterHandler(MsgType.Connect, OnClient1Connected);
            localClient.RegisterHandler(MsgType.Disconnect, OnClient1Disconnected);
        }
        else //for numClientConnects == 3
        {
            localClient.Disconnect();
        }
    }
}
#pragma warning restore 618
