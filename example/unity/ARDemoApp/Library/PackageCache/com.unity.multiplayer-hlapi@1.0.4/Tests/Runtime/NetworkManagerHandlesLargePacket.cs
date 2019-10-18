using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerHandlesLargePacket
{
    public bool isDone = false;
    const short MsgIdBig = 99;
    const int MsgSize = 10397;// increasing it to bigger number will cause failure

    class BigMessage : MessageBase
    {
        public byte[] data;
    }

    [UnityTest]
    public IEnumerator NetworkManagerHandlesLargePacketTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        GameObject nmObject = new GameObject();
        NetworkManagerWithLargePacket nmanager = nmObject.AddComponent<NetworkManagerWithLargePacket>();
        nmanager.networkAddress = "localhost";
        nmanager.autoCreatePlayer = false;

        nmanager.customConfig = true;
        nmanager.connectionConfig.MinUpdateTimeout = 1;
        nmanager.connectionConfig.MaxSentMessageQueueSize = 200;
        nmanager.channels.Add(QosType.UnreliableFragmented);

        nmanager.StartServer();
        nmanager.StartClient();
        yield return null;
        NetworkServer.RegisterHandler(MsgIdBig, OnBigMessage);

        yield return null;

        Assert.IsTrue(NetworkServer.active, "Server is not started");
        Assert.IsTrue(NetworkClient.active, "Client is not started");
        yield return null;

        while (!isDone)
        {
            yield return null;
        }

        NetworkManager.singleton.StopServer();
        NetworkManager.singleton.StopClient();

        Object.Destroy(nmObject);
    }

    public void OnBigMessage(NetworkMessage netMsg)
    {
        Debug.Log("OnBigMessage");
        var bigMsg = netMsg.ReadMessage<BigMessage>();
        Assert.AreEqual(MsgSize, bigMsg.data.Length);
        isDone = true;
    }

    public class NetworkManagerWithLargePacket : NetworkManager
    {
        public override void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("OnClient Connect");
            base.OnClientConnect(conn);

            var bigMsg = new BigMessage();
            bigMsg.data = new byte[MsgSize];

            var writer = new NetworkWriter();
            writer.StartMessage(MsgIdBig);
            bigMsg.Serialize(writer);
            writer.FinishMessage();
            var data = writer.ToArray();

            client.SendBytes(data, data.Length, 0);
        }
    }
}
#pragma warning restore 618
