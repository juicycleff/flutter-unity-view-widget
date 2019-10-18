using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class ConnectWithDNSWorks
{
    int kListenPort = 7073;
    int steps = 0;

    [UnityTest]
    public IEnumerator ConnectWithDNSWorksTest()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();

        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.ReliableSequenced);
        config.AddChannel(QosType.Unreliable);

        NetworkClient client1 = new NetworkClient();
        if (!client1.Configure(config, 20))
        {
            Assert.Fail("client1 configure failed");
        }
        client1.RegisterHandler(MsgType.Error, OnError1);

        NetworkClient client2 = new NetworkClient();
        if (!client2.Configure(config, 20))
        {
            Assert.Fail("client2 configure failed");
        }
        client2.RegisterHandler(MsgType.Connect, OnConnectIncrementStep);

        NetworkClient client3 = new NetworkClient();
        if (!client3.Configure(config, 20))
        {
            Assert.Fail("client3 configure failed");
        }
        client3.RegisterHandler(MsgType.Connect, OnConnectIncrementStep);

        int retries = 0;
        while (!NetworkServer.Listen("127.0.0.1", ++kListenPort))
        {
            Assert.IsTrue(retries++ < 10, "Couldn't Listen for more than 10 retries");
        }

        // wait for errors from client1
#if PLATFORM_WINRT && !ENABLE_IL2CPP
        LogAssert.Expect(LogType.Error, "DNS resolution failed: HostNotFound");
        LogAssert.Expect(LogType.Error, "UNet Client Error Connect Error: 11");
#else
        LogAssert.Expect(LogType.Error, "DNS resolution failed: 11001");
        LogAssert.Expect(LogType.Error, "UNet Client Error Connect Error: 11");
#endif
        client1.Connect("444.555.444.333", kListenPort);

        // These are successful and should increment the step counter
        client2.Connect("localhost", kListenPort);
        client3.Connect("127.0.0.1", kListenPort);

        while (steps < 3)
        {
            yield return null;
        }
    }

    void OnError1(NetworkMessage netMsg)
    {
        UnityEngine.Networking.NetworkSystem.ErrorMessage msg = netMsg.ReadMessage<UnityEngine.Networking.NetworkSystem.ErrorMessage>();
        Assert.AreEqual(NetworkError.DNSFailure, (NetworkError)msg.errorCode);
        steps += 1;
    }

    void OnConnectIncrementStep(NetworkMessage netMsg)
    {
        steps += 1;
    }
}
#pragma warning restore 618
