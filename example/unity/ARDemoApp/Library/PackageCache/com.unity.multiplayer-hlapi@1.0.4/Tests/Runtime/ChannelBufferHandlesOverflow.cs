using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class ChannelBufferHandlesOverflow
{
    int kListenPort = 7073;
    const int kPacketSize = 1000;
    const short kMsgId = 155;
    const int kNumMsgs = 14000;

    NetworkClient myClient;

    bool isTestDone;

    [UnityTest]
    public IEnumerator ChannelBufferHandlesOverflowTest()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();

        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        myClient = new NetworkClient();
        if (!myClient.Configure(config, 10))
        {
            Assert.Fail("Client configure failed");
        }

        NetworkServer.RegisterHandler(kMsgId, OnServerMsg);
        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);

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
        //Debug.Log("Shutting down");
        //NetworkServer.DisconnectAll();
        //NetworkClient.ShutdownAll();
        //NetworkTransport.Shutdown();

        yield return null;
    }

    public void OnServerMsg(NetworkMessage netMsg)
    {
        //need this method simply to prevent "Unknown ID" error message
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        NetworkWriter writer = new NetworkWriter();
        writer.StartMessage(kMsgId);
        byte[] data = new byte[kPacketSize];
        writer.Write(data, kPacketSize);
        writer.FinishMessage();
        LogAssert.Expect(LogType.Error, "ChannelBuffer buffer limit of 16 packets reached.");

        // these messages do not all fit in the transport layer send queue.
        // to be recieved on the server, they must be buffered by HLAPI
        bool gotFailure = false;
        for (int i = 0; i < kNumMsgs; i++)
        {
            if (!myClient.SendWriter(writer, Channels.DefaultReliable))
            {
                gotFailure = true;
                break;
            }
        }

        Assert.AreEqual(true, gotFailure);
        isTestDone = true;
    }
}
#pragma warning restore 618
