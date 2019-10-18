using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Networking
{
    /// <summary>
    /// A structure that contains data from a NetworkDiscovery server broadcast.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public struct NetworkBroadcastResult
    {
        /// <summary>
        /// The IP address of the server that broadcasts this data.
        /// </summary>
        public string serverAddress;
        /// <summary>
        /// The data broadcast by the server.
        /// </summary>
        public byte[] broadcastData;
    }

    /// <summary>
    /// The NetworkDiscovery component allows Unity games to find each other on a local network. It can broadcast presence and listen for broadcasts, and optionally join matching games using the NetworkManager.
    /// <para>This component can run in server mode (by calling StartAsServer) where it broadcasts to other computers on the local network, or in client mode (by calling StartAsClient) where it listens for broadcasts from a server. This class should be override to receive calls from OnReceivedBroadcast.</para>
    /// <para><b>Note :</b> Do not use void Update() in a class that inherits from NetworkDiscovery. If needed, you must override it and call base.Update() instead.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    /// using System.Collections;
    ///
    /// public class OverriddenNetworkDiscovery : NetworkDiscovery
    /// {
    ///    public override void OnReceivedBroadcast(string fromAddress, string data)
    ///    {
    ///        NetworkManager.singleton.networkAddress = fromAddress;
    ///        NetworkManager.singleton.StartClient();
    ///    }
    /// }
    /// </code>
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkDiscovery")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkDiscovery : MonoBehaviour
    {
        const int k_MaxBroadcastMsgSize = 1024;

        // config data
        [SerializeField]
        int m_BroadcastPort = 47777;

        [SerializeField]
        int m_BroadcastKey = 2222;

        [SerializeField]
        int m_BroadcastVersion = 1;

        [SerializeField]
        int m_BroadcastSubVersion = 1;

        [SerializeField]
        int m_BroadcastInterval = 1000;

        [SerializeField]
        bool m_UseNetworkManager = false;

        [SerializeField]
        string m_BroadcastData = "HELLO";

        [SerializeField]
        bool m_ShowGUI = true;

        [SerializeField]
        int m_OffsetX;

        [SerializeField]
        int m_OffsetY;

        // runtime data
        int m_HostId = -1;
        bool m_Running;

        bool m_IsServer;
        bool m_IsClient;

        byte[] m_MsgOutBuffer;
        byte[] m_MsgInBuffer;
        HostTopology m_DefaultTopology;
        Dictionary<string, NetworkBroadcastResult> m_BroadcastsReceived;

        /// <summary>
        /// The network port to broadcast on and listen to.
        /// </summary>
        public int broadcastPort
        {
            get { return m_BroadcastPort; }
            set { m_BroadcastPort = value; }
        }

        /// <summary>
        /// A key to identify this application in broadcasts.
        /// </summary>
        public int broadcastKey
        {
            get { return m_BroadcastKey; }
            set { m_BroadcastKey = value; }
        }

        /// <summary>
        /// The version of the application to broadcast. This is used to match versions of the same application.
        /// </summary>
        public int broadcastVersion
        {
            get { return m_BroadcastVersion; }
            set { m_BroadcastVersion = value; }
        }

        /// <summary>
        /// The sub-version of the application to broadcast. This is used to match versions of the same application.
        /// </summary>
        public int broadcastSubVersion
        {
            get { return m_BroadcastSubVersion; }
            set { m_BroadcastSubVersion = value; }
        }

        /// <summary>
        /// How often in milliseconds to broadcast when running as a server.
        /// </summary>
        public int broadcastInterval
        {
            get { return m_BroadcastInterval; }
            set { m_BroadcastInterval = value; }
        }

        /// <summary>
        /// True to integrate with the NetworkManager.
        /// <para>When running as a server, this will include the NetworkManager's address in broadcast messages. When running as a client, this will be able to join matching games found by using the NetworkManager.</para>
        /// </summary>
        public bool useNetworkManager
        {
            get { return m_UseNetworkManager; }
            set { m_UseNetworkManager = value; }
        }

        /// <summary>
        /// The data to include in the broadcast message when running as a server.
        /// <para>If using NetworkManager integration, this will be overriden with the NetworkManager's address.</para>
        /// </summary>
        public string broadcastData
        {
            get { return m_BroadcastData; }
            set
            {
                m_BroadcastData = value;
                m_MsgOutBuffer = StringToBytes(m_BroadcastData);
                if (m_UseNetworkManager)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery broadcast data changed while using NetworkManager. This can prevent clients from finding the server. The format of the broadcast data must be 'NetworkManager:IPAddress:Port'."); }
                }
            }
        }

        /// <summary>
        /// True to draw the default Broacast control UI.
        /// </summary>
        public bool showGUI
        {
            get { return m_ShowGUI; }
            set { m_ShowGUI = value; }
        }

        /// <summary>
        /// The horizontal offset of the GUI if active.
        /// </summary>
        public int offsetX
        {
            get { return m_OffsetX; }
            set { m_OffsetX = value; }
        }

        /// <summary>
        /// The vertical offset of the GUI if active.
        /// </summary>
        public int offsetY
        {
            get { return m_OffsetY; }
            set { m_OffsetY = value; }
        }

        /// <summary>
        /// The TransportLayer hostId being used (read-only).
        /// </summary>
        public int hostId
        {
            get { return m_HostId; }
            set { m_HostId = value; }
        }

        /// <summary>
        /// True is broadcasting or listening (read-only).
        /// </summary>
        public bool running
        {
            get { return m_Running; }
            set { m_Running = value; }
        }

        /// <summary>
        /// True if running in server mode (read-only).
        /// </summary>
        public bool isServer
        {
            get { return m_IsServer; }
            set { m_IsServer = value; }
        }

        /// <summary>
        /// True if running in client mode (read-only).
        /// </summary>
        public bool isClient
        {
            get { return m_IsClient; }
            set { m_IsClient = value; }
        }

        /// <summary>
        /// A dictionary of broadcasts received from servers.
        /// <para>The key is the server address, and the value is a NetworkBroadcastResult object that contains the data sent by the server.</para>
        /// </summary>
        public Dictionary<string, NetworkBroadcastResult> broadcastsReceived
        {
            get { return m_BroadcastsReceived; }
        }

        static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Initializes the NetworkDiscovery component.
        /// </summary>
        /// <returns>Return true if the network port was available.</returns>
        public bool Initialize()
        {
            if (m_BroadcastData.Length >= k_MaxBroadcastMsgSize)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + k_MaxBroadcastMsgSize); }
                return false;
            }

            if (!NetworkManager.activeTransport.IsStarted)
            {
                NetworkManager.activeTransport.Init();
            }

            if (m_UseNetworkManager && NetworkManager.singleton != null)
            {
                m_BroadcastData = "NetworkManager:" + NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
                if (LogFilter.logInfo) { Debug.Log("NetworkDiscovery set broadcast data to:" + m_BroadcastData); }
            }

            m_MsgOutBuffer = StringToBytes(m_BroadcastData);
            m_MsgInBuffer = new byte[k_MaxBroadcastMsgSize];
            m_BroadcastsReceived = new Dictionary<string, NetworkBroadcastResult>();

            ConnectionConfig cc = new ConnectionConfig();
            cc.AddChannel(QosType.Unreliable);
            m_DefaultTopology = new HostTopology(cc, 1);

            if (m_IsServer)
                StartAsServer();

            if (m_IsClient)
                StartAsClient();

            return true;
        }

        /// <summary>
        /// Starts listening for broadcasts messages.
        /// </summary>
        /// <returns>True is able to listen.</returns>
        // listen for broadcasts
        public bool StartAsClient()
        {
            if (m_HostId != -1 || m_Running)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery StartAsClient already started"); }
                return false;
            }

            if (m_MsgInBuffer == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsClient, NetworkDiscovery is not initialized"); }
                return false;
            }

            m_HostId = NetworkManager.activeTransport.AddHost(m_DefaultTopology, m_BroadcastPort, null);
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsClient - addHost failed"); }
                return false;
            }

            NetworkTransport.SetMulticastLock(true);

            byte error;
            NetworkManager.activeTransport.SetBroadcastCredentials(m_HostId, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, out error);

            m_Running = true;
            m_IsClient = true;
            if (LogFilter.logDebug) { Debug.Log("StartAsClient Discovery listening"); }
            return true;
        }

        /// <summary>
        /// Starts sending broadcast messages.
        /// </summary>
        /// <returns>True is able to broadcast.</returns>
        // perform actual broadcasts
        public bool StartAsServer()
        {
            if (m_HostId != -1 || m_Running)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery StartAsServer already started"); }
                return false;
            }

            m_HostId = NetworkManager.activeTransport.AddHost(m_DefaultTopology, 0, null);
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsServer - addHost failed"); }
                return false;
            }

            NetworkTransport.SetMulticastLock(true);

            byte err;
            if (!NetworkManager.activeTransport.StartBroadcastDiscovery(m_HostId, m_BroadcastPort, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, m_MsgOutBuffer, m_MsgOutBuffer.Length, m_BroadcastInterval, out err))
            {
                NetworkTransport.RemoveHost(m_HostId);
                m_HostId = -1;
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + err); }
                return false;
            }

            m_Running = true;
            m_IsServer = true;
            if (LogFilter.logDebug) { Debug.Log("StartAsServer Discovery broadcasting"); }
            DontDestroyOnLoad(gameObject);
            return true;
        }

        /// <summary>
        /// Stops listening and broadcasting.
        /// </summary>
        public void StopBroadcast()
        {
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StopBroadcast not initialized"); }
                return;
            }

            if (!m_Running)
            {
                Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
                return;
            }
            if (m_IsServer)
            {
                NetworkManager.activeTransport.StopBroadcastDiscovery();
            }

            NetworkManager.activeTransport.RemoveHost(m_HostId);
            NetworkTransport.SetMulticastLock(false);
            m_HostId = -1;
            m_Running = false;
            m_IsServer = false;
            m_IsClient = false;
            m_MsgInBuffer = null;
            m_BroadcastsReceived = null;
            if (LogFilter.logDebug) { Debug.Log("Stopped Discovery broadcasting"); }
        }

        void Update()
        {
            if (m_HostId == -1)
                return;

            if (m_IsServer)
                return;

            NetworkEventType networkEvent;
            do
            {
                int connectionId;
                int channelId;
                int receivedSize;
                byte error;
                networkEvent = NetworkManager.activeTransport.ReceiveFromHost(m_HostId, out connectionId, out channelId, m_MsgInBuffer, k_MaxBroadcastMsgSize, out receivedSize, out error);

                if (networkEvent == NetworkEventType.BroadcastEvent)
                {
                    NetworkManager.activeTransport.GetBroadcastConnectionMessage(m_HostId, m_MsgInBuffer, k_MaxBroadcastMsgSize, out receivedSize, out error);

                    string senderAddr;
                    int senderPort;
                    NetworkManager.activeTransport.GetBroadcastConnectionInfo(m_HostId, out senderAddr, out senderPort, out error);

                    var recv = new NetworkBroadcastResult();
                    recv.serverAddress = senderAddr;
                    recv.broadcastData = new byte[receivedSize];
                    Buffer.BlockCopy(m_MsgInBuffer, 0, recv.broadcastData, 0, receivedSize);
                    m_BroadcastsReceived[senderAddr] = recv;

                    OnReceivedBroadcast(senderAddr, BytesToString(m_MsgInBuffer));
                }
            }
            while (networkEvent != NetworkEventType.Nothing);
        }

        void OnDestroy()
        {
            if (m_IsServer && m_Running && m_HostId != -1)
            {
                NetworkManager.activeTransport.StopBroadcastDiscovery();
                NetworkManager.activeTransport.RemoveHost(m_HostId);
            }

            if (m_IsClient && m_Running && m_HostId != -1)
            {
                NetworkManager.activeTransport.RemoveHost(m_HostId);
            }

            if (m_Running)
                NetworkTransport.SetMulticastLock(false);
        }

        /// <summary>
        /// This is a virtual function that can be implemented to handle broadcast messages when running as a client.
        /// </summary>
        /// <param name="fromAddress">The IP address of the server.</param>
        /// <param name="data">The data broadcast by the server.</param>
        public virtual void OnReceivedBroadcast(string fromAddress, string data)
        {
            //Debug.Log("Got broadcast from [" + fromAddress + "] " + data);
        }

        void OnGUI()
        {
            if (!m_ShowGUI)
                return;

            int xpos = 10 + m_OffsetX;
            int ypos = 40 + m_OffsetY;
            const int spacing = 24;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GUI.Box(new Rect(xpos, ypos, 200, 20), "( WebGL cannot broadcast )");
                return;
            }

            if (m_MsgInBuffer == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Initialize Broadcast"))
                {
                    Initialize();
                }
                return;
            }
            string suffix = "";
            if (m_IsServer)
                suffix = " (server)";
            if (m_IsClient)
                suffix = " (client)";

            GUI.Label(new Rect(xpos, ypos, 200, 20), "initialized" + suffix);
            ypos += spacing;

            if (m_Running)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop"))
                {
                    StopBroadcast();
                }
                ypos += spacing;

                if (m_BroadcastsReceived != null)
                {
                    foreach (var addr in m_BroadcastsReceived.Keys)
                    {
                        var value = m_BroadcastsReceived[addr];
                        if (GUI.Button(new Rect(xpos, ypos + 20, 200, 20), "Game at " + addr) && m_UseNetworkManager)
                        {
                            string dataString = BytesToString(value.broadcastData);
                            var items = dataString.Split(':');
                            if (items.Length == 3 && items[0] == "NetworkManager")
                            {
                                if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
                                {
                                    NetworkManager.singleton.networkAddress = items[1];
                                    NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
                                    NetworkManager.singleton.StartClient();
                                }
                            }
                        }
                        ypos += spacing;
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Start Broadcasting"))
                {
                    StartAsServer();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Listen for Broadcast"))
                {
                    StartAsClient();
                }
                ypos += spacing;
            }
        }
    }
}
