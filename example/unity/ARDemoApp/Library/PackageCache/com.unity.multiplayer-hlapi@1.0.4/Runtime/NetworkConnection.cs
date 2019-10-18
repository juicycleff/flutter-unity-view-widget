using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Networking
{
    /// <summary>
    /// A High level network connection. This is used for connections from client-to-server and for connection from server-to-client.
    /// <para>A NetworkConnection corresponds to a specific connection for a host in the transport layer. It has a connectionId that is assigned by the transport layer and passed to the Initialize function.</para>
    /// <para>A NetworkClient has one NetworkConnection. A NetworkServerSimple manages multiple NetworkConnections. The NetworkServer has multiple "remote" connections and a "local" connection for the local client.</para>
    /// <para>The NetworkConnection class provides message sending and handling facilities. For sending data over a network, there are methods to send message objects, byte arrays, and NetworkWriter objects. To handle data arriving from the network, handler functions can be registered for message Ids, byte arrays can be processed by HandleBytes(), and NetworkReader object can be processed by HandleReader().</para>
    /// <para>NetworkConnection objects also act as observers for networked objects. When a connection is an observer of a networked object with a NetworkIdentity, then the object will be visible to corresponding client for the connection, and incremental state changes will be sent to the client.</para>
    /// <para>NetworkConnection objects can "own" networked game objects. Owned objects will be destroyed on the server by default when the connection is destroyed. A connection owns the player objects created by its client, and other objects with client-authority assigned to the corresponding client.</para>
    /// <para>There are many virtual functions on NetworkConnection that allow its behaviour to be customized. NetworkClient and NetworkServer can both be made to instantiate custom classes derived from NetworkConnection by setting their networkConnectionClass member variable.</para>
    /// </summary>
    /*
    * wire protocol is a list of :   size   |  msgType     | payload
    *                               (short)  (variable)   (buffer)
    */
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkConnection : IDisposable
    {
        ChannelBuffer[] m_Channels;
        List<PlayerController> m_PlayerControllers = new List<PlayerController>();
        NetworkMessage m_NetMsg = new NetworkMessage();
        HashSet<NetworkIdentity> m_VisList = new HashSet<NetworkIdentity>();
        internal HashSet<NetworkIdentity> visList { get { return m_VisList; } }
        NetworkWriter m_Writer;

        Dictionary<short, NetworkMessageDelegate> m_MessageHandlersDict;
        NetworkMessageHandlers m_MessageHandlers;

        HashSet<NetworkInstanceId> m_ClientOwnedObjects;
        NetworkMessage m_MessageInfo = new NetworkMessage();

        const int k_MaxMessageLogSize = 150;
        private NetworkError error;

        /// <summary>
        /// Transport level host ID for this connection.
        /// <para>This is assigned by the transport layer and passed to the connection instance through the Initialize function.</para>
        /// </summary>
        public int hostId = -1;
        /// <summary>
        /// Unique identifier for this connection that is assigned by the transport layer.
        /// <para>On a server, this Id is unique for every connection on the server. On a client this Id is local to the client, it is not the same as the Id on the server for this connection.</para>
        /// <para>Transport layers connections begin at one. So on a client with a single connection to a server, the connectionId of that connection will be one. In NetworkServer, the connectionId of the local connection is zero.</para>
        /// <para>Clients do not know their connectionId on the server, and do not know the connectionId of other clients on the server.</para>
        /// </summary>
        public int connectionId = -1;
        /// <summary>
        /// Flag that tells if the connection has been marked as "ready" by a client calling ClientScene.Ready().
        /// <para>This property is read-only. It is set by the system on the client when ClientScene.Ready() is called, and set by the system on the server when a ready message is received from a client.</para>
        /// <para>A client that is ready is sent spawned objects by the server and updates to the state of spawned objects. A client that is not ready is not sent spawned objects.</para>
        /// </summary>
        public bool isReady;
        /// <summary>
        /// The IP address associated with the connection.
        /// </summary>
        public string address;
        /// <summary>
        /// The last time that a message was received on this connection.
        /// <para>This includes internal system messages (such as Commands and ClientRpc calls) and user messages.</para>
        /// </summary>
        public float lastMessageTime;
        /// <summary>
        /// The list of players for this connection.
        /// <para>In most cases this will be a single player. But, for "Couch Multiplayer" there could be multiple players for a single client. To see the players on your own client see ClientScene.localPlayers list.</para>
        /// </summary>
        public List<PlayerController> playerControllers { get { return m_PlayerControllers; } }
        /// <summary>
        /// A list of the NetworkIdentity objects owned by this connection.
        /// <para>This includes the player object for the connection - if it has localPlayerAutority set, and any objects spawned with local authority or set with AssignLocalAuthority. This list is read only.</para>
        /// <para>This list can be used to validate messages from clients, to ensure that clients are only trying to control objects that they own.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Handler
        /// {
        ///    static public void HandleTransform(NetworkMessage netMsg)
        ///    {
        ///        NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
        ///        GameObject foundObj = NetworkServer.FindLocalObject(netId);
        ///        if (foundObj == null)
        ///        {
        ///            return;
        ///        }
        ///        NetworkTransform foundSync = foundObj.GetComponent&lt;NetworkTransform&gt;();
        ///        if (foundSync == null)
        ///        {
        ///            return;
        ///        }
        ///        if (!foundSync.localPlayerAuthority)
        ///        {
        ///            return;
        ///        }
        ///        if (netMsg.conn.clientOwnedObjects.Contains(netId))
        ///        {
        ///            // process message
        ///        }
        ///        else
        ///        {
        ///            // error
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        public HashSet<NetworkInstanceId> clientOwnedObjects { get { return m_ClientOwnedObjects; } }
        /// <summary>
        /// Setting this to true will log the contents of network message to the console.
        /// <para>Warning: this can be a lot of data and can be very slow. Both incoming and outgoing messages are logged. The format of the logs is:</para>
        /// <para>ConnectionSend con:1 bytes:11 msgId:5 FB59D743FD120000000000 ConnectionRecv con:1 bytes:27 msgId:8 14F21000000000016800AC3FE090C240437846403CDDC0BD3B0000</para>
        /// <para>Note that these are application-level network messages, not protocol-level packets. There will typically be multiple network messages combined in a single protocol packet.</para>
        /// </summary>
        public bool logNetworkMessages = false;
        /// <summary>
        /// True if the connection is connected to a remote end-point.
        /// <para>This applies to NetworkServer and NetworkClient connections. When not connected, the hostID will be -1. When connected, the hostID will be a positive integer.</para>
        /// </summary>
        public bool isConnected { get { return hostId != -1; }}


        /// <summary>
        /// Structure used to track the number and size of packets of each packets type.
        /// </summary>
        public class PacketStat
        {
            public PacketStat()
            {
                msgType = 0;
                count = 0;
                bytes = 0;
            }

            public PacketStat(PacketStat s)
            {
                msgType = s.msgType;
                count = s.count;
                bytes = s.bytes;
            }

            /// <summary>
            /// The message type these stats are for.
            /// </summary>
            public short msgType;
            /// <summary>
            /// The total number of messages of this type.
            /// </summary>
            public int count;
            /// <summary>
            /// Total bytes of all messages of this type.
            /// </summary>
            public int bytes;

            public override string ToString()
            {
                return MsgType.MsgTypeToString(msgType) + ": count=" + count + " bytes=" + bytes;
            }
        }

        /// <summary>
        /// The last error associated with this connection.
        /// <para>Retrieve the last error that occurred on the connection, this value is set every time an event is received from the NetworkTransport.</para>
        /// <para>In the following example, OnServerDisconnect is overridden from NetworkManager:</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : NetworkManager
        /// {
        ///    public override void OnServerDisconnect(NetworkConnection conn)
        ///    {
        ///        if (conn.lastError != NetworkError.Ok)
        ///        {
        ///            if (LogFilter.logError)
        ///            {
        ///                Debug.LogError("ServerDisconnected due to error: " + conn.lastError);
        ///            }
        ///        }
        ///    }
        ///  }
        /// </code>
        /// </summary>
        public NetworkError lastError { get { return error; } internal set { error = value; } }

        Dictionary<short, PacketStat> m_PacketStats = new Dictionary<short, PacketStat>();
        internal Dictionary<short, PacketStat> packetStats { get { return m_PacketStats; }}

#if UNITY_EDITOR
        static int s_MaxPacketStats = 255;//the same as maximum message types
#endif

        /// <summary>
        /// This inializes the internal data structures of a NetworkConnection object, including channel buffers.
        /// <para>This is called by NetworkServer and NetworkClient on connection objects, but if used outside of that context, this function should be called before the connection is used.</para>
        /// <para>This function can be overriden to perform additional initialization for the connection, but the base class Initialize function should always be called as it is required to setup internal state.</para>
        /// </summary>
        /// <param name="networkAddress">The host or IP connected to.</param>
        /// <param name="networkHostId">The transport hostId for the connection.</param>
        /// <param name="networkConnectionId">The transport connectionId for the connection.</param>
        /// <param name="hostTopology">The topology to be used.</param>
        public virtual void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
        {
            m_Writer = new NetworkWriter();
            address = networkAddress;
            hostId = networkHostId;
            connectionId = networkConnectionId;

            int numChannels = hostTopology.DefaultConfig.ChannelCount;
            int packetSize = hostTopology.DefaultConfig.PacketSize;

            if ((hostTopology.DefaultConfig.UsePlatformSpecificProtocols) && (UnityEngine.Application.platform != RuntimePlatform.PS4))
                throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");

            m_Channels = new ChannelBuffer[numChannels];
            for (int i = 0; i < numChannels; i++)
            {
                var qos = hostTopology.DefaultConfig.Channels[i];
                int actualPacketSize = packetSize;
                if (qos.QOS == QosType.ReliableFragmented || qos.QOS == QosType.UnreliableFragmented)
                {
                    actualPacketSize = hostTopology.DefaultConfig.FragmentSize * 128;
                }
                m_Channels[i] = new ChannelBuffer(this, actualPacketSize, (byte)i, IsReliableQoS(qos.QOS), IsSequencedQoS(qos.QOS));
            }
        }

        // Track whether Dispose has been called.
        bool m_Disposed;

        ~NetworkConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of this connection, releasing channel buffers that it holds.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!m_Disposed && m_Channels != null)
            {
                for (int i = 0; i < m_Channels.Length; i++)
                {
                    m_Channels[i].Dispose();
                }
            }
            m_Channels = null;

            if (m_ClientOwnedObjects != null)
            {
                foreach (var netId in m_ClientOwnedObjects)
                {
                    var obj = NetworkServer.FindLocalObject(netId);
                    if (obj != null)
                    {
                        obj.GetComponent<NetworkIdentity>().ClearClientOwner();
                    }
                }
            }
            m_ClientOwnedObjects = null;

            m_Disposed = true;
        }

        static bool IsSequencedQoS(QosType qos)
        {
            return (qos == QosType.ReliableSequenced || qos == QosType.UnreliableSequenced);
        }

        static bool IsReliableQoS(QosType qos)
        {
            return (qos == QosType.Reliable || qos == QosType.ReliableFragmented || qos == QosType.ReliableSequenced || qos == QosType.ReliableStateUpdate);
        }

        /// <summary>
        /// This sets an option on the network channel.
        /// <para>Channel options are usually advanced tuning parameters.</para>
        /// </summary>
        /// <param name="channelId">The channel the option will be set on.</param>
        /// <param name="option">The option to set.</param>
        /// <param name="value">The value for the option.</param>
        /// <returns>True if the option was set.</returns>
        public bool SetChannelOption(int channelId, ChannelOption option, int value)
        {
            if (m_Channels == null)
                return false;

            if (channelId < 0 || channelId >= m_Channels.Length)
                return false;

            return m_Channels[channelId].SetOption(option, value);
        }

        public NetworkConnection()
        {
            m_Writer = new NetworkWriter();
        }

        /// <summary>
        /// Disconnects this connection.
        /// </summary>
        public void Disconnect()
        {
            address = "";
            isReady = false;
            ClientScene.HandleClientDisconnect(this);
            if (hostId == -1)
            {
                return;
            }
            byte error;
            NetworkManager.activeTransport.Disconnect(hostId, connectionId, out error);

            RemoveObservers();
        }

        internal void SetHandlers(NetworkMessageHandlers handlers)
        {
            m_MessageHandlers = handlers;
            m_MessageHandlersDict = handlers.GetHandlers();
        }

        /// <summary>
        /// This function checks if there is a message handler registered for the message ID.
        /// <para>This is usually not required, as InvokeHandler handles message IDs without handlers.</para>
        /// </summary>
        /// <param name="msgType">The message ID of the handler to look for.</param>
        /// <returns>True if a handler function was found.</returns>
        public bool CheckHandler(short msgType)
        {
            return m_MessageHandlersDict.ContainsKey(msgType);
        }

        /// <summary>
        /// This function invokes the registered handler function for a message, without any message data.
        /// <para>This is useful to invoke handlers that dont have any additional data, such as the handlers for MsgType.Connect.</para>
        /// </summary>
        /// <param name="msgType">The message ID of the handler to invoke.</param>
        /// <returns>True if a handler function was found and invoked.</returns>
        public bool InvokeHandlerNoData(short msgType)
        {
            return InvokeHandler(msgType, null, 0);
        }

        /// <summary>
        /// This function invokes the registered handler function for a message.
        /// <para>Network connections used by the NetworkClient and NetworkServer use this function for handling network messages.</para>
        /// </summary>
        /// <param name="msgType">The message type of the handler to use.</param>
        /// <param name="reader">The stream to read the contents of the message from.</param>
        /// <param name="channelId">The channel that the message arrived on.</param>
        /// <returns>True if a handler function was found and invoked.</returns>
        public bool InvokeHandler(short msgType, NetworkReader reader, int channelId)
        {
            if (m_MessageHandlersDict.ContainsKey(msgType))
            {
                m_MessageInfo.msgType = msgType;
                m_MessageInfo.conn = this;
                m_MessageInfo.reader = reader;
                m_MessageInfo.channelId = channelId;

                NetworkMessageDelegate msgDelegate = m_MessageHandlersDict[msgType];
                if (msgDelegate == null)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkConnection InvokeHandler no handler for " + msgType); }
                    return false;
                }
                msgDelegate(m_MessageInfo);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This function invokes the registered handler function for a message.
        /// <para>Network connections used by the NetworkClient and NetworkServer use this function for handling network messages.</para>
        /// </summary>
        /// <param name="netMsg">The message object to process.</param>
        /// <returns>True if a handler function was found and invoked.</returns>
        public bool InvokeHandler(NetworkMessage netMsg)
        {
            if (m_MessageHandlersDict.ContainsKey(netMsg.msgType))
            {
                NetworkMessageDelegate msgDelegate = m_MessageHandlersDict[netMsg.msgType];
                msgDelegate(netMsg);
                return true;
            }
            return false;
        }

        internal void HandleFragment(NetworkReader reader, int channelId)
        {
            if (channelId < 0 || channelId >= m_Channels.Length)
            {
                return;
            }

            var channel = m_Channels[channelId];
            if (channel.HandleFragment(reader))
            {
                NetworkReader msgReader = new NetworkReader(channel.fragmentBuffer.AsArraySegment().Array);
                msgReader.ReadInt16(); // size
                short msgType = msgReader.ReadInt16();
                InvokeHandler(msgType, msgReader, channelId);
            }
        }

        /// <summary>
        /// This registers a handler function for a message Id.
        /// </summary>
        /// <param name="msgType">The message ID to register.</param>
        /// <param name="handler">The handler function to register.</param>
        public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            m_MessageHandlers.RegisterHandler(msgType, handler);
        }

        /// <summary>
        /// This removes the handler registered for a message Id.
        /// </summary>
        /// <param name="msgType">The message ID to unregister.</param>
        public void UnregisterHandler(short msgType)
        {
            m_MessageHandlers.UnregisterHandler(msgType);
        }

        internal void SetPlayerController(PlayerController player)
        {
            while (player.playerControllerId >= m_PlayerControllers.Count)
            {
                m_PlayerControllers.Add(new PlayerController());
            }

            m_PlayerControllers[player.playerControllerId] = player;
        }

        internal void RemovePlayerController(short playerControllerId)
        {
            int count = m_PlayerControllers.Count;
            while (count >= 0)
            {
                if (playerControllerId == count && playerControllerId == m_PlayerControllers[count].playerControllerId)
                {
                    m_PlayerControllers[count] = new PlayerController();
                    return;
                }
                count -= 1;
            }
            if (LogFilter.logError) { Debug.LogError("RemovePlayer player at playerControllerId " + playerControllerId + " not found"); }
        }

        // Get player controller from connection's list
        internal bool GetPlayerController(short playerControllerId, out PlayerController playerController)
        {
            playerController = null;
            if (playerControllers.Count > 0)
            {
                for (int i = 0; i < playerControllers.Count; i++)
                {
                    if (playerControllers[i].IsValid && playerControllers[i].playerControllerId == playerControllerId)
                    {
                        playerController = playerControllers[i];
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// This causes the channels of the network connection to flush their data to the transport layer.
        /// <para>This is called automatically by connections used by NetworkServer and NetworkClient, but can be called manually for connections used in other contexts.</para>
        /// </summary>
        public void FlushChannels()
        {
            if (m_Channels == null)
            {
                return;
            }
            for (int channelId = 0; channelId < m_Channels.Length; channelId++)
            {
                m_Channels[channelId].CheckInternalBuffer();
            }
        }

        /// <summary>
        /// The maximum time in seconds that messages are buffered before being sent.
        /// <para>If this is set to zero, then there will be no buffering of messages before they are sent to the transport layer. This may reduce latency but can lead to packet queue overflow issues if many small packets are being sent.</para>
        /// </summary>
        /// <param name="seconds">Time in seconds.</param>
        public void SetMaxDelay(float seconds)
        {
            if (m_Channels == null)
            {
                return;
            }
            for (int channelId = 0; channelId < m_Channels.Length; channelId++)
            {
                m_Channels[channelId].maxDelay = seconds;
            }
        }

        /// <summary>
        /// This sends a network message with a message ID on the connection. This message is sent on channel zero, which by default is the reliable channel.
        /// </summary>
        /// <param name="msgType">The ID of the message to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <returns>True if the message was sent.</returns>
        public virtual bool Send(short msgType, MessageBase msg)
        {
            return SendByChannel(msgType, msg, Channels.DefaultReliable);
        }

        /// <summary>
        /// This sends a network message with a message ID on the connection. This message is sent on channel one, which by default is the unreliable channel.
        /// </summary>
        /// <param name="msgType">The message ID to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <returns>True if the message was sent.</returns>
        public virtual bool SendUnreliable(short msgType, MessageBase msg)
        {
            return SendByChannel(msgType, msg, Channels.DefaultUnreliable);
        }

        /// <summary>
        /// This sends a network message on the connection using a specific transport layer channel.
        /// </summary>
        /// <param name="msgType">The message ID to send.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="channelId">The transport layer channel to send on.</param>
        /// <returns>True if the message was sent.</returns>
        public virtual bool SendByChannel(short msgType, MessageBase msg, int channelId)
        {
            m_Writer.StartMessage(msgType);
            msg.Serialize(m_Writer);
            m_Writer.FinishMessage();
            return SendWriter(m_Writer, channelId);
        }

        /// <summary>
        /// This sends an array of bytes on the connection.
        /// </summary>
        /// <param name="bytes">The array of data to be sent.</param>
        /// <param name="numBytes">The number of bytes in the array to be sent.</param>
        /// <param name="channelId">The transport channel to send on.</param>
        /// <returns>Success if data was sent.</returns>
        public virtual bool SendBytes(byte[] bytes, int numBytes, int channelId)
        {
            if (logNetworkMessages)
            {
                LogSend(bytes);
            }
            return CheckChannel(channelId) && m_Channels[channelId].SendBytes(bytes, numBytes);
        }

        /// <summary>
        /// This sends the contents of a NetworkWriter object on the connection.
        /// <para>The example below constructs a writer and sends it on a connection.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    public bool Send(short msgType, MessageBase msg, NetworkConnection conn)
        ///    {
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(msgType);
        ///        msg.Serialize(writer);
        ///        writer.FinishMessage();
        ///        return conn.SendWriter(writer, Channels.DefaultReliable);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="writer">A writer object containing data to send.</param>
        /// <param name="channelId">The transport channel to send on.</param>
        /// <returns>True if the data was sent.</returns>
        public virtual bool SendWriter(NetworkWriter writer, int channelId)
        {
            if (logNetworkMessages)
            {
                LogSend(writer.ToArray());
            }
            return CheckChannel(channelId) && m_Channels[channelId].SendWriter(writer);
        }

        void LogSend(byte[] bytes)
        {
            NetworkReader reader = new NetworkReader(bytes);
            var msgSize = reader.ReadUInt16();
            var msgId = reader.ReadUInt16();

            const int k_PayloadStartPosition = 4;

            StringBuilder msg = new StringBuilder();
            for (int i = k_PayloadStartPosition; i < k_PayloadStartPosition + msgSize; i++)
            {
                msg.AppendFormat("{0:X2}", bytes[i]);
                if (i > k_MaxMessageLogSize) break;
            }
            Debug.Log("ConnectionSend con:" + connectionId + " bytes:" + msgSize + " msgId:" + msgId + " " + msg);
        }

        bool CheckChannel(int channelId)
        {
            if (m_Channels == null)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Channels not initialized sending on id '" + channelId); }
                return false;
            }
            if (channelId < 0 || channelId >= m_Channels.Length)
            {
                if (LogFilter.logError) { Debug.LogError("Invalid channel when sending buffered data, '" + channelId + "'. Current channel count is " + m_Channels.Length); }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Resets the statistics that are returned from NetworkClient.GetConnectionStats().
        /// </summary>
        public void ResetStats()
        {
#if UNITY_EDITOR
            for (short i = 0; i < s_MaxPacketStats; i++)
            {
                if (m_PacketStats.ContainsKey(i))
                {
                    var value = m_PacketStats[i];
                    value.count = 0;
                    value.bytes = 0;
                    NetworkManager.activeTransport.SetPacketStat(0, i, 0, 0);
                    NetworkManager.activeTransport.SetPacketStat(1, i, 0, 0);
                }
            }
#endif
        }

        /// <summary>
        /// This makes the connection process the data contained in the buffer, and call handler functions.
        /// <para>The data is assumed to have come from the network, and contains network messages.</para>
        /// <para>This function is used by network connections when they receive data.</para>
        /// </summary>
        /// <param name="buffer">Data to process.</param>
        /// <param name="receivedSize">Size of the data to process.</param>
        /// <param name="channelId">Channel the data was recieved on.</param>
        protected void HandleBytes(
            byte[] buffer,
            int receivedSize,
            int channelId)
        {
            // build the stream form the buffer passed in
            NetworkReader reader = new NetworkReader(buffer);

            HandleReader(reader, receivedSize, channelId);
        }

        /// <summary>
        /// This makes the connection process the data contained in the stream, and call handler functions.
        /// <para>The data in the stream is assumed to have come from the network, and contains network messages.</para>
        /// <para>This function is used by network connections when they receive data.</para>
        /// </summary>
        /// <param name="reader">Stream that contains data.</param>
        /// <param name="receivedSize">Size of the data.</param>
        /// <param name="channelId">Channel the data was received on.</param>
        protected void HandleReader(
            NetworkReader reader,
            int receivedSize,
            int channelId)
        {
            // read until size is reached.
            // NOTE: stream.Capacity is 1300, NOT the size of the available data
            while (reader.Position < receivedSize)
            {
                // the reader passed to user code has a copy of bytes from the real stream. user code never touches the real stream.
                // this ensures it can never get out of sync if user code reads less or more than the real amount.
                ushort sz = reader.ReadUInt16();
                short msgType = reader.ReadInt16();

                // create a reader just for this message
                //TODO: Allocation!!
                byte[] msgBuffer = reader.ReadBytes(sz);
                NetworkReader msgReader = new NetworkReader(msgBuffer);

                if (logNetworkMessages)
                {
                    StringBuilder msg = new StringBuilder();
                    for (int i = 0; i < sz; i++)
                    {
                        msg.AppendFormat("{0:X2}", msgBuffer[i]);
                        if (i > k_MaxMessageLogSize) break;
                    }
                    Debug.Log("ConnectionRecv con:" + connectionId + " bytes:" + sz + " msgId:" + msgType + " " + msg);
                }

                NetworkMessageDelegate msgDelegate = null;
                if (m_MessageHandlersDict.ContainsKey(msgType))
                {
                    msgDelegate = m_MessageHandlersDict[msgType];
                }
                if (msgDelegate != null)
                {
                    m_NetMsg.msgType = msgType;
                    m_NetMsg.reader = msgReader;
                    m_NetMsg.conn = this;
                    m_NetMsg.channelId = channelId;
                    msgDelegate(m_NetMsg);
                    lastMessageTime = Time.time;

#if UNITY_EDITOR
                    Profiler.IncrementStatIncoming(MsgType.HLAPIMsg);

                    if (msgType > MsgType.Highest)
                    {
                        Profiler.IncrementStatIncoming(MsgType.UserMessage, msgType + ":" + msgType.GetType().Name);
                    }
#endif

#if UNITY_EDITOR
                    if (m_PacketStats.ContainsKey(msgType))
                    {
                        PacketStat stat = m_PacketStats[msgType];
                        stat.count += 1;
                        stat.bytes += sz;
                    }
                    else
                    {
                        PacketStat stat = new PacketStat();
                        stat.msgType = msgType;
                        stat.count += 1;
                        stat.bytes += sz;
                        m_PacketStats[msgType] = stat;
                    }
#endif
                }
                else
                {
                    //NOTE: this throws away the rest of the buffer. Need moar error codes
                    if (LogFilter.logError) { Debug.LogError("Unknown message ID " + msgType + " connId:" + connectionId); }
                    break;
                }
            }
        }

        /// <summary>
        /// Get statistics for outgoing traffic.
        /// </summary>
        /// <param name="numMsgs">Number of messages sent.</param>
        /// <param name="numBufferedMsgs">Number of messages currently buffered for sending.</param>
        /// <param name="numBytes">Number of bytes sent.</param>
        /// <param name="lastBufferedPerSecond">How many messages were buffered in the last second.</param>
        public virtual void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
        {
            numMsgs = 0;
            numBufferedMsgs = 0;
            numBytes = 0;
            lastBufferedPerSecond = 0;

            for (int channelId = 0; channelId < m_Channels.Length; channelId++)
            {
                var channel = m_Channels[channelId];
                numMsgs += channel.numMsgsOut;
                numBufferedMsgs += channel.numBufferedMsgsOut;
                numBytes += channel.numBytesOut;
                lastBufferedPerSecond += channel.lastBufferedPerSecond;
            }
        }

        /// <summary>
        /// Get statistics for incoming traffic.
        /// </summary>
        /// <param name="numMsgs">Number of messages received.</param>
        /// <param name="numBytes">Number of bytes received.</param>
        public virtual void GetStatsIn(out int numMsgs, out int numBytes)
        {
            numMsgs = 0;
            numBytes = 0;

            for (int channelId = 0; channelId < m_Channels.Length; channelId++)
            {
                var channel = m_Channels[channelId];
                numMsgs += channel.numMsgsIn;
                numBytes += channel.numBytesIn;
            }
        }

        /// <summary>
        /// Returns a string representation of the NetworkConnection object state.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("hostId: {0} connectionId: {1} isReady: {2} channel count: {3}", hostId, connectionId, isReady, (m_Channels != null ? m_Channels.Length : 0));
        }

        internal void AddToVisList(NetworkIdentity uv)
        {
            m_VisList.Add(uv);

            // spawn uv for this conn
            NetworkServer.ShowForConnection(uv, this);
        }

        internal void RemoveFromVisList(NetworkIdentity uv, bool isDestroyed)
        {
            m_VisList.Remove(uv);

            if (!isDestroyed)
            {
                // hide uv for this conn
                NetworkServer.HideForConnection(uv, this);
            }
        }

        internal void RemoveObservers()
        {
            foreach (var uv in m_VisList)
            {
                uv.RemoveObserverInternal(this);
            }
            m_VisList.Clear();
        }

        /// <summary>
        /// This virtual function allows custom network connection classes to process data from the network before it is passed to the application.
        /// <para>The default implementation of this function calls HandleBytes() on the received data. Custom implmentations can also use HandleBytes(), but can pass modified versions of the data received or other data.</para>
        /// <para>This example logs the data received to the console, then passes it to HandleBytes.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using System;
        /// using System.Text;
        /// public class DebugConnection : NetworkConnection
        /// {
        ///    public override void TransportReceive(byte[] bytes, int numBytes, int channelId)
        ///    {
        ///        StringBuilder msg = new StringBuilder();
        ///        for (int i = 0; i &lt; numBytes; i++) {
        ///        {
        ///            var s = String.Format("{0:X2}", bytes[i]);
        ///            msg.Append(s);
        ///            if (i > 50) break;
        ///        }
        ///        UnityEngine.Debug.LogError("TransportReceive h:" + hostId + " con:" + connectionId + " bytes:" + numBytes + " " + msg);
        ///        HandleBytes(bytes, numBytes, channelId);
        ///    }
        /// }
        /// </code>
        /// <para>Other uses for this function could be data compression or data encryption.</para>
        /// <para>Custom network connection classes are used by setting NetworkServer.NetworkConnectionClass and NetworkClient.NetworkConnectionClass.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class SpaceManager : NetworkManager
        /// {
        ///    void Start()
        ///    {
        ///        NetworkServer.networkConnectionClass = typeof(DebugConnection);
        ///        NetworkClient.networkConnectionClass = typeof(DebugConnection);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="bytes">The data recieved.</param>
        /// <param name="numBytes">The size of the data recieved.</param>
        /// <param name="channelId">The channel that the data was received on.</param>
        public virtual void TransportReceive(byte[] bytes, int numBytes, int channelId)
        {
            HandleBytes(bytes, numBytes, channelId);
        }

        [Obsolete("TransportRecieve has been deprecated. Use TransportReceive instead.", false)]
        public virtual void TransportRecieve(byte[] bytes, int numBytes, int channelId)
        {
            TransportReceive(bytes, numBytes, channelId);
        }

        /// <summary>
        /// This virtual function allows custom network connection classes to process data send by the application before it goes to the network transport layer.
        /// <para>The default implementation of this function calls NetworkTransport.Send() with the supplied data, but custom implementations can pass modified versions of the data. This example logs the sent data to the console:</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using System;
        /// using System.Text;
        ///
        /// class DebugConnection : NetworkConnection
        /// {
        ///    public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
        ///    {
        ///        StringBuilder msg = new StringBuilder();
        ///        for (int i = 0; i &lt; numBytes; i++)
        ///        {
        ///            var s = String.Format("{0:X2}", bytes[i]);
        ///            msg.Append(s);
        ///            if (i > 50) break;
        ///        }
        ///        UnityEngine.Debug.LogError("TransportSend    h:" + hostId + " con:" + connectionId + " bytes:" + numBytes + " " + msg);
        ///        return NetworkTransport.Send(hostId, connectionId, channelId, bytes, numBytes, out error);
        ///    }
        /// }
        /// </code>
        /// <para>Other uses for this function could be data compression or data encryption.</para>
        /// <para>Custom network connection classes are used by setting NetworkServer.NetworkConnectionClass and NetworkClient.NetworkConnectionClass.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class SpaceManager : NetworkManager
        /// {
        ///    void Start()
        ///    {
        ///        NetworkServer.networkConnectionClass = typeof(DebugConnection);
        ///        NetworkClient.networkConnectionClass = typeof(DebugConnection);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="bytes">Data to send.</param>
        /// <param name="numBytes">Size of data to send.</param>
        /// <param name="channelId">Channel to send data on.</param>
        /// <param name="error">Error code for send.</param>
        /// <returns>True if data was sent.</returns>
        public virtual bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
        {
            return NetworkManager.activeTransport.Send(hostId, connectionId, channelId, bytes, numBytes, out error);
        }

        internal void AddOwnedObject(NetworkIdentity obj)
        {
            if (m_ClientOwnedObjects == null)
            {
                m_ClientOwnedObjects = new HashSet<NetworkInstanceId>();
            }
            m_ClientOwnedObjects.Add(obj.netId);
        }

        internal void RemoveOwnedObject(NetworkIdentity obj)
        {
            if (m_ClientOwnedObjects == null)
            {
                return;
            }
            m_ClientOwnedObjects.Remove(obj.netId);
        }

        internal static void OnFragment(NetworkMessage netMsg)
        {
            netMsg.conn.HandleFragment(netMsg.reader, netMsg.channelId);
        }
    }
}
