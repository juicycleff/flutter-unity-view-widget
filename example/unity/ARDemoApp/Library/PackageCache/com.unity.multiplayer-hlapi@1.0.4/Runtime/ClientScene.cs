using System;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
    /// <summary>
    /// A client manager which contains static client information and functions.
    /// <para>This manager contains references to tracked static local objects such as spawner registrations. It also has the default message handlers used by clients when they registered none themselves. The manager handles adding/removing player objects to the game after a client connection has been set as ready.</para>
    /// <para>The ClientScene is a singleton, and it has static convenience methods such as ClientScene.Ready().</para>
    /// <para>The ClientScene is used by the NetworkManager, but it can be used by itself.</para>
    /// <para>As the ClientScene manages player objects on the client, it is where clients request to add players. The NetworkManager does this via the ClientScene automatically when auto-add-players is set, but it can be done through code using the function ClientScene.AddPlayer(). This sends an AddPlayer message to the server and will cause a player object to be created for this client.</para>
    /// <para>Like NetworkServer, the ClientScene understands the concept of the local client. The function ClientScene.ConnectLocalServer() is used to become a host by starting a local client (when a server is already running).</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ClientScene
    {
        static List<PlayerController> s_LocalPlayers = new List<PlayerController>();
        static NetworkConnection s_ReadyConnection;
        static Dictionary<NetworkSceneId, NetworkIdentity> s_SpawnableObjects;

        static bool s_IsReady;
        static bool s_IsSpawnFinished;
        static NetworkScene s_NetworkScene = new NetworkScene();

        // static message objects to avoid runtime-allocations
        static ObjectSpawnSceneMessage s_ObjectSpawnSceneMessage = new ObjectSpawnSceneMessage();
        static ObjectSpawnFinishedMessage s_ObjectSpawnFinishedMessage = new ObjectSpawnFinishedMessage();
        static ObjectDestroyMessage s_ObjectDestroyMessage = new ObjectDestroyMessage();
        static ObjectSpawnMessage s_ObjectSpawnMessage = new ObjectSpawnMessage();
        static OwnerMessage s_OwnerMessage = new OwnerMessage();
        static ClientAuthorityMessage s_ClientAuthorityMessage = new ClientAuthorityMessage();

        /// <summary>
        /// An invalid reconnect Id.
        /// </summary>
        public const int ReconnectIdInvalid = -1;
        /// <summary>
        /// A constant ID used by the old host when it reconnects to the new host.
        /// </summary>
        public const int ReconnectIdHost = 0;
        static int s_ReconnectId = ReconnectIdInvalid;
        static PeerInfoMessage[] s_Peers;
        static bool hasMigrationPending() { return s_ReconnectId != ReconnectIdInvalid; }

        /// <summary>
        /// Sets the Id that the ClientScene will use when reconnecting to a new host after host migration.
        /// </summary>
        /// <param name="newReconnectId">The Id to use when reconnecting to a game.</param>
        /// <param name="peers">The set of known peers in the game. This may be null.</param>
        static public void SetReconnectId(int newReconnectId, PeerInfoMessage[] peers)
        {
            s_ReconnectId = newReconnectId;
            s_Peers = peers;

            if (LogFilter.logDebug) { Debug.Log("ClientScene::SetReconnectId: " + newReconnectId); }
        }

        static internal void SetNotReady()
        {
            s_IsReady = false;
        }

        struct PendingOwner
        {
            public NetworkInstanceId netId;
            public short playerControllerId;
        }
        static List<PendingOwner> s_PendingOwnerIds = new List<PendingOwner>();

        /// <summary>
        /// A list of all players added to the game.
        /// <para>These are the players on this client, not all of the players in the game on the server. The client has no explicit knowledge of the player objects of other clients.</para>
        /// </summary>
        public static List<PlayerController> localPlayers { get { return s_LocalPlayers; } }
        /// <summary>
        /// Returns true when a client's connection has been set to ready.
        /// <para>A client that is ready recieves state updates from the server, while a client that is not ready does not. This useful when the state of the game is not normal, such as a scene change or end-of-game.</para>
        /// <para>This is read-only. To change the ready state of a client, use ClientScene.Ready(). The server is able to set the ready state of clients using NetworkServer.SetClientReady(), NetworkServer.SetClientNotReady() and NetworkServer.SetAllClientsNotReady().</para>
        /// <para>This is done when changing scenes so that clients don't receive state update messages during scene loading.</para>
        /// </summary>
        public static bool ready { get { return s_IsReady; } }
        /// <summary>
        /// The NetworkConnection object that is currently "ready". This is the connection to the server where objects are spawned from.
        /// <para>This connection can be used to send messages to the server. There can only be one ready connection at a time. There can be multiple NetworkClient instances in existence, each with their own NetworkConnections, but there is only one ClientScene instance and corresponding ready connection.</para>
        /// </summary>
        public static NetworkConnection readyConnection { get { return s_ReadyConnection; }}

        /// <summary>
        /// The reconnectId to use when a client reconnects to the new host of a game after the old host was lost.
        /// <para>This will be ClientScene.ReconnectIdInvalid by default (-1), and will be ClientScene.ReconnectIdHost when the old host is reconnecting to the host of the new game.</para>
        /// </summary>
        public static int reconnectId { get { return s_ReconnectId; }}

        /// <summary>
        /// This is a dictionary of networked objects that have been spawned on the client.
        /// <para>The key of the dictionary is the NetworkIdentity netId of the objects.</para>
        /// </summary>
        //NOTE: spawn handlers, prefabs and local objects now live in NetworkScene
        public static Dictionary<NetworkInstanceId, NetworkIdentity> objects { get { return s_NetworkScene.localObjects; } }
        /// <summary>
        /// This is a dictionary of the prefabs that are registered on the client with ClientScene.RegisterPrefab().
        /// <para>The key to the dictionary is the prefab asset Id.</para>
        /// </summary>
        public static Dictionary<NetworkHash128, GameObject> prefabs { get { return NetworkScene.guidToPrefab; } }
        /// <summary>
        /// This is dictionary of the disabled NetworkIdentity objects in the scene that could be spawned by messages from the server.
        /// <para>The key to the dictionary is the NetworkIdentity sceneId.</para>
        /// </summary>
        public static Dictionary<NetworkSceneId, NetworkIdentity> spawnableObjects { get { return s_SpawnableObjects; } }

        internal static void Shutdown()
        {
            s_NetworkScene.Shutdown();
            s_LocalPlayers = new List<PlayerController>();
            s_PendingOwnerIds = new List<PendingOwner>();
            s_SpawnableObjects = null;
            s_ReadyConnection = null;
            s_IsReady = false;
            s_IsSpawnFinished = false;
            s_ReconnectId = ReconnectIdInvalid;
            NetworkManager.activeTransport.Shutdown();
            NetworkManager.activeTransport.Init();
        }

        internal static bool GetPlayerController(short playerControllerId, out PlayerController player)
        {
            player = null;
            if (playerControllerId >= localPlayers.Count)
            {
                if (LogFilter.logWarn) { Debug.Log("ClientScene::GetPlayer: no local player found for: " + playerControllerId); }
                return false;
            }

            if (localPlayers[playerControllerId] == null)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("ClientScene::GetPlayer: local player is null for: " + playerControllerId); }
                return false;
            }
            player = localPlayers[playerControllerId];
            return player.gameObject != null;
        }

        // this is called from message handler for Owner message
        internal static void InternalAddPlayer(NetworkIdentity view, short playerControllerId)
        {
            if (LogFilter.logDebug) { Debug.LogWarning("ClientScene::InternalAddPlayer: playerControllerId : " + playerControllerId); }

            if (playerControllerId >= s_LocalPlayers.Count)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("ClientScene::InternalAddPlayer: playerControllerId higher than expected: " + playerControllerId); }
                while (playerControllerId >= s_LocalPlayers.Count)
                {
                    s_LocalPlayers.Add(new PlayerController());
                }
            }

            // NOTE: It can be "normal" when changing scenes for the player to be destroyed and recreated.
            // But, the player structures are not cleaned up, we'll just replace the old player
            var newPlayer = new PlayerController {gameObject = view.gameObject, playerControllerId = playerControllerId, unetView = view};
            s_LocalPlayers[playerControllerId] = newPlayer;
            if (s_ReadyConnection == null)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("No ready connection found for setting player controller during InternalAddPlayer"); }
            }
            else
            {
                s_ReadyConnection.SetPlayerController(newPlayer);
            }
        }

        /// <summary>
        /// This adds a player GameObject for this client. This causes an AddPlayer message to be sent to the server, and NetworkManager.OnServerAddPlayer is called. If an extra message was passed to AddPlayer, then OnServerAddPlayer will be called with a NetworkReader that contains the contents of the message.
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client. This is not the global player number.</para>
        /// </summary>
        /// <param name="playerControllerId">The local player ID number.</param>
        /// <returns>True if player was added.</returns>
        // use this if already ready
        public static bool AddPlayer(short playerControllerId)
        {
            return AddPlayer(null, playerControllerId);
        }

        /// <summary>
        /// This adds a player GameObject for this client. This causes an AddPlayer message to be sent to the server, and NetworkManager.OnServerAddPlayer is called. If an extra message was passed to AddPlayer, then OnServerAddPlayer will be called with a NetworkReader that contains the contents of the message.
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client. This is not the global player number.</para>
        /// </summary>
        /// <param name="readyConn">The connection to become ready for this client.</param>
        /// <param name="playerControllerId">The local player ID number.</param>
        /// <returns>True if player was added.</returns>
        // use this to implicitly become ready
        public static bool AddPlayer(NetworkConnection readyConn, short playerControllerId)
        {
            return AddPlayer(readyConn, playerControllerId, null);
        }

        /// <summary>
        /// This adds a player GameObject for this client. This causes an AddPlayer message to be sent to the server, and NetworkManager.OnServerAddPlayer is called. If an extra message was passed to AddPlayer, then OnServerAddPlayer will be called with a NetworkReader that contains the contents of the message.
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client. This is not the global player number.</para>
        /// </summary>
        /// <param name="readyConn">The connection to become ready for this client.</param>
        /// <param name="playerControllerId">The local player ID number.</param>
        /// <param name="extraMessage">An extra message object that can be passed to the server for this player.</param>
        /// <returns>True if player was added.</returns>
        // use this to implicitly become ready
        public static bool AddPlayer(NetworkConnection readyConn, short playerControllerId, MessageBase extraMessage)
        {
            if (playerControllerId < 0)
            {
                if (LogFilter.logError) { Debug.LogError("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " is negative"); }
                return false;
            }
            if (playerControllerId > PlayerController.MaxPlayersPerClient)
            {
                if (LogFilter.logError) { Debug.LogError("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " is too high, max is " + PlayerController.MaxPlayersPerClient); }
                return false;
            }
            if (playerControllerId > PlayerController.MaxPlayersPerClient / 2)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " is unusually high"); }
            }

            // fill out local players array
            while (playerControllerId >= s_LocalPlayers.Count)
            {
                s_LocalPlayers.Add(new PlayerController());
            }

            // ensure valid ready connection
            if (readyConn == null)
            {
                if (!s_IsReady)
                {
                    if (LogFilter.logError) { Debug.LogError("Must call AddPlayer() with a connection the first time to become ready."); }
                    return false;
                }
            }
            else
            {
                s_IsReady = true;
                s_ReadyConnection = readyConn;
            }

            PlayerController existingPlayerController;
            if (s_ReadyConnection.GetPlayerController(playerControllerId, out existingPlayerController))
            {
                if (existingPlayerController.IsValid && existingPlayerController.gameObject != null)
                {
                    if (LogFilter.logError) { Debug.LogError("ClientScene::AddPlayer: playerControllerId of " + playerControllerId + " already in use."); }
                    return false;
                }
            }

            if (LogFilter.logDebug) { Debug.Log("ClientScene::AddPlayer() for ID " + playerControllerId + " called with connection [" + s_ReadyConnection + "]"); }

            if (!hasMigrationPending())
            {
                var msg = new AddPlayerMessage();
                msg.playerControllerId = playerControllerId;
                if (extraMessage != null)
                {
                    var writer = new NetworkWriter();
                    extraMessage.Serialize(writer);
                    msg.msgData = writer.ToArray();
                    msg.msgSize = writer.Position;
                }
                s_ReadyConnection.Send(MsgType.AddPlayer, msg);
            }
            else
            {
                return SendReconnectMessage(extraMessage);
            }
            return true;
        }

        /// <summary>
        /// Send a reconnect message to the new host, used during host migration.
        /// <para>An example usage might be that if you decide to spawn your own player and not use the built in "Auto Create Player" property in the NetworkManager together with HostMigration, you would need to send a reconnect message when your client reconnects. The code below illustrates such an example were we OnClientConnect check if we where disconnected from the host and in that case we send the reconnect message.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class NetworkManagerEx : NetworkManager
        /// {
        ///    public override void OnClientConnect(NetworkConnection conn)
        ///    {
        ///        base.OnClientConnect(conn);
        ///        if (migrationManager.disconnectedFromHost)
        ///        {
        ///            ClientScene.SendReconnectMessage(null);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="extraMessage">Any extra data to send.</param>
        /// <returns>Returns true if the send succeeded.</returns>
        public static bool SendReconnectMessage(MessageBase extraMessage)
        {
            if (!hasMigrationPending())
                return false;

            if (LogFilter.logDebug) { Debug.Log("ClientScene::AddPlayer reconnect " + s_ReconnectId);           }

            if (s_Peers == null)
            {
                SetReconnectId(ReconnectIdInvalid, null);
                if (LogFilter.logError)
                {
                    Debug.LogError("ClientScene::AddPlayer: reconnecting, but no peers.");
                }
                return false;
            }

            // reconnect all the players
            for (int i = 0; i < s_Peers.Length; i++)
            {
                var peer = s_Peers[i];
                if (peer.playerIds == null)
                {
                    // this could be empty if this peer had no players
                    continue;
                }
                if (peer.connectionId == s_ReconnectId)
                {
                    for (int pid = 0; pid < peer.playerIds.Length; pid++)
                    {
                        var msg = new ReconnectMessage();
                        msg.oldConnectionId = s_ReconnectId;
                        msg.netId = peer.playerIds[pid].netId;
                        msg.playerControllerId = peer.playerIds[pid].playerControllerId;
                        if (extraMessage != null)
                        {
                            var writer = new NetworkWriter();
                            extraMessage.Serialize(writer);
                            msg.msgData = writer.ToArray();
                            msg.msgSize = writer.Position;
                        }

                        s_ReadyConnection.Send(MsgType.ReconnectPlayer, msg);
                    }
                }
            }
            // this should only be done once.
            SetReconnectId(ReconnectIdInvalid, null);
            return true;
        }

        /// <summary>
        /// Removes the specified player ID from the game.
        /// <para>Both the client and the server destroy the player GameObject and remove it from the player list. The playerControllerId is scoped to this client, not global to all players or clients.</para>
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        /// <param name="playerControllerId">The local playerControllerId number to be removed.</param>
        /// <returns>Returns true if the player was successfully destoyed and removed.</returns>
        public static bool RemovePlayer(short playerControllerId)
        {
            if (LogFilter.logDebug) { Debug.Log("ClientScene::RemovePlayer() for ID " + playerControllerId + " called with connection [" + s_ReadyConnection + "]"); }

            PlayerController playerController;
            if (s_ReadyConnection.GetPlayerController(playerControllerId, out playerController))
            {
                var msg = new RemovePlayerMessage();
                msg.playerControllerId = playerControllerId;
                s_ReadyConnection.Send(MsgType.RemovePlayer, msg);

                s_ReadyConnection.RemovePlayerController(playerControllerId);
                s_LocalPlayers[playerControllerId] = new PlayerController();

                Object.Destroy(playerController.gameObject);
                return true;
            }
            if (LogFilter.logError) { Debug.LogError("Failed to find player ID " + playerControllerId); }
            return false;
        }

        /// <summary>
        /// Signal that the client connection is ready to enter the game.
        /// <para>This could be for example when a client enters an ongoing game and has finished loading the current scene. The server should respond to the SYSTEM_READY event with an appropriate handler which instantiates the players object for example.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.UI;
        /// using UnityEngine.Networking;
        ///
        /// //This makes the GameObject a NetworkManager GameObject
        /// public class Example : NetworkManager
        /// {
        ///    public bool m_ServerStarted, m_ClientStarted;
        ///    public Button m_ClientButton;
        ///
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnClientConnect(NetworkConnection connection)
        ///    {
        ///        ClientScene.Ready(connection);
        ///        ClientScene.AddPlayer(0);
        ///        m_ClientStarted = true;
        ///        //Output text to show the connection on the client side
        ///        Debug.Log("Client Side : Client " + connection.connectionId + " Connected!");
        ///        //Register and receive the message on the Client's side (NetworkConnection.Send Example)
        ///        client.RegisterHandler(MsgType.Ready, ReadyMessage);
        ///    }
        ///
        ///    //Use this to receive the message from the Server on the Client's side
        ///    public void ReadyMessage(NetworkMessage networkMessage)
        ///    {
        ///        Debug.Log("Client Ready! ");
        ///    }
        ///
        ///    //Detect when a client disconnects from the Server
        ///    public override void OnClientDisconnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection loss on the client side
        ///        Debug.Log("Client Side : Client " + connection.connectionId + " Lost!");
        ///        m_ClientStarted = false;
        ///    }
        ///    public void ClientButton()
        ///    {
        ///        if (!m_ClientStarted)
        ///        {
        ///            NetworkServer.Reset();
        ///            singleton.StartClient();
        ///            m_ClientButton.GetComponentInChildren&lt;Text&gt;().text = "Disconnect";
        ///        }
        ///        else
        ///        {
        ///            singleton.StopClient();
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">The client connection which is ready.</param>
        /// <returns></returns>
        public static bool Ready(NetworkConnection conn)
        {
            if (s_IsReady)
            {
                if (LogFilter.logError) { Debug.LogError("A connection has already been set as ready. There can only be one."); }
                return false;
            }

            if (LogFilter.logDebug) { Debug.Log("ClientScene::Ready() called with connection [" + conn + "]"); }

            if (conn != null)
            {
                var msg = new ReadyMessage();
                conn.Send(MsgType.Ready, msg);
                s_IsReady = true;
                s_ReadyConnection = conn;
                s_ReadyConnection.isReady = true;
                return true;
            }
            if (LogFilter.logError) { Debug.LogError("Ready() called with invalid connection object: conn=null"); }
            return false;
        }

        /// <summary>
        /// Create and connect a local client instance to the local server. This makes the client into a "host" - a client and server in the same process.
        /// <para>The returned local client acts like normal remote client but internally all messages are routed directly to the server process. Commands from a local client are executed synchronously on the server.</para>
        /// </summary>
        /// <returns>A client object for communicating with the local server.</returns>
        static public NetworkClient ConnectLocalServer()
        {
            var newClient = new LocalClient();
            NetworkServer.instance.ActivateLocalClientScene();
            newClient.InternalConnectLocalServer(true);
            return newClient;
        }

        static internal NetworkClient ReconnectLocalServer()
        {
            LocalClient newClient = new LocalClient();
            NetworkServer.instance.ActivateLocalClientScene();
            newClient.InternalConnectLocalServer(false);
            return newClient;
        }

        static internal void ClearLocalPlayers()
        {
            s_LocalPlayers.Clear();
        }

        static internal void HandleClientDisconnect(NetworkConnection conn)
        {
            if (s_ReadyConnection == conn && s_IsReady)
            {
                s_IsReady = false;
                s_ReadyConnection = null;
            }
        }

        internal static void PrepareToSpawnSceneObjects()
        {
            //NOTE: what is there are already objects in this dict?! should we merge with them?
            s_SpawnableObjects = new Dictionary<NetworkSceneId, NetworkIdentity>();
            var uvs = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
            for (int i = 0; i < uvs.Length; i++)
            {
                var uv = uvs[i];
                if (uv.gameObject.activeSelf)
                {
                    // already active, cannot spawn it
                    continue;
                }

                if (uv.gameObject.hideFlags == HideFlags.NotEditable || uv.gameObject.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (uv.sceneId.IsEmpty())
                    continue;

                s_SpawnableObjects[uv.sceneId] = uv;

                if (LogFilter.logDebug) { Debug.Log("ClientScene::PrepareSpawnObjects sceneId:" + uv.sceneId); }
            }
        }

        internal static NetworkIdentity SpawnSceneObject(NetworkSceneId sceneId)
        {
            if (s_SpawnableObjects.ContainsKey(sceneId))
            {
                NetworkIdentity foundId = s_SpawnableObjects[sceneId];
                s_SpawnableObjects.Remove(sceneId);
                return foundId;
            }
            return null;
        }

        static internal void RegisterSystemHandlers(NetworkClient client, bool localClient)
        {
            if (localClient)
            {
                client.RegisterHandlerSafe(MsgType.ObjectDestroy, OnLocalClientObjectDestroy);
                client.RegisterHandlerSafe(MsgType.ObjectHide, OnLocalClientObjectHide);
                client.RegisterHandlerSafe(MsgType.ObjectSpawn, OnLocalClientObjectSpawn);
                client.RegisterHandlerSafe(MsgType.ObjectSpawnScene, OnLocalClientObjectSpawnScene);
                client.RegisterHandlerSafe(MsgType.LocalClientAuthority, OnClientAuthority);
            }
            else
            {
                // LocalClient shares the sim/scene with the server, no need for these events
                client.RegisterHandlerSafe(MsgType.ObjectSpawn, OnObjectSpawn);
                client.RegisterHandlerSafe(MsgType.ObjectSpawnScene, OnObjectSpawnScene);
                client.RegisterHandlerSafe(MsgType.SpawnFinished, OnObjectSpawnFinished);
                client.RegisterHandlerSafe(MsgType.ObjectDestroy, OnObjectDestroy);
                client.RegisterHandlerSafe(MsgType.ObjectHide, OnObjectDestroy);
                client.RegisterHandlerSafe(MsgType.UpdateVars, OnUpdateVarsMessage);
                client.RegisterHandlerSafe(MsgType.Owner, OnOwnerMessage);
                client.RegisterHandlerSafe(MsgType.SyncList, OnSyncListMessage);
                client.RegisterHandlerSafe(MsgType.Animation, NetworkAnimator.OnAnimationClientMessage);
                client.RegisterHandlerSafe(MsgType.AnimationParameters, NetworkAnimator.OnAnimationParametersClientMessage);
                client.RegisterHandlerSafe(MsgType.LocalClientAuthority, OnClientAuthority);
            }

            client.RegisterHandlerSafe(MsgType.Rpc, OnRPCMessage);
            client.RegisterHandlerSafe(MsgType.SyncEvent, OnSyncEventMessage);
            client.RegisterHandlerSafe(MsgType.AnimationTrigger, NetworkAnimator.OnAnimationTriggerClientMessage);
        }

        // ------------------------ NetworkScene pass-throughs ---------------------

        static internal string GetStringForAssetId(NetworkHash128 assetId)
        {
            GameObject prefab;
            if (NetworkScene.GetPrefab(assetId, out prefab))
            {
                return prefab.name;
            }

            SpawnDelegate handler;
            if (NetworkScene.GetSpawnHandler(assetId, out handler))
            {
                return handler.GetMethodName();
            }

            return "unknown";
        }

        /// <summary>
        /// Registers a prefab with the UNET spawning system.
        /// <para>When a NetworkIdentity object is spawned on a server with NetworkServer.SpawnObject(), and the prefab that the object was created from was registered with RegisterPrefab(), the client will use that prefab to instantiate a corresponding client object with the same netId.</para>
        /// <para>The NetworkManager has a list of spawnable prefabs, it uses this function to register those prefabs with the ClientScene.</para>
        /// <para>The set of current spawnable object is available in the ClientScene static member variable ClientScene.prefabs, which is a dictionary of NetworkAssetIds and prefab references.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class PlantSpawner : NetworkBehaviour
        /// {
        ///    public GameObject plantPrefab;
        ///
        ///    public override void OnStartClient()
        ///    {
        ///        ClientScene.RegisterPrefab(plantPrefab);
        ///    }
        ///
        ///    [Server]
        ///    public void ServerSpawnPlant(Vector3 pos, Quaternion rot)
        ///    {
        ///        var plant = (GameObject)Instantiate(plantPrefab, pos, rot);
        ///        NetworkServer.Spawn(plant);
        ///    }
        /// }
        /// </code>
        /// <para>The optional custom spawn and un-spawn handler functions can be used to implement more advanced spawning strategies such as object pools.</para>
        /// </summary>
        /// <param name="prefab">A Prefab that will be spawned.</param>
        /// <param name="newAssetId">An assetId to be assigned to this prefab. This allows a dynamically created game object to be registered for an already known asset Id.</param>
        // this assigns the newAssetId to the prefab. This is for registering dynamically created game objects for already know assetIds.
        static public void RegisterPrefab(GameObject prefab, NetworkHash128 newAssetId)
        {
            NetworkScene.RegisterPrefab(prefab, newAssetId);
        }

        /// <summary>
        /// Registers a prefab with the UNET spawning system.
        /// <para>When a NetworkIdentity object is spawned on a server with NetworkServer.SpawnObject(), and the prefab that the object was created from was registered with RegisterPrefab(), the client will use that prefab to instantiate a corresponding client object with the same netId.</para>
        /// <para>The NetworkManager has a list of spawnable prefabs, it uses this function to register those prefabs with the ClientScene.</para>
        /// <para>The set of current spawnable object is available in the ClientScene static member variable ClientScene.prefabs, which is a dictionary of NetworkAssetIds and prefab references.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class PlantSpawner : NetworkBehaviour
        /// {
        ///    public GameObject plantPrefab;
        ///
        ///    public override void OnStartClient()
        ///    {
        ///        ClientScene.RegisterPrefab(plantPrefab);
        ///    }
        ///
        ///    [Server]
        ///    public void ServerSpawnPlant(Vector3 pos, Quaternion rot)
        ///    {
        ///        var plant = (GameObject)Instantiate(plantPrefab, pos, rot);
        ///        NetworkServer.Spawn(plant);
        ///    }
        /// }
        /// </code>
        /// <para>The optional custom spawn and un-spawn handler functions can be used to implement more advanced spawning strategies such as object pools.</para>
        /// </summary>
        /// <param name="prefab">A Prefab that will be spawned.</param>
        static public void RegisterPrefab(GameObject prefab)
        {
            NetworkScene.RegisterPrefab(prefab);
        }

        /// <summary>
        /// Registers a prefab with the UNET spawning system.
        /// <para>When a NetworkIdentity object is spawned on a server with NetworkServer.SpawnObject(), and the prefab that the object was created from was registered with RegisterPrefab(), the client will use that prefab to instantiate a corresponding client object with the same netId.</para>
        /// <para>The NetworkManager has a list of spawnable prefabs, it uses this function to register those prefabs with the ClientScene.</para>
        /// <para>The set of current spawnable object is available in the ClientScene static member variable ClientScene.prefabs, which is a dictionary of NetworkAssetIds and prefab references.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class PlantSpawner : NetworkBehaviour
        /// {
        ///    public GameObject plantPrefab;
        ///
        ///    public override void OnStartClient()
        ///    {
        ///        ClientScene.RegisterPrefab(plantPrefab);
        ///    }
        ///
        ///    [Server]
        ///    public void ServerSpawnPlant(Vector3 pos, Quaternion rot)
        ///    {
        ///        var plant = (GameObject)Instantiate(plantPrefab, pos, rot);
        ///        NetworkServer.Spawn(plant);
        ///    }
        /// }
        /// </code>
        /// <para>The optional custom spawn and un-spawn handler functions can be used to implement more advanced spawning strategies such as object pools.</para>
        /// </summary>
        /// <param name="prefab">A Prefab that will be spawned.</param>
        /// <param name="spawnHandler">A method to use as a custom spawnhandler on clients.</param>
        /// <param name="unspawnHandler">A method to use as a custom un-spawnhandler on clients.</param>
        static public void RegisterPrefab(GameObject prefab, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
        {
            NetworkScene.RegisterPrefab(prefab, spawnHandler, unspawnHandler);
        }

        /// <summary>
        /// Removes a registered spawn prefab that was setup with ClientScene.RegisterPrefab.
        /// </summary>
        /// <param name="prefab">The prefab to be removed from registration.</param>
        static public void UnregisterPrefab(GameObject prefab)
        {
            NetworkScene.UnregisterPrefab(prefab);
        }

        /// <summary>
        /// This is an advanced spawning function that registers a custom assetId with the UNET spawning system.
        /// <para>This can be used to register custom spawning methods for an assetId - instead of the usual method of registering spawning methods for a prefab. This should be used when no prefab exists for the spawned objects - such as when they are constructed dynamically at runtime from configuration data.</para>
        /// </summary>
        /// <param name="assetId">Custom assetId string.</param>
        /// <param name="spawnHandler">A method to use as a custom spawnhandler on clients.</param>
        /// <param name="unspawnHandler">A method to use as a custom un-spawnhandler on clients.</param>
        static public void RegisterSpawnHandler(NetworkHash128 assetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
        {
            NetworkScene.RegisterSpawnHandler(assetId, spawnHandler, unspawnHandler);
        }

        /// <summary>
        /// Removes a registered spawn handler function that was registered with ClientScene.RegisterHandler().
        /// </summary>
        /// <param name="assetId">The assetId for the handler to be removed for.</param>
        static public void UnregisterSpawnHandler(NetworkHash128 assetId)
        {
            NetworkScene.UnregisterSpawnHandler(assetId);
        }

        /// <summary>
        /// This clears the registered spawn prefabs and spawn handler functions for this client.
        /// </summary>
        static public void ClearSpawners()
        {
            NetworkScene.ClearSpawners();
        }

        /// <summary>
        /// Destroys all networked objects on the client.
        /// <para>This can be used to clean up when a network connection is closed.</para>
        /// </summary>
        static public void DestroyAllClientObjects()
        {
            s_NetworkScene.DestroyAllClientObjects();
        }

        /// <summary>
        /// NetId is a unique number assigned to all objects with NetworkIdentity components in a game.
        /// <para>This number is the same on the server and all connected clients for a particular object, so it can be used to identify objects across the network. The FindLocalObject() function is called on a client to transform a netId received from a server to a local game object.</para>
        /// </summary>
        /// <param name="netId">NetId of object.</param>
        /// <param name="obj">Networked object.</param>
        static public void SetLocalObject(NetworkInstanceId netId, GameObject obj)
        {
            // if still receiving initial state, dont set isClient
            s_NetworkScene.SetLocalObject(netId, obj, s_IsSpawnFinished, false);
        }

        /// <summary>
        /// This finds the local NetworkIdentity object with the specified network Id.
        /// <para>NetId is a unique number assigned to all objects with NetworkIdentity components in a game. This number is the same on the server and all connected clients for a particular object, so it can be used to identify objects across the network. The FindLocalObject() function is called on a client to transform a netId received from a server to a local game object.</para>
        /// </summary>
        /// <param name="netId">The id of the networked object.</param>
        /// <returns>The game object that matches the netId.</returns>
        static public GameObject FindLocalObject(NetworkInstanceId netId)
        {
            return s_NetworkScene.FindLocalObject(netId);
        }

        static void ApplySpawnPayload(NetworkIdentity uv, Vector3 position, byte[] payload, NetworkInstanceId netId, GameObject newGameObject, NetworkMessage netMsg)
        {
            if (!uv.gameObject.activeSelf)
            {
                uv.gameObject.SetActive(true);
            }
            uv.transform.position = position;
            if (payload != null && payload.Length > 0)
            {
                var payloadReader = new NetworkReader(payload);
                uv.OnUpdateVars(payloadReader, true, netMsg);
            }
            if (newGameObject == null)
            {
                return;
            }

            newGameObject.SetActive(true);
            uv.SetNetworkInstanceId(netId);
            SetLocalObject(netId, newGameObject);

            // objects spawned as part of initial state are started on a second pass
            if (s_IsSpawnFinished)
            {
                uv.OnStartClient();
                CheckForOwner(uv);
            }
        }

        static void OnObjectSpawn(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectSpawnMessage);

            if (!s_ObjectSpawnMessage.assetId.IsValid())
            {
                if (LogFilter.logError) { Debug.LogError("OnObjSpawn netId: " + s_ObjectSpawnMessage.netId + " has invalid asset Id"); }
                return;
            }
            if (LogFilter.logDebug) { Debug.Log("Client spawn handler instantiating [netId:" + s_ObjectSpawnMessage.netId + " asset ID:" + s_ObjectSpawnMessage.assetId + " pos:" + s_ObjectSpawnMessage.position + "]"); }

#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.ObjectSpawn, GetStringForAssetId(s_ObjectSpawnMessage.assetId));
#endif

            NetworkIdentity localNetworkIdentity;
            if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnMessage.netId, out localNetworkIdentity))
            {
                // this object already exists (was in the scene), just apply the update to existing object
                ApplySpawnPayload(localNetworkIdentity, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.payload, s_ObjectSpawnMessage.netId, null, netMsg);
                return;
            }

            GameObject prefab;
            SpawnDelegate handler;
            if (NetworkScene.GetPrefab(s_ObjectSpawnMessage.assetId, out prefab))
            {
                var obj = (GameObject)Object.Instantiate(prefab, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.rotation);
                if (LogFilter.logDebug)
                {
                    Debug.Log("Client spawn handler instantiating [netId:" + s_ObjectSpawnMessage.netId + " asset ID:" + s_ObjectSpawnMessage.assetId + " pos:" + s_ObjectSpawnMessage.position + " rotation: " + s_ObjectSpawnMessage.rotation + "]");
                }

                localNetworkIdentity = obj.GetComponent<NetworkIdentity>();
                if (localNetworkIdentity == null)
                {
                    if (LogFilter.logError) { Debug.LogError("Client object spawned for " + s_ObjectSpawnMessage.assetId + " does not have a NetworkIdentity"); }
                    return;
                }
                localNetworkIdentity.Reset();
                ApplySpawnPayload(localNetworkIdentity, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.payload, s_ObjectSpawnMessage.netId, obj, netMsg);
            }
            // lookup registered factory for type:
            else if (NetworkScene.GetSpawnHandler(s_ObjectSpawnMessage.assetId, out handler))
            {
                GameObject obj = handler(s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.assetId);
                if (obj == null)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("Client spawn handler for " + s_ObjectSpawnMessage.assetId + " returned null"); }
                    return;
                }
                localNetworkIdentity = obj.GetComponent<NetworkIdentity>();
                if (localNetworkIdentity == null)
                {
                    if (LogFilter.logError) { Debug.LogError("Client object spawned for " + s_ObjectSpawnMessage.assetId + " does not have a network identity"); }
                    return;
                }
                localNetworkIdentity.Reset();
                localNetworkIdentity.SetDynamicAssetId(s_ObjectSpawnMessage.assetId);
                ApplySpawnPayload(localNetworkIdentity, s_ObjectSpawnMessage.position, s_ObjectSpawnMessage.payload, s_ObjectSpawnMessage.netId, obj, netMsg);
            }
            else
            {
                if (LogFilter.logError) { Debug.LogError("Failed to spawn server object, did you forget to add it to the NetworkManager? assetId=" + s_ObjectSpawnMessage.assetId + " netId=" + s_ObjectSpawnMessage.netId); }
            }
        }

        static void OnObjectSpawnScene(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectSpawnSceneMessage);

            if (LogFilter.logDebug) { Debug.Log("Client spawn scene handler instantiating [netId:" + s_ObjectSpawnSceneMessage.netId + " sceneId:" + s_ObjectSpawnSceneMessage.sceneId + " pos:" + s_ObjectSpawnSceneMessage.position); }


#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.ObjectSpawnScene, "sceneId");
#endif

            NetworkIdentity localNetworkIdentity;
            if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnSceneMessage.netId, out localNetworkIdentity))
            {
                // this object already exists (was in the scene)
                ApplySpawnPayload(localNetworkIdentity, s_ObjectSpawnSceneMessage.position, s_ObjectSpawnSceneMessage.payload, s_ObjectSpawnSceneMessage.netId, localNetworkIdentity.gameObject, netMsg);
                return;
            }

            NetworkIdentity spawnedId = SpawnSceneObject(s_ObjectSpawnSceneMessage.sceneId);
            if (spawnedId == null)
            {
                if (LogFilter.logError) { Debug.LogError("Spawn scene object not found for " + s_ObjectSpawnSceneMessage.sceneId); }
                return;
            }

            if (LogFilter.logDebug) { Debug.Log("Client spawn for [netId:" + s_ObjectSpawnSceneMessage.netId + "] [sceneId:" + s_ObjectSpawnSceneMessage.sceneId + "] obj:" + spawnedId.gameObject.name); }
            ApplySpawnPayload(spawnedId, s_ObjectSpawnSceneMessage.position, s_ObjectSpawnSceneMessage.payload, s_ObjectSpawnSceneMessage.netId, spawnedId.gameObject, netMsg);
        }

        static void OnObjectSpawnFinished(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectSpawnFinishedMessage);
            if (LogFilter.logDebug) { Debug.Log("SpawnFinished:" + s_ObjectSpawnFinishedMessage.state); }

            if (s_ObjectSpawnFinishedMessage.state == 0)
            {
                PrepareToSpawnSceneObjects();
                s_IsSpawnFinished = false;
                return;
            }

            foreach (var uv in objects.Values)
            {
                if (!uv.isClient)
                {
                    uv.OnStartClient();
                    CheckForOwner(uv);
                }
            }
            s_IsSpawnFinished = true;
        }

        static void OnObjectDestroy(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectDestroyMessage);
            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnObjDestroy netId:" + s_ObjectDestroyMessage.netId); }

            NetworkIdentity localObject;
            if (s_NetworkScene.GetNetworkIdentity(s_ObjectDestroyMessage.netId, out localObject))
            {
#if UNITY_EDITOR
                Profiler.IncrementStatIncoming(MsgType.ObjectDestroy, GetStringForAssetId(localObject.assetId));
#endif
                localObject.OnNetworkDestroy();

                if (!NetworkScene.InvokeUnSpawnHandler(localObject.assetId, localObject.gameObject))
                {
                    // default handling
                    if (localObject.sceneId.IsEmpty())
                    {
                        Object.Destroy(localObject.gameObject);
                    }
                    else
                    {
                        // scene object.. disable it in scene instead of destroying
                        localObject.gameObject.SetActive(false);
                        s_SpawnableObjects[localObject.sceneId] = localObject;
                    }
                }
                s_NetworkScene.RemoveLocalObject(s_ObjectDestroyMessage.netId);
                localObject.MarkForReset();
            }
            else
            {
                if (LogFilter.logDebug) { Debug.LogWarning("Did not find target for destroy message for " + s_ObjectDestroyMessage.netId); }
            }
        }

        static void OnLocalClientObjectDestroy(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectDestroyMessage);
            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnLocalObjectObjDestroy netId:" + s_ObjectDestroyMessage.netId); }

            s_NetworkScene.RemoveLocalObject(s_ObjectDestroyMessage.netId);
        }

        static void OnLocalClientObjectHide(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectDestroyMessage);
            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnLocalObjectObjHide netId:" + s_ObjectDestroyMessage.netId); }

            NetworkIdentity localObject;
            if (s_NetworkScene.GetNetworkIdentity(s_ObjectDestroyMessage.netId, out localObject))
            {
                localObject.OnSetLocalVisibility(false);
            }
        }

        static void OnLocalClientObjectSpawn(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectSpawnMessage);
            NetworkIdentity localObject;
            if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnMessage.netId, out localObject))
            {
                localObject.OnSetLocalVisibility(true);
            }
        }

        static void OnLocalClientObjectSpawnScene(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ObjectSpawnSceneMessage);
            NetworkIdentity localObject;
            if (s_NetworkScene.GetNetworkIdentity(s_ObjectSpawnSceneMessage.netId, out localObject))
            {
                localObject.OnSetLocalVisibility(true);
            }
        }

        static void OnUpdateVarsMessage(NetworkMessage netMsg)
        {
            NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
            if (LogFilter.logDev) { Debug.Log("ClientScene::OnUpdateVarsMessage " + netId + " channel:" + netMsg.channelId); }


            NetworkIdentity localObject;
            if (s_NetworkScene.GetNetworkIdentity(netId, out localObject))
            {
                localObject.OnUpdateVars(netMsg.reader, false, netMsg);
            }
            else
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Did not find target for sync message for " + netId); }
            }
        }

        static void OnRPCMessage(NetworkMessage netMsg)
        {
            var cmdHash = (int)netMsg.reader.ReadPackedUInt32();
            var netId = netMsg.reader.ReadNetworkId();

            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnRPCMessage hash:" + cmdHash + " netId:" + netId); }

            NetworkIdentity uv;
            if (s_NetworkScene.GetNetworkIdentity(netId, out uv))
            {
                uv.HandleRPC(cmdHash, netMsg.reader);
            }
            else
            {
                if (LogFilter.logWarn)
                {
                    string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                    Debug.LogWarningFormat("Could not find target object with netId:{0} for RPC call {1}", netId, errorCmdName);
                }
            }
        }

        static void OnSyncEventMessage(NetworkMessage netMsg)
        {
            var cmdHash = (int)netMsg.reader.ReadPackedUInt32();
            var netId = netMsg.reader.ReadNetworkId();

            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnSyncEventMessage " + netId); }

            NetworkIdentity uv;
            if (s_NetworkScene.GetNetworkIdentity(netId, out uv))
            {
                uv.HandleSyncEvent(cmdHash, netMsg.reader);
            }
            else
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Did not find target for SyncEvent message for " + netId); }
            }

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.SyncEvent, NetworkBehaviour.GetCmdHashHandlerName(cmdHash));
#endif
        }

        static void OnSyncListMessage(NetworkMessage netMsg)
        {
            var netId = netMsg.reader.ReadNetworkId();
            var cmdHash = (int)netMsg.reader.ReadPackedUInt32();

            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnSyncListMessage " + netId); }

            NetworkIdentity uv;
            if (s_NetworkScene.GetNetworkIdentity(netId, out uv))
            {
                uv.HandleSyncList(cmdHash, netMsg.reader);
            }
            else
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Did not find target for SyncList message for " + netId); }
            }

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.SyncList, NetworkBehaviour.GetCmdHashHandlerName(cmdHash));
#endif
        }

        static void OnClientAuthority(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_ClientAuthorityMessage);

            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnClientAuthority for  connectionId=" + netMsg.conn.connectionId + " netId: " + s_ClientAuthorityMessage.netId); }

            NetworkIdentity uv;
            if (s_NetworkScene.GetNetworkIdentity(s_ClientAuthorityMessage.netId, out uv))
            {
                uv.HandleClientAuthority(s_ClientAuthorityMessage.authority);
            }
        }

        // OnClientAddedPlayer?
        static void OnOwnerMessage(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_OwnerMessage);

            if (LogFilter.logDebug) { Debug.Log("ClientScene::OnOwnerMessage - connectionId=" + netMsg.conn.connectionId + " netId: " + s_OwnerMessage.netId); }


            // is there already an owner that is a different object??
            PlayerController oldOwner;
            if (netMsg.conn.GetPlayerController(s_OwnerMessage.playerControllerId, out oldOwner))
            {
                oldOwner.unetView.SetNotLocalPlayer();
            }

            NetworkIdentity localNetworkIdentity;
            if (s_NetworkScene.GetNetworkIdentity(s_OwnerMessage.netId, out localNetworkIdentity))
            {
                // this object already exists
                localNetworkIdentity.SetConnectionToServer(netMsg.conn);
                localNetworkIdentity.SetLocalPlayer(s_OwnerMessage.playerControllerId);
                InternalAddPlayer(localNetworkIdentity, s_OwnerMessage.playerControllerId);
            }
            else
            {
                var pendingOwner = new PendingOwner { netId = s_OwnerMessage.netId, playerControllerId = s_OwnerMessage.playerControllerId };
                s_PendingOwnerIds.Add(pendingOwner);
            }
        }

        static void CheckForOwner(NetworkIdentity uv)
        {
            for (int i = 0; i < s_PendingOwnerIds.Count; i++)
            {
                var pendingOwner = s_PendingOwnerIds[i];

                if (pendingOwner.netId == uv.netId)
                {
                    // found owner, turn into a local player

                    // Set isLocalPlayer to true on this NetworkIdentity and trigger OnStartLocalPlayer in all scripts on the same GO
                    uv.SetConnectionToServer(s_ReadyConnection);
                    uv.SetLocalPlayer(pendingOwner.playerControllerId);

                    if (LogFilter.logDev) { Debug.Log("ClientScene::OnOwnerMessage - player=" + uv.gameObject.name); }
                    if (s_ReadyConnection.connectionId < 0)
                    {
                        if (LogFilter.logError) { Debug.LogError("Owner message received on a local client."); }
                        return;
                    }
                    InternalAddPlayer(uv, pendingOwner.playerControllerId);

                    s_PendingOwnerIds.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
