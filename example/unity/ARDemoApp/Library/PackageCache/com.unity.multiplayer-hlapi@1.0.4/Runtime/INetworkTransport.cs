using System;
using System.Net;

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public interface INetworkTransport
    {
        void Init();

        void Init(GlobalConfig config);

        bool IsStarted { get; }

        void Shutdown();

        int AddHost(HostTopology topology, int port, string ip);

        int AddWebsocketHost(HostTopology topology, int port, string ip);

        int ConnectWithSimulator(int hostId, string address, int port, int specialConnectionId, out byte error, ConnectionSimulatorConfig conf);

        int Connect(int hostId, string address, int port, int specialConnectionId, out byte error);

        void ConnectAsNetworkHost(int hostId, string address, int port, NetworkID network, SourceID source, NodeID node, out byte error);

        int ConnectToNetworkPeer(int hostId, string address, int port, int specialConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, out byte error);

        int ConnectEndPoint(int hostId, EndPoint endPoint, int specialConnectionId, out byte error);

        bool DoesEndPointUsePlatformProtocols(EndPoint endPoint);

        int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, int port);

        bool RemoveHost(int hostId);

        bool Send(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error);

        NetworkEventType Receive(out int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

        NetworkEventType ReceiveFromHost(int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

        NetworkEventType ReceiveRelayEventFromHost(int hostId, out byte error);

        int GetCurrentRTT(int hostId, int connectionId, out byte error);

        void GetConnectionInfo(int hostId, int connectionId, out string address, out int port, out NetworkID network, out NodeID dstNode, out byte error);

        bool Disconnect(int hostId, int connectionId, out byte error);

        void SetBroadcastCredentials(int hostId, int key, int version, int subversion, out byte error);

        bool StartBroadcastDiscovery(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error);

        void GetBroadcastConnectionInfo(int hostId, out string address, out int port, out byte error);

        void GetBroadcastConnectionMessage(int hostId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

        void StopBroadcastDiscovery();

        void SetPacketStat(int direction, int packetStatId, int numMsgs, int numBytes);
    }
}
