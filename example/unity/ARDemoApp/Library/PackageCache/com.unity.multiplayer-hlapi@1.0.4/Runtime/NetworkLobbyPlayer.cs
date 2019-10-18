
using System;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This component works in conjunction with the NetworkLobbyManager to make up the multiplayer lobby system.
    /// <para>The LobbyPrefab object of the NetworkLobbyManager must have this component on it. This component holds basic lobby player data required for the lobby to function. Game specific data for lobby players can be put in other components on the LobbyPrefab or in scripts derived from NetworkLobbyPlayer.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkLobbyPlayer")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkLobbyPlayer : NetworkBehaviour
    {
        /// <summary>
        /// This flag controls whether the default UI is shown for the lobby player.
        /// <para>As this UI is rendered using the old GUI system, it is only recommended for testing purposes.</para>
        /// </summary>
        [Tooltip("Enable to show the default lobby GUI for this player.")]
        [SerializeField] public bool ShowLobbyGUI = true;

        byte m_Slot;
        bool m_ReadyToBegin;

        /// <summary>
        /// The slot within the lobby that this player inhabits.
        /// <para>Lobby slots are global for the game - each player has a unique slotId.</para>
        /// </summary>
        public byte slot { get { return m_Slot; } set { m_Slot = value; }}
        /// <summary>
        /// This is a flag that control whether this player is ready for the game to begin.
        /// <para>When all players are ready to begin, the game will start. This should not be set directly, the SendReadyToBeginMessage function should be called on the client to set it on the server.</para>
        /// </summary>
        public bool readyToBegin { get { return m_ReadyToBegin; } set { m_ReadyToBegin = value; } }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public override void OnStartClient()
        {
            var lobby = GetLobbyManager();
            if (lobby)
            {
                lobby.lobbySlots[m_Slot] = this;
                m_ReadyToBegin = false;
                OnClientEnterLobby();
            }
            else
            {
                Debug.LogError("LobbyPlayer could not find a NetworkLobbyManager. The LobbyPlayer requires a NetworkLobbyManager object to function. Make sure that there is one in the scene.");
            }
        }

        /// <summary>
        /// This is used on clients to tell the server that this player is ready for the game to begin.
        /// </summary>
        public void SendReadyToBeginMessage()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyPlayer SendReadyToBeginMessage"); }

            var lobby = GetLobbyManager();
            if (lobby)
            {
                var msg = new LobbyReadyToBeginMessage();
                msg.slotId = (byte)playerControllerId;
                msg.readyState = true;
                lobby.client.Send(MsgType.LobbyReadyToBegin, msg);
            }
        }

        /// <summary>
        /// This is used on clients to tell the server that this player is not ready for the game to begin.
        /// </summary>
        public void SendNotReadyToBeginMessage()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyPlayer SendReadyToBeginMessage"); }

            var lobby = GetLobbyManager();
            if (lobby)
            {
                var msg = new LobbyReadyToBeginMessage();
                msg.slotId = (byte)playerControllerId;
                msg.readyState = false;
                lobby.client.Send(MsgType.LobbyReadyToBegin, msg);
            }
        }

        /// <summary>
        /// This is used on clients to tell the server that the client has switched from the lobby to the GameScene and is ready to play.
        /// <para>This message triggers the server to replace the lobby player with the game player.</para>
        /// </summary>
        public void SendSceneLoadedMessage()
        {
            if (LogFilter.logDebug) { Debug.Log("NetworkLobbyPlayer SendSceneLoadedMessage"); }

            var lobby = GetLobbyManager();
            if (lobby)
            {
                var msg = new IntegerMessage(playerControllerId);
                lobby.client.Send(MsgType.LobbySceneLoaded, msg);
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var lobby = GetLobbyManager();
            if (lobby)
            {
                // dont even try this in the startup scene
                // Should we check if the LoadSceneMode is Single or Additive??
                // Can the lobby scene be loaded Additively??
                string loadedSceneName = scene.name;
                if (loadedSceneName == lobby.lobbyScene)
                {
                    return;
                }
            }

            if (isLocalPlayer)
            {
                SendSceneLoadedMessage();
            }
        }

        NetworkLobbyManager GetLobbyManager()
        {
            return NetworkManager.singleton as NetworkLobbyManager;
        }

        /// <summary>
        /// This removes this player from the lobby.
        /// <para>This player object will be destroyed - on the server and on all clients.</para>
        /// </summary>
        public void RemovePlayer()
        {
            if (isLocalPlayer && !m_ReadyToBegin)
            {
                if (LogFilter.logDebug) { Debug.Log("NetworkLobbyPlayer RemovePlayer"); }

                ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
            }
        }

        // ------------------------ callbacks ------------------------

        /// <summary>
        /// This is a hook that is invoked on all player objects when entering the lobby.
        /// <para>Note: isLocalPlayer is not guaranteed to be set until OnStartLocalPlayer is called.</para>
        /// </summary>
        public virtual void OnClientEnterLobby()
        {
        }

        /// <summary>
        /// This is a hook that is invoked on all player objects when exiting the lobby.
        /// </summary>
        public virtual void OnClientExitLobby()
        {
        }

        /// <summary>
        /// This is a hook that is invoked on clients when a LobbyPlayer switches between ready or not ready.
        /// <para>This function is called when the a client player calls SendReadyToBeginMessage() or SendNotReadyToBeginMessage().</para>
        /// </summary>
        /// <param name="readyState">Whether the player is ready or not.</param>
        public virtual void OnClientReady(bool readyState)
        {
        }

        // ------------------------ Custom Serialization ------------------------

        public override bool OnSerialize(NetworkWriter writer, bool initialState)
        {
            // dirty flag
            writer.WritePackedUInt32(1);

            writer.Write(m_Slot);
            writer.Write(m_ReadyToBegin);
            return true;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            var dirty = reader.ReadPackedUInt32();
            if (dirty == 0)
                return;

            m_Slot = reader.ReadByte();
            m_ReadyToBegin = reader.ReadBoolean();
        }

        // ------------------------ optional UI ------------------------

        void OnGUI()
        {
            if (!ShowLobbyGUI)
                return;

            var lobby = GetLobbyManager();
            if (lobby)
            {
                if (!lobby.showLobbyGUI)
                    return;

                string loadedSceneName = SceneManager.GetSceneAt(0).name;
                if (loadedSceneName != lobby.lobbyScene)
                    return;
            }

            Rect rec = new Rect(100 + m_Slot * 100, 200, 90, 20);

            if (isLocalPlayer)
            {
                string youStr;
                if (m_ReadyToBegin)
                {
                    youStr = "(Ready)";
                }
                else
                {
                    youStr = "(Not Ready)";
                }
                GUI.Label(rec, youStr);

                if (m_ReadyToBegin)
                {
                    rec.y += 25;
                    if (GUI.Button(rec, "STOP"))
                    {
                        SendNotReadyToBeginMessage();
                    }
                }
                else
                {
                    rec.y += 25;
                    if (GUI.Button(rec, "START"))
                    {
                        SendReadyToBeginMessage();
                    }

                    rec.y += 25;
                    if (GUI.Button(rec, "Remove"))
                    {
                        ClientScene.RemovePlayer(GetComponent<NetworkIdentity>().playerControllerId);
                    }
                }
            }
            else
            {
                GUI.Label(rec, "Player [" + netId + "]");
                rec.y += 25;
                GUI.Label(rec, "Ready [" + m_ReadyToBegin + "]");
            }
        }
    }
}
