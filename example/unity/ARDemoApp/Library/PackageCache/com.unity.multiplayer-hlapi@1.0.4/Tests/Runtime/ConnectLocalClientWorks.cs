using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class ConnectLocalClientWorks
{
    const int kMsgTest = 555;
    const int kMsgTest2 = 556;
    bool isTestDone;

    public class TestMessage : MessageBase
    {
        public int number;
        public string str;
    };

    [UnityTest]
    public IEnumerator ConnectLocalClientWorksTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);
        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        NetworkServer.RegisterHandler(kMsgTest, OnServerTestMsg);

        NetworkServer.Listen(9999);
        NetworkClient client = ClientScene.ConnectLocalServer();

        client.RegisterHandler(MsgType.Connect, OnClientConnected);
        client.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);
        client.RegisterHandler(kMsgTest, OnClientTestMsg);
        client.RegisterHandler(kMsgTest2, OnClientTestMsg2);

        while (!isTestDone)
        {
            yield return null;
        }
    }

    public void OnServerConnected(NetworkMessage netMsg)
    {
        Debug.Log("Server received client connection.");
    }

    public void OnAddPlayer(NetworkMessage netMsg)
    {
        GameObject go = new GameObject();
        go.AddComponent<NetworkIdentity>();
        NetworkServer.AddPlayerForConnection(netMsg.conn, go, 0);

        TestMessage outMsg = new TestMessage();
        outMsg.number = 99;
        outMsg.str = "addPlayer";
        NetworkServer.SendToAll(kMsgTest2, outMsg);
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        Debug.Log("Client connected to server.");

        TestMessage msg = new TestMessage();
        msg.number = 77;
        msg.str = "testFromClient";

        NetworkClient.allClients[0].Send(kMsgTest, msg);
    }

    public void OnServerTestMsg(NetworkMessage netMsg)
    {
        TestMessage msg = netMsg.ReadMessage<TestMessage>();
        Assert.AreEqual(77, msg.number);
        Assert.AreEqual("testFromClient", msg.str);

        TestMessage outMsg = new TestMessage();
        outMsg.number = 99;
        outMsg.str = "testFromServer";

        NetworkServer.SendToAll(kMsgTest, outMsg);
    }

    public void OnClientTestMsg(NetworkMessage netMsg)
    {
        Debug.Log("Client received test message");
        TestMessage msg = netMsg.ReadMessage<TestMessage>();
        Assert.AreEqual(99, msg.number);
        Assert.AreEqual("testFromServer", msg.str);
        ClientScene.AddPlayer(netMsg.conn, 0);
    }

    public void OnClientTestMsg2(NetworkMessage netMsg)
    {
        Assert.AreEqual(ClientScene.localPlayers.Count, 1);
        NetworkClient.allClients[0].Disconnect();
    }

    public void OnClientDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Client disconnected from server.");
        isTestDone = true;
    }
}
#pragma warning restore 618
