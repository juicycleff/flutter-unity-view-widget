using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Networking;

#pragma warning disable 618
public class NetworkManagerConfigWorksTest
{
    NetworkManager netManager;

    [UnityTest]
    public IEnumerator NetworkManagerConfigCheck()
    {
        NetworkServer.Reset();

        var netManagerObj = new GameObject();
        netManager = netManagerObj.AddComponent<NetworkManager>();

        netManager.GetComponent<NetworkManager>().customConfig = true;

        foreach (QosType channel in Enum.GetValues(typeof(QosType)))
        {
            netManager.GetComponent<NetworkManager>().
            connectionConfig.AddChannel(channel);
        }

        Assert.AreEqual(netManager.connectionConfig.ChannelCount, Enum.GetValues(typeof(QosType)).Length, "Not all channels are added");

        netManager.connectionConfig.AckDelay = 33;
        netManager.connectionConfig.AcksType = ConnectionAcksType.Acks32;
        netManager.connectionConfig.AllCostTimeout = 20;
        netManager.connectionConfig.FragmentSize = 500;
        netManager.connectionConfig.ConnectTimeout = 500;
        netManager.connectionConfig.DisconnectTimeout = 2000;

        NetworkHostCanBeStartedWithConfig();
        NetworkServerClientCanBeStartedWithConfig();

        yield return null;
        UnityEngine.Object.Destroy(netManager);
    }

    //check that Host can be started
    public IEnumerator NetworkHostCanBeStartedWithConfig()
    {
        NetworkClient netClient = new NetworkClient();

        if (!netManager.isNetworkActive)
            netClient = netManager.StartHost();

        if (!netClient.isConnected)
            yield return null;

        Assert.IsTrue(netClient.isConnected,
            "Network is not active.");

        netManager.StopHost();
    }

    //check that Server/Client can be started
    public IEnumerator NetworkServerClientCanBeStartedWithConfig()
    {
        string netAddress = "127.0.0.1";
        int netPort = 8887;
        netManager.networkAddress = netAddress;
        netManager.networkPort = netPort;

        netManager.StartServer();

        NetworkClient netClient = netManager.StartClient();

        netClient.Connect(netAddress, netPort);

        if (!netClient.isConnected)
        {
            yield return null;
        }

        Assert.IsTrue(netClient.isConnected,
            "Client did not connect to server");

        netManager.StopServer();
    }
}
#pragma warning restore 618
