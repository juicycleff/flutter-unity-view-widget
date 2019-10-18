using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

#pragma warning disable 618
public class TwoInstancesOfServerSimpleWorkInOneProcess
{
    NetworkServerSimple server1;
    NetworkServerSimple server2;
    NetworkClient client1;
    NetworkClient client2;

    const int port1 = 7003;
    const int port2 = 7004;

    const short TestMsgId = 1000;
    int actualMsgCount = 0;
    int expectedMsgCount = 0;

    [UnityTest]
    public IEnumerator TwoInstancesOfServerSimpleWorkInOneProcessTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        server1 = new NetworkServerSimple();
        server1.RegisterHandler(MsgType.Connect, OnServerConnect1);
        server1.RegisterHandler(TestMsgId, OnServerMsg1);

        server2 = new NetworkServerSimple();
        server2.RegisterHandler(MsgType.Connect, OnServerConnect2);
        server2.RegisterHandler(TestMsgId, OnServerMsg2);

        Assert.IsTrue(server1.Listen(port1), "Server1 Listen failed");
        Assert.IsTrue(server2.Listen(port2), "Server2 Listen failed");

        client1 = new NetworkClient();
        client1.RegisterHandler(MsgType.Connect, OnClientConnect1);
        client1.RegisterHandler(TestMsgId, OnClientMsg1);

        client2 = new NetworkClient();
        client2.RegisterHandler(MsgType.Connect, OnClientConnect2);
        client2.RegisterHandler(TestMsgId, OnClientMsg2);

        client1.Connect("localhost", port1);
        client2.Connect("localhost", port2);

        while (actualMsgCount != expectedMsgCount)
        {
            yield return null;
        }
    }

    void OnServerConnect1(NetworkMessage netMsg)
    {
        actualMsgCount += 1;
    }

    void OnServerConnect2(NetworkMessage netMsg)
    {
        actualMsgCount += 1;
    }

    void OnServerMsg1(NetworkMessage netMsg)
    {
        actualMsgCount += 1;
        netMsg.conn.Send(TestMsgId, new EmptyMessage());
    }

    void OnServerMsg2(NetworkMessage netMsg)
    {
        actualMsgCount += 1;
        netMsg.conn.Send(TestMsgId, new EmptyMessage());
    }

    void OnClientConnect1(NetworkMessage netMsg)
    {
        netMsg.conn.Send(TestMsgId, new EmptyMessage());
    }

    void OnClientConnect2(NetworkMessage netMsg)
    {
        netMsg.conn.Send(TestMsgId, new EmptyMessage());
    }

    void OnClientMsg1(NetworkMessage netMsg)
    {
        actualMsgCount += 1;
    }

    void OnClientMsg2(NetworkMessage netMsg)
    {
        actualMsgCount += 1;
    }
}
#pragma warning restore 618
