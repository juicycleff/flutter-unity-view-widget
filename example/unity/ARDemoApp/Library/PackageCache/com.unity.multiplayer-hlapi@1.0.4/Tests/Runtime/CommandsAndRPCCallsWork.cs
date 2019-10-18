using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

#pragma warning disable 618
public class CommandsAndRPCCallsWork
{
    int kListenPort = 7073;
    NetworkClient myClient;
    static bool isTestDone;

    static NetworkHash128 playerHash = NetworkHash128.Parse("abcd1");

    public static GameObject OnSpawnPlayer(Vector3 pos, NetworkHash128 assetId)
    {
        try
        {
            GameObject serverPlayer = new GameObject();
            serverPlayer.name = "MyPlayer";
            serverPlayer.AddComponent<CommandTestPlayerBehaviourExtra>();
            serverPlayer.AddComponent<CommandTestPlayerBehaviour>();
            return serverPlayer;
        }
        catch (Exception e)
        {
            Assert.Fail("Spawn exception " + e);
            return null;
        }
    }

    public static void OnUnSpawnPlayer(GameObject unspawned)
    {
        Object.Destroy(unspawned);
    }

    [UnityTest]
    public IEnumerator CommandsAndRPCCallsWorkTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);// this test requires correct sequence of packets.
        config.AddChannel(QosType.Unreliable);

        myClient = new NetworkClient();
        if (!myClient.Configure(config, 10))
        {
            Assert.Fail("Client configure failed");
        }

        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        myClient.RegisterHandler(MsgType.Error, OnClientError);
        ClientScene.RegisterSpawnHandler(playerHash, OnSpawnPlayer, OnUnSpawnPlayer);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        myClient.Connect("127.0.0.1", kListenPort);

        while (!isTestDone)
        {
            yield return null;
        }
    }

    public void OnAddPlayer(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<UnityEngine.Networking.NetworkSystem.AddPlayerMessage>();

        GameObject obj = OnSpawnPlayer(Vector3.zero, playerHash);
        NetworkServer.AddPlayerForConnection(netMsg.conn, obj, msg.playerControllerId, playerHash);

        CommandTestPlayerBehaviour beh = obj.GetComponent<CommandTestPlayerBehaviour>();
        Assert.IsNotNull(beh, "No component CommandTestPlayerBehaviour");

        string args =  "foo";
        beh.RpcTestOnClient(args);

        beh.TargetTestOnOne(netMsg.conn, "one");
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        ClientScene.AddPlayer(netMsg.conn, 1);
    }

    public void OnClientError(NetworkMessage netMsg)
    {
        Assert.Fail("Connect Error");
    }

    // extra NetworkBehaviour component on the player, to check for multiple NetworkBehaviour issues
    public class CommandTestPlayerBehaviourExtra : NetworkBehaviour
    {
        [SyncVar]
        int extraData;
    }

    public class CommandTestPlayerBehaviour : NetworkBehaviour
    {
        public struct Inner
        {
            public string aString;
            public double aDouble;
        }
        public struct Outer
        {
            public float aFloat;
            public int aInt;
            public Inner aInner;
        }

        public static int numEvents = 0;

        public delegate void IntegerEventDelegate(int value);

        private int testInt = 77;
        private float testFloat = 55.5f;
        private int testValue = 100;
        private int testCmdCount = 0;
        private int testCmdValidate = 0;

        [SyncEvent]
        public event IntegerEventDelegate EventDoInt1;

        [SyncEvent]
        public event IntegerEventDelegate EventDoInt2;

        void Awake()
        {
            // test multiple events in this script
            EventDoInt1 += OnIntegerEvent1;
            EventDoInt1 += OnIntegerEvent2;

            EventDoInt2 += OnIntegerEvent2;
        }

        void OnIntegerEvent1(int value)
        {
            Debug.Log("OnIntegerEvent1");
            Assert.AreEqual(testValue, value);
            numEvents += 1;
        }

        void OnIntegerEvent2(int value)
        {
            Debug.Log("OnIntegerEvent2");
            Assert.AreEqual(testValue, value);
            numEvents += 1;
        }

        private void Update()
        {
            // 3 = 2 events from EventDo1 + 1 event from EventDo2
            if (numEvents == 3 && isClient)
            {
                // this tests that all commands arrive in the correct order
                CmdCount(testCmdCount++);
                if (testCmdCount == 100)
                {
                    isTestDone = true;
                }

                Outer outer = new Outer();
                outer.aInt = 99;
                outer.aInner = new Inner();
                outer.aInner.aDouble = 1.2;
                CmdDoOuter(outer);
            }
        }

        [Command]
        void CmdCount(int count)
        {
            Assert.AreEqual(count, testCmdValidate++);
        }

        [Command]
        void CmdDoOuter(Outer outer)
        {
            Assert.AreEqual(99, outer.aInt);
            Assert.AreEqual(1.2, outer.aInner.aDouble, 0.001);
        }

        [Command]
        public void CmdTestCommandOnServer(int arg1, float arg2)
        {
            Assert.AreEqual(testInt, arg1);
            Assert.AreEqual(testFloat, arg2);

            if (EventDoInt1 != null)
            {
                EventDoInt1(testValue);
            }

            if (EventDoInt2 != null)
            {
                EventDoInt2(testValue);
            }
        }

        [TargetRpc]
        public void TargetTestOnOne(NetworkConnection target, string arg)
        {
            Assert.AreEqual(arg, "one");
        }

        [ClientRpc]
        public void RpcTestOnClient(string arg)
        {
            Assert.AreEqual(arg, "foo");
            CmdTestCommandOnServer(testInt, testFloat);
        }
    }
}
#pragma warning restore 618
