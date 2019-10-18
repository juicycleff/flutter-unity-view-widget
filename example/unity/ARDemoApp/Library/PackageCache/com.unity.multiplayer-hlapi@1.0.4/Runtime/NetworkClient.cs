using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This is a network client class used by the networking system. It contains a NetworkConnection that is used to connect to a network server.
    /// <para>The <see cref="NetworkClient">NetworkClient</see> handle connection state, messages handlers, and connection configuration. There can be many <see cref="NetworkClient">NetworkClient</see> instances in a process at a time, but only one that is connected to a game server (<see cref="NetworkServer">NetworkServer</see>) that uses spawned objects.</para>
    /// <para><see cref="NetworkClient">NetworkClient</see> has an internal update function where it handles events from the transport layer. This includes asynchronous connect events, disconnect events and incoming data from a server.</para>
    /// <para>The <see cref="NetworkManager">NetworkManager</see> has a NetworkClient instance that it uses for games that it starts, but the NetworkClient may be used by itself.</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkClient
    {
        Type m_NetworkConnectionClass = typeof(NetworkConnection);

        const int k_MaxEventsPerFrame = 500;

        static List<NetworkClient> s_Clients = new List<NetworkClient>();
        static bool s_IsActive;

        /// <summary>
        /// A list of all the active network clients in the current process.
        /// <para>This is NOT a list of all clients that are connected to the remote server, it is client instances on the local game.</para>
        /// </summary>
        public static List<NetworkClient> allClients { get { return s_Clients; } }
        /// <summary>
        /// True if a network client is currently active.
        /// </summary>
        public static bool active { get { return s_IsActive; } }

        HostTopology m_HostTopology;
        int m_HostPort;

        bool m_UseSimulator;
        int m_SimulatedLatency;
        float m_PacketLoss;

        string m_ServerIp = "";
        int m_ServerPort;
        int m_ClientId = -1;
        int m_ClientConnectionId = -1;
        //int m_RelaySlotId = -1;

        int m_StatResetTime;

        EndPoint m_RemoteEndPoint;

        // static message objects to avoid runtime-allocations
        static CRCMessage s_CRCMessage = new CRCMessage();

        NetworkMessageHandlers m_MessageHandlers = new NetworkMessageHandlers();
        protected NetworkConnection m_Connection;

        byte[] m_MsgBuffer;
        NetworkReader m_MsgReader;

        protected enum ConnectState
        {
            None,
            Resolving,
            Resolved,
            Connecting,
            Connected,
            Disconnected,
            Failed
        }
        protected ConnectState m_AsyncConnect = ConnectState.None;
        string m_RequestedServerHost = "";

        internal void SetHandlers(NetworkConnection conn)
        {
            conn.SetHandlers(m_MessageHandlers);
        }

        /// <summary>
        /// The IP address of the server that this client is connected to.
        /// <para>This will be empty if the client has not connected yet.</para>
        /// </summary>
        public string serverIp { get { return m_ServerIp; } }
        /// <summary>
        /// The port of the server that this client is connected to.
        /// <para>This will be zero if the client has not connected yet.</para>
        /// </summary>
        public int serverPort { get { return m_ServerPort; } }
        /// <summary>
        /// The NetworkConnection object this client is using.
        /// </summary>
        public NetworkConnection connection { get { return m_Connection; } }

        [Obsolete("Moved to NetworkMigrationManager.")]
        public PeerInfoMessage[] peers { get { return null; } }

        internal int hostId { get { return m_ClientId; } }
        /// <summary>
        /// The registered network message handlers.
        /// </summary>
        public Dictionary<short, NetworkMessageDelegate> handlers { get { return m_MessageHandlers.GetHandlers(); } }
        /// <summary>
        /// The number of QoS channels currently configured for this client.
        /// </summary>
        public int numChannels { get { return m_HostTopology.DefaultConfig.ChannelCount; } }
        /// <summary>
        /// The host topology that this client is using.
        /// <para>This is read-only once the client is started.</para>
        /// </summary>
        public HostTopology hostTopology { get { return m_HostTopology; }}
        /// <summary>
        /// The local port that the network client uses to connect to the server.
        /// <para>It defaults to 0, which means the network client will use a free port of system choice.</para>
        /// </summary>
        public int hostPort
        {
            get { return m_HostPort; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Port must not be a negative number.");

                if (value > 65535)
                    throw new ArgumentException("Port must not be greater than 65535.");

                m_HostPort = value;
            }
        }

        /// <summary>
        /// This gives the current connection status of the client.
        /// </summary>
        public bool isConnected { get { return m_AsyncConnect == ConnectState.Connected; }}

        /// <summary>
        /// The class to use when creating new NetworkConnections.
        /// <para>This can be set with SetNetworkConnectionClass. This allows custom classes that do special processing of data from the transport layer to be used with the NetworkClient.</para>
        /// <para>See NetworkConnection.TransportSend and NetworkConnection.TransportReceive for details.</para>
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

        /// <summary>
        /// Creates a new NetworkClient instance.
        /// </summary>
        public NetworkClient()
        {
            if (LogFilter.logDev) { Debug.Log("Client created version " + Version.Current); }
            m_MsgBuffer = new byte[NetworkMessage.MaxMessageSize];
            m_MsgReader = new NetworkReader(m_MsgBuffer);
            AddClient(this);
        }

        public NetworkClient(NetworkConnection conn)
        {
            if (LogFilter.logDev) { Debug.Log("Client created version " + Version.Current); }
            m_MsgBuffer = new byte[NetworkMessage.MaxMessageSize];
            m_MsgReader = new NetworkReader(m_MsgBuffer);
            AddClient(this);

            SetActive(true);
            m_Connection = conn;
            m_AsyncConnect = ConnectState.Connected;
            conn.SetHandlers(m_MessageHandlers);
            RegisterSystemHandlers(false);
        }

        /// <summary>
        /// This configures the transport layer settings for a client.
        /// <para>The settings in the ConnectionConfig or HostTopology object will be used to configure the transport layer connection used by this client. This must match the configuration of the server.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///    void DoConnect()
        ///    {
        ///        ConnectionConfig config = new ConnectionConfig();
        ///        config.AddChannel(QosType.ReliableSequenced);
        ///        config.AddChannel(QosType.UnreliableSequenced);
        ///        config.PacketSize = 500;
        ///        NetworkClient client = new NetworkClient();
        ///        client.Configure(config, 1);
        ///        client.Connect("127.0.0.1", 7070);
        ///    }
        /// };
        /// </code>
        /// </summary>
        /// <param name="config">Transport layer configuration object.</param>
        /// <param name="maxConnections">The maximum number of connections to allow.</param>
        /// <returns>True if the configuration was successful.</returns>
        public bool Configure(ConnectionConfig config, int maxConnections)
        {
            HostTopology top = new HostTopology(config, maxConnections);
            return Configure(top);
        }

        /// <summary>
        /// This configures the transport layer settings for a client.
        /// <para>The settings in the ConnectionConfig or HostTopology object will be used to configure the transport layer connection used by this client. This must match the configuration of the server.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///    void DoConnect()
        ///    {
        ///        ConnectionConfig config = new ConnectionConfig();
        ///        config.AddChannel(QosType.ReliableSequenced);
        ///        config.AddChannel(QosType.UnreliableSequenced);
        ///        config.PacketSize = 500;
        ///        NetworkClient client = new NetworkClient();
        ///        client.Configure(config, 1);
        ///        client.Connect("127.0.0.1", 7070);
        ///    }
        /// };
        /// </code>
        /// </summary>
        /// <param name="topology">Transport layer topology object.</param>
        /// <returns>True if the configuration was successful.</returns>
        public bool Configure(HostTopology topology)
        {
            //NOTE: this maxConnections is across all clients that use this tuner, so it is
            //      effectively the number of _clients_.
            m_HostTopology = topology;
            return true;
        }

        public void Connect(MatchInfo matchInfo)
        {
            PrepareForConnect();
            ConnectWithRelay(matchInfo);
        }

        /// <summary>
        /// This is used by a client that has lost the connection to the old host, to reconnect to the new host of a game.
        /// </summary>
        /// <param name="serverIp">The IP address of the new host.</param>
        /// <param name="serverPort">The port of the new host.</param>
        /// <returns>True if able to reconnect.</returns>
        public bool ReconnectToNewHost(string serverIp, int serverPort)
        {
            if (!NetworkClient.active)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect - NetworkClient must be active"); }
                return false;
            }

            if (m_Connection == null)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect - no old connection exists"); }
                return false;
            }

            if (LogFilter.logInfo) { Debug.Log("NetworkClient Reconnect " + serverIp + ":" + serverPort); }

            ClientScene.HandleClientDisconnect(m_Connection);
            ClientScene.ClearLocalPlayers();

            m_Connection.Disconnect();
            m_Connection = null;
            m_ClientId = NetworkManager.activeTransport.AddHost(m_HostTopology, m_HostPort, null);

            string hostnameOrIp = serverIp;
            m_ServerPort = serverPort;

            //TODO: relay reconnect
            /*
            if (Match.NetworkMatch.matchSingleton != null)
            {
                hostnameOrIp = Match.NetworkMatch.matchSingleton.address;
                m_ServerPort = Match.NetworkMatch.matchSingleton.port;
            }*/

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                m_ServerIp = hostnameOrIp;
                m_AsyncConnect = ConnectState.Resolved;
            }
            else if (serverIp.Equals("127.0.0.1") || serverIp.Equals("localhost"))
            {
                m_ServerIp = "127.0.0.1";
                m_AsyncConnect = ConnectState.Resolved;
            }
            else
            {
                if (LogFilter.logDebug) { Debug.Log("Async DNS START:" + hostnameOrIp); }
                m_AsyncConnect = ConnectState.Resolving;
                Dns.BeginGetHostAddresses(hostnameOrIp, new AsyncCallback(GetHostAddressesCallback), this);
            }
            return true;
        }

        public bool ReconnectToNewHost(EndPoint secureTunnelEndPoint)
        {
            if (!NetworkClient.active)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect - NetworkClient must be active"); }
                return false;
            }

            if (m_Connection == null)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect - no old connection exists"); }
                return false;
            }

            if (LogFilter.logInfo) { Debug.Log("NetworkClient Reconnect to remoteSockAddr"); }

            ClientScene.HandleClientDisconnect(m_Connection);
            ClientScene.ClearLocalPlayers();

            m_Connection.Disconnect();
            m_Connection = null;
            m_ClientId = NetworkManager.activeTransport.AddHost(m_HostTopology, m_HostPort, null);

            if (secureTunnelEndPoint == null)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect failed: null endpoint passed in"); }
                m_AsyncConnect = ConnectState.Failed;
                return false;
            }

            // Make sure it's either IPv4 or IPv6
            if (secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetwork && secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetworkV6)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect failed: Endpoint AddressFamily must be either InterNetwork or InterNetworkV6"); }
                m_AsyncConnect = ConnectState.Failed;
                return false;
            }

            // Make sure it's an Endpoint we know what to do with
            string endPointType = secureTunnelEndPoint.GetType().FullName;
            if (endPointType == "System.Net.IPEndPoint")
            {
                IPEndPoint tmp = (IPEndPoint)secureTunnelEndPoint;
                Connect(tmp.Address.ToString(), tmp.Port);
                return m_AsyncConnect != ConnectState.Failed;
            }
            if ((endPointType != "UnityEngine.XboxOne.XboxOneEndPoint") && (endPointType != "UnityEngine.PS4.SceEndPoint"))
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect failed: invalid Endpoint (not IPEndPoint or XboxOneEndPoint or SceEndPoint)"); }
                m_AsyncConnect = ConnectState.Failed;
                return false;
            }

            byte error = 0;
            // regular non-relay connect
            m_RemoteEndPoint = secureTunnelEndPoint;
            m_AsyncConnect = ConnectState.Connecting;

            try
            {
                m_ClientConnectionId = NetworkManager.activeTransport.ConnectEndPoint(m_ClientId, m_RemoteEndPoint, 0, out error);
            }
            catch (Exception ex)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect failed: Exception when trying to connect to EndPoint: " + ex); }
                m_AsyncConnect = ConnectState.Failed;
                return false;
            }
            if (m_ClientConnectionId == 0)
            {
                if (LogFilter.logError) { Debug.LogError("Reconnect failed: Unable to connect to EndPoint (" + error + ")"); }
                m_AsyncConnect = ConnectState.Failed;
                return false;
            }

            m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
            m_Connection.SetHandlers(m_MessageHandlers);
            m_Connection.Initialize(m_ServerIp, m_ClientId, m_ClientConnectionId, m_HostTopology);
            return true;
        }

        /// <summary>
        /// Connect client to a NetworkServer instance with simulated latency and packet loss.
        /// </summary>
        /// <param name="serverIp">Target IP address or hostname.</param>
        /// <param name="serverPort">Target port number.</param>
        /// <param name="latency">Simulated latency in milliseconds.</param>
        /// <param name="packetLoss">Simulated packet loss percentage.</param>
        public void ConnectWithSimulator(string serverIp, int serverPort, int latency, float packetLoss)
        {
            m_UseSimulator = true;
            m_SimulatedLatency = latency;
            m_PacketLoss = packetLoss;
            Connect(serverIp, serverPort);
        }

        static bool IsValidIpV6(string address)
        {
            for (int i = 0; i < address.Length; i++)
            {
                var c = address[i];
                if (
                    (c == ':') ||
                    (c >= '0' && c <= '9') ||
                    (c >= 'a' && c <= 'f') ||
                    (c >= 'A' && c <= 'F')
                )
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Connect client to a NetworkServer instance.
        /// <para>Connecting to a server is asynchronous. There is connection message that is fired when the client connects. If the connection fails, a MsgType.Error message will be generated. Once a connection is established you are able to send messages on the connection using NetworkClient.Send(). If using other features of the high level api, the client should call NetworkClient.IsReady() once it is ready to participate in the game. At that point the client will be sent spawned objects and state update messages.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class NetClient
        /// {
        ///    NetworkClient myClient;
        ///
        ///    public void OnConnected(NetworkConnection conn, NetworkReader reader)
        ///    {
        ///        Debug.Log("Connected to server");
        ///    }
        ///
        ///    public void OnDisconnected(NetworkConnection conn, NetworkReader reader)
        ///    {
        ///        Debug.Log("Disconnected from server");
        ///    }
        ///
        ///    public void OnError(NetworkConnection conn, NetworkReader reader)
        ///    {
        ///        SystemErrorMessage errorMsg = reader.SmartRead&lt;SystemErrorMessage&gt;();
        ///        Debug.Log("Error connecting with code " + errorMsg.errorCode);
        ///    }
        ///
        ///    public void Start()
        ///    {
        ///        myClient = NetworkClient.Instance;
        ///        myClient.RegisterHandler(MsgType.SYSTEM_CONNECT, OnConnected);
        ///        myClient.RegisterHandler(MsgType.SYSTEM_DISCONNECT, OnDisconnected);
        ///        myClient.RegisterHandler(MsgType.SYSTEM_ERROR, OnError);
        ///        myClient.Connect("127.0.0.1", 8888);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="serverIp">Target IP address or hostname.</param>
        /// <param name="serverPort">Target port number.</param>
        public void Connect(string serverIp, int serverPort)
        {
            PrepareForConnect();

            if (LogFilter.logDebug) { Debug.Log("Client Connect: " + serverIp + ":" + serverPort); }

            string hostnameOrIp = serverIp;
            m_ServerPort = serverPort;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                m_ServerIp = hostnameOrIp;
                m_AsyncConnect = ConnectState.Resolved;
            }
            else if (serverIp.Equals("127.0.0.1") || serverIp.Equals("localhost"))
            {
                m_ServerIp = "127.0.0.1";
                m_AsyncConnect = ConnectState.Resolved;
            }
            else if (serverIp.IndexOf(":") != -1 && IsValidIpV6(serverIp))
            {
                m_ServerIp = serverIp;
                m_AsyncConnect = ConnectState.Resolved;
            }
            else
            {
                if (LogFilter.logDebug) { Debug.Log("Async DNS START:" + hostnameOrIp); }
                m_RequestedServerHost = hostnameOrIp;
                m_AsyncConnect = ConnectState.Resolving;
                Dns.BeginGetHostAddresses(hostnameOrIp, GetHostAddressesCallback, this);
            }
        }

        public void Connect(EndPoint secureTunnelEndPoint)
        {
            bool usePlatformSpecificProtocols = NetworkManager.activeTransport.DoesEndPointUsePlatformProtocols(secureTunnelEndPoint);
            PrepareForConnect(usePlatformSpecificProtocols);

            if (LogFilter.logDebug) { Debug.Log("Client Connect to remoteSockAddr"); }

            if (secureTunnelEndPoint == null)
            {
                if (LogFilter.logError) { Debug.LogError("Connect failed: null endpoint passed in"); }
                m_AsyncConnect = ConnectState.Failed;
                return;
            }

            // Make sure it's either IPv4 or IPv6
            if (secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetwork && secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetworkV6)
            {
                if (LogFilter.logError) { Debug.LogError("Connect failed: Endpoint AddressFamily must be either InterNetwork or InterNetworkV6"); }
                m_AsyncConnect = ConnectState.Failed;
                return;
            }

            // Make sure it's an Endpoint we know what to do with
            string endPointType = secureTunnelEndPoint.GetType().FullName;
            if (endPointType == "System.Net.IPEndPoint")
            {
                IPEndPoint tmp = (IPEndPoint)secureTunnelEndPoint;
                Connect(tmp.Address.ToString(), tmp.Port);
                return;
            }
            if ((endPointType != "UnityEngine.XboxOne.XboxOneEndPoint") && (endPointType != "UnityEngine.PS4.SceEndPoint"))
            {
                if (LogFilter.logError) { Debug.LogError("Connect failed: invalid Endpoint (not IPEndPoint or XboxOneEndPoint or SceEndPoint)"); }
                m_AsyncConnect = ConnectState.Failed;
                return;
            }

            byte error = 0;
            // regular non-relay connect
            m_RemoteEndPoint = secureTunnelEndPoint;
            m_AsyncConnect = ConnectState.Connecting;

            try
            {
                m_ClientConnectionId = NetworkManager.activeTransport.ConnectEndPoint(m_ClientId, m_RemoteEndPoint, 0, out error);
            }
            catch (Exception ex)
            {
                if (LogFilter.logError) { Debug.LogError("Connect failed: Exception when trying to connect to EndPoint: " + ex); }
                m_AsyncConnect = ConnectState.Failed;
                return;
            }
            if (m_ClientConnectionId == 0)
            {
                if (LogFilter.logError) { Debug.LogError("Connect failed: Unable to connect to EndPoint (" + error + ")"); }
                m_AsyncConnect = ConnectState.Failed;
                return;
            }

            m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
            m_Connection.SetHandlers(m_MessageHandlers);
            m_Connection.Initialize(m_ServerIp, m_ClientId, m_ClientConnectionId, m_HostTopology);
        }

        void PrepareForConnect()
        {
            PrepareForConnect(false);
        }

        void PrepareForConnect(bool usePlatformSpecificProtocols)
        {
            SetActive(true);
            RegisterSystemHandlers(false);

            if (m_HostTopology == null)
            {
                var config = new ConnectionConfig();
                config.AddChannel(QosType.ReliableSequenced);
                config.AddChannel(QosType.Unreliable);
                config.UsePlatformSpecificProtocols = usePlatformSpecificProtocols;
                m_HostTopology = new HostTopology(config, 8);
            }

            if (m_UseSimulator)
            {
                int minTimeout = (m_SimulatedLatency / 3) - 1;
                if (minTimeout < 1)
                {
                    minTimeout = 1;
                }
                int maxTimeout = m_SimulatedLatency * 3;

                if (LogFilter.logDebug) { Debug.Log("AddHost Using Simulator " + minTimeout + "/" + maxTimeout); }
                m_ClientId = NetworkManager.activeTransport.AddHostWithSimulator(m_HostTopology, minTimeout, maxTimeout, m_HostPort);
            }
            else
            {
                m_ClientId = NetworkManager.activeTransport.AddHost(m_HostTopology, m_HostPort, null);
            }
        }

        // this called in another thread! Cannot call Update() here.
        internal static void GetHostAddressesCallback(IAsyncResult ar)
        {
            try
            {
                IPAddress[] ip = Dns.EndGetHostAddresses(ar);
                NetworkClient client = (NetworkClient)ar.AsyncState;

                if (ip.Length == 0)
                {
                    if (LogFilter.logError) { Debug.LogError("DNS lookup failed for:" + client.m_RequestedServerHost); }
                    client.m_AsyncConnect = ConnectState.Failed;
                    return;
                }

                client.m_ServerIp = ip[0].ToString();
                client.m_AsyncConnect = ConnectState.Resolved;
                if (LogFilter.logDebug) { Debug.Log("Async DNS Result:" + client.m_ServerIp + " for " + client.m_RequestedServerHost + ": " + client.m_ServerIp); }
            }
            catch (SocketException e)
            {
                NetworkClient client = (NetworkClient)ar.AsyncState;
                if (LogFilter.logError) { Debug.LogError("DNS resolution failed: " + e.GetErrorCode()); }
                if (LogFilter.logDebug) { Debug.Log("Exception:" + e); }
                client.m_AsyncConnect = ConnectState.Failed;
            }
        }

        internal void ContinueConnect()
        {
            byte error;
            // regular non-relay connect
            if (m_UseSimulator)
            {
                int simLatency = m_SimulatedLatency / 3;
                if (simLatency < 1)
                {
                    simLatency = 1;
                }

                if (LogFilter.logDebug) { Debug.Log("Connect Using Simulator " + (m_SimulatedLatency / 3) + "/" + m_SimulatedLatency); }
                var simConfig = new ConnectionSimulatorConfig(
                    simLatency,
                    m_SimulatedLatency,
                    simLatency,
                    m_SimulatedLatency,
                    m_PacketLoss);

                m_ClientConnectionId = NetworkManager.activeTransport.ConnectWithSimulator(m_ClientId, m_ServerIp, m_ServerPort, 0, out error, simConfig);
            }
            else
            {
                m_ClientConnectionId = NetworkManager.activeTransport.Connect(m_ClientId, m_ServerIp, m_ServerPort, 0, out error);
            }

            m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
            m_Connection.SetHandlers(m_MessageHandlers);
            m_Connection.Initialize(m_ServerIp, m_ClientId, m_ClientConnectionId, m_HostTopology);
        }

        void ConnectWithRelay(MatchInfo info)
        {
            m_AsyncConnect = ConnectState.Connecting;

            Update();

            byte error;
            m_ClientConnectionId = NetworkManager.activeTransport.ConnectToNetworkPeer(
                m_ClientId,
                info.address,
                info.port,
                0,
                0,
                info.networkId,
                Utility.GetSourceID(),
                info.nodeId,
                out error);

            m_Connection = (NetworkConnection)Activator.CreateInstance(m_NetworkConnectionClass);
            m_Connection.SetHandlers(m_MessageHandlers);
            m_Connection.Initialize(info.address, m_ClientId, m_ClientConnectionId, m_HostTopology);

            if (error != 0) { Debug.LogError("ConnectToNetworkPeer Error: " + error); }
        }

        /// <summary>
        /// Disconnect from server.
        /// <para>The disconnect message will be invoked.</para>
        /// </summary>
        public virtual void Disconnect()
        {
            m_AsyncConnect = ConnectState.Disconnected;
            ClientScene.HandleClientDisconnect(m_Connection);
            if (m_Connection != null)
            {
                m_Connection.Disconnect();
                m_Connection.Dispose();
                m_Connection = null;
                if (m_ClientId != -1)
                {
                    NetworkManager.activeTransport.RemoveHost(m_ClientId);
                    m_ClientId = -1;
                }
            }
        }

        /// <summary>
        /// This sends a network message with a message Id to the server. This message is sent on channel zero, which by default is the reliable channel.
        /// <para>The message must be an instance of a class derived from MessageBase.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class RegisterHostMessage : MessageBase
        /// {
        ///    public string gameName;
        ///    public string comment;
        ///    public bool passwordProtected;
        /// }
        ///
        /// public class MasterClient
        /// {
        ///    public NetworkClient client;
        ///
        ///    public const short RegisterHostMsgId = 888;
        ///
        ///    public void RegisterHost(string name)
        ///    {
        ///        RegisterHostMessage msg = new RegisterHostMessage();
        ///        msg.gameName = name;
        ///        msg.comment = "test";
        ///        msg.passwordProtected = false;
        ///        client.Send(RegisterHostMsgId, msg);
        ///    }
        /// }
        /// </code>
        /// <para>The message id passed to Send() is used to identify the handler function to invoke on the server when the message is received.</para>
        /// </summary>
        /// <param name="msgType">The id of the message to send.</param>
        /// <param name="msg">A message instance to send.</param>
        /// <returns>True if message was sent.</returns>
        public bool Send(short msgType, MessageBase msg)
        {
            if (m_Connection != null)
            {
                if (m_AsyncConnect != ConnectState.Connected)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkClient Send when not connected to a server"); }
                    return false;
                }
#if UNITY_EDITOR
                Profiler.IncrementStatOutgoing(MsgType.UserMessage, msgType + ":" + msg.GetType().Name);
#endif
                return m_Connection.Send(msgType, msg);
            }
            if (LogFilter.logError) { Debug.LogError("NetworkClient Send with no connection"); }
            return false;
        }

        /// <summary>
        /// This sends the contents of the NetworkWriter's buffer to the connected server on the specified channel.
        /// <para>The format of the data in the writer must be properly formatted for it to be processed as a message by the server. The functions StartMessage() and FinishMessage() can be used to properly format messages:</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class TestClient
        /// {
        ///    public NetworkClient client;
        ///
        ///    public const int RegisterHostMsgId = 888;
        ///
        ///    public void RegisterHost(string name)
        ///    {
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(RegisterHostMsgId);
        ///        writer.Write(name);
        ///        writer.FinishMessage();
        ///        client.SendWriter(writer, Channels.DefaultReliable);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="writer">Writer object containing data to send.</param>
        /// <param name="channelId">QoS channel to send data on.</param>
        /// <returns>True if data successfully sent.</returns>
        public bool SendWriter(NetworkWriter writer, int channelId)
        {
            if (m_Connection != null)
            {
                if (m_AsyncConnect != ConnectState.Connected)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkClient SendWriter when not connected to a server"); }
                    return false;
                }
                return m_Connection.SendWriter(writer, channelId);
            }
            if (LogFilter.logError) { Debug.LogError("NetworkClient SendWriter with no connection"); }
            return false;
        }

        /// <summary>
        /// This sends the data in an array of bytes to the server that the client is connected to.
        /// <para>The data must be properly formatted.</para>
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="numBytes">Number of bytes of data.</param>
        /// <param name="channelId">The QoS channel to send data on.</param>
        /// <returns>True if successfully sent.</returns>
        public bool SendBytes(byte[] data, int numBytes, int channelId)
        {
            if (m_Connection != null)
            {
                if (m_AsyncConnect != ConnectState.Connected)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkClient SendBytes when not connected to a server"); }
                    return false;
                }
                return m_Connection.SendBytes(data, numBytes, channelId);
            }
            if (LogFilter.logError) { Debug.LogError("NetworkClient SendBytes with no connection"); }
            return false;
        }

        /// <summary>
        /// This sends a network message with a message Id to the server on channel one, which by default is the unreliable channel.
        /// <para>This does the same thing as NetworkClient.Send(), except that it send on the unreliable channel.</para>
        /// </summary>
        /// <param name="msgType">The message id to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <returns>True if the message was sent.</returns>
        public bool SendUnreliable(short msgType, MessageBase msg)
        {
            if (m_Connection != null)
            {
                if (m_AsyncConnect != ConnectState.Connected)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkClient SendUnreliable when not connected to a server"); }
                    return false;
                }
#if UNITY_EDITOR
                Profiler.IncrementStatOutgoing(MsgType.UserMessage, msgType + ":" + msg.GetType().Name);
#endif
                return m_Connection.SendUnreliable(msgType, msg);
            }
            if (LogFilter.logError) { Debug.LogError("NetworkClient SendUnreliable with no connection"); }
            return false;
        }

        /// <summary>
        /// This sends a network message with a message Id to the server on a specific channel.
        /// <para>This does the same thing as NetworkClient.Send(), but allows a transport layer QoS channel to be specified.</para>
        /// </summary>
        /// <param name="msgType">The id of the message to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="channelId">The channel to send the message on.</param>
        /// <returns>True if the message was sent.</returns>
        public bool SendByChannel(short msgType, MessageBase msg, int channelId)
        {
#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.UserMessage, msgType + ":" + msg.GetType().Name);
#endif
            if (m_Connection != null)
            {
                if (m_AsyncConnect != ConnectState.Connected)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkClient SendByChannel when not connected to a server"); }
                    return false;
                }
                return m_Connection.SendByChannel(msgType, msg, channelId);
            }
            if (LogFilter.logError) { Debug.LogError("NetworkClient SendByChannel with no connection"); }
            return false;
        }

        /// <summary>
        /// Set the maximum amount of time that can pass for transmitting the send buffer.
        /// </summary>
        /// <param name="seconds">Delay in seconds.</param>
        public void SetMaxDelay(float seconds)
        {
            if (m_Connection == null)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("SetMaxDelay failed, not connected."); }
                return;
            }
            m_Connection.SetMaxDelay(seconds);
        }

        /// <summary>
        /// Shut down a client.
        /// <para>This should be done when a client is no longer going to be used.</para>
        /// </summary>
        public void Shutdown()
        {
            if (LogFilter.logDebug) Debug.Log("Shutting down client " + m_ClientId);
            if (m_ClientId != -1)
            {
                NetworkManager.activeTransport.RemoveHost(m_ClientId);
                m_ClientId = -1;
            }
            RemoveClient(this);
            if (s_Clients.Count == 0)
            {
                SetActive(false);
            }
        }

        internal virtual void Update()
        {
            if (m_ClientId == -1)
            {
                return;
            }

            switch (m_AsyncConnect)
            {
                case ConnectState.None:
                case ConnectState.Resolving:
                case ConnectState.Disconnected:
                    return;

                case ConnectState.Failed:
                    GenerateConnectError((int)NetworkError.DNSFailure);
                    m_AsyncConnect = ConnectState.Disconnected;
                    return;

                case ConnectState.Resolved:
                    m_AsyncConnect = ConnectState.Connecting;
                    ContinueConnect();
                    return;

                case ConnectState.Connecting:
                case ConnectState.Connected:
                {
                    break;
                }
            }

            if (m_Connection != null)
            {
                if ((int)Time.time != m_StatResetTime)
                {
                    m_Connection.ResetStats();
                    m_StatResetTime = (int)Time.time;
                }
            }

            int numEvents = 0;
            NetworkEventType networkEvent;
            do
            {
                int connectionId;
                int channelId;
                int receivedSize;
                byte error;

                networkEvent = NetworkManager.activeTransport.ReceiveFromHost(m_ClientId, out connectionId, out channelId, m_MsgBuffer, (ushort)m_MsgBuffer.Length, out receivedSize, out error);
                if (m_Connection != null) m_Connection.lastError = (NetworkError)error;

                if (networkEvent != NetworkEventType.Nothing)
                {
                    if (LogFilter.logDev) { Debug.Log("Client event: host=" + m_ClientId + " event=" + networkEvent + " error=" + error); }
                }

                switch (networkEvent)
                {
                    case NetworkEventType.ConnectEvent:

                        if (LogFilter.logDebug) { Debug.Log("Client connected"); }

                        if (error != 0)
                        {
                            GenerateConnectError(error);
                            return;
                        }

                        m_AsyncConnect = ConnectState.Connected;
                        m_Connection.InvokeHandlerNoData(MsgType.Connect);
                        break;

                    case NetworkEventType.DataEvent:
                        if (error != 0)
                        {
                            GenerateDataError(error);
                            return;
                        }

#if UNITY_EDITOR
                        Profiler.IncrementStatIncoming(MsgType.LLAPIMsg);
#endif

                        m_MsgReader.SeekZero();
                        m_Connection.TransportReceive(m_MsgBuffer, receivedSize, channelId);
                        break;

                    case NetworkEventType.DisconnectEvent:
                        if (LogFilter.logDebug) { Debug.Log("Client disconnected"); }

                        m_AsyncConnect = ConnectState.Disconnected;

                        if (error != 0)
                        {
                            if ((NetworkError)error != NetworkError.Timeout)
                            {
                                GenerateDisconnectError(error);
                            }
                        }
                        ClientScene.HandleClientDisconnect(m_Connection);
                        if (m_Connection != null)
                        {
                            m_Connection.InvokeHandlerNoData(MsgType.Disconnect);
                        }
                        break;

                    case NetworkEventType.Nothing:
                        break;

                    default:
                        if (LogFilter.logError) { Debug.LogError("Unknown network message type received: " + networkEvent); }
                        break;
                }

                if (++numEvents >= k_MaxEventsPerFrame)
                {
                    if (LogFilter.logDebug) { Debug.Log("MaxEventsPerFrame hit (" + k_MaxEventsPerFrame + ")"); }
                    break;
                }
                if (m_ClientId == -1)
                {
                    break;
                }
            }
            while (networkEvent != NetworkEventType.Nothing);

            if (m_Connection != null &&  m_AsyncConnect == ConnectState.Connected)
                m_Connection.FlushChannels();
        }

        void GenerateConnectError(int error)
        {
            if (LogFilter.logError) { Debug.LogError("UNet Client Error Connect Error: " + error); }
            GenerateError(error);
        }

        void GenerateDataError(int error)
        {
            NetworkError dataError = (NetworkError)error;
            if (LogFilter.logError) { Debug.LogError("UNet Client Data Error: " + dataError); }
            GenerateError(error);
        }

        void GenerateDisconnectError(int error)
        {
            NetworkError disconnectError = (NetworkError)error;
            if (LogFilter.logError) { Debug.LogError("UNet Client Disconnect Error: " + disconnectError); }
            GenerateError(error);
        }

        void GenerateError(int error)
        {
            NetworkMessageDelegate msgDelegate = m_MessageHandlers.GetHandler(MsgType.Error);
            if (msgDelegate == null)
            {
                msgDelegate = m_MessageHandlers.GetHandler(MsgType.Error);
            }
            if (msgDelegate != null)
            {
                ErrorMessage msg = new ErrorMessage();
                msg.errorCode = error;

                // write the message to a local buffer
                byte[] errorBuffer = new byte[200];
                NetworkWriter writer = new NetworkWriter(errorBuffer);
                msg.Serialize(writer);

                // pass a reader (attached to local buffer) to handler
                NetworkReader reader = new NetworkReader(errorBuffer);

                NetworkMessage netMsg = new NetworkMessage();
                netMsg.msgType = MsgType.Error;
                netMsg.reader = reader;
                netMsg.conn = m_Connection;
                netMsg.channelId = 0;
                msgDelegate(netMsg);
            }
        }

        /// <summary>
        /// Get outbound network statistics for the client.
        /// </summary>
        /// <param name="numMsgs">Number of messages sent so far (including collated messages send through buffer).</param>
        /// <param name="numBufferedMsgs">Number of messages sent through buffer.</param>
        /// <param name="numBytes">Number of bytes sent so far.</param>
        /// <param name="lastBufferedPerSecond">Number of messages buffered for sending per second.</param>
        public void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
        {
            numMsgs = 0;
            numBufferedMsgs = 0;
            numBytes = 0;
            lastBufferedPerSecond = 0;

            if (m_Connection != null)
            {
                m_Connection.GetStatsOut(out numMsgs, out numBufferedMsgs, out numBytes, out lastBufferedPerSecond);
            }
        }

        /// <summary>
        /// Get inbound network statistics for the client.
        /// </summary>
        /// <param name="numMsgs">Number of messages received so far.</param>
        /// <param name="numBytes">Number of bytes received so far.</param>
        public void GetStatsIn(out int numMsgs, out int numBytes)
        {
            numMsgs = 0;
            numBytes = 0;

            if (m_Connection != null)
            {
                m_Connection.GetStatsIn(out numMsgs, out numBytes);
            }
        }

        /// <summary>
        /// Retrieves statistics about the network packets sent on this connection.
        /// </summary>
        /// <returns>Dictionary of packet statistics for the client's connection.</returns>
        public Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
        {
            if (m_Connection == null)
                return null;

            return m_Connection.packetStats;
        }

        /// <summary>
        /// Resets the statistics return by NetworkClient.GetConnectionStats() to zero values.
        /// <para>Useful when building per-second network statistics.</para>
        /// </summary>
        public void ResetConnectionStats()
        {
            if (m_Connection == null)
                return;

            m_Connection.ResetStats();
        }

        /// <summary>
        /// Gets the Return Trip Time for this connection.
        /// <para>This value is calculated by the transport layer.</para>
        /// </summary>
        /// <returns>Return trip time in milliseconds.</returns>
        public int GetRTT()
        {
            if (m_ClientId == -1)
                return 0;

            byte err;
            return NetworkManager.activeTransport.GetCurrentRTT(m_ClientId, m_ClientConnectionId, out err);
        }

        internal void RegisterSystemHandlers(bool localClient)
        {
            ClientScene.RegisterSystemHandlers(this, localClient);
            RegisterHandlerSafe(MsgType.CRC, OnCRC);
            RegisterHandlerSafe(MsgType.Fragment, NetworkConnection.OnFragment);
        }

        void OnCRC(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_CRCMessage);
            NetworkCRC.Validate(s_CRCMessage.scripts, numChannels);
        }

        /// <summary>
        /// Register a handler for a particular message type.
        /// <para>There are several system message types which you can add handlers for. You can also add your own message types.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Server : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        NetworkServer.Listen(7070);
        ///        Debug.Log("Registering server callbacks");
        ///        NetworkClient client = new NetworkClient();
        ///        client.RegisterHandler(MsgType.Connect, OnConnected);
        ///    }
        ///
        ///    void OnConnected(NetworkMessage netMsg)
        ///    {
        ///        Debug.Log("Client connected");
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="msgType">Message type number.</param>
        /// <param name="handler">Function handler which will be invoked for when this message type is received.</param>
        public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            m_MessageHandlers.RegisterHandler(msgType, handler);
        }

        public void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
        {
            m_MessageHandlers.RegisterHandlerSafe(msgType, handler);
        }

        /// <summary>
        /// Unregisters a network message handler.
        /// </summary>
        /// <param name="msgType">The message type to unregister.</param>
        public void UnregisterHandler(short msgType)
        {
            m_MessageHandlers.UnregisterHandler(msgType);
        }

        /// <summary>
        /// Retrieves statistics about the network packets sent on all connections.
        /// </summary>
        /// <returns>Dictionary of stats.</returns>
        static public Dictionary<short, NetworkConnection.PacketStat> GetTotalConnectionStats()
        {
            Dictionary<short, NetworkConnection.PacketStat> stats = new Dictionary<short, NetworkConnection.PacketStat>();
            for (int i = 0; i < s_Clients.Count; i++)
            {
                var client = s_Clients[i];
                var clientStats = client.GetConnectionStats();
                foreach (short k in clientStats.Keys)
                {
                    if (stats.ContainsKey(k))
                    {
                        NetworkConnection.PacketStat s = stats[k];
                        s.count += clientStats[k].count;
                        s.bytes += clientStats[k].bytes;
                        stats[k] = s;
                    }
                    else
                    {
                        stats[k] = new NetworkConnection.PacketStat(clientStats[k]);
                    }
                }
            }
            return stats;
        }

        internal static void AddClient(NetworkClient client)
        {
            s_Clients.Add(client);
        }

        internal static bool RemoveClient(NetworkClient client)
        {
            return s_Clients.Remove(client);
        }

        static internal void UpdateClients()
        {
            for (int i = 0; i < s_Clients.Count; ++i)
            {
                if (s_Clients[i] != null)
                    s_Clients[i].Update();
                else
                    s_Clients.RemoveAt(i);
            }
        }

        /// <summary>
        /// Shuts down all network clients.
        /// <para>This also shuts down the transport layer.</para>
        /// </summary>
        static public void ShutdownAll()
        {
            while (s_Clients.Count != 0)
            {
                s_Clients[0].Shutdown();
            }
            s_Clients = new List<NetworkClient>();
            s_IsActive = false;
            ClientScene.Shutdown();
#if UNITY_EDITOR
            Profiler.ResetAll();
#endif
        }

        internal static void SetActive(bool state)
        {
            // what is this check?
            //if (state == false && s_Clients.Count != 0)
            //  return;

            if (!s_IsActive && state)
            {
                NetworkManager.activeTransport.Init();
            }
            s_IsActive = state;
        }
    };
}
