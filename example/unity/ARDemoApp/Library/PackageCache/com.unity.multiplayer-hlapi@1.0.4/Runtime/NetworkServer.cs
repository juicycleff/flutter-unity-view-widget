using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
using UnityEngineInternal;

namespace UnityEngine.Networking
{
    /// <summary>
    /// The NetworkServer uses a NetworkServerSimple for basic network functionality and adds more game-like functionality.
    /// <para>NetworkServer handles remote connections from remote clients via a NetworkServerSimple instance, and also has a local connection for a local client.</para>
    /// <para>The NetworkServer is a singleton. It has static convenience functions such as NetworkServer.SendToAll() and NetworkServer.Spawn() which automatically use the singleton instance.</para>
    /// <para>The NetworkManager uses the NetworkServer, but it can be used without the NetworkManager.</para>
    /// <para>The set of networked objects that have been spawned is managed by NetworkServer. Objects are spawned with NetworkServer.Spawn() which adds them to this set, and makes them be created on clients. Spawned objects are removed automatically when they are destroyed, or than they can be removed from the spawned set by calling NetworkServer.UnSpawn() - this does not destroy the object.</para>
    /// <para>There are a number of internal messages used by NetworkServer, these are setup when NetworkServer.Listen() is called.</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public sealed class NetworkServer
    {
        static bool s_Active;
        static volatile NetworkServer s_Instance;
        static object s_Sync = new Object();
        static bool m_DontListen;
        bool m_LocalClientActive;

        // only used for localConnection accessor
        List<NetworkConnection> m_LocalConnectionsFakeList = new List<NetworkConnection>();
        ULocalConnectionToClient m_LocalConnection = null;

        NetworkScene m_NetworkScene;
        HashSet<int> m_ExternalConnections;
        ServerSimpleWrapper m_SimpleServerSimple;

        float m_MaxDelay = 0.1f;
        HashSet<NetworkInstanceId> m_RemoveList;
        int m_RemoveListCount;
        const int k_RemoveListInterval = 100;

        // this is cached here for easy access when checking the size of state update packets in NetworkIdentity
        static internal ushort maxPacketSize;

        // static message objects to avoid runtime-allocations
        static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();

        /// <summary>
        /// <para>A list of local connections on the server.</para>
        /// </summary>
        static public List<NetworkConnection> localConnections { get { return instance.m_LocalConnectionsFakeList; } }

        /// <summary>
        /// <para>The port that the server is listening on.</para>
        /// </summary>
        static public int listenPort { get { return instance.m_SimpleServerSimple.listenPort; } }
        /// <summary>
        /// <para>The transport layer hostId used by this server.</para>
        /// </summary>
        static public int serverHostId { get { return instance.m_SimpleServerSimple.serverHostId; } }
        /// <summary>
        /// <para>A list of all the current connections from clients.</para>
        /// <para>The connections in the list are at the index of their connectionId. There may be nulls in this list for disconnected clients.</para>
        /// </summary>
        static public ReadOnlyCollection<NetworkConnection> connections  { get { return instance.m_SimpleServerSimple.connections; } }
        /// <summary>
        /// <para>Dictionary of the message handlers registered with the server.</para>
        /// <para>The key to the dictionary is the message Id.</para>
        /// </summary>
        static public Dictionary<short, NetworkMessageDelegate> handlers { get { return instance.m_SimpleServerSimple.handlers; } }
        /// <summary>
        /// <para>The host topology that the server is using.</para>
        /// <para>This is read-only once the server is started.</para>
        /// </summary>
        static public HostTopology hostTopology { get { return instance.m_SimpleServerSimple.hostTopology; }}
        /// <summary>
        /// <para>This is a dictionary of networked objects that have been spawned on the server.</para>
        /// <para>The key to the dictionary is NetworkIdentity netId.</para>
        /// </summary>
        public static Dictionary<NetworkInstanceId, NetworkIdentity> objects { get { return instance.m_NetworkScene.localObjects; } }

        [Obsolete("Moved to NetworkMigrationManager")]
        public static bool sendPeerInfo { get { return false; } set {} }
        /// <summary>
        /// <para>If you enable this, the server will not listen for incoming connections on the regular network port.</para>
        /// <para>This can be used if the game is running in host mode and does not want external players to be able to connect - making it like a single-player game. Also this can be useful when using AddExternalConnection().</para>
        /// </summary>
        public static bool dontListen { get { return m_DontListen; } set { m_DontListen = value; } }
        /// <summary>
        /// <para>This makes the server listen for WebSockets connections instead of normal transport layer connections.</para>
        /// <para>This allows WebGL clients to connect to this server. Note that WebGL clients cannot listen for WebSocket connections, they can only make outgoing WebSockets connections.</para>
        /// </summary>
        public static bool useWebSockets { get { return instance.m_SimpleServerSimple.useWebSockets; } set { instance.m_SimpleServerSimple.useWebSockets = value; } }

        internal static NetworkServer instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_Sync)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new NetworkServer();
                        }
                    }
                }
                return s_Instance;
            }
        }
        /// <summary>
        /// <para>Checks if the server has been started.</para>
        /// <para>This will be true after NetworkServer.Listen() has been called.</para>
        /// </summary>
        public static bool active { get { return s_Active; } }
        /// <summary>
        /// <para>True is a local client is currently active on the server.</para>
        /// <para>This will be true for "Hosts" on hosted server games.</para>
        /// </summary>
        public static bool localClientActive { get { return instance.m_LocalClientActive; } }
        /// <summary>
        /// <para>The number of channels the network is configure with.</para>
        /// </summary>
        public static int numChannels { get { return instance.m_SimpleServerSimple.hostTopology.DefaultConfig.ChannelCount; } }

        /// <summary>
        /// <para>The maximum delay before sending packets on connections.</para>
        /// <para>In seconds. The default of 0.01 seconds means packets will be delayed at most by 10 milliseconds. Setting this to zero will disable HLAPI connection buffering.</para>
        /// </summary>
        public static float maxDelay { get { return instance.m_MaxDelay; } set { instance.InternalSetMaxDelay(value); } }

        /// <summary>
        /// <para>The class to be used when creating new network connections.</para>
        /// <para>This can be set with SetNetworkConnectionClass. This allows custom classes that do special processing of data from the transport layer to be used with the NetworkServer.</para>
        /// <para>See NetworkConnection.TransportSend and NetworkConnection.TransportReceive for details.</para>
        /// </summary>
        static public Type networkConnectionClass
        {
            get { return instance.m_SimpleServerSimple.networkConnectionClass; }
        }

        /// <summary>
        /// This sets the class used when creating new network connections.
        /// <para>The class must be derived from NetworkConnection.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static public void SetNetworkConnectionClass<T>() where T : NetworkConnection
        {
            instance.m_SimpleServerSimple.SetNetworkConnectionClass<T>();
        }

        NetworkServer()
        {
            NetworkManager.activeTransport.Init();
            if (LogFilter.logDev) { Debug.Log("NetworkServer Created version " + Version.Current); }
            m_RemoveList = new HashSet<NetworkInstanceId>();
            m_ExternalConnections = new HashSet<int>();
            m_NetworkScene = new NetworkScene();
            m_SimpleServerSimple = new ServerSimpleWrapper(this);
        }

        /// <summary>
        /// This configures the transport layer settings for the server.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///    void StartServer()
        ///    {
        ///        ConnectionConfig config = new ConnectionConfig();
        ///        config.AddChannel(QosType.ReliableSequenced);
        ///        config.AddChannel(QosType.UnreliableSequenced);
        ///        config.PacketSize = 500;
        ///        NetworkServer.Configure(config, 10);
        ///        NetworkServer.Listen(7070);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="config">Transport layer confuration object.</param>
        /// <param name="maxConnections">The maximum number of client connections to allow.</param>
        /// <returns>True if successfully configured.</returns>
        static public bool Configure(ConnectionConfig config, int maxConnections)
        {
            return instance.m_SimpleServerSimple.Configure(config, maxConnections);
        }

        /// <summary>
        /// This configures the transport layer settings for the server.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///    void StartServer()
        ///    {
        ///        ConnectionConfig config = new ConnectionConfig();
        ///        config.AddChannel(QosType.ReliableSequenced);
        ///        config.AddChannel(QosType.UnreliableSequenced);
        ///        config.PacketSize = 500;
        ///        NetworkServer.Configure(config, 10);
        ///        NetworkServer.Listen(7070);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="topology">Transport layer topology object to use.</param>
        /// <returns>True if successfully configured.</returns>
        static public bool Configure(HostTopology topology)
        {
            return instance.m_SimpleServerSimple.Configure(topology);
        }

        /// <summary>
        /// Reset the NetworkServer singleton.
        /// </summary>
        public static void Reset()
        {
#if UNITY_EDITOR
            Profiler.ResetAll();
#endif
            NetworkManager.activeTransport.Shutdown();
            NetworkManager.activeTransport.Init();

            s_Instance = null;
            s_Active = false;
        }

        /// <summary>
        /// This shuts down the server and disconnects all clients.
        /// </summary>
        public static void Shutdown()
        {
            if (s_Instance != null)
            {
                s_Instance.InternalDisconnectAll();

                if (m_DontListen)
                {
                    // was never started, so dont stop
                }
                else
                {
                    s_Instance.m_SimpleServerSimple.Stop();
                }

                s_Instance = null;
            }
            m_DontListen = false;
            s_Active = false;
        }
        
        static public bool Listen(MatchInfo matchInfo, int listenPort)
        {
            if (!matchInfo.usingRelay)
                return instance.InternalListen(null, listenPort);

            instance.InternalListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
            return true;
        }

        internal void RegisterMessageHandlers()
        {
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.Ready, OnClientReadyMessage);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.Command, OnCommandMessage);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.LocalPlayerTransform, NetworkTransform.HandleTransform);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.LocalChildTransform, NetworkTransformChild.HandleChildTransform);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.RemovePlayer, OnRemovePlayerMessage);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.Animation, NetworkAnimator.OnAnimationServerMessage);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.AnimationParameters, NetworkAnimator.OnAnimationParametersServerMessage);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.AnimationTrigger, NetworkAnimator.OnAnimationTriggerServerMessage);
            m_SimpleServerSimple.RegisterHandlerSafe(MsgType.Fragment, NetworkConnection.OnFragment);

            // also setup max packet size.
            maxPacketSize = hostTopology.DefaultConfig.PacketSize;
        }

        /// <summary>
        /// Starts a server using a Relay server. This is the manual way of using the Relay server, as the regular NetworkServer.Connect() will automatically use the Relay server if a match exists.
        /// </summary>
        /// <param name="relayIp">Relay server IP Address.</param>
        /// <param name="relayPort">Relay server port.</param>
        /// <param name="netGuid">GUID of the network to create.</param>
        /// <param name="sourceId">This server's sourceId.</param>
        /// <param name="nodeId">The node to join the network with.</param>
        static public void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
        {
            instance.InternalListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
        }

        void InternalListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
        {
            m_SimpleServerSimple.ListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
            s_Active = true;
            RegisterMessageHandlers();
        }

        /// <summary>
        /// Start the server on the given port number. Note that if a match has been created, this will listen using the Relay server instead of a local socket.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Manager : MonoBehaviour
        /// {
        ///    bool isAtStartup = true;
        ///
        ///    void Update()
        ///    {
        ///        if (Input.GetKeyDown(KeyCode.S) &amp;&amp; isAtStartup)
        ///        {
        ///            NetworkServer.Listen(4444);
        ///            NetworkServer.RegisterHandler(MsgType.Ready, OnPlayerReadyMessage);
        ///            isAtStartup = false;
        ///        }
        ///    }
        ///
        ///    public void OnPlayerReadyMessage(NetworkMessage netMsg)
        ///    {
        ///        // TODO: create player and call PlayerIsReady()
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="serverPort">Listen port number.</param>
        /// <returns>True if listen succeeded.</returns>
        static public bool Listen(int serverPort)
        {
            return instance.InternalListen(null, serverPort);
        }

        /// <summary>
        /// Start the server on the given port number. Note that if a match has been created, this will listen using the Relay server instead of a local socket.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Manager : MonoBehaviour
        /// {
        ///    bool isAtStartup = true;
        ///
        ///    void Update()
        ///    {
        ///        if (Input.GetKeyDown(KeyCode.S) && isAtStartup)
        ///        {
        ///            NetworkServer.Listen(4444);
        ///            NetworkServer.RegisterHandler(MsgType.Ready, OnPlayerReadyMessage);
        ///            isAtStartup = false;
        ///        }
        ///    }
        ///
        ///    public void OnPlayerReadyMessage(NetworkMessage netMsg)
        ///    {
        ///        // TODO: create player and call PlayerIsReady()
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="ipAddress">The IP address to bind to (optional).</param>
        /// <param name="serverPort">Listen port number.</param>
        /// <returns>True if listen succeeded.</returns>
        static public bool Listen(string ipAddress, int serverPort)
        {
            return instance.InternalListen(ipAddress, serverPort);
        }

        internal bool InternalListen(string ipAddress, int serverPort)
        {
            if (m_DontListen)
            {
                // dont start simpleServer - this mode uses external connections instead
                m_SimpleServerSimple.Initialize();
            }
            else
            {
                if (!m_SimpleServerSimple.Listen(ipAddress, serverPort))
                    return false;
            }

            maxPacketSize = hostTopology.DefaultConfig.PacketSize;
            s_Active = true;
            RegisterMessageHandlers();
            return true;
        }

        /// <summary>
        /// This allows a client that has been disconnected from a server, to become the host of a new version of the game.
        /// </summary>
        /// <param name="oldClient">The client that was connected to the old host.</param>
        /// <param name="port">The port to listen on.</param>
        /// <param name="matchInfo">Match information (may be null).</param>
        /// <param name="oldConnectionId"></param>
        /// <param name="peers"></param>
        /// <returns></returns>
        static public NetworkClient BecomeHost(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
        {
            return instance.BecomeHostInternal(oldClient, port, matchInfo, oldConnectionId, peers);
        }

        internal NetworkClient BecomeHostInternal(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
        {
            if (s_Active)
            {
                if (LogFilter.logError) { Debug.LogError("BecomeHost already a server."); }
                return null;
            }

            if (!NetworkClient.active)
            {
                if (LogFilter.logError) { Debug.LogError("BecomeHost NetworkClient not active."); }
                return null;
            }

            // setup a server

            NetworkServer.Configure(hostTopology);

            if (matchInfo == null)
            {
                if (LogFilter.logDev) { Debug.Log("BecomeHost Listen on " + port); }

                if (!NetworkServer.Listen(port))
                {
                    if (LogFilter.logError) { Debug.LogError("BecomeHost bind failed."); }
                    return null;
                }
            }
            else
            {
                if (LogFilter.logDev) { Debug.Log("BecomeHost match:" + matchInfo.networkId); }
                NetworkServer.ListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
            }

            // setup server objects
            foreach (var uv in ClientScene.objects.Values)
            {
                if (uv == null || uv.gameObject == null)
                    continue;

                NetworkIdentity.AddNetworkId(uv.netId.Value);

                //NOTE: have to pass false to isServer here so that onStartServer sets object up properly.
                m_NetworkScene.SetLocalObject(uv.netId, uv.gameObject, false, false);
                uv.OnStartServer(true);
            }

            // reset the client peer info(?)

            if (LogFilter.logDev) { Debug.Log("NetworkServer BecomeHost done. oldConnectionId:" + oldConnectionId); }
            RegisterMessageHandlers();

            if (!NetworkClient.RemoveClient(oldClient))
            {
                if (LogFilter.logError) { Debug.LogError("BecomeHost failed to remove client"); }
            }

            if (LogFilter.logDev) { Debug.Log("BecomeHost localClient ready"); }

            // make a localclient for me
            var newLocalClient = ClientScene.ReconnectLocalServer();
            ClientScene.Ready(newLocalClient.connection);

            // cause local players and objects to be reconnected
            ClientScene.SetReconnectId(oldConnectionId, peers);
            ClientScene.AddPlayer(ClientScene.readyConnection, 0);

            return newLocalClient;
        }

        void InternalSetMaxDelay(float seconds)
        {
            // set on existing connections
            for (int i = 0; i < connections.Count; i++)
            {
                NetworkConnection conn = connections[i];
                if (conn != null)
                    conn.SetMaxDelay(seconds);
            }

            // save for future connections
            m_MaxDelay = seconds;
        }

        // called by LocalClient to add itself. dont call directly.
        internal int AddLocalClient(LocalClient localClient)
        {
            if (m_LocalConnectionsFakeList.Count != 0)
            {
                Debug.LogError("Local Connection already exists");
                return -1;
            }

            m_LocalConnection = new ULocalConnectionToClient(localClient);
            m_LocalConnection.connectionId = 0;
            m_SimpleServerSimple.SetConnectionAtIndex(m_LocalConnection);

            // this is for backwards compatibility with localConnections property
            m_LocalConnectionsFakeList.Add(m_LocalConnection);

            m_LocalConnection.InvokeHandlerNoData(MsgType.Connect);

            return 0;
        }

        internal void RemoveLocalClient(NetworkConnection localClientConnection)
        {
            for (int i = 0; i < m_LocalConnectionsFakeList.Count; ++i)
            {
                if (m_LocalConnectionsFakeList[i].connectionId == localClientConnection.connectionId)
                {
                    m_LocalConnectionsFakeList.RemoveAt(i);
                    break;
                }
            }

            if (m_LocalConnection != null)
            {
                m_LocalConnection.Disconnect();
                m_LocalConnection.Dispose();
                m_LocalConnection = null;
            }
            m_LocalClientActive = false;
            m_SimpleServerSimple.RemoveConnectionAtIndex(0);
        }

        internal void SetLocalObjectOnServer(NetworkInstanceId netId, GameObject obj)
        {
            if (LogFilter.logDev) { Debug.Log("SetLocalObjectOnServer " + netId + " " + obj); }

            m_NetworkScene.SetLocalObject(netId, obj, false, true);
        }

        internal void ActivateLocalClientScene()
        {
            if (m_LocalClientActive)
                return;

            // ClientScene for a local connection is becoming active. any spawned objects need to be started as client objects
            m_LocalClientActive = true;
            foreach (var uv in objects.Values)
            {
                if (!uv.isClient)
                {
                    if (LogFilter.logDev) { Debug.Log("ActivateClientScene " + uv.netId + " " + uv.gameObject); }

                    ClientScene.SetLocalObject(uv.netId, uv.gameObject);
                    uv.OnStartClient();
                }
            }
        }

        /// <summary>
        /// Send a message structure with the given type number to all connected clients.
        /// <para>This applies to clients that are ready and not-ready.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class MyMessageTypes
        /// {
        ///    public static short MSG_LOGIN_RESPONSE = 1000;
        ///    public static short MSG_SCORE = 1005;
        /// };
        ///
        /// public class MyScoreMessage : MessageBase
        /// {
        ///    public int score;
        ///    public Vector3 scorePos;
        /// }
        ///
        /// class GameServer
        /// {
        ///    void SendScore(int score, Vector3 scorePos)
        ///    {
        ///        MyScoreMessage msg = new MyScoreMessage();
        ///        msg.score = score;
        ///        msg.scorePos = scorePos;
        ///        NetworkServer.SendToAll(MyMessageTypes.MSG_SCORE, msg);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="msgType">Message type.</param>
        /// <param name="msg">Message structure.</param>
        /// <param name="msgType">Message type.</param>
        /// <returns></returns>
        static public bool SendToAll(short msgType, MessageBase msg)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendToAll msgType:" + msgType); }

            bool result = true;

            // remote connections
            for (int i = 0; i < connections.Count; i++)
            {
                NetworkConnection conn = connections[i];
                if (conn != null)
                    result &= conn.Send(msgType, msg);
            }

            return result;
        }

        // this is like SendToReady - but it doesn't check the ready flag on the connection.
        // this is used for ObjectDestroy messages.
        static bool SendToObservers(GameObject contextObj, short msgType, MessageBase msg)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendToObservers id:" + msgType); }

            bool result = true;
            var uv = contextObj.GetComponent<NetworkIdentity>();
            if (uv == null || uv.observers == null)
                return false;

            int count = uv.observers.Count;
            for (int i = 0; i < count; i++)
            {
                var conn = uv.observers[i];
                result &= conn.Send(msgType, msg);
            }
            return result;
        }

        /// <summary>
        /// Send a message structure with the given type number to only clients which are ready.
        /// <para>See Networking.NetworkClient.Ready.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ReadyMsgTypes
        /// {
        ///    public static short MSG_LOGIN_RESPONSE = 1000;
        ///    public static short MSG_SCORE = 1005;
        /// };
        ///
        /// public class ReadyScoreMessage : MessageBase
        /// {
        ///    public int score;
        ///    public Vector3 scorePos;
        /// }
        ///
        /// class GameServer
        /// {
        ///    public GameObject gameObject;
        ///
        ///    void SendScore(int score, Vector3 scorePos)
        ///    {
        ///        ReadyScoreMessage msg = new ReadyScoreMessage();
        ///        msg.score = score;
        ///        msg.scorePos = scorePos;
        ///        NetworkServer.SendToReady(gameObject, ReadyMsgTypes.MSG_SCORE, msg);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="contextObj"></param>
        /// <param name="msgType">Message type.</param>
        /// <param name="msg">Message structure.</param>
        /// <returns>Success if message is sent.</returns>
        static public bool SendToReady(GameObject contextObj, short msgType, MessageBase msg)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendToReady id:" + msgType); }

            if (contextObj == null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    NetworkConnection conn = connections[i];
                    if (conn != null && conn.isReady)
                    {
                        conn.Send(msgType, msg);
                    }
                }
                return true;
            }

            bool result = true;
            var uv = contextObj.GetComponent<NetworkIdentity>();
            if (uv == null || uv.observers == null)
                return false;

            int count = uv.observers.Count;
            for (int i = 0; i < count; i++)
            {
                var conn = uv.observers[i];
                if (!conn.isReady)
                    continue;

                result &= conn.Send(msgType, msg);
            }
            return result;
        }

        /// <summary>
        /// Sends the contents of a NetworkWriter object to the ready players.
        /// </summary>
        /// <param name="contextObj"></param>
        /// <param name="writer">The writer object to send.</param>
        /// <param name="channelId">The QoS channel to send the data on.</param>
        static public void SendWriterToReady(GameObject contextObj, NetworkWriter writer, int channelId)
        {
            if (writer.AsArraySegment().Count > short.MaxValue)
            {
                throw new UnityException("NetworkWriter used buffer is too big!");
            }
            SendBytesToReady(contextObj, writer.AsArraySegment().Array, writer.AsArraySegment().Count, channelId);
        }

        /// <summary>
        /// This sends an array of bytes to all ready players.
        /// <para>This bypasses the usual serialization and message structures, allowing raw bytes to be send to all ready players. The contents will be processed as a message on the client of the player, so it must be structured properly.</para>
        /// </summary>
        /// <param name="contextObj"></param>
        /// <param name="buffer">Array of bytes to send.</param>
        /// <param name="numBytes">Size of array.</param>
        /// <param name="channelId">Transport layer channel id to send bytes on.</param>
        static public void SendBytesToReady(GameObject contextObj, byte[] buffer, int numBytes, int channelId)
        {
            if (contextObj == null)
            {
                // no context.. send to all ready connections
                bool success = true;
                for (int i = 0; i < connections.Count; i++)
                {
                    NetworkConnection conn = connections[i];
                    if (conn != null && conn.isReady)
                    {
                        if (!conn.SendBytes(buffer, numBytes, channelId))
                        {
                            success = false;
                        }
                    }
                }
                if (!success)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("SendBytesToReady failed"); }
                }
                return;
            }

            var uv = contextObj.GetComponent<NetworkIdentity>();
            try
            {
                bool success = true;
                int count = uv.observers.Count;
                for (int i = 0; i < count; i++)
                {
                    var conn = uv.observers[i];
                    if (!conn.isReady)
                        continue;

                    if (!conn.SendBytes(buffer, numBytes, channelId))
                    {
                        success = false;
                    }
                }
                if (!success)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("SendBytesToReady failed for " + contextObj); }
                }
            }
            catch (NullReferenceException)
            {
                // observers may be null if object has not been spawned
                if (LogFilter.logWarn) { Debug.LogWarning("SendBytesToReady object " + contextObj + " has not been spawned"); }
            }
        }

        /// <summary>
        /// This sends an array of bytes to a specific player.
        /// <para>This bypasses the usual serialization and message structures, allowing raw bytes to be send to a player. The contents will be processed as a message on the client of the player, so it must be structured properly.</para>
        /// </summary>
        /// <param name="player">The player to send the bytes to.</param>
        /// <param name="buffer">Array of bytes to send.</param>
        /// <param name="numBytes">Size of array.</param>
        /// <param name="channelId">Transport layer channel id to send bytes on.</param>
        public static void SendBytesToPlayer(GameObject player, byte[] buffer, int numBytes, int channelId)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn == null)
                    continue;

                for (int j = 0; j < conn.playerControllers.Count; j++)
                {
                    if (conn.playerControllers[j].IsValid && conn.playerControllers[j].gameObject == player)
                    {
                        conn.SendBytes(buffer, numBytes, channelId);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Send given message structure as an unreliable message to all connected clients.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class UnreliableMsgTypes
        /// {
        ///    public static short MSG_LOGIN_RESPONSE = 1000;
        ///    public static short MSG_SCORE = 1005;
        /// };
        ///
        /// public class UnreliableScoreMessage : MessageBase
        /// {
        ///    public int score;
        ///    public Vector3 scorePos;
        /// }
        ///
        /// class GameServer
        /// {
        ///    void SendScore(int score, Vector3 scorePos)
        ///    {
        ///        UnreliableScoreMessage msg = new UnreliableScoreMessage();
        ///        msg.score = score;
        ///        msg.scorePos = scorePos;
        ///        NetworkServer.SendUnreliableToAll(UnreliableMsgTypes.MSG_SCORE, msg);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="msgType">Message type.</param>
        /// <param name="msg">Message structure.</param>
        /// <returns>Success if message is sent.</returns>
        static public bool SendUnreliableToAll(short msgType, MessageBase msg)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendUnreliableToAll msgType:" + msgType); }

            bool result = true;
            for (int i = 0; i < connections.Count; i++)
            {
                NetworkConnection conn = connections[i];
                if (conn != null)
                    result &= conn.SendUnreliable(msgType, msg);
            }
            return result;
        }

        /// <summary>
        /// Send given message structure as an unreliable message only to ready clients.
        /// <para>See Networking.NetworkClient.Ready.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class UnreliableMessageTypes
        /// {
        ///    public static short MSG_LOGIN_RESPONSE = 1000;
        ///    public static short MSG_SCORE = 1005;
        /// };
        ///
        /// public class UnreliableMessage : MessageBase
        /// {
        ///    public int score;
        ///    public Vector3 scorePos;
        /// }
        ///
        /// class GameServer
        /// {
        ///    public GameObject gameObject;
        ///
        ///    void SendScore(int score, Vector3 scorePos)
        ///    {
        ///        UnreliableMessage msg = new UnreliableMessage();
        ///        msg.score = score;
        ///        msg.scorePos = scorePos;
        ///        NetworkServer.SendUnreliableToReady(gameObject, UnreliableMessageTypes.MSG_SCORE, msg);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="contextObj"></param>
        /// <param name="msgType">Message type.</param>
        /// <param name="msg">Message structure.</param>
        /// <returns>Success if message is sent.</returns>
        static public bool SendUnreliableToReady(GameObject contextObj, short msgType, MessageBase msg)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendUnreliableToReady id:" + msgType); }

            if (contextObj == null)
            {
                // no context.. send to all ready connections
                for (int i = 0; i < connections.Count; i++)
                {
                    var conn = connections[i];
                    if (conn != null && conn.isReady)
                    {
                        conn.SendUnreliable(msgType, msg);
                    }
                }
                return true;
            }

            bool result = true;
            var uv = contextObj.GetComponent<NetworkIdentity>();
            int count = uv.observers.Count;
            for (int i = 0; i < count; i++)
            {
                var conn = uv.observers[i];
                if (!conn.isReady)
                    continue;

                result &= conn.SendUnreliable(msgType, msg);
            }
            return result;
        }

        /// <summary>
        /// Sends a network message to all connected clients on a specified transport layer QoS channel.
        /// </summary>
        /// <param name="msgType">The message id.</param>
        /// <param name="msg">	The message to send.</param>
        /// <param name="channelId">The transport layer channel to use.</param>
        /// <returns>True if the message was sent.</returns>
        static public bool SendByChannelToAll(short msgType, MessageBase msg, int channelId)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendByChannelToAll id:" + msgType); }

            bool result = true;

            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                    result &= conn.SendByChannel(msgType, msg, channelId);
            }
            return result;
        }

        /// <summary>
        /// Sends a network message to all connected clients that are "ready" on a specified transport layer QoS channel.
        /// </summary>
        /// <param name="contextObj">An object to use for context when calculating object visibility. If null, then the message is sent to all ready clients.</param>
        /// <param name="msgType">The message id.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="channelId">The transport layer channel to send on.</param>
        /// <returns>True if the message was sent.</returns>
        static public bool SendByChannelToReady(GameObject contextObj, short msgType, MessageBase msg, int channelId)
        {
            if (LogFilter.logDev) { Debug.Log("Server.SendByChannelToReady msgType:" + msgType); }

            if (contextObj == null)
            {
                // no context.. send to all ready connections
                for (int i = 0; i < connections.Count; i++)
                {
                    var conn = connections[i];
                    if (conn != null && conn.isReady)
                    {
                        conn.SendByChannel(msgType, msg, channelId);
                    }
                }
                return true;
            }

            bool result = true;
            var uv = contextObj.GetComponent<NetworkIdentity>();
            int count = uv.observers.Count;
            for (int i = 0; i < count; i++)
            {
                var conn = uv.observers[i];
                if (!conn.isReady)
                    continue;

                result &= conn.SendByChannel(msgType, msg, channelId);
            }
            return result;
        }

        /// <summary>
        /// Disconnect all currently connected clients.
        /// <para>This can only be called on the server. Clients will receive the Disconnect message.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///    enum GameState
        ///    {
        ///        kInit,
        ///        kStart
        ///    }
        ///    GameState state;
        ///
        ///    public void Update()
        ///    {
        ///        if (state != GameState.kInit)
        ///        {
        ///            if (Input.GetKey(KeyCode.Escape))
        ///            {
        ///                Debug.Log("Disconnecting all!");
        ///                NetworkServer.DisconnectAll();
        ///                Application.LoadLevel("empty");
        ///                state = GameState.kStart;
        ///            }
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        static public void DisconnectAll()
        {
            instance.InternalDisconnectAll();
        }

        internal void InternalDisconnectAll()
        {
            m_SimpleServerSimple.DisconnectAllConnections();

            if (m_LocalConnection != null)
            {
                m_LocalConnection.Disconnect();
                m_LocalConnection.Dispose();
                m_LocalConnection = null;
            }

            m_LocalClientActive = false;
        }

        // The user should never need to pump the update loop manually
        internal static void Update()
        {
            if (s_Instance != null)
                s_Instance.InternalUpdate();
        }

        void UpdateServerObjects()
        {
            foreach (var uv in objects.Values)
            {
                try
                {
                    uv.UNetUpdate();
                }
                catch (NullReferenceException)
                {
                    //ignore nulls here.. they will be cleaned up by CheckForNullObjects below
                }
                catch (MissingReferenceException)
                {
                    //ignore missing ref here.. they will be cleaned up by CheckForNullObjects below
                }
            }

            // check for nulls in this list every N updates. doing it every frame is expensive and unneccessary
            if (m_RemoveListCount++ % k_RemoveListInterval == 0)
                CheckForNullObjects();
        }

        void CheckForNullObjects()
        {
            // cant iterate through Values here, since we need the keys of null objects to add to remove list.
            foreach (var k in objects.Keys)
            {
                var uv = objects[k];
                if (uv == null || uv.gameObject == null)
                {
                    m_RemoveList.Add(k);
                }
            }
            if (m_RemoveList.Count > 0)
            {
                foreach (var remove in m_RemoveList)
                {
                    objects.Remove(remove);
                }
                m_RemoveList.Clear();
            }
        }

        internal void InternalUpdate()
        {
            m_SimpleServerSimple.Update();

            if (m_DontListen)
            {
                m_SimpleServerSimple.UpdateConnections();
            }

            UpdateServerObjects();
        }

        void OnConnected(NetworkConnection conn)
        {
            if (LogFilter.logDebug) { Debug.Log("Server accepted client:" + conn.connectionId); }

            // add player info
            conn.SetMaxDelay(m_MaxDelay);

            conn.InvokeHandlerNoData(MsgType.Connect);

            SendCrc(conn);
        }

        void OnDisconnected(NetworkConnection conn)
        {
            conn.InvokeHandlerNoData(MsgType.Disconnect);

            for (int i = 0; i < conn.playerControllers.Count; i++)
            {
                if (conn.playerControllers[i].gameObject != null)
                {
                    //NOTE: should there be default behaviour here to destroy the associated player?
                    if (LogFilter.logWarn) { Debug.LogWarning("Player not destroyed when connection disconnected."); }
                }
            }

            if (LogFilter.logDebug) { Debug.Log("Server lost client:" + conn.connectionId); }
            conn.RemoveObservers();
            conn.Dispose();
        }

        void OnData(NetworkConnection conn, int receivedSize, int channelId)
        {
#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.LLAPIMsg);
#endif
            conn.TransportReceive(m_SimpleServerSimple.messageBuffer, receivedSize, channelId);
        }

        private void GenerateConnectError(int error)
        {
            if (LogFilter.logError) { Debug.LogError("UNet Server Connect Error: " + error); }
            GenerateError(null, error);
        }

        private void GenerateDataError(NetworkConnection conn, int error)
        {
            NetworkError dataError = (NetworkError)error;
            if (LogFilter.logError) { Debug.LogError("UNet Server Data Error: " + dataError); }
            GenerateError(conn, error);
        }

        private void GenerateDisconnectError(NetworkConnection conn, int error)
        {
            NetworkError disconnectError = (NetworkError)error;
            if (LogFilter.logError) { Debug.LogError("UNet Server Disconnect Error: " + disconnectError + " conn:[" + conn + "]:" + conn.connectionId); }
            GenerateError(conn, error);
        }

        private void GenerateError(NetworkConnection conn, int error)
        {
            if (handlers.ContainsKey(MsgType.Error))
            {
                ErrorMessage msg = new ErrorMessage();
                msg.errorCode = error;

                // write the message to a local buffer
                NetworkWriter writer = new NetworkWriter();
                msg.Serialize(writer);

                // pass a reader (attached to local buffer) to handler
                NetworkReader reader = new NetworkReader(writer);
                conn.InvokeHandler(MsgType.Error, reader, 0);
            }
        }

        /// <summary>
        /// Register a handler for a particular message type.
        /// <para>There are several system message types which you can add handlers for. You can also add your own message types.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class MyServer : NetworkManager
        /// {
        ///    void Start()
        ///    {
        ///        Debug.Log("Registering server callbacks");
        ///        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        ///    }
        ///
        ///    void OnConnected(NetworkMessage netMsg)
        ///    {
        ///        Debug.Log("Client connected");
        ///    }
        /// }
        /// </code>
        /// <para>The system message types are listed below:</para>
        /// <code>
        /// class MsgType
        /// {
        ///    public const short ObjectDestroy = 1;
        ///    public const short Rpc = 2;
        ///    public const short ObjectSpawn = 3;
        ///    public const short Owner = 4;
        ///    public const short Command = 5;
        ///    public const short LocalPlayerTransform = 6;
        ///    public const short SyncEvent = 7;
        ///    public const short UpdateVars = 8;
        ///    public const short SyncList = 9;
        ///    public const short ObjectSpawnScene = 10;
        ///    public const short NetworkInfo = 11;
        ///    public const short SpawnFinished = 12;
        ///    public const short ObjectHide = 13;
        ///    public const short CRC = 14;
        ///    public const short LocalClientAuthority = 15;
        /// }
        ///</code>
        ///<para>Most of these messages are for internal use only. Users should not define message ids in this range.</para>
        /// </summary>
        /// <param name="msgType">Message type number.</param>
        /// <param name="handler">Function handler which will be invoked for when this message type is received.</param>
        static public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            instance.m_SimpleServerSimple.RegisterHandler(msgType, handler);
        }

        /// <summary>
        /// Unregisters a handler for a particular message type.
        /// </summary>
        /// <param name="msgType">The message type to remove the handler for.</param>
        static public void UnregisterHandler(short msgType)
        {
            instance.m_SimpleServerSimple.UnregisterHandler(msgType);
        }

        /// <summary>
        /// Clear all registered callback handlers.
        /// </summary>
        static public void ClearHandlers()
        {
            instance.m_SimpleServerSimple.ClearHandlers();
        }

        /// <summary>
        /// Clears all registered spawn prefab and spawn handler functions for this server.
        /// </summary>
        static public void ClearSpawners()
        {
            NetworkScene.ClearSpawners();
        }

        /// <summary>
        /// Get outbound network statistics for the client.
        /// </summary>
        /// <param name="numMsgs">Number of messages sent so far (including collated messages send through buffer).</param>
        /// <param name="numBufferedMsgs">Number of messages sent through buffer.</param>
        /// <param name="numBytes">Number of bytes sent so far.</param>
        /// <param name="lastBufferedPerSecond">Number of messages buffered for sending per second.</param>
        static public void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
        {
            numMsgs = 0;
            numBufferedMsgs = 0;
            numBytes = 0;
            lastBufferedPerSecond = 0;

            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                {
                    int snumMsgs;
                    int snumBufferedMsgs;
                    int snumBytes;
                    int slastBufferedPerSecond;

                    conn.GetStatsOut(out snumMsgs, out snumBufferedMsgs, out snumBytes, out slastBufferedPerSecond);

                    numMsgs += snumMsgs;
                    numBufferedMsgs += snumBufferedMsgs;
                    numBytes += snumBytes;
                    lastBufferedPerSecond += slastBufferedPerSecond;
                }
            }
        }

        /// <summary>
        /// Get inbound network statistics for the server.
        /// </summary>
        /// <param name="numMsgs">Number of messages received so far.</param>
        /// <param name="numBytes">Number of bytes received so far.</param>
        static public void GetStatsIn(out int numMsgs, out int numBytes)
        {
            numMsgs = 0;
            numBytes = 0;
            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                {
                    int cnumMsgs;
                    int cnumBytes;

                    conn.GetStatsIn(out cnumMsgs, out cnumBytes);

                    numMsgs += cnumMsgs;
                    numBytes += cnumBytes;
                }
            }
        }

        /// <summary>
        /// Send a message to the client which owns the given player object instance.
        /// <para>This function is not very efficient. It is better to send a message directly on the connection object of the player - which can be obtained from the "connectionToClient" member variable on NetworkBehaviour components.</para>
        /// </summary>
        /// <param name="player">The players game object.</param>
        /// <param name="msgType">Message type.</param>
        /// <param name="msg">Message struct.</param>
        // send this message to the player only
        static public void SendToClientOfPlayer(GameObject player, short msgType, MessageBase msg)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                {
                    for (int j = 0; j < conn.playerControllers.Count; j++)
                    {
                        if (conn.playerControllers[j].IsValid && conn.playerControllers[j].gameObject == player)
                        {
                            conn.Send(msgType, msg);
                            return;
                        }
                    }
                }
            }

            if (LogFilter.logError) { Debug.LogError("Failed to send message to player object '" + player.name + ", not found in connection list"); }
        }

        /// <summary>
        /// Send a message to the client which owns the given connection ID.
        /// <para>It accepts the connection ID as a parameter as well as a message and MsgType. Remember to set the client up for receiving the messages by using NetworkClient.RegisterHandler. Also, for user messages you must use a MsgType with a higher ID number than MsgType.Highest.</para>
        /// <code>
        /// //The code shows how to set up a message, the MsgType and how to get the connectionID.
        /// //It also shows how to send the message to the client, as well as receive it.
        /// //Attach this script to a GameObject
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.Networking.NetworkSystem;
        ///
        /// //Create a class for the message you send to the Client
        /// public class RegisterHostMessage : MessageBase
        /// {
        ///    public string m_Name;
        ///    public string m_Comment;
        /// }
        ///
        /// public class Example : NetworkManager
        /// {
        ///    RegisterHostMessage m_Message;
        ///    //This is the Message Type you want to send to the Client. User messages must be above the Highest Message Type.
        ///    public const short m_MessageType = MsgType.Highest + 1;
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnServerConnect(NetworkConnection connection)
        ///    {
        ///        //Change the message to read the Player's connection ID and a comment
        ///        EditMessage("Player " + connection.connectionId, "Hi there.");
        ///        //Send the new message to the Client using the Server
        ///        NetworkServer.SendToClient(connection.connectionId, m_MessageType, m_Message);
        ///    }
        ///
        ///    //On the Client's side, detect when it connects to a Server
        ///    public override void OnClientConnect(NetworkConnection connection)
        ///    {
        ///        //Register and receive the message on the Client's side
        ///        client.RegisterHandler(m_MessageType, ReceiveMessage);
        ///    }
        ///
        ///    //Use this to edit the message to read what you want
        ///    void EditMessage(string myName, string myComment)
        ///    {
        ///        m_Message = new RegisterHostMessage();
        ///        //Change the message name and comment to be the ones you set
        ///        m_Message.m_Name = myName;
        ///        m_Message.m_Comment = myComment;
        ///    }
        ///
        ///    //Use this to receive the message from the Server on the Client's side
        ///    public void ReceiveMessage(NetworkMessage networkMessage)
        ///    {
        ///        //Read the message that comes in
        ///        RegisterHostMessage hostMessage = networkMessage.ReadMessage&lt;RegisterHostMessage&gt;();
        ///        //Store the name and comment as variables
        ///        string receivedName = hostMessage.m_Name;
        ///        string receivedComment = hostMessage.m_Comment;
        ///        //Output the Player name and comment
        ///        Debug.Log("Player Name : " + receivedName);
        ///        Debug.Log("Player Comment : " + receivedComment);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="connectionId">Client connection ID.</param>
        /// <param name="msgType">Message struct to send.</param>
        /// <param name="msg">Message type.</param>
        static public void SendToClient(int connectionId, short msgType, MessageBase msg)
        {
            if (connectionId < connections.Count)
            {
                var conn = connections[connectionId];
                if (conn != null)
                {
                    conn.Send(msgType, msg);
                    return;
                }
            }
            if (LogFilter.logError) { Debug.LogError("Failed to send message to connection ID '" + connectionId + ", not found in connection list"); }
        }

        static public bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
        {
            NetworkIdentity id;
            if (GetNetworkIdentity(player, out id))
            {
                id.SetDynamicAssetId(assetId);
            }
            return instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
        }

        /// <summary>
        /// This replaces the player object for a connection with a different player object. The old player object is not destroyed.
        /// <para>If a connection already has a player object, this can be used to replace that object with a different player object. This does NOT change the ready state of the connection, so it can safely be used while changing scenes.</para>
        /// </summary>
        /// <param name="conn">Connection which is adding the player.</param>
        /// <param name="player">Player object spawned for the player.</param>
        /// <param name="playerControllerId">The player controller ID number as specified by client.</param>
        /// <returns>True if player was replaced.</returns>
        static public bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
        {
            return instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
        }

        static public bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
        {
            NetworkIdentity id;
            if (GetNetworkIdentity(player, out id))
            {
                id.SetDynamicAssetId(assetId);
            }
            return instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
        }
        /// <summary>
        /// <para>When an AddPlayer message handler has received a request from a player, the server calls this to associate the player object with the connection.</para>
        /// <para>When a player is added for a connection, the client for that connection is made ready automatically. The player object is automatically spawned, so you do not need to call NetworkServer.Spawn for that object. This function is used for "adding" a player, not for "replacing" the player on a connection. If there is already a player on this playerControllerId for this connection, this will fail.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// class MyServer : MonoBehaviour
        /// {
        ///    public GameObject playerPrefab;
        ///
        ///    void Start()
        ///    {
        ///        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayerMessage);
        ///    }
        ///
        ///    void OnAddPlayerMessage(NetworkMessage netMsg)
        ///    {
        ///        GameObject thePlayer = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        ///        // This spawns the new player on all clients
        ///        NetworkServer.AddPlayerForConnection(conn, thePlayer, 0);
        ///    }
        /// }
        ///</code>
        /// </summary>
        /// <param name="conn">Connection which is adding the player.</param>
        /// <param name="player">Player object spawned for the player.</param>
        /// <param name="playerControllerId">The player controller ID number as specified by client.</param>
        /// <returns>True if player was added.</returns>
        static public bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
        {
            return instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
        }

        internal bool InternalAddPlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
        {
            NetworkIdentity playerNetworkIdentity;
            if (!GetNetworkIdentity(playerGameObject, out playerNetworkIdentity))
            {
                if (LogFilter.logError) { Debug.Log("AddPlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject); }
                return false;
            }
            playerNetworkIdentity.Reset();

            if (!CheckPlayerControllerIdForConnection(conn, playerControllerId))
                return false;

            // cannot have a player object in "Add" version
            PlayerController oldController = null;
            GameObject oldPlayer = null;
            if (conn.GetPlayerController(playerControllerId, out oldController))
            {
                oldPlayer = oldController.gameObject;
            }
            if (oldPlayer != null)
            {
                if (LogFilter.logError) { Debug.Log("AddPlayer: player object already exists for playerControllerId of " + playerControllerId); }
                return false;
            }

            PlayerController newPlayerController = new PlayerController(playerGameObject, playerControllerId);
            conn.SetPlayerController(newPlayerController);

            // Set the playerControllerId on the NetworkIdentity on the server, NetworkIdentity.SetLocalPlayer is not called on the server (it is on clients and that sets the playerControllerId there)
            playerNetworkIdentity.SetConnectionToClient(conn, newPlayerController.playerControllerId);

            SetClientReady(conn);

            if (SetupLocalPlayerForConnection(conn, playerNetworkIdentity, newPlayerController))
            {
                return true;
            }

            if (LogFilter.logDebug) { Debug.Log("Adding new playerGameObject object netId: " + playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + playerGameObject.GetComponent<NetworkIdentity>().assetId); }

            FinishPlayerForConnection(conn, playerNetworkIdentity, playerGameObject);
            if (playerNetworkIdentity.localPlayerAuthority)
            {
                playerNetworkIdentity.SetClientOwner(conn);
            }
            return true;
        }

        static bool CheckPlayerControllerIdForConnection(NetworkConnection conn, short playerControllerId)
        {
            if (playerControllerId < 0)
            {
                if (LogFilter.logError) { Debug.LogError("AddPlayer: playerControllerId of " + playerControllerId + " is negative"); }
                return false;
            }
            if (playerControllerId > PlayerController.MaxPlayersPerClient)
            {
                if (LogFilter.logError) { Debug.Log("AddPlayer: playerControllerId of " + playerControllerId + " is too high. max is " + PlayerController.MaxPlayersPerClient); }
                return false;
            }
            if (playerControllerId > PlayerController.MaxPlayersPerClient / 2)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("AddPlayer: playerControllerId of " + playerControllerId + " is unusually high"); }
            }
            return true;
        }

        bool SetupLocalPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, PlayerController newPlayerController)
        {
            if (LogFilter.logDev) { Debug.Log("NetworkServer SetupLocalPlayerForConnection netID:" + uv.netId); }

            var localConnection = conn as ULocalConnectionToClient;
            if (localConnection != null)
            {
                if (LogFilter.logDev) { Debug.Log("NetworkServer AddPlayer handling ULocalConnectionToClient"); }

                // Spawn this player for other players, instead of SpawnObject:
                if (uv.netId.IsEmpty())
                {
                    // it is allowed to provide an already spawned object as the new player object.
                    // so dont spawn it again.
                    uv.OnStartServer(true);
                }
                uv.RebuildObservers(true);
                SendSpawnMessage(uv, null);

                // Set up local player instance on the client instance and update local object map
                localConnection.localClient.AddLocalPlayer(newPlayerController);
                uv.SetClientOwner(conn);

                // Trigger OnAuthority
                uv.ForceAuthority(true);

                // Trigger OnStartLocalPlayer
                uv.SetLocalPlayer(newPlayerController.playerControllerId);
                return true;
            }
            return false;
        }

        static void FinishPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, GameObject playerGameObject)
        {
            if (uv.netId.IsEmpty())
            {
                // it is allowed to provide an already spawned object as the new player object.
                // so dont spawn it again.
                Spawn(playerGameObject);
            }

            OwnerMessage owner = new OwnerMessage();
            owner.netId = uv.netId;
            owner.playerControllerId = uv.playerControllerId;
            conn.Send(MsgType.Owner, owner);
        }

        internal bool InternalReplacePlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
        {
            NetworkIdentity playerNetworkIdentity;
            if (!GetNetworkIdentity(playerGameObject, out playerNetworkIdentity))
            {
                if (LogFilter.logError) { Debug.LogError("ReplacePlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject); }
                return false;
            }

            if (!CheckPlayerControllerIdForConnection(conn, playerControllerId))
                return false;

            //NOTE: there can be an existing player
            if (LogFilter.logDev) { Debug.Log("NetworkServer ReplacePlayer"); }

            // is there already an owner that is a different object??
            PlayerController oldOwner;
            if (conn.GetPlayerController(playerControllerId, out oldOwner))
            {
                oldOwner.unetView.SetNotLocalPlayer();
                oldOwner.unetView.ClearClientOwner();
            }

            PlayerController newPlayerController = new PlayerController(playerGameObject, playerControllerId);
            conn.SetPlayerController(newPlayerController);

            // Set the playerControllerId on the NetworkIdentity on the server, NetworkIdentity.SetLocalPlayer is not called on the server (it is on clients and that sets the playerControllerId there)
            playerNetworkIdentity.SetConnectionToClient(conn, newPlayerController.playerControllerId);

            //NOTE: DONT set connection ready.

            if (LogFilter.logDev) { Debug.Log("NetworkServer ReplacePlayer setup local"); }

            if (SetupLocalPlayerForConnection(conn, playerNetworkIdentity, newPlayerController))
            {
                return true;
            }

            if (LogFilter.logDebug) { Debug.Log("Replacing playerGameObject object netId: " + playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + playerGameObject.GetComponent<NetworkIdentity>().assetId); }

            FinishPlayerForConnection(conn, playerNetworkIdentity, playerGameObject);
            if (playerNetworkIdentity.localPlayerAuthority)
            {
                playerNetworkIdentity.SetClientOwner(conn);
            }
            return true;
        }

        static bool GetNetworkIdentity(GameObject go, out NetworkIdentity view)
        {
            view = go.GetComponent<NetworkIdentity>();
            if (view == null)
            {
                if (LogFilter.logError) { Debug.LogError("UNET failure. GameObject doesn't have NetworkIdentity."); }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sets the client to be ready.
        /// <para>When a client has signaled that it is ready, this method tells the server that the client is ready to receive spawned objects and state synchronization updates. This is usually called in a handler for the SYSTEM_READY message. If there is not specific action a game needs to take for this message, relying on the default ready handler function is probably fine, so this call wont be needed.</para>
        /// </summary>
        /// <param name="conn">The connection of the client to make ready.</param>
        static public void SetClientReady(NetworkConnection conn)
        {
            instance.SetClientReadyInternal(conn);
        }

        internal void SetClientReadyInternal(NetworkConnection conn)
        {
            if (LogFilter.logDebug) { Debug.Log("SetClientReadyInternal for conn:" + conn.connectionId); }

            if (conn.isReady)
            {
                if (LogFilter.logDebug) { Debug.Log("SetClientReady conn " + conn.connectionId + " already ready"); }
                return;
            }

            if (conn.playerControllers.Count == 0)
            {
                // this is now allowed
                if (LogFilter.logDebug) { Debug.LogWarning("Ready with no player object"); }
            }

            conn.isReady = true;

            var localConnection = conn as ULocalConnectionToClient;
            if (localConnection != null)
            {
                if (LogFilter.logDev) { Debug.Log("NetworkServer Ready handling ULocalConnectionToClient"); }

                // Setup spawned objects for local player
                // Only handle the local objects for the first player (no need to redo it when doing more local players)
                // and don't handle player objects here, they were done above
                foreach (NetworkIdentity uv in objects.Values)
                {
                    // Need to call OnStartClient directly here, as it's already been added to the local object dictionary
                    // in the above SetLocalPlayer call
                    if (uv != null && uv.gameObject != null)
                    {
                        var vis = uv.OnCheckObserver(conn);
                        if (vis)
                        {
                            uv.AddObserver(conn);
                        }
                        if (!uv.isClient)
                        {
                            if (LogFilter.logDev) { Debug.Log("LocalClient.SetSpawnObject calling OnStartClient"); }
                            uv.OnStartClient();
                        }
                    }
                }
                return;
            }

            // Spawn/update all current server objects
            if (LogFilter.logDebug) { Debug.Log("Spawning " + objects.Count + " objects for conn " + conn.connectionId); }

            ObjectSpawnFinishedMessage msg = new ObjectSpawnFinishedMessage();
            msg.state = 0;
            conn.Send(MsgType.SpawnFinished, msg);

            foreach (NetworkIdentity uv in objects.Values)
            {
                if (uv == null)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("Invalid object found in server local object list (null NetworkIdentity)."); }
                    continue;
                }
                if (!uv.gameObject.activeSelf)
                {
                    continue;
                }

                if (LogFilter.logDebug) { Debug.Log("Sending spawn message for current server objects name='" + uv.gameObject.name + "' netId=" + uv.netId); }

                var vis = uv.OnCheckObserver(conn);
                if (vis)
                {
                    uv.AddObserver(conn);
                }
            }

            msg.state = 1;
            conn.Send(MsgType.SpawnFinished, msg);
        }

        static internal void ShowForConnection(NetworkIdentity uv, NetworkConnection conn)
        {
            if (conn.isReady)
                instance.SendSpawnMessage(uv, conn);
        }

        static internal void HideForConnection(NetworkIdentity uv, NetworkConnection conn)
        {
            ObjectDestroyMessage msg = new ObjectDestroyMessage();
            msg.netId = uv.netId;
            conn.Send(MsgType.ObjectHide, msg);
        }

        /// <summary>
        /// Marks all connected clients as no longer ready.
        /// <para>All clients will no longer be sent state synchronization updates. The player's clients can call ClientManager.Ready() again to re-enter the ready state. This is useful when switching scenes.</para>
        /// </summary>
        // call this to make all the clients not ready, such as when changing levels.
        static public void SetAllClientsNotReady()
        {
            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                {
                    SetClientNotReady(conn);
                }
            }
        }

        /// <summary>
        /// Sets the client of the connection to be not-ready.
        /// <para>Clients that are not ready do not receive spawned objects or state synchronization updates. They client can be made ready again by calling SetClientReady().</para>
        /// </summary>
        /// <param name="conn">The connection of the client to make not ready.</param>
        static public void SetClientNotReady(NetworkConnection conn)
        {
            instance.InternalSetClientNotReady(conn);
        }

        internal void InternalSetClientNotReady(NetworkConnection conn)
        {
            if (conn.isReady)
            {
                if (LogFilter.logDebug) { Debug.Log("PlayerNotReady " + conn); }
                conn.isReady = false;
                conn.RemoveObservers();

                NotReadyMessage msg = new NotReadyMessage();
                conn.Send(MsgType.NotReady, msg);
            }
        }

        // default ready handler.
        static void OnClientReadyMessage(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("Default handler for ready message from " + netMsg.conn); }
            SetClientReady(netMsg.conn);
        }

        // default remove player handler
        static void OnRemovePlayerMessage(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_RemovePlayerMessage);

            PlayerController player = null;
            netMsg.conn.GetPlayerController(s_RemovePlayerMessage.playerControllerId, out player);
            if (player != null)
            {
                netMsg.conn.RemovePlayerController(s_RemovePlayerMessage.playerControllerId);
                Destroy(player.gameObject);
            }
            else
            {
                if (LogFilter.logError) { Debug.LogError("Received remove player message but could not find the player ID: " + s_RemovePlayerMessage.playerControllerId); }
            }
        }

        // Handle command from specific player, this could be one of multiple players on a single client
        static  void OnCommandMessage(NetworkMessage netMsg)
        {
            int cmdHash = (int)netMsg.reader.ReadPackedUInt32();
            var netId = netMsg.reader.ReadNetworkId();

            var cmdObject = FindLocalObject(netId);
            if (cmdObject == null)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Instance not found when handling Command message [netId=" + netId + "]"); }
                return;
            }

            var uv = cmdObject.GetComponent<NetworkIdentity>();
            if (uv == null)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkIdentity deleted when handling Command message [netId=" + netId + "]"); }
                return;
            }

            // Commands can be for player objects, OR other objects with client-authority
            bool foundOwner = false;
            for (int i = 0; i < netMsg.conn.playerControllers.Count; i++)
            {
                var p = netMsg.conn.playerControllers[i];
                if (p.gameObject != null && p.gameObject.GetComponent<NetworkIdentity>().netId == uv.netId)
                {
                    foundOwner = true;
                    break;
                }
            }
            if (!foundOwner)
            {
                if (uv.clientAuthorityOwner != netMsg.conn)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("Command for object without authority [netId=" + netId + "]"); }
                    return;
                }
            }

            if (LogFilter.logDev) { Debug.Log("OnCommandMessage for netId=" + netId + " conn=" + netMsg.conn); }
            uv.HandleCommand(cmdHash, netMsg.reader);
        }

        internal void SpawnObject(GameObject obj)
        {
            if (!NetworkServer.active)
            {
                if (LogFilter.logError) { Debug.LogError("SpawnObject for " + obj + ", NetworkServer is not active. Cannot spawn objects without an active server."); }
                return;
            }

            NetworkIdentity objNetworkIdentity;
            if (!GetNetworkIdentity(obj, out objNetworkIdentity))
            {
                if (LogFilter.logError) { Debug.LogError("SpawnObject " + obj + " has no NetworkIdentity. Please add a NetworkIdentity to " + obj); }
                return;
            }
            objNetworkIdentity.Reset();

            objNetworkIdentity.OnStartServer(false);

            if (LogFilter.logDebug) { Debug.Log("SpawnObject instance ID " + objNetworkIdentity.netId + " asset ID " + objNetworkIdentity.assetId); }

            objNetworkIdentity.RebuildObservers(true);
            //SendSpawnMessage(objNetworkIdentity, null);
        }

        /*
          TODO: optimize BuildSpawnMsg to not do allocations.
                - this would need a static m_MsgStreamOut and m_MsgWriter.
                - payload needs to be separate sub-msg?

        internal short BuildSpawnBytes(NetworkIdentity uv)
        {
            m_MsgStreamIn.Seek(0, SeekOrigin.Begin);
            m_MsgWriter.Serialize((short)0); // space for size
            m_MsgWriter.UWriteUInt32((uint)MsgType.ObjectSpawn);
            m_MsgWriter.UWriteUInt32(uv.netId);
            m_MsgWriter.Serialize(uv.spawnType);
            m_MsgWriter.Serialize(uv.assetId);
            m_MsgWriter.Serialize(uv.transform.position);
            //payload - this is optional?
            uv.UNetSerializeTransform(m_MsgWriter, true);
            uv.UNetSerializeVars(m_MsgWriter, true);

            short sz = (short)(m_MsgStreamIn.Position - sizeof(short));
            m_MsgStreamIn.Seek(0, SeekOrigin.Begin);
            m_MsgWriter.Serialize(sz);

            return (short)(sz + sizeof(short));
        }*/

        internal void SendSpawnMessage(NetworkIdentity uv, NetworkConnection conn)
        {
            if (uv.serverOnly)
                return;

            if (uv.sceneId.IsEmpty())
            {
                ObjectSpawnMessage msg = new ObjectSpawnMessage();
                msg.netId = uv.netId;
                msg.assetId = uv.assetId;
                msg.position = uv.transform.position;
                msg.rotation = uv.transform.rotation;

                // include synch data
                NetworkWriter writer = new NetworkWriter();
                uv.UNetSerializeAllVars(writer);
                if (writer.Position > 0)
                {
                    msg.payload = writer.ToArray();
                }

                if (conn != null)
                {
                    conn.Send(MsgType.ObjectSpawn, msg);
                }
                else
                {
                    SendToReady(uv.gameObject, MsgType.ObjectSpawn, msg);
                }

#if UNITY_EDITOR
                Profiler.IncrementStatOutgoing(MsgType.ObjectSpawn, uv.assetId.ToString());
#endif
            }
            else
            {
                ObjectSpawnSceneMessage msg = new ObjectSpawnSceneMessage();
                msg.netId = uv.netId;
                msg.sceneId = uv.sceneId;
                msg.position = uv.transform.position;

                // include synch data
                NetworkWriter writer = new NetworkWriter();
                uv.UNetSerializeAllVars(writer);
                if (writer.Position > 0)
                {
                    msg.payload = writer.ToArray();
                }

                if (conn != null)
                {
                    conn.Send(MsgType.ObjectSpawnScene, msg);
                }
                else
                {
                    SendToReady(uv.gameObject, MsgType.ObjectSpawn, msg);
                }

#if UNITY_EDITOR
                Profiler.IncrementStatOutgoing(MsgType.ObjectSpawnScene, "sceneId");
#endif
            }
        }

        /// <summary>
        /// This destroys all the player objects associated with a NetworkConnections on a server.
        /// <para>This is used when a client disconnects, to remove the players for that client. This also destroys non-player objects that have client authority set for this connection.</para>
        /// </summary>
        /// <param name="conn">The connections object to clean up for.</param>
        static public void DestroyPlayersForConnection(NetworkConnection conn)
        {
            if (conn.playerControllers.Count == 0)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Empty player list given to NetworkServer.Destroy(), nothing to do."); }
                return;
            }

            if (conn.clientOwnedObjects != null)
            {
                var tmp = new HashSet<NetworkInstanceId>(conn.clientOwnedObjects);
                foreach (var netId in tmp)
                {
                    var obj = FindLocalObject(netId);
                    if (obj != null)
                    {
                        DestroyObject(obj);
                    }
                }
            }

            for (int i = 0; i < conn.playerControllers.Count; i++)
            {
                var player = conn.playerControllers[i];
                if (player.IsValid)
                {
                    if (player.unetView == null)
                    {
                        // the playerController's object has been destroyed, but RemovePlayerForConnection was never called.
                        // this is ok, just dont double destroy it.
                    }
                    else
                    {
                        DestroyObject(player.unetView, true);
                    }
                    player.gameObject = null;
                }
            }
            conn.playerControllers.Clear();
        }

        static void UnSpawnObject(GameObject obj)
        {
            if (obj == null)
            {
                if (LogFilter.logDev) { Debug.Log("NetworkServer UnspawnObject is null"); }
                return;
            }

            NetworkIdentity objNetworkIdentity;
            if (!GetNetworkIdentity(obj, out objNetworkIdentity)) return;

            UnSpawnObject(objNetworkIdentity);
        }

        static void UnSpawnObject(NetworkIdentity uv)
        {
            DestroyObject(uv, false);
        }

        static void DestroyObject(GameObject obj)
        {
            if (obj == null)
            {
                if (LogFilter.logDev) { Debug.Log("NetworkServer DestroyObject is null"); }
                return;
            }

            NetworkIdentity objNetworkIdentity;
            if (!GetNetworkIdentity(obj, out objNetworkIdentity)) return;

            DestroyObject(objNetworkIdentity, true);
        }

        static void DestroyObject(NetworkIdentity uv, bool destroyServerObject)
        {
            if (LogFilter.logDebug) { Debug.Log("DestroyObject instance:" + uv.netId); }
            if (objects.ContainsKey(uv.netId))
            {
                objects.Remove(uv.netId);
            }

            if (uv.clientAuthorityOwner != null)
            {
                uv.clientAuthorityOwner.RemoveOwnedObject(uv);
            }

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.ObjectDestroy, uv.assetId.ToString());
#endif

            ObjectDestroyMessage msg = new ObjectDestroyMessage();
            msg.netId = uv.netId;
            SendToObservers(uv.gameObject, MsgType.ObjectDestroy, msg);

            uv.ClearObservers();
            if (NetworkClient.active && instance.m_LocalClientActive)
            {
                uv.OnNetworkDestroy();
                ClientScene.SetLocalObject(msg.netId, null);
            }

            // when unspawning, dont destroy the server's object
            if (destroyServerObject)
            {
                Object.Destroy(uv.gameObject);
            }
            uv.MarkForReset();
        }

        /// <summary>
        /// This clears all of the networked objects that the server is aware of. This can be required if a scene change deleted all of the objects without destroying them in the normal manner.
        /// </summary>
        static public void ClearLocalObjects()
        {
            objects.Clear();
        }

        /// <summary>
        /// Spawn the given game object on all clients which are ready.
        /// <para>This will cause a new object to be instantiated from the registered prefab, or from a custom spawn function.</para>
        /// <code>
        /// //Attach this script to the GameObject you would like to be spawned.
        /// //Attach a NetworkIdentity component to your GameObject. Click and drag the GameObject into the Assets directory so that it becomes a prefab.
        /// //The GameObject you assign in the Inspector spawns when the Client connects. To spawn a prefab GameObject, use Instantiate first before spawning the GameObject.
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : NetworkBehaviour
        /// {
        ///    //Assign the prefab in the Inspector
        ///    public GameObject m_MyGameObject;
        ///    GameObject m_MyInstantiated;
        ///
        ///    void Start()
        ///    {
        ///        //Instantiate the prefab
        ///        m_MyInstantiated = Instantiate(m_MyGameObject);
        ///        //Spawn the GameObject you assign in the Inspector
        ///        NetworkServer.Spawn(m_MyInstantiated);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="obj">Game object with NetworkIdentity to spawn.</param>
        static public void Spawn(GameObject obj)
        {
            if (!VerifyCanSpawn(obj))
            {
                return;
            }

            instance.SpawnObject(obj);
        }

        static bool CheckForPrefab(GameObject obj)
        {
#if UNITY_EDITOR
            return UnityEditor.PrefabUtility.IsPartOfPrefabAsset(obj);
#else
            return false;
#endif
        }

        static bool VerifyCanSpawn(GameObject obj)
        {
            if (CheckForPrefab(obj))
            {
                Debug.LogErrorFormat("GameObject {0} is a prefab, it can't be spawned. This will cause errors in builds.", obj.name);
                return false;
            }

            return true;
        }

        /// <summary>
        /// This spawns an object like NetworkServer.Spawn() but also assigns Client Authority to the specified client.
        /// <para>This is the same as calling NetworkIdentity.AssignClientAuthority on the spawned object.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// 
        /// class TestBehaviour : NetworkBehaviour
        /// {
        ///    public GameObject otherPrefab;
        ///    [Command]
        ///    public void CmdSpawn()
        ///    {
        ///        GameObject go = (GameObject)Instantiate(otherPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        ///        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="obj">The object to spawn.</param>
        /// <param name="player">The player object to set Client Authority to.</param>
        /// <returns></returns>
        static public Boolean SpawnWithClientAuthority(GameObject obj, GameObject player)
        {
            var uv = player.GetComponent<NetworkIdentity>();
            if (uv == null)
            {
                Debug.LogError("SpawnWithClientAuthority player object has no NetworkIdentity");
                return false;
            }

            if (uv.connectionToClient == null)
            {
                Debug.LogError("SpawnWithClientAuthority player object is not a player.");
                return false;
            }

            return SpawnWithClientAuthority(obj, uv.connectionToClient);
        }

        /// <summary>
        /// This spawns an object like NetworkServer.Spawn() but also assigns Client Authority to the specified client.
        /// <para>This is the same as calling NetworkIdentity.AssignClientAuthority on the spawned object.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// 
        /// class TestBehaviour : NetworkBehaviour
        /// {
        ///    public GameObject otherPrefab;
        ///    [Command]
        ///    public void CmdSpawn()
        ///    {
        ///        GameObject go = (GameObject)Instantiate(otherPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        ///        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="obj">The object to spawn.</param>
        /// <param name="conn">The connection to set Client Authority to.</param>
        /// <returns></returns>
        static public bool SpawnWithClientAuthority(GameObject obj, NetworkConnection conn)
        {
            if (!conn.isReady)
            {
                Debug.LogError("SpawnWithClientAuthority NetworkConnection is not ready!");
                return false;
            }

            Spawn(obj);

            var uv = obj.GetComponent<NetworkIdentity>();
            if (uv == null || !uv.isServer)
            {
                // spawning the object failed.
                return false;
            }

            return uv.AssignClientAuthority(conn);
        }

        /// <summary>
        /// This spawns an object like NetworkServer.Spawn() but also assigns Client Authority to the specified client.
        /// <para>This is the same as calling NetworkIdentity.AssignClientAuthority on the spawned object.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// 
        /// class TestBehaviour : NetworkBehaviour
        /// {
        ///    public GameObject otherPrefab;
        ///    [Command]
        ///    public void CmdSpawn()
        ///    {
        ///        GameObject go = (GameObject)Instantiate(otherPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        ///        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="obj">The object to spawn.</param>
        /// <param name="assetId">The assetId of the object to spawn. Used for custom spawn handlers.</param>
        /// <param name="conn">The connection to set Client Authority to.</param>
        /// <returns></returns>
        static public bool SpawnWithClientAuthority(GameObject obj, NetworkHash128 assetId, NetworkConnection conn)
        {
            Spawn(obj, assetId);

            var uv = obj.GetComponent<NetworkIdentity>();
            if (uv == null || !uv.isServer)
            {
                // spawning the object failed.
                return false;
            }

            return uv.AssignClientAuthority(conn);
        }

        static public void Spawn(GameObject obj, NetworkHash128 assetId)
        {
            if (!VerifyCanSpawn(obj))
            {
                return;
            }

            NetworkIdentity id;
            if (GetNetworkIdentity(obj, out id))
            {
                id.SetDynamicAssetId(assetId);
            }
            instance.SpawnObject(obj);
        }

        /// <summary>
        /// Destroys this object and corresponding objects on all clients.
        /// <para>In some cases it is useful to remove an object but not delete it on the server. For that, use NetworkServer.UnSpawn() instead of NetworkServer.Destroy().</para>
        /// </summary>
        /// <param name="obj">Game object to destroy.</param>
        static public void Destroy(GameObject obj)
        {
            DestroyObject(obj);
        }

        /// <summary>
        /// This takes an object that has been spawned and un-spawns it.
        /// <para>The object will be removed from clients that it was spawned on, or the custom spawn handler function on the client will be called for the object.</para>
        /// <para>Unlike when calling NetworkServer.Destroy(), on the server the object will NOT be destroyed. This allows the server to re-use the object, even spawn it again later.</para>
        /// </summary>
        /// <param name="obj">The spawned object to be unspawned.</param>
        static public void UnSpawn(GameObject obj)
        {
            UnSpawnObject(obj);
        }

        internal bool InvokeBytes(ULocalConnectionToServer conn, byte[] buffer, int numBytes, int channelId)
        {
            NetworkReader reader = new NetworkReader(buffer);

            reader.ReadInt16(); // size
            short msgType = reader.ReadInt16();

            if (handlers.ContainsKey(msgType) && m_LocalConnection != null)
            {
                // this must be invoked with the connection to the client, not the client's connection to the server
                m_LocalConnection.InvokeHandler(msgType, reader, channelId);
                return true;
            }
            return false;
        }

        // invoked for local clients
        internal bool InvokeHandlerOnServer(ULocalConnectionToServer conn, short msgType, MessageBase msg, int channelId)
        {
            if (handlers.ContainsKey(msgType) && m_LocalConnection != null)
            {
                // write the message to a local buffer
                NetworkWriter writer = new NetworkWriter();
                msg.Serialize(writer);

                // pass a reader (attached to local buffer) to handler
                NetworkReader reader = new NetworkReader(writer);

                // this must be invoked with the connection to the client, not the client's connection to the server
                m_LocalConnection.InvokeHandler(msgType, reader, channelId);
                return true;
            }
            if (LogFilter.logError) { Debug.LogError("Local invoke: Failed to find local connection to invoke handler on [connectionId=" + conn.connectionId + "] for MsgId:" + msgType); }
            return false;
        }

        /// <summary>
        /// This finds the local NetworkIdentity object with the specified network Id.
        /// <para>Since netIds are the same on the server and all clients for a game, this allows clients to send a netId of a local game objects, and have the server find the corresponding server object.</para>
        /// 
        /// </summary>
        /// <param name="netId">The netId of the NetworkIdentity object to find.</param>
        /// <returns>The game object that matches the netId.</returns>
        static public GameObject FindLocalObject(NetworkInstanceId netId)
        {
            return instance.m_NetworkScene.FindLocalObject(netId);
        }

        /// <summary>
        /// Gets aggregate packet stats for all connections.
        /// </summary>
        /// <returns>Dictionary of msg types and packet statistics.</returns>
        static public Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
        {
            Dictionary<short, NetworkConnection.PacketStat> stats = new Dictionary<short, NetworkConnection.PacketStat>();

            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                {
                    foreach (short k in conn.packetStats.Keys)
                    {
                        if (stats.ContainsKey(k))
                        {
                            NetworkConnection.PacketStat s = stats[k];
                            s.count += conn.packetStats[k].count;
                            s.bytes += conn.packetStats[k].bytes;
                            stats[k] = s;
                        }
                        else
                        {
                            stats[k] = new NetworkConnection.PacketStat(conn.packetStats[k]);
                        }
                    }
                }
            }
            return stats;
        }

        /// <summary>
        /// Resets the packet stats on all connections.
        /// </summary>
        static public void ResetConnectionStats()
        {
            for (int i = 0; i < connections.Count; i++)
            {
                var conn = connections[i];
                if (conn != null)
                {
                    conn.ResetStats();
                }
            }
        }

        /// <summary>
        /// <para>This accepts a network connection from another external source and adds it to the server.</para>
        /// <para>This connection will use the callbacks registered with the server, and can have players added to it like any other connection.</para>
        /// </summary>
        /// <param name="conn">Network connection to add.</param>
        /// <returns>True if added.</returns>
        static public bool AddExternalConnection(NetworkConnection conn)
        {
            return instance.AddExternalConnectionInternal(conn);
        }

        bool AddExternalConnectionInternal(NetworkConnection conn)
        {
            if (conn.connectionId < 0)
                return false;

            if (conn.connectionId < connections.Count && connections[conn.connectionId] != null)
            {
                if (LogFilter.logError) { Debug.LogError("AddExternalConnection failed, already connection for id:" + conn.connectionId); }
                return false;
            }

            if (LogFilter.logDebug) { Debug.Log("AddExternalConnection external connection " + conn.connectionId); }
            m_SimpleServerSimple.SetConnectionAtIndex(conn);
            m_ExternalConnections.Add(conn.connectionId);
            conn.InvokeHandlerNoData(MsgType.Connect);

            return true;
        }

        /// <summary>
        /// This removes an external connection added with AddExternalConnection().
        /// </summary>
        /// <param name="connectionId">The id of the connection to remove.</param>
        static public void RemoveExternalConnection(int connectionId)
        {
            instance.RemoveExternalConnectionInternal(connectionId);
        }

        bool RemoveExternalConnectionInternal(int connectionId)
        {
            if (!m_ExternalConnections.Contains(connectionId))
            {
                if (LogFilter.logError) { Debug.LogError("RemoveExternalConnection failed, no connection for id:" + connectionId); }
                return false;
            }
            if (LogFilter.logDebug) { Debug.Log("RemoveExternalConnection external connection " + connectionId); }

            var conn = m_SimpleServerSimple.FindConnection(connectionId);
            if (conn != null)
            {
                conn.RemoveObservers();
            }
            m_SimpleServerSimple.RemoveConnectionAtIndex(connectionId);

            return true;
        }

        static bool ValidateSceneObject(NetworkIdentity netId)
        {
            if (netId.gameObject.hideFlags == HideFlags.NotEditable || netId.gameObject.hideFlags == HideFlags.HideAndDontSave)
                return false;

#if UNITY_EDITOR
            if (UnityEditor.EditorUtility.IsPersistent(netId.gameObject))
                return false;
#endif

            // If not a scene object
            if (netId.sceneId.IsEmpty())
                return false;

            return true;
        }

        /// <summary>
        /// This causes NetworkIdentity objects in a scene to be spawned on a server.
        /// <para>NetworkIdentity objects in a scene are disabled by default. Calling SpawnObjects() causes these scene objects to be enabled and spawned. It is like calling NetworkServer.Spawn() for each of them.</para>
        /// </summary>
        /// <returns>Success if objects where spawned.</returns>
        static public bool SpawnObjects()
        {
            if (!active)
                return true;

            NetworkIdentity[] netIds = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
            for (int i = 0; i < netIds.Length; i++)
            {
                var netId = netIds[i];
                if (!ValidateSceneObject(netId))
                    continue;

                if (LogFilter.logDebug) { Debug.Log("SpawnObjects sceneId:" + netId.sceneId + " name:" + netId.gameObject.name); }
                netId.Reset();
                netId.gameObject.SetActive(true);
            }
            for (int i = 0; i < netIds.Length; i++)
            {
                var netId = netIds[i];
                if (!ValidateSceneObject(netId))
                    continue;

                Spawn(netId.gameObject);

                // these objects are server authority - even if "localPlayerAuthority" is set on them
                netId.ForceAuthority(true);
            }
            return true;
        }

        static void SendCrc(NetworkConnection targetConnection)
        {
            if (NetworkCRC.singleton == null)
                return;

            if (NetworkCRC.scriptCRCCheck == false)
                return;

            CRCMessage crcMsg = new CRCMessage();

            // build entries
            List<CRCMessageEntry> entries = new List<CRCMessageEntry>();
            foreach (var name in NetworkCRC.singleton.scripts.Keys)
            {
                CRCMessageEntry entry = new CRCMessageEntry();
                entry.name = name;
                entry.channel = (byte)NetworkCRC.singleton.scripts[name];
                entries.Add(entry);
            }
            crcMsg.scripts = entries.ToArray();

            targetConnection.Send(MsgType.CRC, crcMsg);
        }

        [Obsolete("moved to NetworkMigrationManager")]
        public void SendNetworkInfo(NetworkConnection targetConnection)
        {
        }

        class ServerSimpleWrapper : NetworkServerSimple
        {
            NetworkServer m_Server;

            public ServerSimpleWrapper(NetworkServer server)
            {
                m_Server = server;
            }

            public override void OnConnectError(int connectionId, byte error)
            {
                m_Server.GenerateConnectError(error);
            }

            public override void OnDataError(NetworkConnection conn, byte error)
            {
                m_Server.GenerateDataError(conn, error);
            }

            public override void OnDisconnectError(NetworkConnection conn, byte error)
            {
                m_Server.GenerateDisconnectError(conn, error);
            }

            public override void OnConnected(NetworkConnection conn)
            {
                m_Server.OnConnected(conn);
            }

            public override void OnDisconnected(NetworkConnection conn)
            {
                m_Server.OnDisconnected(conn);
            }

            public override void OnData(NetworkConnection conn, int receivedSize, int channelId)
            {
                m_Server.OnData(conn, receivedSize, channelId);
            }
        }
    };
}
