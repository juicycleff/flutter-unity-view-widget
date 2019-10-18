using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

#pragma warning disable 618
public class PlayWithReadyState
{
    public bool isDone = false;
    static NetworkHash128 playerHash = NetworkHash128.Parse("abcd1");
    NetworkClient client1;
    NetworkClient client2;

    const short MsgId1 = 99;
    const short MsgId2 = 98;
    const short MsgId3 = 97;
    const short FromClientMsg1 = 95;
    const short FromClientMsg2 = 94;
    const short FromClientMsg3 = 93;

    public int numClientsConnected = 0;

    public int msg1Count = 0;
    public int msg3Count = 0;

    public List<string> actualListOfCallbacks = new List<string>();
    public List<string> resultListOfCallbacks = new List<string>()
    {
        "CheckClientsConnected:1",
        "CheckClientsConnected:2",
        "OnServerFromClientMsg1",
        "Msg1",
        "Msg1",
        "OnServerFromClientMsg2",
        "Msg3",
        "Msg3",
        "OnServerFromClientMsg3"
    };

    [UnityTest]
    public IEnumerator PlayWithReadyStateTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        GameObject nmObject = new GameObject();
        PlayWithReadyStateNetworkManager nmanager = nmObject.AddComponent<PlayWithReadyStateNetworkManager>();
        nmanager.networkAddress = "localhost";

        ClientScene.RegisterSpawnHandler(playerHash, PlayWithReadyStateNetworkManager.OnSpawnPlayer, PlayWithReadyStateNetworkManager.OnUnSpawnPlayer);

        NetworkServer.RegisterHandler(FromClientMsg1, OnServerFromClientMsg1);
        NetworkServer.RegisterHandler(FromClientMsg2, OnServerFromClientMsg2);
        NetworkServer.RegisterHandler(FromClientMsg3, OnServerFromClientMsg3);

        nmanager.StartServer();
        client1 = nmanager.StartClient();
        client1.RegisterHandler(MsgType.Connect, OnClient1Connect);
        client1.RegisterHandler(MsgId1, OnMsg1);
        client1.RegisterHandler(MsgId2, OnMsg2);
        client1.RegisterHandler(MsgId3, OnMsg3);

        // client2 is never ready, so should not recieve msgs
        client2 = new NetworkClient();
        client2.RegisterHandler(MsgType.Connect, OnClient2Connect);
        client2.RegisterHandler(MsgId1, OnMsg1);
        client2.RegisterHandler(MsgId2, OnMsg2);
        client2.RegisterHandler(MsgId3, OnMsg3);
        client2.RegisterHandler(MsgType.NotReady, OnNotReady);

        Assert.IsTrue(NetworkServer.active, "Server is not started");
        Assert.IsTrue(NetworkClient.active, "Client is not started");

        client2.Connect(NetworkManager.singleton.networkAddress, NetworkManager.singleton.networkPort);

        yield return null;

        while (!isDone)
        {
            yield return null;
        }

        CollectionAssert.AreEqual(resultListOfCallbacks, actualListOfCallbacks, "Wrong order of callbacks or some callback is missing");

        nmanager.StopServer();
        nmanager.StopClient();
        NetworkClient.ShutdownAll();
        UnityEngine.Object.Destroy(nmObject);
    }

    //need to handle this message as it is sent by NetworkManager,
    //but it can appear with delay - so we can't guarantee order
    void OnNotReady(NetworkMessage netMsg)
    {
    }

    // Server Flow
    // This block results in Msg1 printed twice (sent to same client twice)
    void OnServerFromClientMsg1(NetworkMessage netMsg)
    {
        // both clients are connected now
        actualListOfCallbacks.Add("OnServerFromClientMsg1");
        NetworkServer.SetClientReady(netMsg.conn);

        // this will go to only 1 client
        NetworkServer.SendToReady(null, MsgId1, new EmptyMessage());

        // this will go to only 1 client
        var tm = NetworkManager.singleton as PlayWithReadyStateNetworkManager;
        NetworkServer.SendToReady(tm.thePlayer, MsgId1, new EmptyMessage());
    }

    // This block results in Msg2 printed twice (sent to both clients)
    void OnServerFromClientMsg2(NetworkMessage netMsg)
    {
        actualListOfCallbacks.Add("OnServerFromClientMsg2");
        NetworkServer.SetAllClientsNotReady();

        // clients should NOT receive this
        NetworkServer.SendToReady(null, MsgId2, new EmptyMessage());

        // both clients SHOULD receive this
        NetworkServer.SendToAll(MsgId3, new EmptyMessage());
    }

    private void OnServerFromClientMsg3(NetworkMessage netMsg)
    {
        actualListOfCallbacks.Add("OnServerFromClientMsg3");
        isDone = true;
    }

    // Client Flow
    void OnClient1Connect(NetworkMessage netMsg)
    {
        numClientsConnected += 1;
        CheckClientsConnected(netMsg.conn);
    }

    void OnClient2Connect(NetworkMessage netMsg)
    {
        numClientsConnected += 1;
        CheckClientsConnected(netMsg.conn);
    }

    void CheckClientsConnected(NetworkConnection conn)
    {
        actualListOfCallbacks.Add("CheckClientsConnected:" + numClientsConnected);
        if (numClientsConnected == 2)
        {
            conn.Send(FromClientMsg1, new EmptyMessage());
        }
    }

    void OnMsg1(NetworkMessage netMsg)
    {
        actualListOfCallbacks.Add("Msg1");
        msg1Count += 1;
        if (msg1Count == 2)
        {
            netMsg.conn.Send(FromClientMsg2, new EmptyMessage());
        }
    }

    void OnMsg2(NetworkMessage netMsg)
    {
        // should not ever be received!
        Assert.Fail("This message should not be received: Msg2 " + netMsg.conn.connectionId);
    }

    void OnMsg3(NetworkMessage netMsg)
    {
        actualListOfCallbacks.Add("Msg3");
        msg3Count += 1;
        if (msg3Count == 2)
        {
            netMsg.conn.Send(FromClientMsg3, new EmptyMessage());
        }
    }

    public class PlayWithReadyStateNetworkManager : NetworkManager
    {
        public GameObject thePlayer;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            thePlayer = (GameObject)OnSpawnPlayer(Vector3.zero, playerHash);
            NetworkServer.AddPlayerForConnection(conn, thePlayer, playerControllerId, playerHash);
        }

        public static GameObject OnSpawnPlayer(Vector3 pos, NetworkHash128 assetId)
        {
            try
            {
                GameObject thePlayer = new GameObject();
                thePlayer.name = "PlayWithReadyStatePrefab";
                thePlayer.AddComponent<NetworkIdentity>();
                return thePlayer;
            }
            catch (Exception e)
            {
                Assert.Fail("Spawn exception " + e);
                return null;
            }
        }

        public static void OnUnSpawnPlayer(GameObject unspawned)
        {
            Destroy(unspawned);
        }
    }
}
#pragma warning restore 618
