using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking.Types;
using System.Collections.ObjectModel;


namespace UnityEngine.Networking
{
    /// <summary>
    /// The NetworkServerSimple is a basic server class without the "game" related functionality that the NetworkServer class has.
    /// <para>This class has no scene management, spawning, player objects, observers, or static interface like the NetworkServer class. It is simply a server that listens on a port, manages connections, and handles messages. There can be more than one instance of this class in a process.</para>
    /// <para>Like the NetworkServer and NetworkClient classes, it allows the type of NetworkConnection class created for new connections to be specified with SetNetworkConnectionClass(), so custom types of network connections can be used with it.</para>
    /// <para>This class can be used by overriding the virtual functions OnConnected, OnDisconnected and OnData; or by registering message handlers.</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkServerSimple
    {
        bool m_Initialized = false;
        int m_ListenPort;
        int m_ServerHostId = -1;
        int m_RelaySlotId = -1;
        bool m_UseWebSockets;

        byte[] m_MsgBuffer = null;
        NetworkReader m_MsgReader = null;

        Type m_NetworkConnectionClass = typeof(NetworkConnection);
        HostTopology m_HostTopology;
        List<NetworkConnection> m_Connections = new List<NetworkConnection>();
        ReadOnlyCollection<NetworkConnection> m_ConnectionsReadOnly;

        NetworkMessageHandlers m_MessageHandlers = new NetworkMessageHandlers();

        /// <summary>
        /// The network port that the server is listening on.
        /// </summary>
        public int listenPort { get { return m_ListenPort; } set { m_ListenPort = value; }}
        /// <summary>
        /// The transport layer hostId of the server.
        /// </summary>
        public int serverHostId { get { return m_ServerHostId; } set { m_ServerHostId = value; }}
        /// <summary>
        /// The transport layer host-topology that the server is configured with.
        /// <para>A host topology object can be passed to the Listen() function, or a default host topology that is compatible with the default topology of NetworkClient will be used.</para>
        /// </summary>
        public HostTopology hostTopology { get { return m_HostTopology; }}
        /// <summary>
        /// This causes the server to listen for WebSocket connections instead of regular transport layer connections.
        /// <para>This allows WebGL clients to talk to the server.</para>
        /// </summary>
        public bool useWebSockets { get { return m_UseWebSockets; } set { m_UseWebSockets = value; } }
        /// <summary>
        /// A read-only list of the current connections being managed.
        /// </summary>
        public ReadOnlyCollection<NetworkConnection> connections { get { return m_ConnectionsReadOnly; }}
        /// <summary>
        /// The message handler functions that are registered.
        /// </summary>
        public Dictionary<short, NetworkMessageDelegate> handlers { get { return m_MessageHandlers.GetHandlers(); } }

        /// <summary>
        /// The internal buffer that the server reads data from the network into. This will contain the most recent data read from the network when OnData() is called.
        /// </summary>
        public byte[] messageBuffer { get { return m_MsgBuffer; }}
        /// <summary>
        /// A NetworkReader object that is bound to the server's messageBuffer.
        /// </summary>
        public NetworkReader messageReader { get { return m_MsgReader; }}

        /// <summary>
        /// The type of class to be created for new network connections from clients.
        /// <para>By default this is the NetworkConnection class, but it can be changed with SetNetworkConnectionClass() to classes derived from NetworkConnections.</para>
        /// </summary>
        public Type networkConnectionClass
        {
            get { return m_NetworkConnectionClass; }
        }

        /// <summary>
        /// This sets the class that is used when creating new network connections.
        /// <para>The class must be derived from NetworkConnection.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SetNetworkConnectionClass<T>() where T : NetworkConnection
        {
            m_NetworkConnectionClass = typeof(T);
        }

        public NetworkServerSimple()
        {
            m_ConnectionsReadOnly = new ReadOnlyCollection<NetworkConnection>(m_Connections);
        }

        /// <summary>
        /// Initialization function that is invoked when the server starts listening. This can be overridden to perform custom initialization such as setting the NetworkConnectionClass.
        /// </summary>
        public virtual void Initialize()
        {
            if (m_Initialized)
                return;

            m_Initialized = true;
            NetworkManager.activeTransport.Init();

            m_MsgBuffer = new byte[NetworkMessage.MaxMessageSize];
            m_MsgReader = new NetworkReader(m_MsgBuffer);

            if (m_HostTopology == null)
            {
                var config = new ConnectionConfig();
                config.AddChannel(QosType.ReliableSequenced);
                config.AddChannel(QosType.Unreliable);
                m_HostTopology = new HostTopology(config, 8);
            }

            if (LogFilter.logDebug) { Debug.Log("NetworkServerSimple initialize."); }
        }

        /// <summary>
        /// This configures the network transport layer of the server.
        /// </summary>
        /// <param name="config">The transport layer configuration to use.</param>
        /// <param name="maxConnections">Maximum number of network connections to allow.</param>
        /// <returns>True if configured.</returns>
        public bool Configure(ConnectionConfig config, int maxConnections)
        {
            HostTopology top = new HostTopology(config, maxConnections);
            return Configure(top);
        }

        /// <summary>
        /// This configures the network transport layer of the server.
        /// </summary>
        /// <param name="topology">The transport layer host topology to use.</param>
        /// <returns>True if configured.</returns>
        public bool Configure(HostTopology topology)
        {
            m_HostTopology = topology;
            return true;
        }

        /// <summary>
        /// This starts the server listening for connections on the specified port.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="serverListenPort">The port to listen on.</param>
        /// <returns>True if able to listen.</returns>
        public bool Listen(string ipAddress, int serverListenPort)
        {
            Initialize();
            m_ListenPort = serverListenPort;

            if (m_UseWebSockets)
            {
                m_ServerHostId = NetworkManager.activeTransport.AddWebsocketHost(m_HostTopology, serverListenPort, ipAddress);
            }
            else
            {
                m_ServerHostId = NetworkManager.activeTransport.AddHost(m_HostTopology, serverListenPort, ipAddress);
            }

            if (m_ServerHostId == -1)
            {
                return false;
            }

            if (LogFilter.logDebug) { Debug.Log("NetworkServerSimple listen: " + ipAddress + ":" + m_ListenPort); }
            return true;
        }

        /// <summary>
        /// This starts the server listening for connections on the specified port.
        /// </summary>
        /// <param name="serverListenPort">The port to listen on.</param>
        /// <returns></returns>
        public bool Listen(int serverListenPort)
        {
            return Listen(serverListenPort, m_HostTopology);
        }

        /// <summary>
        /// This starts the server listening for connections on the specified port.
        /// </summary>
        /// <param name="serverListenPort">The port to listen on.</param>
        /// <param name="topology">The transport layer host toplogy to configure with.</param>
        /// <returns></returns>
        public bool Listen(int serverListenPort, HostTopology topology)
        {
            m_HostTopology = topology;
            Initialize();
            m_ListenPort = serverListenPort;

            if (m_UseWebSockets)
            {
                m_ServerHostId = NetworkManager.activeTransport.AddWebsocketHost(m_HostTopology, serverListenPort, null);
            }
            else
            {
                m_ServerHostId = NetworkManager.activeTransport.AddHost(m_HostTopology, serverListenPort, null);
            }

            if (m_ServerHostId == -1)
            {
                return false;
            }

            if (LogFilter.logDebug) { Debug.Log("NetworkServerSimple listen " + m_ListenPort); }
            return true;
        }

        /// <summary>
        /// Starts a server using a Relay server. This is the manual way of using the Relay server, as the regular NetworkServer.Connect() will automatically use the Relay server if a match exists.
        /// </summary>
        /// <param name="relayIp">Relay server IP Address.</param>
        /// <param name="relayPort">Relay server port.</param>
        /// <param name="netGuid">GUID of the network to create.</param>
        /// <param name="sourceId">This server's sourceId.</param>
        /// <param name="nodeId">The node to join the network with.</param>
        public void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
        {
            Initialize();

            m_ServerHostId = NetworkManager.activeTransport.AddHost(m_HostTopology, listenPort, null);
            if (LogFilter.logDebug) { Debug.Log("Server Host Slot Id: " + m_ServerHostId); }

            Update();

            byte error;
            NetworkManager.activeTransport.ConnectAsNetworkHost(
                m_ServerHostId,
                relayIp,
                relayPort,
                netGuid,
                sourceId,
                nodeId,
                out error);

            m_RelaySlotId = 0;
            if (LogFilter.logDebug) { Debug.Log("Relay Slot Id: " + m_RelaySlotId); }
        }

        /// <summary>
        /// This stops a server from listening.
        /// </summary>
        public void Stop()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkServerSimple stop "); }
            NetworkManager.activeTransport.RemoveHost(m_ServerHostId);
            m_ServerHostId = -1;
        }

        internal void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
        {
            m_MessageHandlers.RegisterHandlerSafe(msgType, handler);
        }

        /// <summary>
        /// This registers a handler function for a message Id.
        /// </summary>
        /// <param name="msgType">Message Id to register handler for.</param>
        /// <param name="handler">Handler function.</param>
        public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            m_MessageHandlers.RegisterHandler(msgType, handler);
        }

        /// <summary>
        /// This unregisters a registered message handler function.
        /// </summary>
        /// <param name="msgType">The message id to unregister.</param>
        public void UnregisterHandler(short msgType)
        {
            m_MessageHandlers.UnregisterHandler(msgType);
        }

        /// <summary>
        /// Clears the message handlers that are registered.
        /// </summary>
        public void ClearHandlers()
        {
            m_MessageHandlers.ClearMessageHandlers();
        }

        /// <summary>
        /// This function causes pending outgoing data on connections to be sent, but unlike Update() it works when the server is not listening.
        /// <para>When the server is using externally added connections and the dontListen flag is set, the regular connection flush in the Update() function does not happen. In this case, UpdateConnections can be called to pump the external connections. This is an advanced usage that should not be required unless the server uses custom NetworkConnection classes that do not use the built-in transport layer.</para>
        /// </summary>
        // this can be used independantly of Update() - such as when using external connections and not listening.
        public void UpdateConnections()
        {
            for (int i = 0; i < m_Connections.Count; i++)
            {
                NetworkConnection conn = m_Connections[i];
                if (conn != null)
                    conn.FlushChannels();
            }
        }

        /// <summary>
        /// This function pumps the server causing incoming network data to be processed, and pending outgoing data to be sent.
        /// <para>This should be called each frame, and is called automatically for the server used by NetworkServer.</para>
        /// </summary>
        public void Update()
        {
            if (m_ServerHostId == -1)
                return;

            int connectionId;
            int channelId;
            int receivedSize;
            byte error;

            var networkEvent = NetworkEventType.DataEvent;
            if (m_RelaySlotId != -1)
            {
                networkEvent = NetworkManager.activeTransport.ReceiveRelayEventFromHost(m_ServerHostId, out error);
                if (NetworkEventType.Nothing != networkEvent)
                {
                    if (LogFilter.logDebug) { Debug.Log("NetGroup event:" + networkEvent); }
                }
                if (networkEvent == NetworkEventType.ConnectEvent)
                {
                    if (LogFilter.logDebug) { Debug.Log("NetGroup server connected"); }
                }
                if (networkEvent == NetworkEventType.DisconnectEvent)
                {
                    if (LogFilter.logDebug) { Debug.Log("NetGroup server disconnected"); }
                }
            }

            do
            {
                networkEvent = NetworkManager.activeTransport.ReceiveFromHost(m_ServerHostId, out connectionId, out channelId, m_MsgBuffer, (int)m_MsgBuffer.Length, out receivedSize, out error);
                if (networkEvent != NetworkEventType.Nothing)
                {
                    if (LogFilter.logDev) { Debug.Log("Server event: host=" + m_ServerHostId + " event=" + networkEvent + " error=" + error); }
                }

                switch (networkEvent)
                {
                    case NetworkEventType.ConnectEvent:
                    {
                        HandleConnect(connectionId, error);
                        break;
                    }

                    case NetworkEventType.DataEvent:
                    {
                        HandleData(connectionId, channelId, receivedSize, error);
                        break;
                    }

                    case NetworkEventType.DisconnectEvent:
                    {
                        HandleDisconnect(connectionId, error);
                        break;
                    }

                    case NetworkEventType.Nothing:
                        break;

                    default:
                        if (LogFilter.logError) { Debug.LogError("Unknown network message type received: " + networkEvent); }
                        break;
                }
            }
            while (networkEvent != NetworkEventType.Nothing);

            UpdateConnections();
        }

        /// <summary>
        /// This looks up the network connection object for the specified connection Id.
        /// </summary>
        /// <param name="connectionId">The connection id to look up.</param>
        /// <returns>A NetworkConnection objects, or null if no connection found.</returns>
        public NetworkConnection FindConnection(int connectionId)
        {
            if (connectionId < 0 || connectionId >= m_Connections.Count)
                return null;

            return m_Connections[connectionId];
        }

        /// <summary>
        /// This adds a connection created by external code to the server's list of connections, at the connection's connectionId index.
        /// <para>Connections are usually added automatically, this is a low-level function for the rare special case of externally created connections.</para>
        /// </summary>
        /// <param name="conn">A new connection object.</param>
        /// <returns>True if added.</returns>
        public bool SetConnectionAtIndex(NetworkConnection conn)
        {
            while (m_Connections.Count <= conn.connectionId)
            {
                m_Connections.Add(null);
            }

            if (m_Connections[conn.connectionId] != null)
            {
                // already a connection at this index
                return false;
            }

            m_Connections[conn.connectionId] = conn;
            conn.SetHandlers(m_MessageHandlers);
            return true;
        }

        /// <summary>
        /// This removes a connection object from the server's list of connections.
        /// <para>This is a low-level function that should not be used for regular connections. It is only safe to remove connections added with SetConnectionAtIndex() using this function.</para>
        /// </summary>
        /// <param name="connectionId">The id of the connection to remove.</param>
        /// <returns>True if removed.</returns>
        public bool RemoveConnectionAtIndex(int connectionId)
        {
            if (connectionId < 0 || connectionId >= m_Connections.Count)
                return false;

            m_Connections[connectionId] = null;
            return true;
        }

        void HandleConnect(int connectionId, byte error)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkServerSimple accepted client:" + connectionId); }

            if (error != 0)
            {
                OnConnectError(connectionId, error);
                return;
            }

            string address;
            int port;
            NetworkID networkId;
            NodeID node;
            byte error2;
            NetworkManager.activeTransport.GetConnectionInfo(m_ServerHostId, connectionId, out address, out port, out networkId, out node, out error2);

            NetworkConnection conn = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
            conn.SetHandlers(m_MessageHandlers);
            conn.Initialize(address, m_ServerHostId, connectionId, m_HostTopology);
            conn.lastError = (NetworkError)error2;

            // add connection at correct index
            while (m_Connections.Count <= connectionId)
            {
                m_Connections.Add(null);
            }
            m_Connections[connectionId] = conn;

            OnConnected(conn);
        }

        void HandleDisconnect(int connectionId, byte error)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkServerSimple disconnect client:" + connectionId); }

            var conn = FindConnection(connectionId);
            if (conn == null)
            {
                return;
            }
            conn.lastError = (NetworkError)error;

            if (error != 0)
            {
                if ((NetworkError)error != NetworkError.Timeout)
                {
                    m_Connections[connectionId] = null;
                    if (LogFilter.logError) { Debug.LogError("Server client disconnect error, connectionId: " + connectionId + " error: " + (NetworkError)error); }

                    OnDisconnectError(conn, error);
                    return;
                }
            }

            conn.Disconnect();
            m_Connections[connectionId] = null;
            if (LogFilter.logDebug) { Debug.Log("Server lost client:" + connectionId); }

            OnDisconnected(conn);
        }

        void HandleData(int connectionId, int channelId, int receivedSize, byte error)
        {
            var conn = FindConnection(connectionId);
            if (conn == null)
            {
                if (LogFilter.logError) { Debug.LogError("HandleData Unknown connectionId:" + connectionId); }
                return;
            }
            conn.lastError = (NetworkError)error;

            if (error != 0)
            {
                OnDataError(conn, error);
                return;
            }

            m_MsgReader.SeekZero();
            OnData(conn, receivedSize, channelId);
        }

        /// <summary>
        /// This sends the data in an array of bytes to the connected client.
        /// </summary>
        /// <param name="connectionId">The id of the connection to send on.</param>
        /// <param name="bytes">The data to send.</param>
        /// <param name="numBytes">The size of the data to send.</param>
        /// <param name="channelId">The channel to send the data on.</param>
        public void SendBytesTo(int connectionId, byte[] bytes, int numBytes, int channelId)
        {
            var outConn = FindConnection(connectionId);
            if (outConn == null)
            {
                return;
            }
            outConn.SendBytes(bytes, numBytes, channelId);
        }

        /// <summary>
        /// This sends the contents of a NetworkWriter object to the connected client.
        /// </summary>
        /// <param name="connectionId">The id of the connection to send on.</param>
        /// <param name="writer">The writer object to send.</param>
        /// <param name="channelId">The channel to send the data on.</param>
        public void SendWriterTo(int connectionId, NetworkWriter writer, int channelId)
        {
            var outConn = FindConnection(connectionId);
            if (outConn == null)
            {
                return;
            }
            outConn.SendWriter(writer, channelId);
        }

        /// <summary>
        /// This disconnects the connection of the corresponding connection id.
        /// </summary>
        /// <param name="connectionId">The id of the connection to disconnect.</param>
        public void Disconnect(int connectionId)
        {
            var outConn = FindConnection(connectionId);
            if (outConn == null)
            {
                return;
            }
            outConn.Disconnect();
            m_Connections[connectionId] = null;
        }

        /// <summary>
        /// This disconnects all of the active connections.
        /// </summary>
        public void DisconnectAllConnections()
        {
            for (int i = 0; i < m_Connections.Count; i++)
            {
                NetworkConnection conn = m_Connections[i];
                if (conn != null)
                {
                    conn.Disconnect();
                    conn.Dispose();
                }
            }
        }

        // --------------------------- virtuals ---------------------------------------

        /// <summary>
        /// A virtual function that is invoked when there is a connection error.
        /// </summary>
        /// <param name="connectionId">The id of the connection with the error.</param>
        /// <param name="error">The error code.</param>
        public virtual void OnConnectError(int connectionId, byte error)
        {
            Debug.LogError("OnConnectError error:" + error);
        }

        /// <summary>
        /// A virtual function that is called when a data error occurs on a connection.
        /// </summary>
        /// <param name="conn">The connection object that the error occured on.</param>
        /// <param name="error">The error code.</param>
        public virtual void OnDataError(NetworkConnection conn, byte error)
        {
            Debug.LogError("OnDataError error:" + error);
        }

        /// <summary>
        /// A virtual function that is called when a disconnect error happens.
        /// </summary>
        /// <param name="conn">The connection object that the error occured on.</param>
        /// <param name="error">The error code.</param>
        public virtual void OnDisconnectError(NetworkConnection conn, byte error)
        {
            Debug.LogError("OnDisconnectError error:" + error);
        }

        /// <summary>
        /// This virtual function can be overridden to perform custom functionality for new network connections.
        /// <para>By default OnConnected just invokes a connect event on the new connection.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public abstract class ExampleScript : NetworkManager
        /// {
        ///    public virtual void OnConnected(NetworkConnection conn)
        ///    {
        ///        conn.InvokeHandlerNoData(MsgType.Connect);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">The new connection object.</param>
        public virtual void OnConnected(NetworkConnection conn)
        {
            conn.InvokeHandlerNoData(MsgType.Connect);
        }

        /// <summary>
        /// This virtual function can be overridden to perform custom functionality for disconnected network connections.
        /// <para>By default OnConnected just invokes a disconnect event on the new connection.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public abstract class ExampleScript : <see cref="NetworkManager">NetworkManager</see>
        /// {
        ///    public virtual void OnDisconnected(<see cref="NetworkConnection">NetworkConnection</see> conn)
        ///    {
        ///        conn.InvokeHandlerNoData(MsgType.Disconnect);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn"></param>
        public virtual void OnDisconnected(NetworkConnection conn)
        {
            conn.InvokeHandlerNoData(MsgType.Disconnect);
        }

        /// <summary>
        /// This virtual function can be overridden to perform custom functionality when data is received for a connection.
        /// <para>By default this function calls HandleData() which will process the data and invoke message handlers for any messages that it finds.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public abstract class ExampleScript : <see cref="NetworkManager">NetworkManager</see>
        /// {
        ///    byte[] msgBuffer = new byte[1024];
        ///
        ///    public virtual void OnData(<see cref="NetworkConnection">NetworkConnection</see> conn, int channelId, int receivedSize)
        ///    {
        ///        conn.TransportRecieve(msgBuffer, receivedSize, channelId);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="receivedSize"></param>
        /// <param name="channelId"></param>
        public virtual void OnData(NetworkConnection conn, int receivedSize, int channelId)
        {
            conn.TransportReceive(m_MsgBuffer, receivedSize, channelId);
        }
    }
}
