using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

#pragma warning disable 618
[TestFixture]
public class NetworkClientTest
{
    private NetworkClient m_Client;
    private static string s_LatestLogMessage;

    static void HandleLog(string logString, string stackTrace, LogType type)
    {
        s_LatestLogMessage = type + ": " + logString + "\n" + stackTrace;
    }

    [SetUp]
    public void Setup()
    {
        Application.logMessageReceived += HandleLog;
    }

    [TearDown]
    public void Teardown()
    {
        Application.logMessageReceived -= HandleLog;
    }

    [Test]
    public void DisconnectWithoutConnectedConnection()
    {
        m_Client = new NetworkClient(new NetworkConnection());
        m_Client.Disconnect();
        Assert.AreEqual(null, s_LatestLogMessage);
    }
}
#pragma warning restore 618
