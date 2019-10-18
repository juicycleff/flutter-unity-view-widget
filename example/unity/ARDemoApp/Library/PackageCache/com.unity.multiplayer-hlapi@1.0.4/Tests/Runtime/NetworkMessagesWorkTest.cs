using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkMessagesWorkTest
{
    string m_ip = "127.0.0.1";
    int m_port0 = 8888;

    bool isDone = false;
    NetworkClient client;

    [UnityTest]
    public IEnumerator NetworkMessagesWorkCheck()
    {
        NetworkServer.Reset();

        ConnectionConfig connectionConfig = new ConnectionConfig();
        connectionConfig.AddChannel(QosType.Reliable);
        connectionConfig.AddChannel(QosType.AllCostDelivery);
        connectionConfig.AcksType = ConnectionAcksType.Acks96;
        NetworkServer.Configure(connectionConfig, 4);

        NetworkServer.RegisterHandler(MessageTypes.CSUpdateMsgType, OnClientUpdate);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnect);
        NetworkServer.RegisterHandler(MessageTypes.CSHelloMsgType, OnClientHello);

        bool isServerStarted = NetworkServer.Listen(m_ip, m_port0);
        Assert.IsTrue(isServerStarted, "Server is not started.");

        client = new NetworkClient();
        client.Configure(connectionConfig, 4);
        client.Connect(m_ip, m_port0);
        while (!client.isConnected)
        {
            yield return null;
        }

        client.RegisterHandler(MessageTypes.SCUpdateMsgType, OnServerUpdate);

        CSHelloMessage msg = new CSHelloMessage(client.connection.connectionId);
        client.Send(MessageTypes.CSHelloMsgType, msg);

        while (!isDone)
        {
            yield return null;
        }
    }

    public void SendServerUpdateMessage()
    {
        NetworkServer.SendToAll(MessageTypes.SCUpdateMsgType,
            new SCUpdateMessage((byte)NetworkServer.serverHostId,
                NetworkServer.active));
    }

    public void SendClientUpdateMessage(NetworkClient client)
    {
        Vector3 vec = new Vector3(1, 1, 1);
        client.Send(MessageTypes.CSUpdateMsgType,
            new CSUpdateMessage((byte)client.connection.hostId, vec));
    }

    public void OnClientHello(NetworkMessage msg)
    {
        Assert.AreEqual(msg.msgType, MessageTypes.CSHelloMsgType);
        SendServerUpdateMessage();
    }

    public void OnClientConnect(NetworkMessage msg)
    {
        Assert.AreEqual(msg.msgType, MsgType.Connect);
    }

    public void OnServerUpdate(NetworkMessage msg)
    {
        Assert.AreEqual(msg.msgType, MessageTypes.SCUpdateMsgType);
        SendClientUpdateMessage(client);
    }

    public void OnClientUpdate(NetworkMessage msg)
    {
        Assert.AreEqual(msg.msgType, MessageTypes.CSUpdateMsgType);
        msg.reader.SeekZero();
        Vector3 recVecor = msg.ReadMessage<CSUpdateMessage>().position;
        Vector3 vec = new Vector3(1, 1, 1);
        Assert.AreEqual(vec, recVecor);
        isDone = true;
    }
}
#pragma warning restore 618
