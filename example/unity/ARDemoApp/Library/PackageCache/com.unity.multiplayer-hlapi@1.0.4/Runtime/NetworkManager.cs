using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
    /// <summary>
    /// Enumeration of methods of where to spawn player objects in multiplayer games.
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class PlayerSpawnMethodExample : MonoBehaviour
    /// {
    ///    void Update()
    ///    {
    ///        //Press the space key to switch to spawning on a random spawn point
    ///        if (Input.GetKeyDown(KeyCode.Space))
    ///        {
    ///            //Check that the PlayerSpawnMethod is currently RoundRobin
    ///            if (NetworkManager.singleton.playerSpawnMethod == PlayerSpawnMethod.RoundRobin)
    ///                //Switch it to Random spawning if it is
    ///                NetworkManager.singleton.playerSpawnMethod = PlayerSpawnMethod.Random;
    ///            //Otherwise switch it to RoundRobin
    ///            else NetworkManager.singleton.playerSpawnMethod = PlayerSpawnMethod.RoundRobin;
    ///        }
    ///    }
    /// }
    /// </code>
    /// </summary>
    public enum PlayerSpawnMethod
    {
        Random,
        RoundRobin
    };

    /// <summary>
    /// The NetworkManager is a convenience class for the HLAPI for managing networking systems.
    /// <para>For simple network applications the NetworkManager can be used to control the HLAPI. It provides simple ways to start and stop client and servers, to manage scenes, and has virtual functions that user code can use to implement handlers for network events. The NetworkManager deals with one client at a time. The example below shows a minimal network setup.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Manager : NetworkManager
    /// {
    ///    public override void OnServerConnect(NetworkConnection conn)
    ///    {
    ///        Debug.Log("OnPlayerConnected");
    ///    }
    /// }
    /// </code>
    /// </summary>
    [AddComponentMenu("Network/NetworkManager")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkManager : MonoBehaviour
    {
        // configuration
        [SerializeField] int m_NetworkPort = 7777;
        [SerializeField] bool m_ServerBindToIP;
        [SerializeField] string m_ServerBindAddress = "";
        [SerializeField] string m_NetworkAddress = "localhost";
        [SerializeField] bool m_DontDestroyOnLoad = true;
        [SerializeField] bool m_RunInBackground = true;
        [SerializeField] bool m_ScriptCRCCheck = true;
        [SerializeField] float m_MaxDelay = 0.01f;
        [SerializeField] LogFilter.FilterLevel m_LogLevel = (LogFilter.FilterLevel)LogFilter.Info;
        [SerializeField] GameObject m_PlayerPrefab;
        [SerializeField] bool m_AutoCreatePlayer = true;
        [SerializeField] PlayerSpawnMethod m_PlayerSpawnMethod;
        [SerializeField] string m_OfflineScene = "";
        [SerializeField] string m_OnlineScene = "";
        [SerializeField] List<GameObject> m_SpawnPrefabs = new List<GameObject>();

        [SerializeField] bool m_CustomConfig;
        [SerializeField] int m_MaxConnections = 4;
        [SerializeField] ConnectionConfig m_ConnectionConfig;
        [SerializeField] GlobalConfig m_GlobalConfig;
        [SerializeField] List<QosType> m_Channels = new List<QosType>();

        [SerializeField] bool m_UseWebSockets;
        [SerializeField] bool m_UseSimulator;
        [SerializeField] int m_SimulatedLatency = 1;
        [SerializeField] float m_PacketLossPercentage;

        [SerializeField] int m_MaxBufferedPackets = ChannelBuffer.MaxPendingPacketCount;
        [SerializeField] bool m_AllowFragmentation = true;

        // matchmaking configuration
        [SerializeField] string m_MatchHost = "mm.unet.unity3d.com";
        [SerializeField] int m_MatchPort = 443;
        /// <summary>
        /// The name of the current match.
        /// <para>A text string indicating the name of the current match in progress.</para>
        /// </summary>
        [SerializeField] public string matchName = "default";
        /// <summary>
        /// The maximum number of players in the current match.
        /// </summary>
        [SerializeField] public uint matchSize = 4;


        NetworkMigrationManager m_MigrationManager;

        private EndPoint m_EndPoint;
        bool m_ClientLoadedScene;

        static INetworkTransport s_ActiveTransport = new DefaultNetworkTransport();

        // properties
        /// <summary>
        /// The network port currently in use.
        /// <para>For clients, this is the port of the server connected to. For servers, this is the listen port.</para>
        /// </summary>
        public int networkPort               { get { return m_NetworkPort; } set { m_NetworkPort = value; } }
        /// <summary>
        /// Flag to tell the server whether to bind to a specific IP address.
        /// <para>If this is false, then no specific IP address is bound to (IP_ANY).</para>
        /// </summary>
        public bool serverBindToIP           { get { return m_ServerBindToIP; } set { m_ServerBindToIP = value; }}
        /// <summary>
        /// The IP address to bind the server to.
        /// <para>This is only used if serverBindToIP is set to true.</para>
        /// </summary>
        public string serverBindAddress  { get { return m_ServerBindAddress; } set { m_ServerBindAddress = value; }}
        /// <summary>
        /// The network address currently in use.
        /// <para>For clients, this is the address of the server that is connected to. For servers, this is the local address.</para>
        /// </summary>
        public string networkAddress         { get { return m_NetworkAddress; }  set { m_NetworkAddress = value; } }
        /// <summary>
        /// A flag to control whether the NetworkManager object is destroyed when the scene changes.
        /// <para>This should be set if your game has a single NetworkManager that exists for the lifetime of the process. If there is a NetworkManager in each scene, then this should not be set.</para>
        /// </summary>
        public bool dontDestroyOnLoad        { get { return m_DontDestroyOnLoad; }  set { m_DontDestroyOnLoad = value; } }
        /// <summary>
        /// Controls whether the program runs when it is in the background.
        /// <para>This is required when multiple instances of a program using networking are running on the same machine, such as when testing using localhost. But this is not recommended when deploying to mobile platforms.</para>
        /// </summary>
        public bool runInBackground          { get { return m_RunInBackground; }  set { m_RunInBackground = value; } }
        /// <summary>
        /// Flag for using the script CRC check between server and clients.
        /// <para>Enables a CRC check between server and client that ensures the NetworkBehaviour scripts match. This may not be appropriate in some cases, such a when the client and server are different Unity projects.</para>
        /// </summary>
        public bool scriptCRCCheck           { get { return m_ScriptCRCCheck; } set { m_ScriptCRCCheck = value;  }}

        [Obsolete("moved to NetworkMigrationManager")]
        public bool sendPeerInfo             { get { return false; } set {} }

        /// <summary>
        /// The maximum delay before sending packets on connections.
        /// <para>In seconds. The default of 0.01 seconds means packets will be delayed at most by 10 milliseconds. Setting this to zero will disable HLAPI connection buffering.</para>
        /// </summary>
        public float maxDelay                { get { return m_MaxDelay; }  set { m_MaxDelay = value; } }
        /// <summary>
        /// The log level specifically to user for network log messages.
        /// </summary>
        public LogFilter.FilterLevel logLevel { get { return m_LogLevel; }  set { m_LogLevel = value; LogFilter.currentLogLevel = (int)value; } }
        /// <summary>
        /// The default prefab to be used to create player objects on the server.
        /// <para>Player objects are created in the default handler for AddPlayer() on the server. Implementing OnServerAddPlayer overrides this behaviour.</para>
        /// </summary>
        public GameObject playerPrefab       { get { return m_PlayerPrefab; }  set { m_PlayerPrefab = value; } }
        /// <summary>
        /// A flag to control whether or not player objects are automatically created on connect, and on scene change.
        /// </summary>
        public bool autoCreatePlayer         { get { return m_AutoCreatePlayer; } set { m_AutoCreatePlayer = value; } }
        /// <summary>
        /// The current method of spawning players used by the NetworkManager.
        /// <code>
        /// //Attach this script to a GameObject
        /// //This script switches the Player spawn method between Round Robin spawning and Random spawning when you press the space key in Play Mode.
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : NetworkManager
        /// {
        ///    void Start()
        ///    {
        ///        //Change the Player Spawn Method to be Round Robin (spawn at the spawn points in order)
        ///        playerSpawnMethod = PlayerSpawnMethod.RoundRobin;
        ///    }
        ///    
        ///    void Update()
        ///    {
        ///        //Press the space key to switch the spawn method
        ///        if (Input.GetKeyDown(KeyCode.Space))
        ///        {
        ///            //Press the space key to switch from RoundRobin method to Random method (spawn at the spawn points in a random order)
        ///            if (playerSpawnMethod == PlayerSpawnMethod.RoundRobin)
        ///                playerSpawnMethod = PlayerSpawnMethod.Random;
        ///            //Otherwise switch back to RoundRobin at the press of the space key
        ///            else playerSpawnMethod = PlayerSpawnMethod.RoundRobin;
        ///        }
        ///    }
        /// }
        /// </code>
        /// </summary>
        public PlayerSpawnMethod playerSpawnMethod { get { return m_PlayerSpawnMethod; } set { m_PlayerSpawnMethod = value; } }
        /// <summary>
        /// The scene to switch to when offline.
        /// <para>Setting this makes the NetworkManager do scene management. This scene will be switched to when a network session is completed - such as a client disconnect, or a server shutdown.</para>
        /// </summary>
        public string offlineScene           { get { return m_OfflineScene; }  set { m_OfflineScene = value; } }
        /// <summary>
        /// The scene to switch to when online.
        /// <para>Setting this makes the NetworkManager do scene management. This scene will be switched to when a network session is started - such as a client connect, or a server listen.</para>
        /// </summary>
        public string onlineScene            { get { return m_OnlineScene; }  set { m_OnlineScene = value; } }
        /// <summary>
        /// List of prefabs that will be registered with the spawning system.
        /// <para>For each of these prefabs, ClientManager.RegisterPrefab() will be automatically invoke.</para>
        /// </summary>
        public List<GameObject> spawnPrefabs { get { return m_SpawnPrefabs; }}

        /// <summary>
        /// The list of currently registered player start positions for the current scene.
        /// </summary>
        public List<Transform> startPositions { get { return s_StartPositions; }}

        /// <summary>
        /// Flag to enable custom network configuration.
        /// </summary>
        public bool customConfig             { get { return m_CustomConfig; } set { m_CustomConfig = value; } }
        /// <summary>
        /// The custom network configuration to use.
        /// <para>This will be used to configure the network transport layer.</para>
        /// </summary>
        public ConnectionConfig connectionConfig { get { if (m_ConnectionConfig == null) { m_ConnectionConfig = new ConnectionConfig(); } return m_ConnectionConfig; } }
        /// <summary>
        /// The transport layer global configuration to be used.
        /// <para>This defines global settings for the operation of the transport layer.</para>
        /// </summary>
        public GlobalConfig globalConfig     { get { if (m_GlobalConfig == null) { m_GlobalConfig = new GlobalConfig(); } return m_GlobalConfig; } }
        /// <summary>
        /// The maximum number of concurrent network connections to support.
        /// <para>The effects the memory usage of the network layer.</para>
        /// </summary>
        public int maxConnections            { get { return m_MaxConnections; } set { m_MaxConnections = value; } }
        /// <summary>
        /// The Quality-of-Service channels to use for the network transport layer.
        /// </summary>
        public List<QosType> channels        { get { return m_Channels; } }

        /// <summary>
        /// Allows you to specify an EndPoint object instead of setting networkAddress and networkPort (required for some platforms such as Xbox One).
        /// <para>Setting this object overrides the networkAddress and networkPort fields, and will be used instead of making connections.</para>
        /// </summary>
        public EndPoint secureTunnelEndpoint { get { return m_EndPoint; } set { m_EndPoint = value; } }

        /// <summary>
        /// This makes the NetworkServer listen for WebSockets connections instead of normal transport layer connections.
        /// <para>This allows WebGL clients to connect to the server.</para>
        /// </summary>
        public bool useWebSockets            { get { return m_UseWebSockets; } set { m_UseWebSockets = value; } }
        /// <summary>
        /// Flag that control whether clients started by this NetworkManager will use simulated latency and packet loss.
        /// </summary>
        public bool useSimulator             { get { return m_UseSimulator; } set { m_UseSimulator = value; }}
        /// <summary>
        /// The delay in milliseconds to be added to incoming and outgoing packets for clients.
        /// <para>This is only used when useSimulator is set.</para>
        /// </summary>
        public int simulatedLatency          { get { return m_SimulatedLatency; } set { m_SimulatedLatency = value; } }
        /// <summary>
        /// The percentage of incoming and outgoing packets to be dropped for clients.
        /// <para>This is only used when useSimulator is set.</para>
        /// </summary>
        public float packetLossPercentage    { get { return m_PacketLossPercentage; } set { m_PacketLossPercentage = value; } }

        /// <summary>
        /// The hostname of the matchmaking server.
        /// <para>The default address for the MatchMaker is mm.unet.unity3d.com That will connect a client to the nearest datacenter geographically. However because data centers are siloed from each other, players will only see matches occurring inside the data center they are currently connected to. If a player of your game is traveling to another part of the world, for instance, they may interact with a different set of players that are in that data center. You can override this behavior by specifying a particular data center. Keep in mind generally as distance grows so does latency, which is why we run data centers spread out over the world.</para>
        /// <para>To connect to a specific data center use one of the following addresses:</para>
        /// <para>United States: us1-mm.unet.unity3d.com Europe: eu1-mm.unet.unity3d.com Singapore: ap1-mm.unet.unity3d.com.</para>
        /// </summary>
        public string matchHost              { get { return m_MatchHost; } set { m_MatchHost = value; } }
        /// <summary>
        /// The port of the matchmaking service.
        /// </summary>
        public int matchPort                 { get { return m_MatchPort; } set { m_MatchPort = value; } }
        /// <summary>
        /// This is true if the client loaded a new scene when connecting to the server.
        /// <para>This is set before OnClientConnect is called, so it can be checked there to perform different logic if a scene load occurred.</para>
        /// </summary>
        public bool clientLoadedScene        { get { return m_ClientLoadedScene; } set { m_ClientLoadedScene = value; } }

        /// <summary>
        /// The migration manager being used with the NetworkManager.
        /// </summary>
        public NetworkMigrationManager migrationManager { get { return m_MigrationManager; }}

        /// <summary>
        /// NumPlayers is the number of active player objects across all connections on the server.
        /// <para>This is only valid on the host / server.</para>
        /// </summary>
        // only really valid on the server
        public int numPlayers
        {
            get
            {
                int numPlayers = 0;
                for (int i = 0; i < NetworkServer.connections.Count; i++)
                {
                    var conn = NetworkServer.connections[i];
                    if (conn == null)
                        continue;

                    for (int ii = 0; ii < conn.playerControllers.Count; ii++)
                    {
                        if (conn.playerControllers[ii].IsValid)
                        {
                            numPlayers += 1;
                        }
                    }
                }
                return numPlayers;
            }
        }

        public static INetworkTransport defaultTransport
        {
            get
            {
                return new DefaultNetworkTransport();
            }
        }

        public static INetworkTransport activeTransport
        {
            get
            {
                return s_ActiveTransport;
            }
            set
            {
                if (s_ActiveTransport != null && s_ActiveTransport.IsStarted)
                {
                    throw new InvalidOperationException("Cannot change network transport when current transport object is in use.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("Cannot set active transport to null.");
                }

                s_ActiveTransport = value;
            }
        }

        // runtime data
        /// <summary>
        /// The name of the current network scene.
        /// <para>This is populated if the NetworkManager is doing scene management. This should not be changed directly. Calls to ServerChangeScene() cause this to change. New clients that connect to a server will automatically load this scene.</para>
        /// </summary>
        static public string networkSceneName = "";
        /// <summary>
        /// True if the NetworkServer or NetworkClient isactive.
        /// <para>This is read-only. Calling StopServer() or StopClient() turns this off.</para>
        /// </summary>
        public bool isNetworkActive;
        /// <summary>
        /// The current NetworkClient being used by the manager.
        /// <para>This is populated when StartClient or StartLocalClient are called.</para>
        /// </summary>
        public NetworkClient client;
        static List<Transform> s_StartPositions = new List<Transform>();
        static int s_StartPositionIndex;

        /// <summary>
        /// A MatchInfo instance that will be used when StartServer() or StartClient() are called.
        /// <para>This should be populated from the data handed to the callback for NetworkMatch.CreateMatch or NetworkMatch.JoinMatch. It contains all the information necessary to connect to the match in question.</para>
        /// </summary>
        // matchmaking runtime data
        public MatchInfo matchInfo;
        /// <summary>
        /// The UMatch MatchMaker object.
        /// <para>This is populated if StartMatchMaker() has been called. It is used to communicate with the matchmaking service. This should be shut down after the match is complete to clean up its internal state. If this object is null then the client is not setup to communicate with MatchMaker yet.</para>
        /// </summary>
        public NetworkMatch matchMaker;
        /// <summary>
        /// The list of matches that are available to join.
        /// <para>This will be populated if UMatch.ListMatches() has been called. It will contain the most recent set of results from calling ListMatches.</para>
        /// </summary>
        public List<MatchInfoSnapshot> matches;
        /// <summary>
        /// The NetworkManager singleton object.
        /// <code>
        /// //Create a GameObject and attach this script
        /// //Create two buttons. To do this, go to Create>UI>Button for each.
        /// //Click each Button in the Hierarchy, and navigate to the Inspector window. Scroll down to the On Click() section and press the + button to add an action
        /// //Attach your GameObject to access the appropriate function you want your Button to do.
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Example : NetworkManager
        /// {
        ///    public void StartHostButton()
        ///    {
        ///        singleton.StartHost();
        ///    }
        ///
        ///    //Press the "Disconnect" Button to stop the Host
        ///    public void StopHostButton()
        ///    {
        ///        singleton.StopHost();
        ///    }
        /// }
        /// </code>
        /// </summary>
        public static NetworkManager singleton;

        // static message objects to avoid runtime-allocations
        static AddPlayerMessage s_AddPlayerMessage = new AddPlayerMessage();
        static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();
        static ErrorMessage s_ErrorMessage = new ErrorMessage();

        static AsyncOperation s_LoadingSceneAsync;
        static NetworkConnection s_ClientReadyConnection;

        // this is used to persist network address between scenes.
        static string s_Address;

#if UNITY_EDITOR
        static bool s_DomainReload;
        static NetworkManager s_PendingSingleton;

        internal static void OnDomainReload()
        {
            s_DomainReload = true;
        }

        public NetworkManager()
        {
            s_PendingSingleton = this;
        }

#endif

        void Awake()
        {
            InitializeSingleton();
        }

        void InitializeSingleton()
        {
            if (singleton != null && singleton == this)
            {
                return;
            }

            // do this early
            var logLevel = (int)m_LogLevel;
            if (logLevel != LogFilter.SetInScripting)
            {
                LogFilter.currentLogLevel = logLevel;
            }

            if (m_DontDestroyOnLoad)
            {
                if (singleton != null)
                {
                    if (LogFilter.logDev) { Debug.Log("Multiple NetworkManagers detected in the scene. Only one NetworkManager can exist at a time. The duplicate NetworkManager will not be used."); }
                    Destroy(gameObject);
                    return;
                }
                if (LogFilter.logDev) { Debug.Log("NetworkManager created singleton (DontDestroyOnLoad)"); }
                singleton = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (LogFilter.logDev) { Debug.Log("NetworkManager created singleton (ForScene)"); }
                singleton = this;
            }

            if (m_NetworkAddress != "")
            {
                s_Address = m_NetworkAddress;
            }
            else if (s_Address != "")
            {
                m_NetworkAddress = s_Address;
            }
        }

        void OnValidate()
        {
            if (m_SimulatedLatency < 1) m_SimulatedLatency = 1;
            if (m_SimulatedLatency > 500) m_SimulatedLatency = 500;

            if (m_PacketLossPercentage < 0) m_PacketLossPercentage = 0;
            if (m_PacketLossPercentage > 99) m_PacketLossPercentage = 99;

            if (m_MaxConnections <= 0) m_MaxConnections = 1;
            if (m_MaxConnections > 32000) m_MaxConnections = 32000;

            if (m_MaxBufferedPackets <= 0) m_MaxBufferedPackets = 0;
            if (m_MaxBufferedPackets > ChannelBuffer.MaxBufferedPackets)
            {
                m_MaxBufferedPackets = ChannelBuffer.MaxBufferedPackets;
                if (LogFilter.logError) { Debug.LogError("NetworkManager - MaxBufferedPackets cannot be more than " + ChannelBuffer.MaxBufferedPackets); }
            }

            if (m_PlayerPrefab != null && m_PlayerPrefab.GetComponent<NetworkIdentity>() == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkManager - playerPrefab must have a NetworkIdentity."); }
                m_PlayerPrefab = null;
            }

            if (m_ConnectionConfig != null && m_ConnectionConfig.MinUpdateTimeout <= 0)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkManager MinUpdateTimeout cannot be zero or less. The value will be reset to 1 millisecond"); }
                m_ConnectionConfig.MinUpdateTimeout = 1;
            }

            if (m_GlobalConfig != null)
            {
                if (m_GlobalConfig.ThreadAwakeTimeout <= 0)
                {
                    if (LogFilter.logError) { Debug.LogError("NetworkManager ThreadAwakeTimeout cannot be zero or less. The value will be reset to 1 millisecond"); }
                    m_GlobalConfig.ThreadAwakeTimeout = 1;
                }
            }
        }

        internal void RegisterServerMessages()
        {
            NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnectInternal);
            NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnectInternal);
            NetworkServer.RegisterHandler(MsgType.Ready, OnServerReadyMessageInternal);
            NetworkServer.RegisterHandler(MsgType.AddPlayer, OnServerAddPlayerMessageInternal);
            NetworkServer.RegisterHandler(MsgType.RemovePlayer, OnServerRemovePlayerMessageInternal);
            NetworkServer.RegisterHandler(MsgType.Error, OnServerErrorInternal);
        }

        /// <summary>
        /// This sets up a NetworkMigrationManager object to work with this NetworkManager.
        /// <para>The NetworkManager will automatically call functions on the migration manager, such as NetworkMigrationManager.LostHostOnClient when network events happen.</para>
        /// </summary>
        /// <param name="man">The migration manager object to use with the NetworkManager.</param>
        public void SetupMigrationManager(NetworkMigrationManager man)
        {
            m_MigrationManager = man;
        }

        public bool StartServer(ConnectionConfig config, int maxConnections)
        {
            return StartServer(null, config, maxConnections);
        }

        /// <summary>
        /// This starts a new server.
        /// <para>This uses the networkPort property as the listen port.</para>
        /// <code>
        /// //This is a script that creates a Toggle that you enable to start the Server.
        /// //Attach this script to an empty GameObject
        /// //Create a Toggle GameObject by going to <b>Create&gt;UI&gt;Toggle</b>.
        /// //Click on your empty GameObject.
        /// //Click and drag the Toggle GameObject from the Hierarchy to the Toggle section in the Inspector window.
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        /// using UnityEngine.Networking;
        ///
        /// //This makes the GameObject a NetworkManager GameObject
        /// public class Example : NetworkManager
        /// {
        ///    public Toggle m_Toggle;
        ///    Text m_ToggleText;
        ///
        ///    void Start()
        ///    {
        ///        //Fetch the Text of the Toggle to allow you to change it later
        ///        m_ToggleText = m_Toggle.GetComponentInChildren&lt;Text&gt;();
        ///        OnOff(false);
        ///    }
        ///
        ///    //Connect this function to the Toggle to start and stop the Server
        ///    public void OnOff(bool change)
        ///    {
        ///        //Detect when the Toggle returns false
        ///        if (change == false)
        ///        {
        ///            //Stop the Server
        ///            StopServer();
        ///            //Change the text of the Toggle
        ///            m_ToggleText.text = "Connect Server";
        ///        }
        ///        //Detect when the Toggle returns true
        ///        if (change == true)
        ///        {
        ///            //Start the Server
        ///            StartServer();
        ///            //Change the Toggle Text
        ///            m_ToggleText.text = "Disconnect Server";
        ///        }
        ///    }
        ///
        ///    //Detect when the Server starts and output the status
        ///    public override void OnStartServer()
        ///    {
        ///        //Output that the Server has started
        ///        Debug.Log("Server Started!");
        ///    }
        ///
        ///    //Detect when the Server stops
        ///    public override void OnStopServer()
        ///    {
        ///        //Output that the Server has stopped
        ///        Debug.Log("Server Stopped!");
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <returns>True is the server was started.</returns>
        public bool StartServer()
        {
            return StartServer(null);
        }

        public bool StartServer(MatchInfo info)
        {
            return StartServer(info, null, -1);
        }

        bool StartServer(MatchInfo info, ConnectionConfig config, int maxConnections)
        {
            InitializeSingleton();

            OnStartServer();

            if (m_RunInBackground)
                Application.runInBackground = true;

            NetworkCRC.scriptCRCCheck = scriptCRCCheck;
            NetworkServer.useWebSockets = m_UseWebSockets;

            if (m_GlobalConfig != null)
            {
                NetworkManager.activeTransport.Init(m_GlobalConfig);
            }

            // passing a config overrides setting the connectionConfig property
            if (m_CustomConfig && m_ConnectionConfig != null && config == null)
            {
                m_ConnectionConfig.Channels.Clear();
                for (int channelId = 0; channelId < m_Channels.Count; channelId++)
                {
                    m_ConnectionConfig.AddChannel(m_Channels[channelId]);
                }
                NetworkServer.Configure(m_ConnectionConfig, m_MaxConnections);
            }

            if (config != null)
            {
                NetworkServer.Configure(config, maxConnections);
            }

            if (info != null)
            {
                if (!NetworkServer.Listen(info, m_NetworkPort))
                {
                    if (LogFilter.logError) { Debug.LogError("StartServer listen failed."); }
                    return false;
                }
            }
            else
            {
                if (m_ServerBindToIP && !string.IsNullOrEmpty(m_ServerBindAddress))
                {
                    if (!NetworkServer.Listen(m_ServerBindAddress, m_NetworkPort))
                    {
                        if (LogFilter.logError) { Debug.LogError("StartServer listen on " + m_ServerBindAddress + " failed."); }
                        return false;
                    }
                }
                else
                {
                    if (!NetworkServer.Listen(m_NetworkPort))
                    {
                        if (LogFilter.logError) { Debug.LogError("StartServer listen failed."); }
                        return false;
                    }
                }
            }

            // this must be after Listen(), since that registers the default message handlers
            RegisterServerMessages();

            if (LogFilter.logDebug) { Debug.Log("NetworkManager StartServer port:" + m_NetworkPort); }
            isNetworkActive = true;

            // Only change scene if the requested online scene is not blank, and is not already loaded
            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (!string.IsNullOrEmpty(m_OnlineScene) && m_OnlineScene != loadedSceneName && m_OnlineScene != m_OfflineScene)
            {
                ServerChangeScene(m_OnlineScene);
            }
            else
            {
                NetworkServer.SpawnObjects();
            }
            return true;
        }

        internal void RegisterClientMessages(NetworkClient client)
        {
            client.RegisterHandler(MsgType.Connect, OnClientConnectInternal);
            client.RegisterHandler(MsgType.Disconnect, OnClientDisconnectInternal);
            client.RegisterHandler(MsgType.NotReady, OnClientNotReadyMessageInternal);
            client.RegisterHandler(MsgType.Error, OnClientErrorInternal);
            client.RegisterHandler(MsgType.Scene, OnClientSceneInternal);

            if (m_PlayerPrefab != null)
            {
                ClientScene.RegisterPrefab(m_PlayerPrefab);
            }
            for (int i = 0; i < m_SpawnPrefabs.Count; i++)
            {
                var prefab = m_SpawnPrefabs[i];
                if (prefab != null)
                {
                    ClientScene.RegisterPrefab(prefab);
                }
            }
        }

        /// <summary>
        /// This allows the NetworkManager to use a client object created externally to the NetworkManager instead of using StartClient().
        /// <para>The StartClient() function creates a client object, but this is not always what is desired. UseExternalClient allows a NetworkClient object to be created by other code and used with the NetworkManager.</para>
        /// <para>The client object will have the standard NetworkManager message handlers registered on it.</para>
        /// </summary>
        /// <param name="externalClient">The NetworkClient object to use.</param>
        public void UseExternalClient(NetworkClient externalClient)
        {
            if (m_RunInBackground)
                Application.runInBackground = true;

            if (externalClient != null)
            {
                client = externalClient;
                isNetworkActive = true;
                RegisterClientMessages(client);
                OnStartClient(client);
            }
            else
            {
                OnStopClient();

                // this should stop any game-related systems, but not close the connection
                ClientScene.DestroyAllClientObjects();
                ClientScene.HandleClientDisconnect(client.connection);
                client = null;
                if (!string.IsNullOrEmpty(m_OfflineScene))
                {
                    ClientChangeScene(m_OfflineScene, false);
                }
            }
            s_Address = m_NetworkAddress;
        }

        public NetworkClient StartClient(MatchInfo info, ConnectionConfig config, int hostPort)
        {
            InitializeSingleton();

            matchInfo = info;
            if (m_RunInBackground)
                Application.runInBackground = true;

            isNetworkActive = true;

            if (m_GlobalConfig != null)
            {
                NetworkManager.activeTransport.Init(m_GlobalConfig);
            }

            client = new NetworkClient();
            client.hostPort = hostPort;

            if (config != null)
            {
                if ((config.UsePlatformSpecificProtocols) && (UnityEngine.Application.platform != RuntimePlatform.PS4))
                    throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");

                client.Configure(config, 1);
            }
            else
            {
                if (m_CustomConfig && m_ConnectionConfig != null)
                {
                    m_ConnectionConfig.Channels.Clear();
                    for (int i = 0; i < m_Channels.Count; i++)
                    {
                        m_ConnectionConfig.AddChannel(m_Channels[i]);
                    }
                    if ((m_ConnectionConfig.UsePlatformSpecificProtocols) && (UnityEngine.Application.platform != RuntimePlatform.PS4))
                        throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
                    client.Configure(m_ConnectionConfig, m_MaxConnections);
                }
            }

            RegisterClientMessages(client);
            if (matchInfo != null)
            {
                if (LogFilter.logDebug) { Debug.Log("NetworkManager StartClient match: " + matchInfo); }
                client.Connect(matchInfo);
            }
            else if (m_EndPoint != null)
            {
                if (LogFilter.logDebug) { Debug.Log("NetworkManager StartClient using provided SecureTunnel"); }
                client.Connect(m_EndPoint);
            }
            else
            {
                if (string.IsNullOrEmpty(m_NetworkAddress))
                {
                    if (LogFilter.logError) { Debug.LogError("Must set the Network Address field in the manager"); }
                    return null;
                }
                if (LogFilter.logDebug) { Debug.Log("NetworkManager StartClient address:" + m_NetworkAddress + " port:" + m_NetworkPort); }

                if (m_UseSimulator)
                {
                    client.ConnectWithSimulator(m_NetworkAddress, m_NetworkPort, m_SimulatedLatency, m_PacketLossPercentage);
                }
                else
                {
                    client.Connect(m_NetworkAddress, m_NetworkPort);
                }
            }

            if (m_MigrationManager != null)
            {
                m_MigrationManager.Initialize(client, matchInfo);
            }

            OnStartClient(client);
            s_Address = m_NetworkAddress;
            return client;
        }

        public NetworkClient StartClient(MatchInfo matchInfo)
        {
            return StartClient(matchInfo, null);
        }

        /// <summary>
        /// This starts a network client. It uses the networkAddress and networkPort properties as the address to connect to.
        /// <para>This makes the newly created client connect to the server immediately.</para>
        /// </summary>
        /// <returns>The client object created.</returns>
        public NetworkClient StartClient()
        {
            return StartClient(null, null);
        }

        public NetworkClient StartClient(MatchInfo info, ConnectionConfig config)
        {
            return StartClient(info, config, 0);
        }

        public virtual NetworkClient StartHost(ConnectionConfig config, int maxConnections)
        {
            OnStartHost();
            if (StartServer(null, config, maxConnections))
            {
                var client = ConnectLocalClient();
                OnServerConnect(client.connection);
                OnStartClient(client);
                return client;
            }
            return null;
        }

        public virtual NetworkClient StartHost(MatchInfo info)
        {
            OnStartHost();
            matchInfo = info;
            if (StartServer(info))
            {
                var client = ConnectLocalClient();
                OnStartClient(client);
                return client;
            }
            return null;
        }

        /// <summary>
        /// This starts a network "host" - a server and client in the same application.
        /// <para>The client returned from StartHost() is a special "local" client that communicates to the in-process server using a message queue instead of the real network. But in almost all other cases, it can be treated as a normal client.</para>
        /// </summary>
        /// <returns>The client object created - this is a "local client".</returns>
        public virtual NetworkClient StartHost()
        {
            OnStartHost();
            if (StartServer())
            {
                var localClient = ConnectLocalClient();
                OnStartClient(localClient);
                return localClient;
            }
            return null;
        }

        NetworkClient ConnectLocalClient()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager StartHost port:" + m_NetworkPort); }
            m_NetworkAddress = "localhost";
            client = ClientScene.ConnectLocalServer();
            RegisterClientMessages(client);

            if (m_MigrationManager != null)
            {
                m_MigrationManager.Initialize(client, matchInfo);
            }
            return client;
        }

        /// <summary>
        /// This stops both the client and the server that the manager is using.
        /// </summary>
        public void StopHost()
        {
            var serverWasActive = NetworkServer.active;
            OnStopHost();

            StopServer();
            StopClient();

            if (m_MigrationManager != null)
            {
                if (serverWasActive)
                {
                    m_MigrationManager.LostHostOnHost();
                }
            }
        }

        /// <summary>
        /// Stops the server that the manager is using.
        /// </summary>
        public void StopServer()
        {
            if (!NetworkServer.active)
                return;

            OnStopServer();

            if (LogFilter.logDebug) { Debug.Log("NetworkManager StopServer"); }
            isNetworkActive = false;
            NetworkServer.Shutdown();
            StopMatchMaker();
            if (!string.IsNullOrEmpty(m_OfflineScene))
            {
                ServerChangeScene(m_OfflineScene);
            }
            CleanupNetworkIdentities();
        }

        /// <summary>
        /// Stops the client that the manager is using.
        /// </summary>
        public void StopClient()
        {
            OnStopClient();

            if (LogFilter.logDebug) { Debug.Log("NetworkManager StopClient"); }
            isNetworkActive = false;
            if (client != null)
            {
                // only shutdown this client, not ALL clients.
                client.Disconnect();
                client.Shutdown();
                client = null;
            }
            StopMatchMaker();

            ClientScene.DestroyAllClientObjects();
            if (!string.IsNullOrEmpty(m_OfflineScene))
            {
                ClientChangeScene(m_OfflineScene, false);
            }
            CleanupNetworkIdentities();
        }

        /// <summary>
        /// This causes the server to switch scenes and sets the networkSceneName.
        /// <para>Clients that connect to this server will automatically switch to this scene. This is called autmatically if onlineScene or offlineScene are set, but it can be called from user code to switch scenes again while the game is in progress. This automatically sets clients to be not-ready. The clients must call NetworkClient.Ready() again to participate in the new scene.</para>
        /// </summary>
        /// <param name="newSceneName">The name of the scene to change to. The server will change scene immediately, and a message will be sent to connected clients to ask them to change scene also.</param>
        public virtual void ServerChangeScene(string newSceneName)
        {
            if (string.IsNullOrEmpty(newSceneName))
            {
                if (LogFilter.logError) { Debug.LogError("ServerChangeScene empty scene name"); }
                return;
            }

            if (LogFilter.logDebug) { Debug.Log("ServerChangeScene " + newSceneName); }
            NetworkServer.SetAllClientsNotReady();
            networkSceneName = newSceneName;

            s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);

            StringMessage msg = new StringMessage(networkSceneName);
            NetworkServer.SendToAll(MsgType.Scene, msg);

            s_StartPositionIndex = 0;
            s_StartPositions.Clear();
        }

        void CleanupNetworkIdentities()
        {
            foreach (NetworkIdentity netId in Resources.FindObjectsOfTypeAll<NetworkIdentity>())
            {
                netId.MarkForReset();
            }
        }

        internal void ClientChangeScene(string newSceneName, bool forceReload)
        {
            if (string.IsNullOrEmpty(newSceneName))
            {
                if (LogFilter.logError) { Debug.LogError("ClientChangeScene empty scene name"); }
                return;
            }

            if (LogFilter.logDebug) { Debug.Log("ClientChangeScene newSceneName:" + newSceneName + " networkSceneName:" + networkSceneName); }


            if (newSceneName == networkSceneName)
            {
                if (m_MigrationManager != null)
                {
                    // special case for rejoining a match after host migration
                    FinishLoadScene();
                    return;
                }

                if (!forceReload)
                {
                    FinishLoadScene();
                    return;
                }
            }

            s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
            networkSceneName = newSceneName;
        }

        void FinishLoadScene()
        {
            // NOTE: this cannot use NetworkClient.allClients[0] - that client may be for a completely different purpose.

            if (client != null)
            {
                if (s_ClientReadyConnection != null)
                {
                    m_ClientLoadedScene = true;
                    OnClientConnect(s_ClientReadyConnection);
                    s_ClientReadyConnection = null;
                }
            }
            else
            {
                if (LogFilter.logDev) { Debug.Log("FinishLoadScene client is null"); }
            }

            if (NetworkServer.active)
            {
                NetworkServer.SpawnObjects();
                OnServerSceneChanged(networkSceneName);
            }

            if (IsClientConnected() && client != null)
            {
                RegisterClientMessages(client);
                OnClientSceneChanged(client.connection);
            }
        }

        internal static void UpdateScene()
        {
#if UNITY_EDITOR
            // In the editor, reloading scripts in play mode causes a Mono Domain Reload.
            // This gets the transport layer (C++) and HLAPI (C#) out of sync.
            // This check below detects that problem and shuts down the transport layer to bring both systems back in sync.
            if (singleton == null && s_PendingSingleton != null && s_DomainReload)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkManager detected a script reload in the editor. This has caused the network to be shut down."); }

                s_DomainReload = false;
                s_PendingSingleton.InitializeSingleton();

                // destroy network objects
                var uvs = FindObjectsOfType<NetworkIdentity>();
                foreach (var uv in uvs)
                {
                    GameObject.Destroy(uv.gameObject);
                }

                singleton.StopHost();

                NetworkManager.activeTransport.Shutdown();
            }
#endif
            if (singleton == null)
                return;

            if (s_LoadingSceneAsync == null)
                return;

            if (!s_LoadingSceneAsync.isDone)
                return;

            if (LogFilter.logDebug) { Debug.Log("ClientChangeScene done readyCon:" + s_ClientReadyConnection); }
            singleton.FinishLoadScene();
            s_LoadingSceneAsync.allowSceneActivation = true;
            s_LoadingSceneAsync = null;
        }

        void OnDestroy()
        {
            if (LogFilter.logDev) { Debug.Log("NetworkManager destroyed"); }
        }

        /// <summary>
        /// Registers the transform of a game object as a player spawn location.
        /// <para>This is done automatically by NetworkStartPosition components, but can be done manually from user script code.</para>
        /// </summary>
        /// <param name="start">Transform to register.</param>
        static public void RegisterStartPosition(Transform start)
        {
            if (LogFilter.logDebug) { Debug.Log("RegisterStartPosition: (" + start.gameObject.name + ") " + start.position); }
            s_StartPositions.Add(start);
        }

        /// <summary>
        /// Unregisters the transform of a game object as a player spawn location.
        /// <para>This is done automatically by the <see cref="NetworkStartPosition">NetworkStartPosition</see> component, but can be done manually from user code.</para>
        /// </summary>
        /// <param name="start"></param>
        static public void UnRegisterStartPosition(Transform start)
        {
            if (LogFilter.logDebug) { Debug.Log("UnRegisterStartPosition: (" + start.gameObject.name + ") " + start.position); }
            s_StartPositions.Remove(start);
        }

        /// <summary>
        /// This checks if the NetworkManager has a client and that it is connected to a server.
        /// <para>This is more specific than NetworkClient.isActive, which will be true if there are any clients active, rather than just the NetworkManager's client.</para>
        /// </summary>
        /// <returns>True if the NetworkManagers client is connected to a server.</returns>
        public bool IsClientConnected()
        {
            return client != null && client.isConnected;
        }

        /// <summary>
        /// Shuts down the NetworkManager completely and destroy the singleton.
        /// <para>This is required if a new NetworkManager instance needs to be created after the original one was destroyed. The example below has a reference to the GameObject with the NetworkManager on it and destroys the instance before calling Shutdown() and switching scenes.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class SwitchToEmptyScene : MonoBehaviour
        /// {
        ///    public GameObject NetworkManagerGameObject;
        ///
        ///    void OnGUI()
        ///    {
        ///        if (GUI.Button(new Rect(10, 10, 200, 20), "Switch"))
        ///        {
        ///            Destroy(NetworkManagerGameObject);
        ///            NetworkManager.Shutdown();
        ///            Application.LoadLevel("empty");
        ///        }
        ///    }
        /// }
        /// </code>
        /// <para>This cleanup allows a new scene with a new NetworkManager to be loaded.</para>
        /// </summary>
        // this is the only way to clear the singleton, so another instance can be created.
        static public void Shutdown()
        {
            if (singleton == null)
                return;

            s_StartPositions.Clear();
            s_StartPositionIndex = 0;
            s_ClientReadyConnection = null;

            singleton.StopHost();
            singleton = null;
        }

        // ----------------------------- Server Internal Message Handlers  --------------------------------

        internal void OnServerConnectInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerConnectInternal"); }

            netMsg.conn.SetMaxDelay(m_MaxDelay);

            if (m_MaxBufferedPackets != ChannelBuffer.MaxBufferedPackets)
            {
                for (int channelId = 0; channelId < NetworkServer.numChannels; channelId++)
                {
                    netMsg.conn.SetChannelOption(channelId, ChannelOption.MaxPendingBuffers, m_MaxBufferedPackets);
                }
            }

            if (!m_AllowFragmentation)
            {
                for (int channelId = 0; channelId < NetworkServer.numChannels; channelId++)
                {
                    netMsg.conn.SetChannelOption(channelId, ChannelOption.AllowFragmentation, 0);
                }
            }

            if (networkSceneName != "" && networkSceneName != m_OfflineScene)
            {
                StringMessage msg = new StringMessage(networkSceneName);
                netMsg.conn.Send(MsgType.Scene, msg);
            }

            if (m_MigrationManager != null)
            {
                m_MigrationManager.SendPeerInfo();
            }
            OnServerConnect(netMsg.conn);
        }

        internal void OnServerDisconnectInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerDisconnectInternal"); }

            if (m_MigrationManager != null)
            {
                m_MigrationManager.SendPeerInfo();
            }
            OnServerDisconnect(netMsg.conn);
        }

        internal void OnServerReadyMessageInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerReadyMessageInternal"); }

            OnServerReady(netMsg.conn);
        }

        internal void OnServerAddPlayerMessageInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerAddPlayerMessageInternal"); }

            netMsg.ReadMessage(s_AddPlayerMessage);

            if (s_AddPlayerMessage.msgSize != 0)
            {
                var reader = new NetworkReader(s_AddPlayerMessage.msgData);
                OnServerAddPlayer(netMsg.conn, s_AddPlayerMessage.playerControllerId, reader);
            }
            else
            {
                OnServerAddPlayer(netMsg.conn, s_AddPlayerMessage.playerControllerId);
            }

            if (m_MigrationManager != null)
            {
                m_MigrationManager.SendPeerInfo();
            }
        }

        internal void OnServerRemovePlayerMessageInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerRemovePlayerMessageInternal"); }

            netMsg.ReadMessage(s_RemovePlayerMessage);

            PlayerController player;
            netMsg.conn.GetPlayerController(s_RemovePlayerMessage.playerControllerId, out player);
            OnServerRemovePlayer(netMsg.conn, player);
            netMsg.conn.RemovePlayerController(s_RemovePlayerMessage.playerControllerId);

            if (m_MigrationManager != null)
            {
                m_MigrationManager.SendPeerInfo();
            }
        }

        internal void OnServerErrorInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnServerErrorInternal"); }

            netMsg.ReadMessage(s_ErrorMessage);
            OnServerError(netMsg.conn, s_ErrorMessage.errorCode);
        }

        // ----------------------------- Client Internal Message Handlers  --------------------------------

        internal void OnClientConnectInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnClientConnectInternal"); }

            netMsg.conn.SetMaxDelay(m_MaxDelay);

            string loadedSceneName = SceneManager.GetSceneAt(0).name;
            if (string.IsNullOrEmpty(m_OnlineScene) || (m_OnlineScene == m_OfflineScene) || (loadedSceneName == m_OnlineScene))
            {
                m_ClientLoadedScene = false;
                OnClientConnect(netMsg.conn);
            }
            else
            {
                // will wait for scene id to come from the server.
                s_ClientReadyConnection = netMsg.conn;
            }
        }

        internal void OnClientDisconnectInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnClientDisconnectInternal"); }

            if (m_MigrationManager != null)
            {
                if (m_MigrationManager.LostHostOnClient(netMsg.conn))
                {
                    // should OnClientDisconnect be called?
                    return;
                }
            }

            if (!string.IsNullOrEmpty(m_OfflineScene))
            {
                ClientChangeScene(m_OfflineScene, false);
            }

            // If we have a valid connection here drop the client in the matchmaker before shutting down below
            if (matchMaker != null && matchInfo != null && matchInfo.networkId != NetworkID.Invalid && matchInfo.nodeId != NodeID.Invalid)
            {
                matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, matchInfo.domain, OnDropConnection);
            }

            OnClientDisconnect(netMsg.conn);
        }

        internal void OnClientNotReadyMessageInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnClientNotReadyMessageInternal"); }

            ClientScene.SetNotReady();
            OnClientNotReady(netMsg.conn);

            // NOTE: s_ClientReadyConnection is not set here! don't want OnClientConnect to be invoked again after scene changes.
        }

        internal void OnClientErrorInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnClientErrorInternal"); }

            netMsg.ReadMessage(s_ErrorMessage);
            OnClientError(netMsg.conn, s_ErrorMessage.errorCode);
        }

        internal void OnClientSceneInternal(NetworkMessage netMsg)
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager:OnClientSceneInternal"); }

            string newSceneName = netMsg.reader.ReadString();

            if (IsClientConnected() && !NetworkServer.active)
            {
                ClientChangeScene(newSceneName, true);
            }
        }

        // ----------------------------- Server System Callbacks --------------------------------

        /// <summary>
        /// Called on the server when a new client connects.
        /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
        /// <code>
        /// //Attach this script to a GameObject and add a NetworkHUD component to the GameObject.
        /// //Create a Text GameObject (Create>UI>Text) and attach it in the Text field in the Inspector.
        /// //This script changes Text on the screen when a client connects to the server
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        ///
        /// public class OnServerConnectExample : NetworkManager
        /// {
        ///    //Assign a Text component in the GameObject's Inspector
        ///    public Text m_Text;
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnServerConnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection and the client's ID
        ///        m_Text.text = "Client " + connection.connectionId + " Connected!";
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public virtual void OnServerConnect(NetworkConnection conn)
        {
        }

        /// <summary>
        /// Called on the server when a client disconnects.
        /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
        /// <code>
        /// //This script outputs a message when a client connects or disconnects from the server
        /// //Attach this script to your GameObject.
        /// //Attach a NetworkManagerHUD to your by clicking Add Component in the Inspector window of the GameObject. Then go to Network>NetworkManagerHUD.
        /// //Create a Text GameObject and attach it to the Text field in the Inspector.
        /// 
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        /// 
        /// public class Example : NetworkManager
        /// {
        ///    //Assign a Text component in the GameObject's Inspector
        ///    public Text m_Text;
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnServerConnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection
        ///        m_Text.text = "Client " + connection.connectionId + " Connected!";
        ///    }
        /// 
        ///    //Detect when a client disconnects from the Server
        ///    public override void OnServerDisconnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the loss of connection
        ///        m_Text.text = "Client " + connection.connectionId + "Connection Lost!";
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public virtual void OnServerDisconnect(NetworkConnection conn)
        {
            NetworkServer.DestroyPlayersForConnection(conn);
            if (conn.lastError != NetworkError.Ok)
            {
                if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }
            }
        }

        /// <summary>
        /// Called on the server when a client is ready.
        /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public virtual void OnServerReady(NetworkConnection conn)
        {
            if (conn.playerControllers.Count == 0)
            {
                // this is now allowed (was not for a while)
                if (LogFilter.logDebug) { Debug.Log("Ready with no player object"); }
            }
            NetworkServer.SetClientReady(conn);
        }

        /// <summary>
        /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.Networking.NetworkSystem;
        ///
        /// class MyManager : NetworkManager
        /// {
        ///    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        ///    {
        ///        if (extraMessageReader != null)
        ///        {
        ///            var s = extraMessageReader.ReadMessage&lt;StringMessage&gt;();
        ///            Debug.Log("my name is " + s.value);
        ///        }
        ///        OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        /// <param name="playerControllerId">Id of the new player.</param>
        /// <param name="extraMessageReader">An extra message object passed for the new player.</param>
        public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            OnServerAddPlayerInternal(conn, playerControllerId);
        }

        public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            OnServerAddPlayerInternal(conn, playerControllerId);
        }

        void OnServerAddPlayerInternal(NetworkConnection conn, short playerControllerId)
        {
            if (m_PlayerPrefab == null)
            {
                if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
                return;
            }

            if (m_PlayerPrefab.GetComponent<NetworkIdentity>() == null)
            {
                if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
                return;
            }

            if (playerControllerId < conn.playerControllers.Count  && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
            {
                if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
                return;
            }

            GameObject player;
            Transform startPos = GetStartPosition();
            if (startPos != null)
            {
                player = (GameObject)Instantiate(m_PlayerPrefab, startPos.position, startPos.rotation);
            }
            else
            {
                player = (GameObject)Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
            }

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        /// <summary>
        /// This finds a spawn position based on NetworkStartPosition objects in the scene.
        /// <para>This is used by the default implementation of OnServerAddPlayer.</para>
        /// </summary>
        /// <returns>Returns the transform to spawn a player at, or null.</returns>
        public Transform GetStartPosition()
        {
            // first remove any dead transforms
            if (s_StartPositions.Count > 0)
            {
                for (int i = s_StartPositions.Count - 1; i >= 0; i--)
                {
                    if (s_StartPositions[i] == null)
                        s_StartPositions.RemoveAt(i);
                }
            }

            if (m_PlayerSpawnMethod == PlayerSpawnMethod.Random && s_StartPositions.Count > 0)
            {
                // try to spawn at a random start location
                int index = Random.Range(0, s_StartPositions.Count);
                return s_StartPositions[index];
            }
            if (m_PlayerSpawnMethod == PlayerSpawnMethod.RoundRobin && s_StartPositions.Count > 0)
            {
                if (s_StartPositionIndex >= s_StartPositions.Count)
                {
                    s_StartPositionIndex = 0;
                }

                Transform startPos = s_StartPositions[s_StartPositionIndex];
                s_StartPositionIndex += 1;
                return startPos;
            }
            return null;
        }

        /// <summary>
        /// Called on the server when a client removes a player.
        /// <para>The default implementation of this function destroys the corresponding player object.</para>
        /// </summary>
        /// <param name="conn">The connection to remove the player from.</param>
        /// <param name="player">The player controller to remove.</param>
        public virtual void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            if (player.gameObject != null)
            {
                NetworkServer.Destroy(player.gameObject);
            }
        }

        /// <summary>
        /// Called on the server when a network error occurs for a client connection.
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        /// <param name="errorCode">Error code.</param>
        public virtual void OnServerError(NetworkConnection conn, int errorCode)
        {
        }

        /// <summary>
        /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
        /// </summary>
        /// <param name="sceneName">The name of the new scene.</param>
        public virtual void OnServerSceneChanged(string sceneName)
        {
        }

        // ----------------------------- Client System Callbacks --------------------------------

        /// <summary>
        /// Called on the client when connected to a server.
        /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
        /// <code>
        /// //Attach this script to a GameObject
        /// //Create a Text GameObject(Create>UI>Text) and attach it to the Text field in the Inspector window
        /// //This script changes the Text depending on if a client connects or disconnects to the server
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        ///
        /// public class Example : NetworkManager
        /// {
        ///    //Assign a Text component in the GameObject's Inspector
        ///    public Text m_ClientText;
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnClientConnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection on the client side
        ///        m_ClientText.text =  " " + connection.connectionId + " Connected!";
        ///    }
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnClientDisconnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection loss on the client side
        ///        m_ClientText.text = "Connection" + connection.connectionId + " Lost!";
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">Connection to the server.</param>
        public virtual void OnClientConnect(NetworkConnection conn)
        {
            if (!clientLoadedScene)
            {
                // Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
                ClientScene.Ready(conn);
                if (m_AutoCreatePlayer)
                {
                    ClientScene.AddPlayer(0);
                }
            }
        }

        /// <summary>
        /// Called on clients when disconnected from a server.
        /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
        /// <code>
        /// //Attach this script to a GameObject
        /// //Create a Text GameObject(Create>UI>Text) and attach it to the Text field in the Inspector window
        /// //This script changes the Text depending on if a client connects or disconnects to the server
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.UI;
        ///
        /// public class OnClientConnectExample : NetworkManager
        /// {
        ///    //Assign a Text component in the GameObject's Inspector
        ///    public Text m_ClientText;
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnClientConnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection on the client side
        ///        m_ClientText.text =  " " + connection.connectionId + " Connected!";
        ///    }
        ///
        ///    //Detect when a client connects to the Server
        ///    public override void OnClientDisconnect(NetworkConnection connection)
        ///    {
        ///        //Change the text to show the connection loss on the client side
        ///        m_ClientText.text = "Connection" + connection.connectionId + " Lost!";
        ///    }
        /// }
        /// </code>
        /// </summary>
        /// <param name="conn">	Connection to the server.</param>
        public virtual void OnClientDisconnect(NetworkConnection conn)
        {
            StopClient();
            if (conn.lastError != NetworkError.Ok)
            {
                if (LogFilter.logError) { Debug.LogError("ClientDisconnected due to error: " + conn.lastError); }
            }
        }

        /// <summary>
        /// Called on clients when a network error occurs.
        /// </summary>
        /// <param name="conn">Connection to a server.</param>
        /// <param name="errorCode">Error code.</param>
        public virtual void OnClientError(NetworkConnection conn, int errorCode)
        {
        }

        /// <summary>
        /// Called on clients when a servers tells the client it is no longer ready.
        /// <para>This is commonly used when switching scenes.</para>
        /// </summary>
        /// <param name="conn">Connection to a server.</param>
        public virtual void OnClientNotReady(NetworkConnection conn)
        {
        }

        /// <summary>
        /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
        /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
        /// </summary>
        /// <param name="conn">The network connection that the scene change message arrived on.</param>
        public virtual void OnClientSceneChanged(NetworkConnection conn)
        {
            // always become ready.
            ClientScene.Ready(conn);

            if (!m_AutoCreatePlayer)
            {
                return;
            }

            bool addPlayer = (ClientScene.localPlayers.Count == 0);
            bool foundPlayer = false;
            for (int i = 0; i < ClientScene.localPlayers.Count; i++)
            {
                if (ClientScene.localPlayers[i].gameObject != null)
                {
                    foundPlayer = true;
                    break;
                }
            }
            if (!foundPlayer)
            {
                // there are players, but their game objects have all been deleted
                addPlayer = true;
            }
            if (addPlayer)
            {
                ClientScene.AddPlayer(0);
            }
        }

        // ----------------------------- Matchmaker --------------------------------

        /// <summary>
        /// This starts MatchMaker for the NetworkManager.
        /// <para>This uses the matchHost and matchPort properties as the address of the MatchMaker service to connect to. Please call SetMatchHost prior to calling this function if you are not using the default MatchMaker address.</para>
        /// </summary>
        public void StartMatchMaker()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkManager StartMatchMaker"); }
            SetMatchHost(m_MatchHost, m_MatchPort, m_MatchPort == 443);
        }

        /// <summary>
        /// Stops the MatchMaker that the NetworkManager is using.
        /// <para>This should be called after a match is complete and before starting or joining a new match.</para>
        /// </summary>
        public void StopMatchMaker()
        {
            // If we have a valid connection here drop the client in the matchmaker before shutting down below
            if (matchMaker != null && matchInfo != null && matchInfo.networkId != NetworkID.Invalid && matchInfo.nodeId != NodeID.Invalid)
            {
                matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, matchInfo.domain, OnDropConnection);
            }

            if (matchMaker != null)
            {
                Destroy(matchMaker);
                matchMaker = null;
            }
            matchInfo = null;
            matches = null;
        }

        /// <summary>
        /// This sets the address of the MatchMaker service.
        /// <para>The default address for the MatchMaker is mm.unet.unity3d.com That will connect a client to the nearest datacenter geographically. However because data centers are siloed from each other, players will only see matches occurring inside the data center they are currently connected to. If a player of your game is traveling to another part of the world, for instance, they may interact with a different set of players that are in that data center. You can override this behavior by specifying a particular data center. Keep in mind generally as distance grows so does latency, which is why we run data centers spread out over the world.</para>
        /// <para>To connect to a specific data center use one of the following addresses:</para>
        /// <para>United States: us1-mm.unet.unity3d.com Europe: eu1-mm.unet.unity3d.com Singapore: ap1-mm.unet.unity3d.com.</para>
        /// </summary>
        /// <param name="newHost">Hostname of MatchMaker service.</param>
        /// <param name="port">Port of MatchMaker service.</param>
        /// <param name="https">Protocol used by MatchMaker service.</param>
        public void SetMatchHost(string newHost, int port, bool https)
        {
            if (matchMaker == null)
            {
                matchMaker = gameObject.AddComponent<NetworkMatch>();
            }
            if (newHost == "127.0.0.1")
            {
                newHost = "localhost";
            }
            string prefix = "http://";
            if (https)
            {
                prefix = "https://";
            }

            if (newHost.StartsWith("http://"))
            {
                newHost = newHost.Replace("http://", "");
            }
            if (newHost.StartsWith("https://"))
            {
                newHost = newHost.Replace("https://", "");
            }

            m_MatchHost = newHost;
            m_MatchPort = port;

            string fullURI = prefix + m_MatchHost + ":" + m_MatchPort;
            if (LogFilter.logDebug) { Debug.Log("SetMatchHost:" + fullURI); }
            matchMaker.baseUri = new Uri(fullURI);
        }

        //------------------------------ Start & Stop callbacks -----------------------------------

        // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
        // their functionality, users would need override all the versions. Instead these callbacks are invoked
        // from all versions, so users only need to implement this one case.

        /// <summary>
        /// This hook is invoked when a host is started.
        /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
        /// </summary>
        public virtual void OnStartHost()
        {
        }

        /// <summary>
        /// This hook is invoked when a server is started - including when a host is started.
        /// StartServer has multiple signatures, but they all cause this hook to be called.
        /// </summary>
        public virtual void OnStartServer()
        {
        }

        /// <summary>
        /// This is a hook that is invoked when the client is started.
        /// <para>StartClient has multiple signatures, but they all cause this hook to be called.</para>
        /// </summary>
        /// <param name="client">The NetworkClient object that was started.</param>
        public virtual void OnStartClient(NetworkClient client)
        {
        }

        /// <summary>
        /// This hook is called when a server is stopped - including when a host is stopped.
        /// </summary>
        public virtual void OnStopServer()
        {
        }

        /// <summary>
        /// This hook is called when a client is stopped.
        /// </summary>
        public virtual void OnStopClient()
        {
        }

        /// <summary>
        /// This hook is called when a host is stopped.
        /// </summary>
        public virtual void OnStopHost()
        {
        }

        //------------------------------ Matchmaker callbacks -----------------------------------

        /// <summary>
        /// Callback that happens when a NetworkMatch.CreateMatch request has been processed on the server.
        /// </summary>
        /// <param name="success">Indicates if the request succeeded.</param>
        /// <param name="extendedInfo">A text description for the error if success is false.</param>
        /// <param name="matchInfo">The information about the newly created match.</param>
        public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (LogFilter.logDebug) { Debug.LogFormat("NetworkManager OnMatchCreate Success:{0}, ExtendedInfo:{1}, matchInfo:{2}", success, extendedInfo, matchInfo); }

            if (success)
                StartHost(matchInfo);
        }

        /// <summary>
        /// Callback that happens when a NetworkMatch.ListMatches request has been processed on the server.
        /// </summary>
        /// <param name="success">Indicates if the request succeeded.</param>
        /// <param name="extendedInfo">A text description for the error if success is false.</param>
        /// <param name="matchList">A list of matches corresponding to the filters set in the initial list request.</param>
        public virtual void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
        {
            if (LogFilter.logDebug) { Debug.LogFormat("NetworkManager OnMatchList Success:{0}, ExtendedInfo:{1}, matchList.Count:{2}", success, extendedInfo, matchList.Count); }

            matches = matchList;
        }

        /// <summary>
        /// Callback that happens when a NetworkMatch.JoinMatch request has been processed on the server.
        /// </summary>
        /// <param name="success">Indicates if the request succeeded.</param>
        /// <param name="extendedInfo">A text description for the error if success is false.</param>
        /// <param name="matchInfo">The info for the newly joined match.</param>
        public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (LogFilter.logDebug) { Debug.LogFormat("NetworkManager OnMatchJoined Success:{0}, ExtendedInfo:{1}, matchInfo:{2}", success, extendedInfo, matchInfo); }

            if (success)
                StartClient(matchInfo);
        }

        /// <summary>
        /// Callback that happens when a NetworkMatch.DestroyMatch request has been processed on the server.        /// </summary>
        /// <param name="success">Indicates if the request succeeded.</param>
        /// <param name="extendedInfo">A text description for the error if success is false.</param>
        public virtual void OnDestroyMatch(bool success, string extendedInfo)
        {
            if (LogFilter.logDebug) { Debug.LogFormat("NetworkManager OnDestroyMatch Success:{0}, ExtendedInfo:{1}", success, extendedInfo); }
        }

        /// <summary>
        /// Callback that happens when a NetworkMatch.DropConnection match request has been processed on the server.
        /// </summary>
        /// <param name="success">Indicates if the request succeeded.</param>
        /// <param name="extendedInfo">A text description for the error if success is false.</param>
        public virtual void OnDropConnection(bool success, string extendedInfo)
        {
            if (LogFilter.logDebug) { Debug.LogFormat("NetworkManager OnDropConnection Success:{0}, ExtendedInfo:{1}", success, extendedInfo); }
        }

        /// <summary>
        /// Callback that happens when a NetworkMatch.SetMatchAttributes has been processed on the server.
        /// </summary>
        /// <param name="success">Indicates if the request succeeded.</param>
        /// <param name="extendedInfo">A text description for the error if success is false.</param>
        public virtual void OnSetMatchAttributes(bool success, string extendedInfo)
        {
            if (LogFilter.logDebug) { Debug.LogFormat("NetworkManager OnSetMatchAttributes Success:{0}, ExtendedInfo:{1}", success, extendedInfo); }
        }
    }
}
