using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    class DefaultNetworkTransport : INetworkTransport
    {
        public DefaultNetworkTransport()
        {
        }

        public bool IsStarted
        {
            get
            {
                return NetworkTransport.IsStarted;
            }
        }

        public int AddHost(HostTopology topology, int port, string ip)
        {
            return NetworkTransport.AddHost(topology, port, ip);
        }

        public int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, int port)
        {
            return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port);
        }

        public int AddWebsocketHost(HostTopology topology, int port, string ip)
        {
            return NetworkTransport.AddWebsocketHost(topology, port, ip);
        }

        public int Connect(int hostId, string address, int port, int specialConnectionId, out byte error)
        {
            return NetworkTransport.Connect(hostId, address, port, specialConnectionId, out error);
        }

        public void ConnectAsNetworkHost(int hostId, string address, int port, NetworkID network, SourceID source, NodeID node, out byte error)
        {
            NetworkTransport.ConnectAsNetworkHost(hostId, address, port, network, source, node, out error);
        }

        public int ConnectEndPoint(int hostId, EndPoint endPoint, int specialConnectionId, out byte error)
        {
            return NetworkTransport.ConnectEndPoint(hostId, endPoint, specialConnectionId, out error);
        }

        public int ConnectToNetworkPeer(int hostId, string address, int port, int specialConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, out byte error)
        {
            return NetworkTransport.ConnectToNetworkPeer(hostId, address, port, specialConnectionId, relaySlotId, network, source, node, out error);
        }

        public int ConnectWithSimulator(int hostId, string address, int port, int specialConnectionId, out byte error, ConnectionSimulatorConfig conf)
        {
            return NetworkTransport.ConnectWithSimulator(hostId, address, port, specialConnectionId, out error, conf);
        }

        public bool Disconnect(int hostId, int connectionId, out byte error)
        {
            return NetworkTransport.Disconnect(hostId, connectionId, out error);
        }

        public bool DoesEndPointUsePlatformProtocols(EndPoint endPoint)
        {
            return NetworkTransport.DoesEndPointUsePlatformProtocols(endPoint);
        }

        public void GetBroadcastConnectionInfo(int hostId, out string address, out int port, out byte error)
        {
            NetworkTransport.GetBroadcastConnectionInfo(hostId, out address, out port, out error);
        }

        public void GetBroadcastConnectionMessage(int hostId, byte[] buffer, int bufferSize, out int receivedSize, out byte error)
        {
            NetworkTransport.GetBroadcastConnectionMessage(hostId, buffer, bufferSize, out receivedSize, out error);
        }

        public void GetConnectionInfo(int hostId, int connectionId, out string address, out int port, out NetworkID network, out NodeID dstNode, out byte error)
        {
            NetworkTransport.GetConnectionInfo(hostId, connectionId, out address, out port, out network, out dstNode, out error);
        }

        public int GetCurrentRTT(int hostId, int connectionId, out byte error)
        {
            return NetworkTransport.GetCurrentRTT(hostId, connectionId, out error);
        }

        public void Init()
        {
            NetworkTransport.Init();
        }

        public void Init(GlobalConfig config)
        {
            NetworkTransport.Init(config);
        }

        public NetworkEventType Receive(out int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error)
        {
            return NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, bufferSize, out receivedSize, out error);
        }

        public NetworkEventType ReceiveFromHost(int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error)
        {
            return NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, buffer, bufferSize, out receivedSize, out error);
        }

        public NetworkEventType ReceiveRelayEventFromHost(int hostId, out byte error)
        {
            return NetworkTransport.ReceiveRelayEventFromHost(hostId, out error);
        }

        public bool RemoveHost(int hostId)
        {
            return NetworkTransport.RemoveHost(hostId);
        }

        public bool Send(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error)
        {
            return NetworkTransport.Send(hostId, connectionId, channelId, buffer, size, out error);
        }

        public void SetBroadcastCredentials(int hostId, int key, int version, int subversion, out byte error)
        {
            NetworkTransport.SetBroadcastCredentials(hostId, key, version, subversion, out error);
        }

        public void SetPacketStat(int direction, int packetStatId, int numMsgs, int numBytes)
        {
            NetworkTransport.SetPacketStat(direction, packetStatId, numMsgs, numBytes);
        }

        public void Shutdown()
        {
            NetworkTransport.Shutdown();
        }

        public bool StartBroadcastDiscovery(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error)
        {
            return NetworkTransport.StartBroadcastDiscovery(hostId, broadcastPort, key, version, subversion, buffer, size, timeout, out error);
        }

        public void StopBroadcastDiscovery()
        {
            NetworkTransport.StopBroadcastDiscovery();
        }
    }
}
