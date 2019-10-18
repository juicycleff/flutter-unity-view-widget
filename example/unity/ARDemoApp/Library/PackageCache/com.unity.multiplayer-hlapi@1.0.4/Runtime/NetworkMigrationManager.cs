
using System;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
    /// <summary>
    /// A component that manages the process of a new host taking over a game when the old host is lost. This is referred to as "host migration". The migration manager sends information about each peer in the game to all the clients, and when the host is lost because of a crash or network outage, the clients are able to choose a new host, and continue the game.
    /// <para>The old host is able to rejoin the new game on the new host.</para>
    /// <para>The state of SyncVars and SyncLists on all objects with NetworkIdentities in the scene is maintained during a host migration. This also applies to custom serialized data for objects.</para>
    /// <para>All of the player objects in the game are disabled when the host is lost. Then, when the other clients rejoin the new game on the new host, the corresponding players for those clients are re-enabled on the host, and respawned on the other clients. No player state data is lost during a host migration.</para>
    /// <para>This class provides a simple default UI for controlling the behaviour when the host is lost. The UI can be disabled with the showGUI property. There are a number of virtual functions that can be implemented to customize the behaviour of host migration.</para>
    /// <para>Note that only data that is available to clients will be preserved during a host migration. If there is data that is only on the server, then it will not be available to the client that becomes the new host. This means data on the host that is not in SyncVars or SyncLists will not be available after a host migration.</para>
    /// <para>The callback function OnStartServer is invoked for all networked objects when the client becomes a new host.</para>
    /// <para>On the new host, the NetworkMigrationManager uses the function NetworkServer.BecomeNewHost() to construct a networked server scene from the state in the current ClientScene.</para>
    /// <para>The peers in a game with host migration enabled are identified by their connectionId on the server. When a client reconnects to the new host of a game, this connectionId is passed to the new host so that it can match this client with the client that was connected to the old host. This Id is set on the ClientScene as the "reconnectId".</para>
    /// <para>The old host of the game, the one that crashed or lost its network connection, can also reconnect to the new game as a client. This client uses the special ReconnectId of ClientScene.ReconnectIdHost (which is zero).</para>
    /// </summary>
    [AddComponentMenu("Network/NetworkMigrationManager")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkMigrationManager : MonoBehaviour
    {
        /// <summary>
        /// An enumeration of how to handle scene changes when the connection to the host is lost.
        /// </summary>
        public enum SceneChangeOption
        {
            /// <summary>
            /// The client should stay in the online scene.
            /// </summary>
            StayInOnlineScene,
            /// <summary>
            /// The client should return to the offline scene.
            /// </summary>
            SwitchToOfflineScene
        }

        [SerializeField]
        bool m_HostMigration = true;

        [SerializeField]
        bool m_ShowGUI = true;

        [SerializeField]
        int m_OffsetX = 10;

        [SerializeField]
        int m_OffsetY = 300;

        NetworkClient m_Client;
        bool m_WaitingToBecomeNewHost;
        bool m_WaitingReconnectToNewHost;
        bool m_DisconnectedFromHost;
        bool m_HostWasShutdown;

        MatchInfo m_MatchInfo;
        int m_OldServerConnectionId = -1;
        string m_NewHostAddress;

        PeerInfoMessage m_NewHostInfo = new PeerInfoMessage();
        PeerListMessage m_PeerListMessage = new PeerListMessage();

        PeerInfoMessage[] m_Peers;

        /// <summary>
        /// Information about a player object from another peer.
        /// </summary>
        // There can be multiple pending players for a connectionId, distinguished by oldNetId/playerControllerId
        public struct PendingPlayerInfo
        {
            /// <summary>
            /// The networkId of the player object.
            /// </summary>
            public NetworkInstanceId netId;
            /// <summary>
            /// The playerControllerId of the player GameObject.
            /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
            /// </summary>
            public short playerControllerId;
            /// <summary>
            /// The gameObject for the player.
            /// </summary>
            public GameObject obj;
        }

        /// <summary>
        /// The player objects for connections to the old host.
        /// <para>This is used when clients reconnect to the new host.</para>
        /// </summary>
        public struct ConnectionPendingPlayers
        {
            /// <summary>
            /// The list of players for a connection.
            /// </summary>
            public List<PendingPlayerInfo> players;
        }
        Dictionary<int, ConnectionPendingPlayers> m_PendingPlayers = new Dictionary<int, ConnectionPendingPlayers>();

        void AddPendingPlayer(GameObject obj, int connectionId, NetworkInstanceId netId, short playerControllerId)
        {
            if (!m_PendingPlayers.ContainsKey(connectionId))
            {
                var pending = new ConnectionPendingPlayers();
                pending.players = new List<PendingPlayerInfo>();
                m_PendingPlayers[connectionId] = pending;
            }
            PendingPlayerInfo info = new PendingPlayerInfo();
            info.netId = netId;
            info.playerControllerId = playerControllerId;
            info.obj = obj;
            m_PendingPlayers[connectionId].players.Add(info);
        }

        GameObject FindPendingPlayer(int connectionId, NetworkInstanceId netId, short playerControllerId)
        {
            if (m_PendingPlayers.ContainsKey(connectionId))
            {
                for (int i = 0; i < m_PendingPlayers[connectionId].players.Count; i++)
                {
                    var info = m_PendingPlayers[connectionId].players[i];
                    if (info.netId == netId && info.playerControllerId == playerControllerId)
                    {
                        return info.obj;
                    }
                }
            }
            return null;
        }

        void RemovePendingPlayer(int connectionId)
        {
            m_PendingPlayers.Remove(connectionId);
        }

        /// <summary>
        /// Controls whether host migration is active.
        /// <para>If this is not true, then SendPeerInfo() will not send peer information to clients.</para>
        /// </summary>
        public bool hostMigration
        {
            get { return m_HostMigration; }
            set { m_HostMigration = value; }
        }

        /// <summary>
        /// Flag to toggle display of the default UI.
        /// </summary>
        public bool showGUI
        {
            get { return m_ShowGUI; }
            set { m_ShowGUI = value; }
        }

        /// <summary>
        /// The X offset in pixels of the migration manager default GUI.
        /// </summary>
        public int offsetX
        {
            get { return m_OffsetX; }
            set { m_OffsetX = value; }
        }

        /// <summary>
        /// The Y offset in pixels of the migration manager default GUI.
        /// </summary>
        public int offsetY
        {
            get { return m_OffsetY; }
            set { m_OffsetY = value; }
        }

        /// <summary>
        /// The client instance that is being used to connect to the host.
        /// <para>This is populated by the Initialize() method. It will be set automatically by the NetworkManager if one is being used.</para>
        /// </summary>
        public NetworkClient client
        {
            get { return m_Client; }
        }

        /// <summary>
        /// True if this is a client that was disconnected from the host, and was chosen as the new host.
        /// </summary>
        public bool waitingToBecomeNewHost
        {
            get { return m_WaitingToBecomeNewHost; }
            set { m_WaitingToBecomeNewHost = value; }
        }

        /// <summary>
        /// True if this is a client that was disconnected from the host and is now waiting to reconnect to the new host.
        /// </summary>
        public bool waitingReconnectToNewHost
        {
            get { return m_WaitingReconnectToNewHost; }
            set { m_WaitingReconnectToNewHost = value; }
        }

        /// <summary>
        /// True is this is a client that has been disconnected from a host.
        /// </summary>
        public bool disconnectedFromHost
        {
            get { return m_DisconnectedFromHost; }
        }

        /// <summary>
        /// True if this was the host and the host has been shut down.
        /// </summary>
        public bool hostWasShutdown
        {
            get { return m_HostWasShutdown; }
        }

        /// <summary>
        /// Information about the match. This may be null if there is no match.
        /// </summary>
        public MatchInfo matchInfo
        {
            get { return m_MatchInfo; }
        }

        /// <summary>
        /// The connectionId that this client was assign on the old host.
        /// <para>This is the Id that will be set on the ClientScene as the ReconnectId. This Id will be used to identify the client when it connects to the new host.</para>
        /// </summary>
        public int oldServerConnectionId
        {
            get { return m_OldServerConnectionId; }
        }

        /// <summary>
        /// The IP address of the new host to connect to.
        /// <para>The FindNewHost utility function will set this address. Methods of choosing the new host that are implemented by users should also set this address.</para>
        /// <para>The default UI button to "Reconnect to New Host" uses this address.</para>
        /// </summary>
        public string newHostAddress
        {
            get { return m_NewHostAddress; }
            set { m_NewHostAddress = value; }
        }

        /// <summary>
        /// The set of peers involved in the game. This includes the host and this client.
        /// <para>This is populated on clients when they recieve a MsgType.NetworkInfo message from the host. That message is sent when SendPeerInfo() is called on the host.</para>
        /// </summary>
        public PeerInfoMessage[] peers
        {
            get { return m_Peers; }
        }

        /// <summary>
        /// The player objects that have been disabled, and are waiting for their corresponding clients to reconnect.
        /// <para>There may be multiple pending player GameObjects for each peer. Each will have a different playerControllerId.</para>
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public Dictionary<int, ConnectionPendingPlayers> pendingPlayers
        {
            get { return m_PendingPlayers; }
        }

        void Start()
        {
            Reset(ClientScene.ReconnectIdInvalid);
        }

        /// <summary>
        /// Resets the migration manager, and sets the ClientScene's ReconnectId.
        /// </summary>
        /// <param name="reconnectId">The connectionId for the ClientScene to use when reconnecting.</param>
        public void Reset(int reconnectId)
        {
            m_OldServerConnectionId = -1;
            m_WaitingToBecomeNewHost = false;
            m_WaitingReconnectToNewHost = false;
            m_DisconnectedFromHost = false;
            m_HostWasShutdown = false;
            ClientScene.SetReconnectId(reconnectId, m_Peers);

            if (NetworkManager.singleton != null)
            {
                NetworkManager.singleton.SetupMigrationManager(this);
            }
        }

        internal void AssignAuthorityCallback(NetworkConnection conn, NetworkIdentity uv, bool authorityState)
        {
            var msg = new PeerAuthorityMessage();
            msg.connectionId = conn.connectionId;
            msg.netId = uv.netId;
            msg.authorityState = authorityState;

            if (LogFilter.logDebug) { Debug.Log("AssignAuthorityCallback send for netId" + uv.netId); }

            for (int i = 0; i < NetworkServer.connections.Count; i++)
            {
                var c = NetworkServer.connections[i];
                if (c != null)
                {
                    c.Send(MsgType.PeerClientAuthority, msg);
                }
            }
        }

        /// <summary>
        /// Used to initialize the migration manager with client and match information.
        /// <para>This is called automatically by the NetworkManager from within StartClient() if a NetworkManager is being used with the migration manager.</para>
        /// </summary>
        /// <param name="newClient">The NetworkClient being used to connect to the host.</param>
        /// <param name="newMatchInfo">Information about the match being used. This may be null if there is no match.</param>
        public void Initialize(NetworkClient newClient, MatchInfo newMatchInfo)
        {
            if (LogFilter.logDev) { Debug.Log("NetworkMigrationManager initialize"); }

            m_Client = newClient;
            m_MatchInfo = newMatchInfo;
            newClient.RegisterHandlerSafe(MsgType.NetworkInfo, OnPeerInfo);
            newClient.RegisterHandlerSafe(MsgType.PeerClientAuthority, OnPeerClientAuthority);

            NetworkIdentity.clientAuthorityCallback = AssignAuthorityCallback;
        }

        /// <summary>
        /// This causes objects for known players to be disabled.
        /// <para>These objects are added to the pendingPlayers list, and will be re-enabled when their clients reconnect.</para>
        /// <para>This happens when the connection to the host of the game is lost.</para>
        /// </summary>
        public void DisablePlayerObjects()
        {
            if (LogFilter.logDev) { Debug.Log("NetworkMigrationManager DisablePlayerObjects"); }

            if (m_Peers == null)
                return;

            for (int peerId = 0; peerId < m_Peers.Length; peerId++)
            {
                var peer = m_Peers[peerId];
                if (peer.playerIds != null)
                {
                    for (int i = 0; i < peer.playerIds.Length; i++)
                    {
                        var info = peer.playerIds[i];
                        if (LogFilter.logDev) { Debug.Log("DisablePlayerObjects disable player for " + peer.address + " netId:" + info.netId + " control:" + info.playerControllerId); }

                        GameObject playerObj = ClientScene.FindLocalObject(info.netId);
                        if (playerObj != null)
                        {
                            playerObj.SetActive(false);

                            AddPendingPlayer(playerObj, peer.connectionId, info.netId, info.playerControllerId);
                        }
                        else
                        {
                            if (LogFilter.logWarn) { Debug.LogWarning("DisablePlayerObjects didnt find player Conn:" + peer.connectionId + " NetId:" + info.netId); }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This sends the set of peers in the game to all the peers in the game.
        /// <para>This is called automatically by the NetworkManager if one is active. It happens when clients connect to and disconnect from the server, and when players are added and removed from clients. The function SendPeers() udpates all clients with the information about which client owns which objects. It is automatically called when players are added and removed via the NetworkManager, but there is no hook in the NetworkManager when non-player client authority objects are added and removed. SendPeerInfo() is NOT called automatically. It is up to user code to call SendPeerInfo() when they want to update the set of client-owned objects.</para>
        /// </summary>
        public void SendPeerInfo()
        {
            if (!m_HostMigration)
                return;

            var listMsg = new PeerListMessage();
            var addresses = new List<PeerInfoMessage>();

            for (int i = 0; i < NetworkServer.connections.Count; i++)
            {
                var conn = NetworkServer.connections[i];
                if (conn != null)
                {
                    var peerInfo = new PeerInfoMessage();

                    string address;
                    int port;
                    NetworkID networkId;
                    NodeID node;
                    byte error2;
                    NetworkManager.activeTransport.GetConnectionInfo(NetworkServer.serverHostId, conn.connectionId, out address, out port, out networkId, out node, out error2);

                    peerInfo.connectionId = conn.connectionId;
                    peerInfo.port = port;
                    if (i == 0)
                    {
                        peerInfo.port = NetworkServer.listenPort;
                        peerInfo.isHost = true;
                        peerInfo.address = "<host>";
                    }
                    else
                    {
                        peerInfo.address = address;
                        peerInfo.isHost = false;
                    }
                    var playerIds = new List<PeerInfoPlayer>();
                    for (int pid = 0; pid < conn.playerControllers.Count; pid++)
                    {
                        var player = conn.playerControllers[pid];
                        if (player != null && player.unetView != null)
                        {
                            PeerInfoPlayer info;
                            info.netId = player.unetView.netId;
                            info.playerControllerId = player.unetView.playerControllerId;
                            playerIds.Add(info);
                        }
                    }

                    if (conn.clientOwnedObjects != null)
                    {
                        foreach (var netId in conn.clientOwnedObjects)
                        {
                            var obj = NetworkServer.FindLocalObject(netId);
                            if (obj == null)
                                continue;

                            var objUV = obj.GetComponent<NetworkIdentity>();
                            if (objUV.playerControllerId != -1)
                            {
                                // already added players
                                continue;
                            }

                            PeerInfoPlayer info;
                            info.netId = netId;
                            info.playerControllerId = -1;
                            playerIds.Add(info);
                        }
                    }
                    if (playerIds.Count > 0)
                    {
                        peerInfo.playerIds = playerIds.ToArray();
                    }
                    addresses.Add(peerInfo);
                }
            }

            listMsg.peers = addresses.ToArray();

            // (re)send all peers to all peers (including the new one)
            for (int i = 0; i < NetworkServer.connections.Count; i++)
            {
                var conn = NetworkServer.connections[i];
                if (conn != null)
                {
                    listMsg.oldServerConnectionId = conn.connectionId;
                    conn.Send(MsgType.NetworkInfo, listMsg);
                }
            }
        }

        // received on both host and clients
        void OnPeerClientAuthority(NetworkMessage netMsg)
        {
            var msg = netMsg.ReadMessage<PeerAuthorityMessage>();

            if (LogFilter.logDebug) { Debug.Log("OnPeerClientAuthority for netId:" + msg.netId); }

            if (m_Peers == null)
            {
                // havent received peers yet. just ignore this. the peer list will contain this data.
                return;
            }

            // find the peer for connId
            for (int peerId = 0; peerId < m_Peers.Length; peerId++)
            {
                var p = m_Peers[peerId];
                if (p.connectionId == msg.connectionId)
                {
                    if (p.playerIds == null)
                    {
                        p.playerIds = new PeerInfoPlayer[0];
                    }

                    if (msg.authorityState)
                    {
                        for (int i = 0; i < p.playerIds.Length; i++)
                        {
                            if (p.playerIds[i].netId == msg.netId)
                            {
                                // already in list
                                return;
                            }
                        }
                        var newPlayerId = new PeerInfoPlayer();
                        newPlayerId.netId = msg.netId;
                        newPlayerId.playerControllerId = -1;

                        var pl = new List<PeerInfoPlayer>(p.playerIds);
                        pl.Add(newPlayerId);
                        p.playerIds = pl.ToArray();
                    }
                    else
                    {
                        for (int i = 0; i < p.playerIds.Length; i++)
                        {
                            if (p.playerIds[i].netId == msg.netId)
                            {
                                var pl = new List<PeerInfoPlayer>(p.playerIds);
                                pl.RemoveAt(i);
                                p.playerIds = pl.ToArray();
                                break;
                            }
                        }
                    }
                }
            }

            var foundObj = ClientScene.FindLocalObject(msg.netId);
            OnAuthorityUpdated(foundObj, msg.connectionId, msg.authorityState);
        }

        // recieved on both host and clients
        void OnPeerInfo(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("OnPeerInfo"); }

            netMsg.ReadMessage(m_PeerListMessage);
            m_Peers = m_PeerListMessage.peers;
            m_OldServerConnectionId = m_PeerListMessage.oldServerConnectionId;

            for (int i = 0; i < m_Peers.Length; i++)
            {
                if (LogFilter.logDebug) { Debug.Log("peer conn " + m_Peers[i].connectionId + " your conn " + m_PeerListMessage.oldServerConnectionId); }

                if (m_Peers[i].connectionId == m_PeerListMessage.oldServerConnectionId)
                {
                    m_Peers[i].isYou = true;
                    break;
                }
            }
            OnPeersUpdated(m_PeerListMessage);
        }

        void OnServerReconnectPlayerMessage(NetworkMessage netMsg)
        {
            var msg = netMsg.ReadMessage<ReconnectMessage>();

            if (LogFilter.logDev) { Debug.Log("OnReconnectMessage: connId=" + msg.oldConnectionId + " playerControllerId:" + msg.playerControllerId + " netId:" + msg.netId); }

            var playerObject = FindPendingPlayer(msg.oldConnectionId, msg.netId, msg.playerControllerId);
            if (playerObject == null)
            {
                if (LogFilter.logError) { Debug.LogError("OnReconnectMessage connId=" + msg.oldConnectionId + " player null for netId:" + msg.netId + " msg.playerControllerId:" + msg.playerControllerId); }
                return;
            }

            if (playerObject.activeSelf)
            {
                if (LogFilter.logError) { Debug.LogError("OnReconnectMessage connId=" + msg.oldConnectionId + " player already active?"); }
                return;
            }

            if (LogFilter.logDebug) { Debug.Log("OnReconnectMessage: player=" + playerObject); }


            NetworkReader extraDataReader = null;
            if (msg.msgSize != 0)
            {
                extraDataReader = new NetworkReader(msg.msgData);
            }

            if (msg.playerControllerId != -1)
            {
                if (extraDataReader == null)
                {
                    OnServerReconnectPlayer(netMsg.conn, playerObject, msg.oldConnectionId, msg.playerControllerId);
                }
                else
                {
                    OnServerReconnectPlayer(netMsg.conn, playerObject, msg.oldConnectionId, msg.playerControllerId, extraDataReader);
                }
            }
            else
            {
                OnServerReconnectObject(netMsg.conn, playerObject, msg.oldConnectionId);
            }
        }

        /// <summary>
        /// This re-establishes a non-player object with client authority with a client that is reconnected. It is similar to NetworkServer.SpawnWithClientAuthority().
        /// <para>This is called by the default implementation of OnServerReconnectObject.</para>
        /// </summary>
        /// <param name="newConnection">The connection of the new client.</param>
        /// <param name="oldObject">The object with client authority that is being reconnected.</param>
        /// <param name="oldConnectionId">This client's connectionId on the old host.</param>
        /// <returns>True if the object was reconnected.</returns>
        // call this on the server to re-setup an object for a new connection
        public bool ReconnectObjectForConnection(NetworkConnection newConnection, GameObject oldObject, int oldConnectionId)
        {
            if (!NetworkServer.active)
            {
                if (LogFilter.logError) { Debug.LogError("ReconnectObjectForConnection must have active server"); }
                return false;
            }

            if (LogFilter.logDebug) { Debug.Log("ReconnectObjectForConnection: oldConnId=" + oldConnectionId + " obj=" + oldObject + " conn:" + newConnection); }

            if (!m_PendingPlayers.ContainsKey(oldConnectionId))
            {
                if (LogFilter.logError) { Debug.LogError("ReconnectObjectForConnection oldConnId=" + oldConnectionId + " not found."); }
                return false;
            }

            oldObject.SetActive(true);
            oldObject.GetComponent<NetworkIdentity>().SetNetworkInstanceId(new NetworkInstanceId(0));

            if (!NetworkServer.SpawnWithClientAuthority(oldObject, newConnection))
            {
                if (LogFilter.logError) { Debug.LogError("ReconnectObjectForConnection oldConnId=" + oldConnectionId + " SpawnWithClientAuthority failed."); }
                return false;
            }

            return true;
        }

        /// <summary>
        /// This re-establishes a player object with a client that is reconnected. It is similar to NetworkServer.AddPlayerForConnection(). The player game object will become the player object for the new connection.
        /// <para>This is called by the default implementation of OnServerReconnectPlayer.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// class MyMigrationManager : <see cref="NetworkMigrationManager">NetworkMigrationManager</see>
        /// {
        ///    protected override void OnServerReconnectPlayer(<see cref="NetworkConnection">NetworkConnection</see> newConnection, <see cref="GameObject">GameObject</see> oldPlayer, int oldConnectionId, short playerControllerId)
        ///    {
        ///        Debug.Log("Reconnecting oldPlayer:" + oldPlayer);
        ///        ReconnectPlayerForConnection(newConnection, oldPlayer, oldConnectionId, playerControllerId);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="newConnection">The connection of the new client.</param>
        /// <param name="oldPlayer">The player object.</param>
        /// <param name="oldConnectionId">This client's connectionId on the old host.</param>
        /// <param name="playerControllerId">The playerControllerId of the player that is rejoining.</param>
        /// <returns>True if able to re-add this player.</returns>
        // call this on the server to re-setup a reconnecting player for a new connection
        public bool ReconnectPlayerForConnection(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
        {
            if (!NetworkServer.active)
            {
                if (LogFilter.logError) { Debug.LogError("ReconnectPlayerForConnection must have active server"); }
                return false;
            }

            if (LogFilter.logDebug) { Debug.Log("ReconnectPlayerForConnection: oldConnId=" + oldConnectionId + " player=" + oldPlayer + " conn:" + newConnection); }

            if (!m_PendingPlayers.ContainsKey(oldConnectionId))
            {
                if (LogFilter.logError) { Debug.LogError("ReconnectPlayerForConnection oldConnId=" + oldConnectionId + " not found."); }
                return false;
            }

            oldPlayer.SetActive(true);

            // this ensures the observers are rebuilt for the player object
            NetworkServer.Spawn(oldPlayer);

            if (!NetworkServer.AddPlayerForConnection(newConnection, oldPlayer, playerControllerId))
            {
                if (LogFilter.logError) { Debug.LogError("ReconnectPlayerForConnection oldConnId=" + oldConnectionId + " AddPlayerForConnection failed."); }
                return false;
            }

            //NOTE. cannot remove the pending player here - could be more owned objects to come in later messages.

            if (NetworkServer.localClientActive)
            {
                SendPeerInfo();
            }

            return true;
        }

        /// <summary>
        /// This should be called on a client when it has lost its connection to the host.
        /// <para>This will caus the virtual function OnClientDisconnectedFromHost to be invoked. This is called automatically by the NetworkManager if one is in use.</para>
        /// </summary>
        /// <param name="conn">The connection of the client that was connected to the host.</param>
        /// <returns>True if the client should stay in the on-line scene.</returns>
        // called by NetworkManager on clients when connection to host is lost.
        // return true to stay in online scene
        public bool LostHostOnClient(NetworkConnection conn)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkMigrationManager client OnDisconnectedFromHost"); }

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                if (LogFilter.logError) { Debug.LogError("LostHostOnClient: Host migration not supported on WebGL"); }
                return false;
            }

            if (m_Client == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkMigrationManager LostHostOnHost client was never initialized."); }
                return false;
            }

            if (!m_HostMigration)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkMigrationManager LostHostOnHost migration not enabled."); }
                return false;
            }

            m_DisconnectedFromHost = true;
            DisablePlayerObjects();


            byte error;
            NetworkManager.activeTransport.Disconnect(m_Client.hostId, m_Client.connection.connectionId, out error);

            if (m_OldServerConnectionId != -1)
            {
                // only call this if we actually connected
                SceneChangeOption sceneOption;
                OnClientDisconnectedFromHost(conn, out sceneOption);
                return sceneOption == SceneChangeOption.StayInOnlineScene;
            }

            // never entered the online scene
            return false;
        }

        /// <summary>
        /// This should be called on a host when it has has been shutdown.
        /// <para>This causes the virtual function OnServerHostShutdown to be invoked. This is called automatically by the NetworkManager if one is in use.</para>
        /// </summary>
        // called by NetworkManager on host when host is closed
        public void LostHostOnHost()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkMigrationManager LostHostOnHost"); }

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                if (LogFilter.logError) { Debug.LogError("LostHostOnHost: Host migration not supported on WebGL"); }
                return;
            }

            OnServerHostShutdown();

            if (m_Peers == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkMigrationManager LostHostOnHost no peers"); }
                return;
            }

            if (m_Peers.Length != 1)
            {
                // there was another player that could become the host
                m_HostWasShutdown = true;
            }
        }

        /// <summary>
        /// This causes a client that has been disconnected from the host to become the new host of the game.
        /// <para>This starts a server, initializes it with the state of the existing networked objects, and starts a local client so that this client becomes a host. The old NetworkClient instance that was connected to the old host is destroyed.</para>
        /// <para>This will cause OnStartServer to be called on networked objects in the scene.</para>
        /// <para>Any player objects for this peer will automatically be re-added through the local client that was created.</para>
        /// </summary>
        /// <param name="port">The network port to listen on.</param>
        /// <returns>True if able to become the new host.</returns>
        public bool BecomeNewHost(int port)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkMigrationManager BecomeNewHost " + m_MatchInfo); }

            NetworkServer.RegisterHandler(MsgType.ReconnectPlayer, OnServerReconnectPlayerMessage);

            var newClient = NetworkServer.BecomeHost(m_Client, port, m_MatchInfo, oldServerConnectionId, peers);
            if (newClient != null)
            {
                if (NetworkManager.singleton != null)
                {
                    NetworkManager.singleton.RegisterServerMessages();
                    NetworkManager.singleton.UseExternalClient(newClient);
                }
                else
                {
                    Debug.LogWarning("MigrationManager BecomeNewHost - No NetworkManager.");
                }

                newClient.RegisterHandlerSafe(MsgType.NetworkInfo, OnPeerInfo);

                RemovePendingPlayer(m_OldServerConnectionId);
                Reset(ClientScene.ReconnectIdInvalid);
                SendPeerInfo();
                return true;
            }
            else
            {
                if (LogFilter.logError) { Debug.LogError("NetworkServer.BecomeHost failed"); }
                return false;
            }
        }

        // ----------------------------- Callbacks ---------------------------------------

        /// <summary>
        /// A virtual function that is called when the client is disconnected from the host.
        /// <para>The sceneChange parameter allows the game to choose to stay in the current scene, or switch to the offline scene.</para>
        /// </summary>
        /// <param name="conn">The connection to the old host.</param>
        /// <param name="sceneChange">How to handle scene changes.</param>
        // called on client after the connection to host is lost. controls whether to switch scenes
        protected virtual void OnClientDisconnectedFromHost(NetworkConnection conn, out SceneChangeOption sceneChange)
        {
            sceneChange = SceneChangeOption.StayInOnlineScene;
        }

        /// <summary>
        /// A virtual function that is called when the host is shutdown.
        /// <para>Calling NetworkManager.StopHost() will cause this function to be invoked if there is an active NetworkMigrationManager. Using the Stop Host button of the NetworkManagerHUD will cause this to be called.</para>
        /// </summary>
        // called on host after the host is lost. host MUST change scenes
        protected virtual void OnServerHostShutdown()
        {
        }

        /// <summary>
        /// A virtual function that is called on the new host when a client from the old host reconnects to the new host.
        /// <para>The base class version of this function calls ReconnectPlayerForConnection() to hookup the new client.</para>
        /// <para>ReconnectPlayerForConnection does not have to be called from within this function, it can be done asynchronously.</para>
        /// </summary>
        /// <param name="newConnection">The connection of the new client.</param>
        /// <param name="oldPlayer">The player object associated with this client.</param>
        /// <param name="oldConnectionId">The connectionId of this client on the old host.</param>
        /// <param name="playerControllerId">The playerControllerId of the player that is re-joining.</param>
        // called on new host (server) when a client from the old host re-connects a player
        protected virtual void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
        {
            ReconnectPlayerForConnection(newConnection, oldPlayer, oldConnectionId, playerControllerId);
        }

        /// <summary>
        /// A virtual function that is called on the new host when a client from the old host reconnects to the new host.
        /// <para>The base class version of this function calls ReconnectPlayerForConnection() to hookup the new client.</para>
        /// <para>ReconnectPlayerForConnection does not have to be called from within this function, it can be done asynchronously.</para>
        /// </summary>
        /// <param name="newConnection">The connection of the new client.</param>
        /// <param name="oldPlayer">The player object associated with this client.</param>
        /// <param name="oldConnectionId">The connectionId of this client on the old host.</param>
        /// <param name="playerControllerId">The playerControllerId of the player that is re-joining.</param>
        /// <param name="extraMessageReader">Additional message data (optional).</param>
        // called on new host (server) when a client from the old host re-connects a player
        protected virtual void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId, NetworkReader extraMessageReader)
        {
            // extraMessageReader is not used in the default version, but it is available for custom versions to use
            ReconnectPlayerForConnection(newConnection, oldPlayer, oldConnectionId, playerControllerId);
        }

        /// <summary>
        /// A virtual function that is called for non-player objects with client authority on the new host when a client from the old host reconnects to the new host.
        /// <para>The base class version of this function calls ReconnectObjectForConnection() to hookup the object for the new client.</para>
        /// </summary>
        /// <param name="newConnection">The connection of the new client.</param>
        /// <param name="oldObject">The object with authority that is being reconnected.</param>
        /// <param name="oldConnectionId">The connectionId of this client on the old host.</param>
        // called on new host (server) when a client from the old host re-connects an object with authority
        protected virtual void OnServerReconnectObject(NetworkConnection newConnection, GameObject oldObject, int oldConnectionId)
        {
            ReconnectObjectForConnection(newConnection, oldObject, oldConnectionId);
        }

        /// <summary>
        /// A virtual function that is called when the set of peers in the game changes.
        /// <para>This happens when a new client connects to the host, a client disconnects from the host, and when players are added and removed from clients.</para>
        /// <para>The list of peers is stored in the member variable peers on the migration manager. This is used when the connection to the host is lost, to choose the new host and to re-add player objects.</para>
        /// </summary>
        /// <param name="peers">The set of peers in the game.</param>
        // called on both host and client when the set of peers is updated
        protected virtual void OnPeersUpdated(PeerListMessage peers)
        {
            if (LogFilter.logDev) { Debug.Log("NetworkMigrationManager NumPeers "  + peers.peers.Length); }
        }

        /// <summary>
        /// A virtual function that is called when the authority of a non-player object changes.
        /// <para>This is called on the host and on clients when the AssignClientAuthority, RemoveClientAuthority and NetworkServer.SpawnWithClientAuthority are used.</para>
        /// </summary>
        /// <param name="go">The game object whose authority has changed.</param>
        /// <param name="connectionId">The id of the connection whose authority changed for this object.</param>
        /// <param name="authorityState">The new authority state for the object.</param>
        // called on both host and client when authority changes on a non-player object
        protected virtual void OnAuthorityUpdated(GameObject go, int connectionId, bool authorityState)
        {
            if (LogFilter.logDev) { Debug.Log("NetworkMigrationManager OnAuthorityUpdated for " + go + " conn:" + connectionId + " state:" + authorityState); }
        }

        /// <summary>
        /// This is a utility function to pick one of the peers in the game as the new host.
        /// <para>This function implements the default host-choosing strategy of picking the peer with the lowest connectionId on the server.</para>
        /// <para>Applications are not required to use this function to choose the new host. They can use any method they want. The choice does not have to be made synchronously, so it is possible to communicate with an external service to choose the new host.</para>
        /// <para>However, the default UI of the NetworkMigrationManager calls into this function.</para>
        /// </summary>
        /// <param name="newHostInfo">Information about the new host, including the IP address.</param>
        /// <param name="youAreNewHost">True if this client is to be the new host.</param>
        /// <returns>True if able to pick a new host.</returns>
        // utility function called by the default UI on client after connection to host was lost, to pick a new host.
        public virtual bool FindNewHost(out NetworkSystem.PeerInfoMessage newHostInfo, out bool youAreNewHost)
        {
            if (m_Peers == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkMigrationManager FindLowestHost no peers"); }
                newHostInfo = null;
                youAreNewHost = false;
                return false;
            }

            if (LogFilter.logDev) { Debug.Log("NetworkMigrationManager FindLowestHost"); }

            const int k_FakeConnectionId = 50000;

            newHostInfo = new PeerInfoMessage();
            newHostInfo.connectionId = k_FakeConnectionId;
            newHostInfo.address = "";
            newHostInfo.port = 0;

            int yourConnectionId = -1;
            youAreNewHost = false;

            for (int peerId = 0; peerId < m_Peers.Length; peerId++)
            {
                var peer = m_Peers[peerId];
                if (peer.connectionId == 0)
                {
                    continue;
                }

                if (peer.isHost)
                {
                    continue;
                }

                if (peer.isYou)
                {
                    yourConnectionId = peer.connectionId;
                }

                if (peer.connectionId < newHostInfo.connectionId)
                {
                    newHostInfo = peer;
                }
            }
            if (newHostInfo.connectionId == k_FakeConnectionId)
            {
                return false;
            }
            if (newHostInfo.connectionId == yourConnectionId)
            {
                youAreNewHost = true;
            }

            if (LogFilter.logDev) { Debug.Log("FindNewHost new host is " + newHostInfo.address); }
            return true;
        }

        // ----------------------------- GUI ---------------------------------------

        void OnGUIHost()
        {
            int ypos = m_OffsetY;
            const int spacing = 25;

            GUI.Label(new Rect(m_OffsetX, ypos, 200, 40), "Host Was Shutdown ID(" + m_OldServerConnectionId + ")");
            ypos += spacing;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GUI.Label(new Rect(m_OffsetX, ypos, 200, 40), "Host Migration not supported for WebGL");
                return;
            }

            if (m_WaitingReconnectToNewHost)
            {
                if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Reconnect as Client"))
                {
                    Reset(ClientScene.ReconnectIdHost);

                    if (NetworkManager.singleton != null)
                    {
                        NetworkManager.singleton.networkAddress = GUI.TextField(new Rect(m_OffsetX + 100, ypos, 95, 20), NetworkManager.singleton.networkAddress);
                        NetworkManager.singleton.StartClient();
                    }
                    else
                    {
                        Debug.LogWarning("MigrationManager Old Host Reconnect - No NetworkManager.");
                    }
                }
                ypos += spacing;
            }
            else
            {
                if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Pick New Host"))
                {
                    bool youAreNewHost;
                    if (FindNewHost(out m_NewHostInfo, out youAreNewHost))
                    {
                        m_NewHostAddress = m_NewHostInfo.address;
                        if (youAreNewHost)
                        {
                            // you cannot be the new host.. you were the old host..?
                            Debug.LogWarning("MigrationManager FindNewHost - new host is self?");
                        }
                        else
                        {
                            m_WaitingReconnectToNewHost = true;
                        }
                    }
                }
                ypos += spacing;
            }

            if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Leave Game"))
            {
                if (NetworkManager.singleton != null)
                {
                    NetworkManager.singleton.SetupMigrationManager(null);
                    NetworkManager.singleton.StopHost();
                }
                else
                {
                    Debug.LogWarning("MigrationManager Old Host LeaveGame - No NetworkManager.");
                }
                Reset(ClientScene.ReconnectIdInvalid);
            }
            ypos += spacing;
        }

        void OnGUIClient()
        {
            int ypos = m_OffsetY;
            const int spacing = 25;

            GUI.Label(new Rect(m_OffsetX, ypos, 200, 40), "Lost Connection To Host ID(" + m_OldServerConnectionId + ")");
            ypos += spacing;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GUI.Label(new Rect(m_OffsetX, ypos, 200, 40), "Host Migration not supported for WebGL");
                return;
            }

            if (m_WaitingToBecomeNewHost)
            {
                GUI.Label(new Rect(m_OffsetX, ypos, 200, 40), "You are the new host");
                ypos += spacing;

                if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Start As Host"))
                {
                    if (NetworkManager.singleton != null)
                    {
                        BecomeNewHost(NetworkManager.singleton.networkPort);
                    }
                    else
                    {
                        Debug.LogWarning("MigrationManager Client BecomeNewHost - No NetworkManager.");
                    }
                }
                ypos += spacing;
            }
            else if (m_WaitingReconnectToNewHost)
            {
                GUI.Label(new Rect(m_OffsetX, ypos, 200, 40), "New host is " + m_NewHostAddress);
                ypos += spacing;

                if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Reconnect To New Host"))
                {
                    Reset(m_OldServerConnectionId);

                    if (NetworkManager.singleton != null)
                    {
                        NetworkManager.singleton.networkAddress = m_NewHostAddress;
                        NetworkManager.singleton.client.ReconnectToNewHost(m_NewHostAddress, NetworkManager.singleton.networkPort);
                    }
                    else
                    {
                        Debug.LogWarning("MigrationManager Client reconnect - No NetworkManager.");
                    }
                }
                ypos += spacing;
            }
            else
            {
                if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Pick New Host"))
                {
                    bool youAreNewHost;
                    if (FindNewHost(out m_NewHostInfo, out youAreNewHost))
                    {
                        m_NewHostAddress = m_NewHostInfo.address;
                        if (youAreNewHost)
                        {
                            m_WaitingToBecomeNewHost = true;
                        }
                        else
                        {
                            m_WaitingReconnectToNewHost = true;
                        }
                    }
                }
                ypos += spacing;
            }

            if (GUI.Button(new Rect(m_OffsetX, ypos, 200, 20), "Leave Game"))
            {
                if (NetworkManager.singleton != null)
                {
                    NetworkManager.singleton.SetupMigrationManager(null);
                    NetworkManager.singleton.StopHost();
                }
                else
                {
                    Debug.LogWarning("MigrationManager Client LeaveGame - No NetworkManager.");
                }
                Reset(ClientScene.ReconnectIdInvalid);
            }
            ypos += spacing;
        }

        void OnGUI()
        {
            if (!m_ShowGUI)
                return;

            if (m_HostWasShutdown)
            {
                OnGUIHost();
                return;
            }

            if (m_DisconnectedFromHost && m_OldServerConnectionId != -1)
            {
                OnGUIClient();
            }
        }
    }
}
