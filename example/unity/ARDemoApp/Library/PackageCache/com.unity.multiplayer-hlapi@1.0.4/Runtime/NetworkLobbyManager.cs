
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This is a specialized NetworkManager that includes a networked lobby.
    /// <para>The lobby has slots that track the joined players, and a maximum player count that is enforced. It requires that the NetworkLobbyPlayer component be on the lobby player objects.</para>
    /// <para>NetworkLobbyManager is derived from NetworkManager, and so it implements many of the virtual functions provided by the NetworkManager class. To avoid accidentally replacing functionality of the NetworkLobbyManager, there are new virtual functions on the NetworkLobbyManager that begin with "OnLobby". These should be used on classes derived from NetworkLobbyManager instead of the virtual functions on NetworkManager.</para>
    /// <para>The OnLobby*() functions have empty implementations on the NetworkLobbyManager base class, so the base class functions do not have to be called.</para>
    /// </summary>
    [AddComponentMenu("Network/NetworkLobbyManager")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkLobbyManager : NetworkManager
    {
        struct PendingPlayer
        {
            public NetworkConnection conn;
            public GameObject lobbyPlayer;
        }

        // configuration
        [SerializeField] bool m_ShowLobbyGUI = true;
        [SerializeField] int m_MaxPlayers = 4;
        [SerializeField] int m_MaxPlayersPerConnection = 1;
        [SerializeField] int m_MinPlayers;
        [SerializeField] NetworkLobbyPlayer m_LobbyPlayerPrefab;
        [SerializeField] GameObject m_GamePlayerPrefab;
        [SerializeField] string m_LobbyScene = "";
        [SerializeField] string m_PlayScene = "";

        // runtime data
        List<PendingPlayer> m_PendingPlayers = new List<PendingPlayer>();
        /// <summary>
        /// These slots track players that enter the lobby.
        /// <para>The slotId on players is global to the game - across all players.</para>
        /// </summary>
        public NetworkLobbyPlayer[] lobbySlots;

        // static message objects to avoid runtime-allocations
        static LobbyReadyToBeginMessage s_ReadyToBeginMessage = new LobbyReadyToBeginMessage();
        static IntegerMessage s_SceneLoadedMessage = new IntegerMessage();
        static LobbyReadyToBeginMessage s_LobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();

        // properties
        /// <summary>
        /// This flag enables display of the default lobby UI.
        /// <para>This is rendered using the old GUI system, so is only recommended for testing purposes.</para>
        /// </summary>
        public bool showLobbyGUI             { get { return m_ShowLobbyGUI; } set { m_ShowLobbyGUI = value; } }
        /// <summary>
        /// The maximum number of players allowed in the game.
        /// <para>Note that this is the number "players" not clients or connections. There can be multiple players per client.</para>
        /// </summary>
        public int maxPlayers                { get { return m_MaxPlayers; } set { m_MaxPlayers = value; } }
        /// <summary>
        /// The maximum number of players per connection.
        /// <para>Calling ClientScene.AddPlayer will fail if this limit is reached.</para>
        /// </summary>
        public int maxPlayersPerConnection   { get { return m_MaxPlayersPerConnection; } set { m_MaxPlayersPerConnection = value; } }
        /// <summary>
        /// The minimum number of players required to be ready for the game to start.
        /// <para>If this is zero then the game can start with any number of players.</para>
        /// </summary>
        public int minPlayers                { get { return m_MinPlayers; } set { m_MinPlayers = value; } }
        /// <summary>
        /// This is the prefab of the player to be created in the LobbyScene.
        /// <para>This prefab must have a NetworkLobbyPlayer component on it.</para>
        /// <para>In the lobby scene, this will be the active player object, but in other scenes while the game is running, this will be replaced by a player object created from the GamePlayerPrefab. But once returned to the lobby scene this will again become the active player object.</para>
        /// <para>This can be used to store user data that persists for the lifetime of the session, such as color choices or weapon choices.</para>
        /// </summary>
        public NetworkLobbyPlayer lobbyPlayerPrefab { get { return m_LobbyPlayerPrefab; } set { m_LobbyPlayerPrefab = value; } }
        /// <summary>
        /// This is the prefab of the player to be created in the PlayScene.
        /// <para>When CheckReadyToBegin starts the game from the lobby, a new player object is created from this prefab, and that object is made the active player object using NetworkServer.ReplacePlayerForConnection.</para>
        /// </summary>
        public GameObject gamePlayerPrefab   { get { return m_GamePlayerPrefab; } set { m_GamePlayerPrefab = value; } }
        /// <summary>
        /// The scene to use for the lobby. This is similar to the offlineScene of the NetworkManager.
        /// </summary>
        public string lobbyScene             { get { return m_LobbyScene; } set { m_LobbyScene = value; offlineScene = value; } }
        /// <summary>
        /// The scene to use for the playing the game from the lobby. This is similar to the onlineScene of the NetworkManager.
        /// </summary>
        public string playScene              { get { return m_PlayScene; } set { m_PlayScene = value; } }

        void OnValidate()
        {
            if (m_MaxPlayers <= 0)
            {
                m_MaxPlayers = 1;
            }

            if (m_MaxPlayersPerConnection <= 0)
            {
                m_MaxPlayersPerConnection = 1;
            }

            if (m_MaxPlayersPerConnection > maxPlayers)
            {
                m_MaxPlayersPerConnection = maxPlayers;
            }

            if (m_MinPlayers < 0)
            {
                m_MinPlayers = 0;
            }

            if (m_MinPlayers > m_MaxPlayers)
            {
                m_MinPlayers = m_MaxPlayers;
            }

            if (m_LobbyPlayerPrefab != null)
            {
                var uv = m_LobbyPlayerPrefab.GetComponent<NetworkIdentity>();
                if (uv == null)
                {
                    m_LobbyPlayerPrefab = null;
                    Debug.LogWarning("LobbyPlayer prefab must have a NetworkIdentity component.");
                }
            }

            if (m_GamePlayerPrefab != null)
            {
                var uv = m_GamePlayerPrefab.GetComponent<NetworkIdentity>();
                if (uv == null)
                {
                    m_GamePlayerPrefab = null;
                    Debug.LogWarning("GamePlayer prefab must have a NetworkIdentity component.");
                }
            }
        }

        Byte FindSlot()
        {
            for (byte i = 0; i < maxPlayers; i++)
            {
                if (lobbySlots[i] == null)
                {
                    return i;
                }
            }
            return Byte.MaxValue;
        }

        void SceneLoadedForPlayer(NetworkConnection conn, GameObject lobbyPlayerGameObject)
        {
            var lobbyPlayer = lobbyPlayerGameObject.GetComponent<NetworkLobbyPlayer>();
            if (lobbyPlayer == null)
            {
                // not a lobby player.. dont replace it
                return;
            }

            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (LogFilter.logDebug) { Debug.Log("NetworkLobby SceneLoadedForPlayer scene:" + loadedSceneName + " " + conn); }

            if (loadedSceneName == m_LobbyScene)
            {
                // cant be ready in lobby, add to ready list
                PendingPlayer pending;
                pending.conn = conn;
                pending.lobbyPlayer = lobbyPlayerGameObject;
                m_PendingPlayers.Add(pending);
                return;
            }

            var controllerId = lobbyPlayerGameObject.GetComponent<NetworkIdentity>().playerControllerId;
            var gamePlayer = OnLobbyServerCreateGamePlayer(conn, controllerId);
            if (gamePlayer == null)
            {
                // get start position from base class
                Transform startPos = GetStartPosition();
                if (startPos != null)
                {
                    gamePlayer = (GameObject)Instantiate(gamePlayerPrefab, startPos.position, startPos.rotation);
                }
                else
                {
                    gamePlayer = (GameObject)Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity);
                }
            }

            if (!OnLobbyServerSceneLoadedForPlayer(lobbyPlayerGameObject, gamePlayer))
            {
                return;
            }

            // replace lobby player with game player
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, controllerId);
        }

        static int CheckConnectionIsReadyToBegin(NetworkConnection conn)
        {
            int countPlayers = 0;
            for (int i = 0; i < conn.playerControllers.Count; i++)
            {
                var player = conn.playerControllers[i];
                if (player.IsValid)
                {
                    var lobbyPlayer = player.gameObject.GetComponent<NetworkLobbyPlayer>();
                    if (lobbyPlayer.readyToBegin)
                    {
                        countPlayers += 1;
                    }
                }
            }
            return countPlayers;
        }

        /// <summary>
        /// CheckReadyToBegin checks all of the players in the lobby to see if their readyToBegin flag is set.
        /// <para>If all of the players are ready, then the server switches from the LobbyScene to the PlayScene - essentially starting the game. This is called automatically in response to NetworkLobbyPlayer.SendReadyToBeginMessage().</para>
        /// </summary>
        public void CheckReadyToBegin()
        {
            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (loadedSceneName != m_LobbyScene)
            {
                return;
            }

            int readyCount = 0;
            int playerCount = 0;

            for (int i = 0; i < NetworkServer.connections.Count; i++)
            {
                var conn = NetworkServer.connections[i];

                if (conn == null)
                    continue;

                playerCount += 1;
                readyCount += CheckConnectionIsReadyToBegin(conn);
            }
            if (m_MinPlayers > 0 && readyCount < m_MinPlayers)
            {
                // not enough players ready yet.
                return;
            }

            if (readyCount < playerCount)
            {
                // not all players are ready yet
                return;
            }

            m_PendingPlayers.Clear();
            OnLobbyServerPlayersReady();
        }

        /// <summary>
        /// Calling this causes the server to switch back to the lobby scene.
        /// </summary>
        public void ServerReturnToLobby()
        {
            if (!NetworkServer.active)
            {
                Debug.Log("ServerReturnToLobby called on client");
                return;
            }
            ServerChangeScene(m_LobbyScene);
        }

        void CallOnClientEnterLobby()
        {
            OnLobbyClientEnter();
            for (int i = 0; i < lobbySlots.Length; i++)
            {
                var player = lobbySlots[i];
                if (player == null)
                    continue;

                player.readyToBegin = false;
                player.OnClientEnterLobby();
            }
        }

        void CallOnClientExitLobby()
        {
            OnLobbyClientExit();
            for (int i = 0; i < lobbySlots.Length; i++)
            {
                var player = lobbySlots[i];
                if (player == null)
                    continue;

                player.OnClientExitLobby();
            }
        }

        /// <summary>
        /// Sends a message to the server to make the game return to the lobby scene.
        /// </summary>
        /// <returns>True if message was sent.</returns>
        public bool SendReturnToLobby()
        {
            if (client == null || !client.isConnected)
            {
                return false;
            }

            var msg = new EmptyMessage();
            client.Send(MsgType.LobbyReturnToLobby, msg);
            return true;
        }

        // ------------------------ server handlers ------------------------

        public override void OnServerConnect(NetworkConnection conn)
        {
            // numPlayers returns the player count including this one, so ok to be equal
            if (numPlayers > maxPlayers)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkLobbyManager can't accept new connection [" + conn + "], too many players connected."); }
                conn.Disconnect();
                return;
            }

            // cannot join game in progress
            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (loadedSceneName != m_LobbyScene)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkLobbyManager can't accept new connection [" + conn + "], not in lobby and game already in progress."); }
                conn.Disconnect();
                return;
            }

            base.OnServerConnect(conn);

            // when a new client connects, set all old players as dirty so their current ready state is sent out
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i])
                {
                    lobbySlots[i].SetDirtyBit(1);
                }
            }

            OnLobbyServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            // if lobbyplayer for this connection has not been destroyed by now, then destroy it here
            for (int i = 0; i < lobbySlots.Length; i++)
            {
                var player = lobbySlots[i];
                if (player == null)
                    continue;

                if (player.connectionToClient == conn)
                {
                    lobbySlots[i] = null;
                    NetworkServer.Destroy(player.gameObject);
                }
            }

            OnLobbyServerDisconnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (loadedSceneName != m_LobbyScene)
            {
                return;
            }

            // check MaxPlayersPerConnection
            int numPlayersForConnection = 0;
            for (int i = 0; i < conn.playerControllers.Count; i++)
            {
                if (conn.playerControllers[i].IsValid)
                    numPlayersForConnection += 1;
            }

            if (numPlayersForConnection >= maxPlayersPerConnection)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkLobbyManager no more players for this connection."); }

                var errorMsg = new EmptyMessage();
                conn.Send(MsgType.LobbyAddPlayerFailed, errorMsg);
                return;
            }

            byte slot = FindSlot();
            if (slot == Byte.MaxValue)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkLobbyManager no space for more players"); }

                var errorMsg = new EmptyMessage();
                conn.Send(MsgType.LobbyAddPlayerFailed, errorMsg);
                return;
            }

            var newLobbyGameObject = OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
            if (newLobbyGameObject == null)
            {
                newLobbyGameObject = (GameObject)Instantiate(lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
            }

            var newLobbyPlayer = newLobbyGameObject.GetComponent<NetworkLobbyPlayer>();
            newLobbyPlayer.slot = slot;
            lobbySlots[slot] = newLobbyPlayer;

            NetworkServer.AddPlayerForConnection(conn, newLobbyGameObject, playerControllerId);
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            var playerControllerId = player.playerControllerId;
            byte slot = player.gameObject.GetComponent<NetworkLobbyPlayer>().slot;
            lobbySlots[slot] = null;
            base.OnServerRemovePlayer(conn, player);

            for (int i = 0; i < lobbySlots.Length; i++)
            {
                var lobbyPlayer = lobbySlots[i];
                if (lobbyPlayer != null)
                {
                    lobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;

                    s_LobbyReadyToBeginMessage.slotId = lobbyPlayer.slot;
                    s_LobbyReadyToBeginMessage.readyState = false;
                    NetworkServer.SendToReady(null, MsgType.LobbyReadyToBegin, s_LobbyReadyToBeginMessage);
                }
            }

            OnLobbyServerPlayerRemoved(conn, playerControllerId);
        }

        public override void ServerChangeScene(string sceneName)
        {
            if (sceneName == m_LobbyScene)
            {
                for (int i = 0; i < lobbySlots.Length; i++)
                {
                    var lobbyPlayer = lobbySlots[i];
                    if (lobbyPlayer == null)
                        continue;

                    // find the game-player object for this connection, and destroy it
                    var uv = lobbyPlayer.GetComponent<NetworkIdentity>();

                    PlayerController playerController;
                    if (uv.connectionToClient.GetPlayerController(uv.playerControllerId, out playerController))
                    {
                        NetworkServer.Destroy(playerController.gameObject);
                    }

                    if (NetworkServer.active)
                    {
                        // re-add the lobby object
                        lobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
                        NetworkServer.ReplacePlayerForConnection(uv.connectionToClient, lobbyPlayer.gameObject, uv.playerControllerId);
                    }
                }
            }
            base.ServerChangeScene(sceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName != m_LobbyScene)
            {
                // call SceneLoadedForPlayer on any players that become ready while we were loading the scene.
                for (int i = 0; i < m_PendingPlayers.Count; i++)
                {
                    var pending = m_PendingPlayers[i];
                    SceneLoadedForPlayer(pending.conn, pending.lobbyPlayer);
                }
                m_PendingPlayers.Clear();
            }

            OnLobbyServerSceneChanged(sceneName);
        }

        void OnServerReadyToBeginMessage(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager OnServerReadyToBeginMessage"); }
            netMsg.ReadMessage(s_ReadyToBeginMessage);

            PlayerController lobbyController;
            if (!netMsg.conn.GetPlayerController((short)s_ReadyToBeginMessage.slotId, out lobbyController))
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager OnServerReadyToBeginMessage invalid playerControllerId " + s_ReadyToBeginMessage.slotId); }
                return;
            }

            // set this player ready
            var lobbyPlayer = lobbyController.gameObject.GetComponent<NetworkLobbyPlayer>();
            lobbyPlayer.readyToBegin = s_ReadyToBeginMessage.readyState;

            // tell every player that this player is ready
            var outMsg = new LobbyReadyToBeginMessage();
            outMsg.slotId = lobbyPlayer.slot;
            outMsg.readyState = s_ReadyToBeginMessage.readyState;
            NetworkServer.SendToReady(null, MsgType.LobbyReadyToBegin, outMsg);

            // maybe start the game
            CheckReadyToBegin();
        }

        void OnServerSceneLoadedMessage(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager OnSceneLoadedMessage"); }

            netMsg.ReadMessage(s_SceneLoadedMessage);

            PlayerController lobbyController;
            if (!netMsg.conn.GetPlayerController((short)s_SceneLoadedMessage.value, out lobbyController))
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager OnServerSceneLoadedMessage invalid playerControllerId " + s_SceneLoadedMessage.value); }
                return;
            }

            SceneLoadedForPlayer(netMsg.conn, lobbyController.gameObject);
        }

        void OnServerReturnToLobbyMessage(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager OnServerReturnToLobbyMessage"); }

            ServerReturnToLobby();
        }

        public override void OnStartServer()
        {
            if (string.IsNullOrEmpty(m_LobbyScene))
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager LobbyScene is empty. Set the LobbyScene in the inspector for the NetworkLobbyMangaer"); }
                return;
            }

            if (string.IsNullOrEmpty(m_PlayScene))
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager PlayScene is empty. Set the PlayScene in the inspector for the NetworkLobbyMangaer"); }
                return;
            }

            if (lobbySlots.Length == 0)
            {
                lobbySlots = new NetworkLobbyPlayer[maxPlayers];
            }

            NetworkServer.RegisterHandler(MsgType.LobbyReadyToBegin, OnServerReadyToBeginMessage);
            NetworkServer.RegisterHandler(MsgType.LobbySceneLoaded, OnServerSceneLoadedMessage);
            NetworkServer.RegisterHandler(MsgType.LobbyReturnToLobby, OnServerReturnToLobbyMessage);

            OnLobbyStartServer();
        }

        public override void OnStartHost()
        {
            OnLobbyStartHost();
        }

        public override void OnStopHost()
        {
            OnLobbyStopHost();
        }

        // ------------------------ client handlers ------------------------

        public override void OnStartClient(NetworkClient lobbyClient)
        {
            if (lobbySlots.Length == 0)
            {
                lobbySlots = new NetworkLobbyPlayer[maxPlayers];
            }

            if (m_LobbyPlayerPrefab == null || m_LobbyPlayerPrefab.gameObject == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager no LobbyPlayer prefab is registered. Please add a LobbyPlayer prefab."); }
            }
            else
            {
                ClientScene.RegisterPrefab(m_LobbyPlayerPrefab.gameObject);
            }

            if (m_GamePlayerPrefab == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager no GamePlayer prefab is registered. Please add a GamePlayer prefab."); }
            }
            else
            {
                ClientScene.RegisterPrefab(m_GamePlayerPrefab);
            }

            lobbyClient.RegisterHandler(MsgType.LobbyReadyToBegin, OnClientReadyToBegin);
            lobbyClient.RegisterHandler(MsgType.LobbyAddPlayerFailed, OnClientAddPlayerFailedMessage);

            OnLobbyStartClient(lobbyClient);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            OnLobbyClientConnect(conn);
            CallOnClientEnterLobby();
            base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            OnLobbyClientDisconnect(conn);
            base.OnClientDisconnect(conn);
        }

        public override void OnStopClient()
        {
            OnLobbyStopClient();
            CallOnClientExitLobby();
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (loadedSceneName == m_LobbyScene)
            {
                if (client.isConnected)
                {
                    CallOnClientEnterLobby();
                }
            }
            else
            {
                CallOnClientExitLobby();
            }

            base.OnClientSceneChanged(conn);
            OnLobbyClientSceneChanged(conn);
        }

        void OnClientReadyToBegin(NetworkMessage netMsg)
        {
            netMsg.ReadMessage(s_LobbyReadyToBeginMessage);

            if (s_LobbyReadyToBeginMessage.slotId >= lobbySlots.Count())
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager OnClientReadyToBegin invalid lobby slot " + s_LobbyReadyToBeginMessage.slotId); }
                return;
            }

            var lobbyPlayer = lobbySlots[s_LobbyReadyToBeginMessage.slotId];
            if (lobbyPlayer == null || lobbyPlayer.gameObject == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkLobbyManager OnClientReadyToBegin no player at lobby slot " + s_LobbyReadyToBeginMessage.slotId); }
                return;
            }

            lobbyPlayer.readyToBegin = s_LobbyReadyToBeginMessage.readyState;
            lobbyPlayer.OnClientReady(s_LobbyReadyToBeginMessage.readyState);
        }

        void OnClientAddPlayerFailedMessage(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager Add Player failed."); }
            OnLobbyClientAddPlayerFailed();
        }

        // ------------------------ lobby server virtuals ------------------------

        /// <summary>
        /// This is called on the host when a host is started.
        /// <code>
        /// //This script shows you how to add extra functionality when the lobby host starts and stops
        /// //Add this script to your GameObject. Make sure there isn&apos;t another NetworkManager in the Scene.
        /// //Create a Host Button (<b>Create&gt;UI&gt;Text</b>) and assign it in the Inspector of the GameObject this script is attached to
        /// //Create a Text GameObject (<b>Create&gt;UI&gt;Text</b>) and attach it to the Status Text field in the Inspector.
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        ///
        /// public class Example : NetworkLobbyManager
        /// {
        ///    public Button m_HostButton;
        ///    public Text m_StatusText;
        ///    bool m_HostStarted;
        ///
        ///    void Start()
        ///    {
        ///        //Set the minimum and maximum number of players
        ///        maxPlayers = 6;
        ///        minPlayers = 2;
        ///        maxPlayersPerConnection = 1;
        ///        //Call these functions when each Button is clicked
        ///        m_HostButton.onClick.AddListener(HostButton);
        ///        m_StatusText.text = "Current Scene : " + lobbyScene;
        ///    }
        ///
        ///    //Output a message when the host joins the lobby
        ///    public override void OnLobbyStartHost()
        ///    {
        ///        //Change the Text to show this message
        ///        m_StatusText.text = "A Host has joined the lobby!";
        ///        m_HostStarted = true;
        ///        //Do the default actions for when the host starts in the lobby
        ///        base.OnLobbyStartHost();
        ///    }
        ///
        ///    // Output a message to the host when the host stops
        ///    public override void OnLobbyStopHost()
        ///    {
        ///        //Output this message when the host stops
        ///        m_StatusText.text = "A Host has left the lobby!";
        ///        //Do the default actions when the host stops
        ///        base.OnLobbyStopHost();
        ///        m_HostStarted = false;
        ///    }
        ///
        ///    /// This is where the Buttons are given functionality
        ///    //Start the host when this Button is pressed
        ///    public void HostButton()
        ///    {
        ///        //Check if the host has already started
        ///        if (m_HostStarted == false)
        ///        {
        ///            //Start the host
        ///            StartHost();
        ///            //Change the Button's Text
        ///            m_HostButton.GetComponentInChildren&lt;Text&gt;().text = "Stop Host";
        ///        }
        ///        else
        ///        {
        ///            //If the host has already started, stop the host
        ///            StopHost();
        ///            //Change the Button's Text
        ///            m_HostButton.GetComponentInChildren&lt;Text&gt;().text = "Start Host";
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        public virtual void OnLobbyStartHost()
        {
        }

        /// <summary>
        /// This is called on the host when the host is stopped.
        /// </summary>
        public virtual void OnLobbyStopHost()
        {
        }

        /// <summary>
        /// This is called on the server when the server is started - including when a host is started.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        ///
        /// public class Example : NetworkLobbyManager
        /// {
        ///    //Add this script to your GameObject. Make sure there isn&apos;t another NetworkManager in the Scene.
        ///    //Create 2 Buttons (<b>Create&gt;UI&gt;Text</b>) and either:
        ///    //1. assign them in the Inspector of the GameObject this script is attached to or
        ///    //2. remove this part and the listeners and alter the OnClick section on each Button to match up with each function
        ///    //Create a Text GameObject (<b>Create&gt;UI&gt;Text</b>) and attach it to the Status Text field in the Inspector.
        ///
        ///    public Button m_ClientButton, m_ServerButton;
        ///    bool m_ServerStarted, m_ClientStarted;
        ///
        ///    void Start()
        ///    {
        ///        showLobbyGUI = true;
        ///        //Call these functions when each Button is clicked
        ///        m_ServerButton.onClick.AddListener(ServerButton);
        ///        m_ClientButton.onClick.AddListener(ClientButton);
        ///    }
        ///
        ///    //Output a message when your client enters the lobby
        ///    public override void OnLobbyClientEnter()
        ///    {
        ///        m_ClientStarted = true;
        ///        base.OnLobbyClientEnter();
        ///        Debug.Log("Your client has entered the lobby!");
        ///    }
        ///
        ///    public override void OnLobbyStopClient()
        ///    {
        ///        Debug.Log("Client stopped");
        ///        base.OnLobbyStopClient();
        ///    }
        ///
        ///    public override void OnLobbyStartServer()
        ///    {
        ///        m_ServerStarted = true;
        ///        base.OnLobbyStartServer();
        ///        Debug.Log("Server Started!");
        ///    }
        ///
        ///    public override void OnStopServer()
        ///    {
        ///        m_ServerStarted = false;
        ///        base.OnStopServer();
        ///        Debug.Log("Server Stopped!");
        ///    }
        ///
        ///    //Start the Client when this Button is pressed
        ///    public void ClientButton()
        ///    {
        ///        if (m_ClientStarted == false)
        ///        {
        ///            StartClient();
        ///            m_ClientButton.GetComponentInChildren&lt;Text&gt;().text = "Stop Client";
        ///        }
        ///        else
        ///        {
        ///            StopClient();
        ///            m_ClientButton.GetComponentInChildren&lt;Text&gt;().text = "Start Client";
        ///        }
        ///    }
        ///
        ///    //Start the Server when this Button is pressed
        ///    public void ServerButton()
        ///    {
        ///        Debug.Log("Server : " + m_ServerStarted);
        ///        if (m_ServerStarted == false)
        ///        {
        ///            StartServer();
        ///            m_ServerButton.GetComponentInChildren&lt;Text&gt;().text = "Stop Server";
        ///        }
        ///        else
        ///        {
        ///            StopServer();
        ///            ServerReturnToLobby();
        ///            m_ServerButton.GetComponentInChildren&lt;Text&gt;().text = "Start Server";
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        public virtual void OnLobbyStartServer()
        {
        }

        /// <summary>
        /// This is called on the server when a new client connects to the server.
        /// </summary>
        /// <param name="conn">The new connection.</param>
        public virtual void OnLobbyServerConnect(NetworkConnection conn)
        {
        }

        /// <summary>
        /// This is called on the server when a client disconnects.
        /// </summary>
        /// <param name="conn">The connection that disconnected.</param>
        public virtual void OnLobbyServerDisconnect(NetworkConnection conn)
        {
        }

        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public virtual void OnLobbyServerSceneChanged(string sceneName)
        {
        }

        /// <summary>
        /// This allows customization of the creation of the lobby-player object on the server.
        /// <para>By default the lobbyPlayerPrefab is used to create the lobby-player, but this function allows that behaviour to be customized.</para>
        /// </summary>
        /// <param name="conn">The connection the player object is for.</param>
        /// <param name="playerControllerId">The controllerId of the player.</param>
        /// <returns>The new lobby-player object.</returns>
        public virtual GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            return null;
        }

        /// <summary>
        /// This allows customization of the creation of the GamePlayer object on the server.
        /// <para>By default the gamePlayerPrefab is used to create the game-player, but this function allows that behaviour to be customized. The object returned from the function will be used to replace the lobby-player on the connection.</para>
        /// </summary>
        /// <param name="conn">The connection the player object is for.</param>
        /// <param name="playerControllerId">The controllerId of the player on the connnection.</param>
        /// <returns>A new GamePlayer object.</returns>
        public virtual GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            return null;
        }

        /// <summary>
        /// This is called on the server when a player is removed.
        /// </summary>
        /// <param name="conn">The connection the player object is for.</param>
        /// <param name="playerControllerId">The controllerId of the player that was removed.</param>
        public virtual void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
        }

        /// <summary>
        /// This is called on the server when it is told that a client has finished switching from the lobby scene to a game player scene.
        /// <para>When switching from the lobby, the lobby-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the lobby-player to the game-player object.</para>
        /// </summary>
        /// <param name="lobbyPlayer">The lobby player object.</param>
        /// <param name="gamePlayer">The game player object.</param>
        /// <returns>False to not allow this player to replace the lobby player.</returns>
        // for users to apply settings from their lobby player object to their in-game player object
        public virtual bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            return true;
        }

        /// <summary>
        /// This is called on the server when all the players in the lobby are ready.
        /// <para>The default implementation of this function uses ServerChangeScene() to switch to the game player scene. By implementing this callback you can customize what happens when all the players in the lobby are ready, such as adding a countdown or a confirmation for a group leader.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class GuiLobby : NetworkLobbyManager
        /// {
        ///    float countTimer = 0;
        ///
        ///    public override void OnLobbyServerPlayersReady()
        ///    {
        ///        countTimer = Time.time + 5;
        ///    }
        ///
        ///    void Update()
        ///    {
        ///        if (countTimer == 0)
        ///            return;
        ///        if (Time.time > countTimer)
        ///        {
        ///            countTimer = 0;
        ///            ServerChangeScene(playScene);
        ///        }
        ///        else
        ///        {
        ///            Debug.Log("Counting down " + (countTimer - Time.time));
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        public virtual void OnLobbyServerPlayersReady()
        {
            // all players are readyToBegin, start the game
            ServerChangeScene(m_PlayScene);
        }

        // ------------------------ lobby client virtuals ------------------------

        /// <summary>
        /// This is a hook to allow custom behaviour when the game client enters the lobby.
        /// </summary>
        public virtual void OnLobbyClientEnter()
        {
        }

        /// <summary>
        /// This is a hook to allow custom behaviour when the game client exits the lobby.
        /// </summary>
        public virtual void OnLobbyClientExit()
        {
        }

        /// <summary>
        /// This is called on the client when it connects to server.
        /// </summary>
        /// <param name="conn">The connection that connected.</param>
        public virtual void OnLobbyClientConnect(NetworkConnection conn)
        {
        }

        /// <summary>
        /// This is called on the client when disconnected from a server.
        /// </summary>
        /// <param name="conn">The connection that disconnected.</param>
        public virtual void OnLobbyClientDisconnect(NetworkConnection conn)
        {
        }

        /// <summary>
        /// This is called on the client when a client is started.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        ///
        /// public class Example : NetworkLobbyManager
        /// {
        ///    //Add this script to your GameObject. Make sure there isn&apos;t another NetworkManager in the Scene.
        ///    //Create 2 Buttons (<b>Create&gt;UI&gt;Text</b>) and either:
        ///    //1. assign them in the Inspector of the GameObject this script is attached to or
        ///    //2. remove this part and the listeners and alter the OnClick section on each Button to match up with each function
        ///    //Create a Text GameObject (<b>Create&gt;UI&gt;Text</b>) and attach it to the Status Text field in the Inspector.
        ///
        ///    public Button m_ClientButton, m_ServerButton;
        ///    bool m_ServerStarted, m_ClientStarted;
        ///
        ///    void Start()
        ///    {
        ///        showLobbyGUI = true;
        ///        //Call these functions when each Button is clicked
        ///        m_ServerButton.onClick.AddListener(ServerButton);
        ///        m_ClientButton.onClick.AddListener(ClientButton);
        ///    }
        ///
        ///    //Output a message when your client enters the lobby
        ///    public override void OnLobbyClientEnter()
        ///    {
        ///        m_ClientStarted = true;
        ///        base.OnLobbyClientEnter();
        ///        Debug.Log("Your client has entered the lobby!");
        ///    }
        ///
        ///    public override void OnLobbyStopClient()
        ///    {
        ///        Debug.Log("Client stopped");
        ///        base.OnLobbyStopClient();
        ///    }
        ///
        ///    public override void OnLobbyStartServer()
        ///    {
        ///        m_ServerStarted = true;
        ///        base.OnLobbyStartServer();
        ///        Debug.Log("Server Started!");
        ///    }
        ///
        ///    public override void OnStopServer()
        ///    {
        ///        m_ServerStarted = false;
        ///        base.OnStopServer();
        ///        Debug.Log("Server Stopped!");
        ///    }
        ///
        ///    //Start the Client when this Button is pressed
        ///    public void ClientButton()
        ///    {
        ///        if (m_ClientStarted == false)
        ///        {
        ///            StartClient();
        ///            m_ClientButton.GetComponentInChildren&lt;Text&gt;().text = "Stop Client";
        ///        }
        ///        else
        ///        {
        ///            StopClient();
        ///            m_ClientButton.GetComponentInChildren&lt;Text&gt;().text = "Start Client";
        ///        }
        ///    }
        ///
        ///    //Start the Server when this Button is pressed
        ///    public void ServerButton()
        ///    {
        ///        Debug.Log("Server : " + m_ServerStarted);
        ///        if (m_ServerStarted == false)
        ///        {
        ///            StartServer();
        ///            m_ServerButton.GetComponentInChildren&lt;Text&gt;().text = "Stop Server";
        ///        }
        ///        else
        ///        {
        ///            StopServer();
        ///            ServerReturnToLobby();
        ///            m_ServerButton.GetComponentInChildren&lt;Text&gt;().text = "Start Server";
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="lobbyClient">The connection for the lobby.</param>
        public virtual void OnLobbyStartClient(NetworkClient lobbyClient)
        {
        }

        /// <summary>
        /// This is called on the client when the client stops.
        /// </summary>
        public virtual void OnLobbyStopClient()
        {
        }

        /// <summary>
        /// This is called on the client when the client is finished loading a new networked scene.
        /// </summary>
        /// <param name="conn">The connection that finished loading a new networked scene.</param>
        public virtual void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
        }

        /// <summary>
        /// Called on the client when adding a player to the lobby fails.
        /// <para>This could be because the lobby is full, or the connection is not allowed to have more players.</para>
        /// </summary>
        // for users to handle adding a player failed on the server
        public virtual void OnLobbyClientAddPlayerFailed()
        {
        }

        // ------------------------ optional UI ------------------------

        void OnGUI()
        {
            if (!showLobbyGUI)
                return;

            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (loadedSceneName != m_LobbyScene)
                return;

            Rect backgroundRec = new Rect(90 , 180, 500, 150);
            GUI.Box(backgroundRec, "Players:");

            if (NetworkClient.active)
            {
                Rect addRec = new Rect(100, 300, 120, 20);
                if (GUI.Button(addRec, "Add Player"))
                {
                    TryToAddPlayer();
                }
            }
        }

        /// <summary>
        /// This is used on clients to attempt to add a player to the game.
        /// <para>This may fail if the game is full or the connection cannot have more players.</para>
        /// </summary>
        public void TryToAddPlayer()
        {
            if (NetworkClient.active)
            {
                short controllerId = -1;
                var controllers = NetworkClient.allClients[0].connection.playerControllers;

                if (controllers.Count < maxPlayers)
                {
                    controllerId = (short)controllers.Count;
                }
                else
                {
                    for (short i = 0; i < maxPlayers; i++)
                    {
                        if (!controllers[i].IsValid)
                        {
                            controllerId = i;
                            break;
                        }
                    }
                }
                if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager TryToAddPlayer controllerId " + controllerId + " ready:" + ClientScene.ready); }

                if (controllerId == -1)
                {
                    if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager No Space!"); }
                    return;
                }

                if (ClientScene.ready)
                {
                    ClientScene.AddPlayer(controllerId);
                }
                else
                {
                    ClientScene.AddPlayer(NetworkClient.allClients[0].connection, controllerId);
                }
            }
            else
            {
                if (LogFilter.logDebug) { Debug.Log("NetworkLobbyManager NetworkClient not active!"); }
            }
        }
    }
}
