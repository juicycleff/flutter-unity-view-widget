using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking.NetworkSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Networking
{
    /// <summary>
    /// The NetworkIdentity identifies objects across the network, between server and clients. Its primary data is a NetworkInstanceId which is allocated by the server and then set on clients. This is used in network communications to be able to lookup game objects on different machines.
    /// <para>The NetworkIdentity is used to synchronize information in the object with the network. Only the server should create instances of objects which have NetworkIdentity as otherwise they will not be properly connected to the system.</para>
    /// <para>For complex objects with a hierarchy of subcomponents, the NetworkIdentity must be on the root of the hierarchy. It is not supported to have multiple NetworkIdentity components on subcomponents of a hierarchy.</para>
    /// <para>NetworkBehaviour scripts require a NetworkIdentity on the game object to be able to function.</para>
    /// <para>The NetworkIdentity manages the dirty state of the NetworkBehaviours of the object. When it discovers that NetworkBehaviours are dirty, it causes an update packet to be created and sent to clients.</para>
    /// <para>The flow for serialization updates managed by the NetworkIdentity is:</para>
    /// <para>* Each NetworkBehaviour has a dirty mask. This mask is available inside OnSerialize as syncVarDirtyBits</para>
    /// <para>* Each SyncVar in a NetworkBehaviour script is assigned a bit in the dirty mask.</para>
    /// <para>* Changing the value of SyncVars causes the bit for that SyncVar to be set in the dirty mask</para>
    /// <para>* Alternatively, calling SetDirtyBit() writes directly to the dirty mask</para>
    /// <para>* NetworkIdentity objects are checked on the server as part of it&apos;s update loop</para>
    /// <para>* If any NetworkBehaviours on a NetworkIdentity are dirty, then an UpdateVars packet is created for that object</para>
    /// <para>* The UpdateVars packet is populated by calling OnSerialize on each NetworkBehaviour on the object</para>
    /// <para>* NetworkBehaviours that are NOT dirty write a zero to the packet for their dirty bits</para>
    /// <para>* NetworkBehaviours that are dirty write their dirty mask, then the values for the SyncVars that have changed</para>
    /// <para>* If OnSerialize returns true for a NetworkBehaviour, the dirty mask is reset for that NetworkBehaviour, so it will not send again until its value changes.</para>
    /// <para>* The UpdateVars packet is sent to ready clients that are observing the object</para>
    /// <para>On the client:</para>
    /// <para>* an UpdateVars packet is received for an object</para>
    /// <para>* The OnDeserialize function is called for each NetworkBehaviour script on the object</para>
    /// <para>* Each NetworkBehaviour script on the object reads a dirty mask.</para>
    /// <para>* If the dirty mask for a NetworkBehaviour is zero, the OnDeserialize functions returns without reading any more</para>
    /// <para>* If the dirty mask is non-zero value, then the OnDeserialize function reads the values for the SyncVars that correspond to the dirty bits that are set</para>
    /// <para>* If there are SyncVar hook functions, those are invoked with the value read from the stream.</para>
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkIdentity")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public sealed class NetworkIdentity : MonoBehaviour
    {
        // configuration
        [SerializeField] NetworkSceneId m_SceneId;
        [SerializeField] NetworkHash128 m_AssetId;
        [SerializeField] bool           m_ServerOnly;
        [SerializeField] bool           m_LocalPlayerAuthority;

        // runtime data
        bool                        m_IsClient;
        bool                        m_IsServer;
        bool                        m_HasAuthority;

        NetworkInstanceId           m_NetId;
        bool                        m_IsLocalPlayer;
        NetworkConnection           m_ConnectionToServer;
        NetworkConnection           m_ConnectionToClient;
        short                       m_PlayerId = -1;
        NetworkBehaviour[]          m_NetworkBehaviours;

        // there is a list AND a hashSet of connections, for fast verification of dupes, but the main operation is iteration over the list.
        HashSet<int>                m_ObserverConnections;
        List<NetworkConnection>     m_Observers;
        NetworkConnection           m_ClientAuthorityOwner;

        // member used to mark a identity for future reset
        // check MarkForReset for more information.
        bool                        m_Reset = false;
        // properties
        /// <summary>
        /// Returns true if running as a client and this object was spawned by a server.
        /// </summary>
        public bool isClient        { get { return m_IsClient; } }

        /// <summary>
        /// Returns true if running as a server, which spawned the object.
        /// </summary>
        public bool isServer
        {
            get
            {
                // if server has stopped, should not still return true here
                return m_IsServer && NetworkServer.active;
            }
        }

        /// <summary>
        /// This returns true if this object is the authoritative version of the object in the distributed network application.
        /// <para>This value is determined at runtime, as opposed to localPlayerAuthority which is set on the prefab. For most objects, authority is held by the server / host. For objects with localPlayerAuthority set, authority is held by the client of that player.</para>
        /// <para>For objects that had their authority set by AssignClientAuthority on the server, this will be true on the client that owns the object. NOT on other clients.</para>
        /// </summary>
        public bool hasAuthority    { get { return m_HasAuthority; } }

        /// <summary>
        /// Unique identifier for this particular object instance, used for tracking objects between networked clients and the server.
        /// <para>This is a unique identifier for this particular GameObject instance. Use it to track GameObjects between networked clients and the server.</para>
        /// <code>
        /// //For this example to work, attach a NetworkIdentity component to your GameObject.
        /// //Then, create a new empty GameObject and drag it under your NetworkIdentity GameObject in the Hierarchy. This makes it the child of the GameObject. //Next, attach a TextMesh component to the child GameObject. You can then place this TextMesh GameObject to be above your GameObject in the Scene.
        /// //Attach this script to the parent GameObject, and it changes the text of the TextMesh to the identity of your GameObject.
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class NetworkIdentityNetID : MonoBehaviour
        /// {
        ///    NetworkIdentity m_Identity;
        ///    //This is a TextMesh component that you attach to the child of the NetworkIdentity GameObject
        ///    TextMesh m_TextMesh;
        ///
        ///    void Start()
        ///    {
        ///        //Fetch the NetworkIdentity component of the GameObject
        ///        m_Identity = GetComponent&lt;<see cref="NetworkIdentity">NetworkIdentity</see>&gt;();
        ///        //Enter the child of your GameObject (the GameObject with the TextMesh you attach)
        ///        //Fetch the TextMesh component of it
        ///        m_TextMesh = GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        ///        //Change the Text of the TextMesh to show the netId
        ///        m_TextMesh.text = "ID : " + m_Identity.netId;
        ///    }
        /// }
        /// </code>
        /// </summary>
        public NetworkInstanceId netId { get { return m_NetId; } }
        /// <summary>
        /// A unique identifier for NetworkIdentity objects within a scene.
        /// <para>This is used for spawning scene objects on clients.</para>
        /// </summary>
        public NetworkSceneId sceneId { get { return m_SceneId; } }
        /// <summary>
        /// Flag to make this object only exist when the game is running as a server (or host).
        /// </summary>
        public bool serverOnly { get { return m_ServerOnly; } set { m_ServerOnly = value; } }
        /// <summary>
        /// localPlayerAuthority means that the client of the "owning" player has authority over their own player object.
        /// <para>Authority for this object will be on the player's client. So hasAuthority will be true on that client - and false on the server and on other clients.</para>
        /// </summary>
        public bool localPlayerAuthority { get { return m_LocalPlayerAuthority; } set { m_LocalPlayerAuthority = value; } }
        /// <summary>
        /// The client that has authority for this object. This will be null if no client has authority.
        /// <para>This is set for player objects with localPlayerAuthority, and for objects set with AssignClientAuthority, and spawned with SpawnWithClientAuthority.</para>
        /// </summary>
        public NetworkConnection clientAuthorityOwner { get { return m_ClientAuthorityOwner; }}

        /// <summary>
        /// Unique identifier used to find the source assets when server spawns the on clients.
        /// </summary>
        public NetworkHash128 assetId
        {
            get
            {
#if UNITY_EDITOR
                // This is important because sometimes OnValidate does not run (like when adding view to prefab with no child links)
                if (!m_AssetId.IsValid())
                    SetupIDs();
#endif
                return m_AssetId;
            }
        }
        internal void SetDynamicAssetId(NetworkHash128 newAssetId)
        {
            if (!m_AssetId.IsValid() || m_AssetId.Equals(newAssetId))
            {
                m_AssetId = newAssetId;
            }
            else
            {
                if (LogFilter.logWarn) { Debug.LogWarning("SetDynamicAssetId object already has an assetId <" + m_AssetId + ">"); }
            }
        }

        // used when adding players
        internal void SetClientOwner(NetworkConnection conn)
        {
            if (m_ClientAuthorityOwner != null)
            {
                if (LogFilter.logError) { Debug.LogError("SetClientOwner m_ClientAuthorityOwner already set!"); }
            }
            m_ClientAuthorityOwner = conn;
            m_ClientAuthorityOwner.AddOwnedObject(this);
        }

        // used during dispose after disconnect
        internal void ClearClientOwner()
        {
            m_ClientAuthorityOwner = null;
        }

        internal void ForceAuthority(bool authority)
        {
            if (m_HasAuthority == authority)
            {
                return;
            }

            m_HasAuthority = authority;
            if (authority)
            {
                OnStartAuthority();
            }
            else
            {
                OnStopAuthority();
            }
        }

        /// <summary>
        /// This returns true if this object is the one that represents the player on the local machine.
        /// <para>This is set when the server has spawned an object for this particular client.</para>
        /// </summary>
        public bool isLocalPlayer { get { return m_IsLocalPlayer; } }
        /// <summary>
        /// The id of the player associated with this GameObject.
        /// <para>This is only valid if this GameObject is for a local player.</para>
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public short playerControllerId { get { return m_PlayerId; } }
        /// <summary>
        /// The UConnection associated with this NetworkIdentity. This is only valid for player objects on a local client.
        /// </summary>
        public NetworkConnection connectionToServer { get { return m_ConnectionToServer; } }
        /// <summary>
        /// The connection associated with this <see cref="NetworkIdentity">NetworkIdentity.</see> This is only valid for player objects on the server.
        /// <para>Use it to return details such as the connection&apos;s identity, IP address and ready status.</para>
        /// <code>
        /// //For this example to work, attach a NetworkIdentity component to your GameObject.
        /// //Make sure your Scene has a NetworkManager and NetworkManagerHUD
        /// //Attach this script to the GameObject, and it outputs the connection of your GameObject to the console.
        ///
        /// using System.Collections;
        /// using System.Collections.Generic;
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class NetworkIdentityNetID : MonoBehaviour
        /// {
        ///    NetworkIdentity m_Identity;
        ///    //This is a TextMesh component that you attach to the child of the NetworkIdentity GameObject
        ///
        ///    void Start()
        ///    {
        ///        //Fetch the NetworkIdentity component of the GameObject
        ///        m_Identity = GetComponent&lt;NetworkIdentity&gt;();
        ///        //Output to the console the connection associated with this NetworkIdentity
        ///        Debug.Log("Connection : " + m_Identity.connectionToClient);
        ///    }
        /// }
        /// </code>
        /// </summary>
        public NetworkConnection connectionToClient { get { return m_ConnectionToClient; } }

        /// <summary>
        /// The set of network connections (players) that can see this object.
        /// </summary>
        public ReadOnlyCollection<NetworkConnection> observers
        {
            get
            {
                if (m_Observers == null)
                    return null;

                return new ReadOnlyCollection<NetworkConnection>(m_Observers);
            }
        }

        static uint s_NextNetworkId = 1;
        static internal NetworkInstanceId GetNextNetworkId()
        {
            uint newId = s_NextNetworkId;
            s_NextNetworkId += 1;
            return new NetworkInstanceId(newId);
        }

        static NetworkWriter s_UpdateWriter = new NetworkWriter();

        void CacheBehaviours()
        {
            if (m_NetworkBehaviours == null)
            {
                m_NetworkBehaviours = GetComponents<NetworkBehaviour>();
            }
        }

        /// <summary>
        /// The delegate type for the clientAuthorityCallback.
        /// </summary>
        /// <param name="conn">The network connection that is gaining or losing authority.</param>
        /// <param name="uv">The object whose client authority status is being changed.</param>
        /// <param name="authorityState">The new state of client authority of the object for the connection.</param>
        public delegate void ClientAuthorityCallback(NetworkConnection conn, NetworkIdentity uv, bool authorityState);
        /// <summary>
        /// A callback that can be populated to be notified when the client-authority state of objects changes.
        /// <para>Whenever an object is spawned using SpawnWithClientAuthority, or the client authority status of an object is changed with AssignClientAuthority or RemoveClientAuthority, then this callback will be invoked.</para>
        /// <para>This callback is used by the NetworkMigrationManager to distribute client authority state to peers for host migration. If the NetworkMigrationManager is not being used, this callback does not need to be populated.</para>
        /// </summary>
        public static ClientAuthorityCallback clientAuthorityCallback;

        static internal void AddNetworkId(uint id)
        {
            if (id >= s_NextNetworkId)
            {
                s_NextNetworkId = (uint)(id + 1);
            }
        }

        // only used during spawning on clients to set the identity.
        internal void SetNetworkInstanceId(NetworkInstanceId newNetId)
        {
            m_NetId = newNetId;
            if (newNetId.Value == 0)
            {
                m_IsServer = false;
            }
        }

        /// <summary>
        /// Force the scene ID to a specific value.
        /// <para>This can be used to fix an invalid scene ID. If you process all the NetworkIdentity components in a scene you can assign them new values starting from 1.</para>
        /// </summary>
        /// <param name="newSceneId">The new scene ID.</param>
        // only used when fixing duplicate scene IDs duing post-processing
        public void ForceSceneId(int newSceneId)
        {
            m_SceneId = new NetworkSceneId((uint)newSceneId);
        }

        // only used in SetLocalObject
        internal void UpdateClientServer(bool isClientFlag, bool isServerFlag)
        {
            m_IsClient |= isClientFlag;
            m_IsServer |= isServerFlag;
        }

        // used when the player object for a connection changes
        internal void SetNotLocalPlayer()
        {
            m_IsLocalPlayer = false;

            if (NetworkServer.active && NetworkServer.localClientActive)
            {
                // dont change authority for objects on the host
                return;
            }
            m_HasAuthority = false;
        }

        // this is used when a connection is destroyed, since the "observers" property is read-only
        internal void RemoveObserverInternal(NetworkConnection conn)
        {
            if (m_Observers != null)
            {
                m_Observers.Remove(conn);
                m_ObserverConnections.Remove(conn.connectionId);
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (m_ServerOnly && m_LocalPlayerAuthority)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Disabling Local Player Authority for " + gameObject + " because it is server-only."); }
                m_LocalPlayerAuthority = false;
            }

            SetupIDs();
        }

        void AssignAssetID(GameObject prefab)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            m_AssetId = NetworkHash128.Parse(AssetDatabase.AssetPathToGUID(path));
        }

        bool ThisIsAPrefab()
        {
            return PrefabUtility.IsPartOfPrefabAsset(gameObject);
        }

        bool ThisIsASceneObjectWithThatReferencesPrefabAsset(out GameObject prefab)
        {
            prefab = null;
            if (!PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject))
                return false;
            prefab = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
            if (prefab == null)
            {
                if (LogFilter.logError) { Debug.LogError("Failed to find prefab parent for scene object [name:" + gameObject.name + "]"); }
                return false;
            }
            return true;
        }

        void SetupIDs()
        {
            GameObject prefab;
            if (ThisIsAPrefab())
            {
                ForceSceneId(0);
                AssignAssetID(gameObject);
            }
            else if (ThisIsASceneObjectWithThatReferencesPrefabAsset(out prefab))
            {
                AssignAssetID(prefab);
            }
            else
            {
                m_AssetId.Reset();
            }
        }

#endif
        void OnDestroy()
        {
            if (m_IsServer && NetworkServer.active)
            {
                NetworkServer.Destroy(gameObject);
            }
        }

        internal void OnStartServer(bool allowNonZeroNetId)
        {
            if (m_IsServer)
            {
                return;
            }
            m_IsServer = true;

            if (m_LocalPlayerAuthority)
            {
                // local player on server has NO authority
                m_HasAuthority = false;
            }
            else
            {
                // enemy on server has authority
                m_HasAuthority = true;
            }

            m_Observers = new List<NetworkConnection>();
            m_ObserverConnections = new HashSet<int>();
            CacheBehaviours();

            // If the instance/net ID is invalid here then this is an object instantiated from a prefab and the server should assign a valid ID
            if (netId.IsEmpty())
            {
                m_NetId = GetNextNetworkId();
            }
            else
            {
                if (allowNonZeroNetId)
                {
                    //allowed
                }
                else
                {
                    if (LogFilter.logError) { Debug.LogError("Object has non-zero netId " + netId + " for " + gameObject); }
                    return;
                }
            }

            if (LogFilter.logDev) { Debug.Log("OnStartServer " + gameObject + " GUID:" + netId); }
            NetworkServer.instance.SetLocalObjectOnServer(netId, gameObject);

            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                try
                {
                    comp.OnStartServer();
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in OnStartServer:" + e.Message + " " + e.StackTrace);
                }
            }

            if (NetworkClient.active && NetworkServer.localClientActive)
            {
                // there will be no spawn message, so start the client here too
                ClientScene.SetLocalObject(netId, gameObject);
                OnStartClient();
            }

            if (m_HasAuthority)
            {
                OnStartAuthority();
            }
        }

        internal void OnStartClient()
        {
            if (!m_IsClient)
            {
                m_IsClient = true;
            }
            CacheBehaviours();

            if (LogFilter.logDev) { Debug.Log("OnStartClient " + gameObject + " GUID:" + netId + " localPlayerAuthority:" + localPlayerAuthority); }
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                try
                {
                    comp.PreStartClient(); // generated startup to resolve object references
                    comp.OnStartClient(); // user implemented startup
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in OnStartClient:" + e.Message + " " + e.StackTrace);
                }
            }
        }

        internal void OnStartAuthority()
        {
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                try
                {
                    comp.OnStartAuthority();
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in OnStartAuthority:" + e.Message + " " + e.StackTrace);
                }
            }
        }

        internal void OnStopAuthority()
        {
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                try
                {
                    comp.OnStopAuthority();
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in OnStopAuthority:" + e.Message + " " + e.StackTrace);
                }
            }
        }

        internal void OnSetLocalVisibility(bool vis)
        {
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                try
                {
                    comp.OnSetLocalVisibility(vis);
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in OnSetLocalVisibility:" + e.Message + " " + e.StackTrace);
                }
            }
        }

        internal bool OnCheckObserver(NetworkConnection conn)
        {
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                try
                {
                    if (!comp.OnCheckObserver(conn))
                        return false;
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception in OnCheckObserver:" + e.Message + " " + e.StackTrace);
                }
            }
            return true;
        }

        // happens on server
        internal void UNetSerializeAllVars(NetworkWriter writer)
        {
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                comp.OnSerialize(writer, true);
            }
        }

        // happens on client
        internal void HandleClientAuthority(bool authority)
        {
            if (!localPlayerAuthority)
            {
                if (LogFilter.logError) { Debug.LogError("HandleClientAuthority " + gameObject + " does not have localPlayerAuthority"); }
                return;
            }

            ForceAuthority(authority);
        }

        // helper function for Handle** functions
        bool GetInvokeComponent(int cmdHash, Type invokeClass, out NetworkBehaviour invokeComponent)
        {
            // dont use GetComponent(), already have a list - avoids an allocation
            NetworkBehaviour foundComp = null;
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                var comp = m_NetworkBehaviours[i];
                if (comp.GetType() == invokeClass || comp.GetType().IsSubclassOf(invokeClass))
                {
                    // found matching class
                    foundComp = comp;
                    break;
                }
            }
            if (foundComp == null)
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logError) { Debug.LogError("Found no behaviour for incoming [" + errorCmdName + "] on " + gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "]."); }
                invokeComponent = null;
                return false;
            }
            invokeComponent = foundComp;
            return true;
        }

        // happens on client
        internal void HandleSyncEvent(int cmdHash, NetworkReader reader)
        {
            // this doesn't use NetworkBehaviour.InvokeSyncEvent function (anymore). this method of calling is faster.
            // The hash is only looked up once, insted of twice(!) per NetworkBehaviour on the object.

            if (gameObject == null)
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("SyncEvent [" + errorCmdName + "] received for deleted object [netId=" + netId + "]"); }
                return;
            }

            // find the matching SyncEvent function and networkBehaviour class
            NetworkBehaviour.CmdDelegate invokeFunction;
            Type invokeClass;
            bool invokeFound = NetworkBehaviour.GetInvokerForHashSyncEvent(cmdHash, out invokeClass, out invokeFunction);
            if (!invokeFound)
            {
                // We don't get a valid lookup of the command name when it doesn't exist...
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logError) { Debug.LogError("Found no receiver for incoming [" + errorCmdName + "] on " + gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "]."); }
                return;
            }

            // find the right component to invoke the function on
            NetworkBehaviour invokeComponent;
            if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("SyncEvent [" + errorCmdName + "] handler not found [netId=" + netId + "]"); }
                return;
            }

            invokeFunction(invokeComponent, reader);

#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.SyncEvent, NetworkBehaviour.GetCmdHashEventName(cmdHash));
#endif
        }

        // happens on client
        internal void HandleSyncList(int cmdHash, NetworkReader reader)
        {
            // this doesn't use NetworkBehaviour.InvokSyncList function (anymore). this method of calling is faster.
            // The hash is only looked up once, insted of twice(!) per NetworkBehaviour on the object.

            if (gameObject == null)
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("SyncList [" + errorCmdName + "] received for deleted object [netId=" + netId + "]"); }
                return;
            }

            // find the matching SyncList function and networkBehaviour class
            NetworkBehaviour.CmdDelegate invokeFunction;
            Type invokeClass;
            bool invokeFound = NetworkBehaviour.GetInvokerForHashSyncList(cmdHash, out invokeClass, out invokeFunction);
            if (!invokeFound)
            {
                // We don't get a valid lookup of the command name when it doesn't exist...
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logError) { Debug.LogError("Found no receiver for incoming [" + errorCmdName + "] on " + gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "]."); }
                return;
            }

            // find the right component to invoke the function on
            NetworkBehaviour invokeComponent;
            if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("SyncList [" + errorCmdName + "] handler not found [netId=" + netId + "]"); }
                return;
            }

            invokeFunction(invokeComponent, reader);

#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.SyncList, NetworkBehaviour.GetCmdHashListName(cmdHash));
#endif
        }

        // happens on server
        internal void HandleCommand(int cmdHash, NetworkReader reader)
        {
            // this doesn't use NetworkBehaviour.InvokeCommand function (anymore). this method of calling is faster.
            // The hash is only looked up once, insted of twice(!) per NetworkBehaviour on the object.

            if (gameObject == null)
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("Command [" + errorCmdName + "] received for deleted object [netId=" + netId + "]"); }
                return;
            }

            // find the matching Command function and networkBehaviour class
            NetworkBehaviour.CmdDelegate invokeFunction;
            Type invokeClass;
            bool invokeFound = NetworkBehaviour.GetInvokerForHashCommand(cmdHash, out invokeClass, out invokeFunction);
            if (!invokeFound)
            {
                // We don't get a valid lookup of the command name when it doesn't exist...
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logError) { Debug.LogError("Found no receiver for incoming [" + errorCmdName + "] on " + gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "]."); }
                return;
            }

            // find the right component to invoke the function on
            NetworkBehaviour invokeComponent;
            if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("Command [" + errorCmdName + "] handler not found [netId=" + netId + "]"); }
                return;
            }

            invokeFunction(invokeComponent, reader);

#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.Command, NetworkBehaviour.GetCmdHashCmdName(cmdHash));
#endif
        }

        // happens on client
        internal void HandleRPC(int cmdHash, NetworkReader reader)
        {
            // this doesn't use NetworkBehaviour.InvokeClientRpc function (anymore). this method of calling is faster.
            // The hash is only looked up once, insted of twice(!) per NetworkBehaviour on the object.

            if (gameObject == null)
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("ClientRpc [" + errorCmdName + "] received for deleted object [netId=" + netId + "]"); }
                return;
            }

            // find the matching ClientRpc function and networkBehaviour class
            NetworkBehaviour.CmdDelegate invokeFunction;
            Type invokeClass;
            bool invokeFound = NetworkBehaviour.GetInvokerForHashClientRpc(cmdHash, out invokeClass, out invokeFunction);
            if (!invokeFound)
            {
                // We don't get a valid lookup of the command name when it doesn't exist...
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logError) { Debug.LogError("Found no receiver for incoming [" + errorCmdName + "] on " + gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + netId + "]."); }
                return;
            }

            // find the right component to invoke the function on
            NetworkBehaviour invokeComponent;
            if (!GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
            {
                string errorCmdName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
                if (LogFilter.logWarn) { Debug.LogWarning("ClientRpc [" + errorCmdName + "] handler not found [netId=" + netId + "]"); }
                return;
            }

            invokeFunction(invokeComponent, reader);

#if UNITY_EDITOR
            Profiler.IncrementStatIncoming(MsgType.Rpc, NetworkBehaviour.GetCmdHashRpcName(cmdHash));
#endif
        }

        // invoked by unity runtime immediately after the regular "Update()" function.
        public void UNetUpdate()
        {
            // check if any behaviours are ready to send
            uint dirtyChannelBits = 0;
            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                int channelId = comp.GetDirtyChannel();
                if (channelId != -1)
                {
                    dirtyChannelBits |= (uint)(1 << channelId);
                }
            }
            if (dirtyChannelBits == 0)
                return;

            for (int channelId = 0; channelId < NetworkServer.numChannels; channelId++)
            {
                if ((dirtyChannelBits & (uint)(1 << channelId)) != 0)
                {
                    s_UpdateWriter.StartMessage(MsgType.UpdateVars);
                    s_UpdateWriter.Write(netId);

                    bool wroteData = false;
                    short oldPos;
                    NetworkBehaviour[] behaviourOfSameChannel = GetBehavioursOfSameChannel(channelId, false);
                    for (int i = 0; i < behaviourOfSameChannel.Length; i++)
                    {
                        oldPos = s_UpdateWriter.Position;
                        NetworkBehaviour comp = behaviourOfSameChannel[i];

                        if (comp.OnSerialize(s_UpdateWriter, false))
                        {
                            comp.ClearAllDirtyBits();

#if UNITY_EDITOR
                            Profiler.IncrementStatOutgoing(MsgType.UpdateVars, comp.GetType().Name);
#endif

                            wroteData = true;
                        }
                        if (s_UpdateWriter.Position - oldPos > NetworkServer.maxPacketSize)
                        {
                            if (LogFilter.logWarn) { Debug.LogWarning("Large state update of " + (s_UpdateWriter.Position - oldPos) + " bytes for netId:" + netId + " from script:" + comp); }
                        }
                    }

                    if (!wroteData)
                    {
                        // nothing to send.. this could be a script with no OnSerialize function setting dirty bits
                        continue;
                    }

                    s_UpdateWriter.FinishMessage();
                    NetworkServer.SendWriterToReady(gameObject, s_UpdateWriter, channelId);
                }
            }
        }

        private NetworkBehaviour[] GetBehavioursOfSameChannel(int channelId, bool initialState)
        {
            List<NetworkBehaviour> channels = new List<NetworkBehaviour>();
            if (initialState && m_NetworkBehaviours == null)
            {
                m_NetworkBehaviours = GetComponents<NetworkBehaviour>();
                return m_NetworkBehaviours;
            }
            for (int itr = 0; itr < m_NetworkBehaviours.Length; itr++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[itr];
                if (comp.GetNetworkChannel() == channelId)
                {
                    channels.Add(comp);
                }
            }
            return channels.ToArray();
        }

        internal void OnUpdateVars(NetworkReader reader, bool initialState, NetworkMessage netMsg)
        {
            NetworkBehaviour[] behaviourOfSameChannel = GetBehavioursOfSameChannel(netMsg.channelId, initialState);
            for (int i = 0; i < behaviourOfSameChannel.Length; i++)
            {
                NetworkBehaviour comp = behaviourOfSameChannel[i];

#if UNITY_EDITOR
                var oldReadPos = reader.Position;
#endif
                comp.OnDeserialize(reader, initialState);
#if UNITY_EDITOR
                if (reader.Position - oldReadPos > 1)
                {
                    Profiler.IncrementStatIncoming(MsgType.UpdateVars, comp.GetType().Name);
                }
#endif
            }
        }

        internal void SetLocalPlayer(short localPlayerControllerId)
        {
            m_IsLocalPlayer = true;
            m_PlayerId = localPlayerControllerId;

            // there is an ordering issue here that originAuthority solves. OnStartAuthority should only be called if m_HasAuthority was false when this function began,
            // or it will be called twice for this object. But that state is lost by the time OnStartAuthority is called below, so the original value is cached
            // here to be checked below.
            bool originAuthority = m_HasAuthority;
            if (localPlayerAuthority)
            {
                m_HasAuthority = true;
            }

            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                comp.OnStartLocalPlayer();

                if (localPlayerAuthority && !originAuthority)
                {
                    comp.OnStartAuthority();
                }
            }
        }

        internal void SetConnectionToServer(NetworkConnection conn)
        {
            m_ConnectionToServer = conn;
        }

        internal void SetConnectionToClient(NetworkConnection conn, short newPlayerControllerId)
        {
            m_PlayerId = newPlayerControllerId;
            m_ConnectionToClient = conn;
        }

        internal void OnNetworkDestroy()
        {
            for (int i = 0;
                 m_NetworkBehaviours != null && i < m_NetworkBehaviours.Length;
                 i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                comp.OnNetworkDestroy();
            }
            m_IsServer = false;
        }

        internal void ClearObservers()
        {
            if (m_Observers != null)
            {
                int count = m_Observers.Count;
                for (int i = 0; i < count; i++)
                {
                    var c = m_Observers[i];
                    c.RemoveFromVisList(this, true);
                }
                m_Observers.Clear();
                m_ObserverConnections.Clear();
            }
        }

        internal void AddObserver(NetworkConnection conn)
        {
            if (m_Observers == null)
            {
                if (LogFilter.logError) { Debug.LogError("AddObserver for " + gameObject + " observer list is null"); }
                return;
            }

            // uses hashset for better-than-list-iteration lookup performance.
            if (m_ObserverConnections.Contains(conn.connectionId))
            {
                if (LogFilter.logDebug) { Debug.Log("Duplicate observer " + conn.address + " added for " + gameObject); }
                return;
            }

            if (LogFilter.logDev) { Debug.Log("Added observer " + conn.address + " added for " + gameObject); }

            m_Observers.Add(conn);
            m_ObserverConnections.Add(conn.connectionId);
            conn.AddToVisList(this);
        }

        internal void RemoveObserver(NetworkConnection conn)
        {
            if (m_Observers == null)
                return;

            // NOTE this is linear performance now..
            m_Observers.Remove(conn);
            m_ObserverConnections.Remove(conn.connectionId);
            conn.RemoveFromVisList(this, false);
        }

        /// <summary>
        /// This causes the set of players that can see this object to be rebuild. The OnRebuildObservers callback function will be invoked on each NetworkBehaviour.
        /// </summary>
        /// <param name="initialize">True if this is the first time.</param>
        public void RebuildObservers(bool initialize)
        {
            if (m_Observers == null)
                return;

            bool changed = false;
            bool result = false;
            HashSet<NetworkConnection> newObservers = new HashSet<NetworkConnection>();
            HashSet<NetworkConnection> oldObservers = new HashSet<NetworkConnection>(m_Observers);

            for (int i = 0; i < m_NetworkBehaviours.Length; i++)
            {
                NetworkBehaviour comp = m_NetworkBehaviours[i];
                result |= comp.OnRebuildObservers(newObservers, initialize);
            }
            if (!result)
            {
                // none of the behaviours rebuilt our observers, use built-in rebuild method
                if (initialize)
                {
                    for (int i = 0; i < NetworkServer.connections.Count; i++)
                    {
                        var conn = NetworkServer.connections[i];
                        if (conn == null) continue;
                        if (conn.isReady)
                            AddObserver(conn);
                    }

                    for (int i = 0; i < NetworkServer.localConnections.Count; i++)
                    {
                        var conn = NetworkServer.localConnections[i];
                        if (conn == null) continue;
                        if (conn.isReady)
                            AddObserver(conn);
                    }
                }
                return;
            }

            // apply changes from rebuild
            foreach (var conn in newObservers)
            {
                if (conn == null)
                {
                    continue;
                }

                if (!conn.isReady)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("Observer is not ready for " + gameObject + " " + conn); }
                    continue;
                }

                if (initialize || !oldObservers.Contains(conn))
                {
                    // new observer
                    conn.AddToVisList(this);
                    if (LogFilter.logDebug) { Debug.Log("New Observer for " + gameObject + " " + conn); }
                    changed = true;
                }
            }

            foreach (var conn in oldObservers)
            {
                if (!newObservers.Contains(conn))
                {
                    // removed observer
                    conn.RemoveFromVisList(this, false);
                    if (LogFilter.logDebug) { Debug.Log("Removed Observer for " + gameObject + " " + conn); }
                    changed = true;
                }
            }

            // special case for local client.
            if (initialize)
            {
                for (int i = 0; i < NetworkServer.localConnections.Count; i++)
                {
                    if (!newObservers.Contains(NetworkServer.localConnections[i]))
                    {
                        OnSetLocalVisibility(false);
                    }
                }
            }

            if (!changed)
                return;

            m_Observers = new List<NetworkConnection>(newObservers);

            // rebuild hashset once we have the final set of new observers
            m_ObserverConnections.Clear();
            for (int i = 0; i < m_Observers.Count; i++)
            {
                m_ObserverConnections.Add(m_Observers[i].connectionId);
            }
        }

        /// <summary>
        /// Removes ownership for an object for a client by its conneciton.
        /// <para>This applies to objects that had authority set by AssignClientAuthority, or NetworkServer.SpawnWithClientAuthority. Authority cannot be removed for player objects.</para>
        /// </summary>
        /// <param name="conn">The connection of the client to remove authority for.</param>
        /// <returns>True if authority is removed.</returns>
        public bool RemoveClientAuthority(NetworkConnection conn)
        {
            if (!isServer)
            {
                if (LogFilter.logError) { Debug.LogError("RemoveClientAuthority can only be call on the server for spawned objects."); }
                return false;
            }

            if (connectionToClient != null)
            {
                if (LogFilter.logError) { Debug.LogError("RemoveClientAuthority cannot remove authority for a player object"); }
                return false;
            }

            if (m_ClientAuthorityOwner == null)
            {
                if (LogFilter.logError) { Debug.LogError("RemoveClientAuthority for " + gameObject + " has no clientAuthority owner."); }
                return false;
            }

            if (m_ClientAuthorityOwner != conn)
            {
                if (LogFilter.logError) { Debug.LogError("RemoveClientAuthority for " + gameObject + " has different owner."); }
                return false;
            }

            m_ClientAuthorityOwner.RemoveOwnedObject(this);
            m_ClientAuthorityOwner = null;

            // server now has authority (this is only called on server)
            ForceAuthority(true);

            // send msg to that client
            var msg = new ClientAuthorityMessage();
            msg.netId = netId;
            msg.authority = false;
            conn.Send(MsgType.LocalClientAuthority, msg);

            if (clientAuthorityCallback != null)
            {
                clientAuthorityCallback(conn, this, false);
            }
            return true;
        }

        /// <summary>
        /// This assigns control of an object to a client via the client's <see cref="NetworkConnection">NetworkConnection.</see>
        /// <para>This causes hasAuthority to be set on the client that owns the object, and NetworkBehaviour.OnStartAuthority will be called on that client. This object then will be in the NetworkConnection.clientOwnedObjects list for the connection.</para>
        /// <para>Authority can be removed with RemoveClientAuthority. Only one client can own an object at any time. Only NetworkIdentities with localPlayerAuthority set can have client authority assigned. This does not need to be called for player objects, as their authority is setup automatically.</para>
        /// </summary>
        /// <param name="conn">	The connection of the client to assign authority to.</param>
        /// <returns>True if authority was assigned.</returns>
        public bool AssignClientAuthority(NetworkConnection conn)
        {
            if (!isServer)
            {
                if (LogFilter.logError) { Debug.LogError("AssignClientAuthority can only be call on the server for spawned objects."); }
                return false;
            }
            if (!localPlayerAuthority)
            {
                if (LogFilter.logError) { Debug.LogError("AssignClientAuthority can only be used for NetworkIdentity component with LocalPlayerAuthority set."); }
                return false;
            }

            if (m_ClientAuthorityOwner != null && conn != m_ClientAuthorityOwner)
            {
                if (LogFilter.logError) { Debug.LogError("AssignClientAuthority for " + gameObject + " already has an owner. Use RemoveClientAuthority() first."); }
                return false;
            }

            if (conn == null)
            {
                if (LogFilter.logError) { Debug.LogError("AssignClientAuthority for " + gameObject + " owner cannot be null. Use RemoveClientAuthority() instead."); }
                return false;
            }

            m_ClientAuthorityOwner = conn;
            m_ClientAuthorityOwner.AddOwnedObject(this);

            // server no longer has authority (this is called on server). Note that local client could re-acquire authority below
            ForceAuthority(false);

            // send msg to that client
            var msg = new ClientAuthorityMessage();
            msg.netId = netId;
            msg.authority = true;
            conn.Send(MsgType.LocalClientAuthority, msg);

            if (clientAuthorityCallback != null)
            {
                clientAuthorityCallback(conn, this, true);
            }
            return true;
        }

        // marks the identity for future reset, this is because we cant reset the identity during destroy
        // as people might want to be able to read the members inside OnDestroy(), and we have no way
        // of invoking reset after OnDestroy is called.
        internal void MarkForReset()
        {
            m_Reset = true;
        }

        // if we have marked an identity for reset we do the actual reset.
        internal void Reset()
        {
            if (!m_Reset)
                return;

            m_Reset = false;
            m_IsServer = false;
            m_IsClient = false;
            m_HasAuthority = false;

            m_NetId = NetworkInstanceId.Zero;
            m_IsLocalPlayer = false;
            m_ConnectionToServer = null;
            m_ConnectionToClient = null;
            m_PlayerId = -1;
            m_NetworkBehaviours = null;

            ClearObservers();
            m_ClientAuthorityOwner = null;
        }


#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void OnInitializeOnLoad()
        {
            // The transport layer has state in C++, so when the C# state is lost (on domain reload), the C++ transport layer must be shutown as well.
            NetworkManager.OnDomainReload();
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeInitializeOnLoad()
        {
            var go = new GameObject("UNETCallbacks");
            go.AddComponent(typeof(NetworkCallbacks));
            go.hideFlags = go.hideFlags | HideFlags.HideAndDontSave;
        }

        static internal void UNetStaticUpdate()
        {
            NetworkServer.Update();
            NetworkClient.UpdateClients();
            NetworkManager.UpdateScene();

#if UNITY_EDITOR
            Profiler.NewProfilerTick();
#endif
        }
    };
}
