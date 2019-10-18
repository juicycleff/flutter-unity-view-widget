using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.Networking
{
    /// <summary>
    /// Base class which should be inherited by scripts which contain networking functionality.
    /// <para>This is a MonoBehaviour class so scripts which need to use the networking feature should inherit this class instead of MonoBehaviour. It allows you to invoke networked actions, receive various callbacks, and automatically synchronize state from server-to-client.</para>
    /// <para>The NetworkBehaviour component requires a NetworkIdentity on the game object. There can be multiple NetworkBehaviours on a single game object. For an object with sub-components in a hierarchy, the NetworkIdentity must be on the root object, and NetworkBehaviour scripts must also be on the root object.</para>
    /// <para>Some of the built-in components of the networking system are derived from NetworkBehaviour, including NetworkTransport, NetworkAnimator and NetworkProximityChecker.</para>
    /// </summary>
    [RequireComponent(typeof(NetworkIdentity))]
    [AddComponentMenu("")]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkBehaviour : MonoBehaviour
    {
        uint m_SyncVarDirtyBits;
        float m_LastSendTime;

        // this prevents recursion when SyncVar hook functions are called.
        bool m_SyncVarGuard;

        /// <summary>
        /// This value is set on the NetworkIdentity and is accessible here for convenient access for scripts.
        /// </summary>
        public bool localPlayerAuthority { get { return myView.localPlayerAuthority; } }
        /// <summary>
        /// Returns true if this object is active on an active server.
        /// <para>This is only true if the object has been spawned. This is different from NetworkServer.active, which is true if the server itself is active rather than this object being active.</para>
        /// </summary>
        public bool isServer { get { return myView.isServer; } }
        /// <summary>
        /// Returns true if running as a client and this object was spawned by a server.
        /// </summary>
        public bool isClient { get { return myView.isClient; } }
        /// <summary>
        /// This returns true if this object is the one that represents the player on the local machine.
        /// <para>In multiplayer games, there are multiple instances of the Player object. The client needs to know which one is for "themselves" so that only that player processes input and potentially has a camera attached. The IsLocalPlayer function will return true only for the player instance that belongs to the player on the local machine, so it can be used to filter out input for non-local players.</para>
        /// <para>This example shows processing input for only the local player.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Player : NetworkBehaviour
        /// {
        ///    int moveX = 0;
        ///    int moveY = 0;
        ///
        ///    void Update()
        ///    {
        ///        if (!isLocalPlayer)
        ///        {
        ///            return;
        ///        }
        ///        // input handling for local player only
        ///        int oldMoveX = moveX;
        ///        int oldMoveY = moveY;
        ///        moveX = 0;
        ///        moveY = 0;
        ///        if (Input.GetKey(KeyCode.LeftArrow))
        ///        {
        ///            moveX -= 1;
        ///        }
        ///        if (Input.GetKey(KeyCode.RightArrow))
        ///        {
        ///            moveX += 1;
        ///        }
        ///        if (Input.GetKey(KeyCode.UpArrow))
        ///        {
        ///            moveY += 1;
        ///        }
        ///        if (Input.GetKey(KeyCode.DownArrow))
        ///        {
        ///            moveY -= 1;
        ///        }
        ///        if (moveX != oldMoveX || moveY != oldMoveY)
        ///        {
        ///            CmdMove(moveX, moveY);
        ///        }
        ///    }
        ///
        ///    [Command]
        ///    void CmdMove(int dx, int dy)
        ///    {
        ///        // move here
        ///    }
        /// }
        /// </code>
        /// </summary>
        public bool isLocalPlayer { get { return myView.isLocalPlayer; } }
        /// <summary>
        /// This returns true if this object is the authoritative version of the object in the distributed network application.
        /// <para>The <see cref="localPlayerAuthority">localPlayerAuthority</see> value on the NetworkIdentity determines how authority is determined. For most objects, authority is held by the server / host. For objects with <see cref="localPlayerAuthority">localPlayerAuthority</see> set, authority is held by the client of that player.</para>
        /// </summary>
        public bool hasAuthority { get { return myView.hasAuthority; } }
        /// <summary>
        /// The unique network Id of this object.
        /// <para>This is assigned at runtime by the network server and will be unique for all objects for that network session.</para>
        /// </summary>
        public NetworkInstanceId netId { get { return myView.netId; } }
        /// <summary>
        /// The <see cref="NetworkConnection">NetworkConnection</see> associated with this <see cref="NetworkIdentity">NetworkIdentity.</see> This is only valid for player objects on the server.
        /// </summary>
        public NetworkConnection connectionToServer { get { return myView.connectionToServer; } }
        /// <summary>
        /// The <see cref="NetworkConnection">NetworkConnection</see> associated with this <see cref="NetworkIdentity">NetworkIdentity.</see> This is only valid for player objects on the server.
        /// <code>
        /// //Attach this script to a GameObject
        /// //Attach a TextMesh to the GameObject. To do this click the GameObject, click the Add Component button in the Inspector window, and go to Mesh>Text Mesh.
        /// //Attach a NetworkIdentity to the GameObject by clicking Add Component, then go to Network>NetworkIdentity. In the component that was added, check the Local Player Authority checkbox.
        /// //Next, create an empty GameObject. Attach a NetworkManager to it by clicking the GameObject, clicking Add Component going to Network>NetworkManager. Also add a NetworkManagerHUD the same way.
        ///
        /// //This script outputs the Connection ID and address to the console when the Client is started
        ///
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ConnectionToClientExample : NetworkBehaviour
        /// {
        ///    //This is a TextMesh component that you attach to the child of the NetworkIdentity GameObject
        ///    TextMesh m_TextMesh;
        ///
        ///    void Start()
        ///    {
        ///        //Output the connection ID and IP address of the connection by using connectionToClient
        ///        Debug.Log("Connection ID : " + connectionToClient.connectionId);
        ///        Debug.Log("Connection Address : " + connectionToClient.address);
        ///        //Check that the connection is marked as ready
        ///        if (connectionToClient.isReady)
        ///        {
        ///            Debug.Log("Ready!");
        ///        }
        ///        //Enter the child of your GameObject (the GameObject with the TextMesh you attach)
        ///        //Fetch the TextMesh component of it
        ///        m_TextMesh = GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        ///        //Change the Text of the TextMesh to show the netId
        ///        m_TextMesh.text = "ID : " + netId;
        ///        //Output the connection to Client
        ///    }
        /// }
        /// </code>
        /// </summary>
        public NetworkConnection connectionToClient { get { return myView.connectionToClient; } }
        /// <summary>
        /// The id of the player associated with the behaviour.
        /// <para>This is only valid if the GameObject is a local player.</para>
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public short playerControllerId { get { return myView.playerControllerId; } }
        protected uint syncVarDirtyBits { get { return m_SyncVarDirtyBits; } }
        protected bool syncVarHookGuard { get { return m_SyncVarGuard; } set { m_SyncVarGuard = value; }}

        internal NetworkIdentity netIdentity { get { return myView; } }

        const float k_DefaultSendInterval = 0.1f;

        NetworkIdentity m_MyView;
        NetworkIdentity myView
        {
            get
            {
                if (m_MyView == null)
                {
                    m_MyView = GetComponent<NetworkIdentity>();
                    if (m_MyView == null)
                    {
                        if (LogFilter.logError) { Debug.LogError("There is no NetworkIdentity on this object. Please add one."); }
                    }
                    return m_MyView;
                }
                return m_MyView;
            }
        }

        // ----------------------------- Commands --------------------------------

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void SendCommandInternal(NetworkWriter writer, int channelId, string cmdName)
        {
            // local players can always send commands, regardless of authority, other objects must have authority.
            if (!(isLocalPlayer || hasAuthority))
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Trying to send command for object without authority."); }
                return;
            }

            if (ClientScene.readyConnection == null)
            {
                if (LogFilter.logError) { Debug.LogError("Send command attempted with no client running [client=" + connectionToServer + "]."); }
                return;
            }

            writer.FinishMessage();
            ClientScene.readyConnection.SendWriter(writer, channelId);

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.Command, cmdName);
#endif
        }

        /// <summary>
        /// Manually invoke a Command.
        /// </summary>
        /// <param name="cmdHash">Hash of the Command name.</param>
        /// <param name="reader">Parameters to pass to the command.</param>
        /// <returns>Returns true if successful.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool InvokeCommand(int cmdHash, NetworkReader reader)
        {
            if (InvokeCommandDelegate(cmdHash, reader))
            {
                return true;
            }
            return false;
        }

        // ----------------------------- Client RPCs --------------------------------

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void SendRPCInternal(NetworkWriter writer, int channelId, string rpcName)
        {
            // This cannot use NetworkServer.active, as that is not specific to this object.
            if (!isServer)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("ClientRpc call on un-spawned object"); }
                return;
            }

            writer.FinishMessage();
            NetworkServer.SendWriterToReady(gameObject, writer, channelId);

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.Rpc, rpcName);
#endif
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void SendTargetRPCInternal(NetworkConnection conn, NetworkWriter writer, int channelId, string rpcName)
        {
            // This cannot use NetworkServer.active, as that is not specific to this object.
            if (!isServer)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("TargetRpc call on un-spawned object"); }
                return;
            }

            writer.FinishMessage();

            conn.SendWriter(writer, channelId);

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.Rpc, rpcName);
#endif
        }

        /// <summary>
        /// Manually invoke an RPC function.
        /// </summary>
        /// <param name="cmdHash">Hash of the RPC name.</param>
        /// <param name="reader">Parameters to pass to the RPC function.</param>
        /// <returns>Returns true if successful.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool InvokeRPC(int cmdHash, NetworkReader reader)
        {
            if (InvokeRpcDelegate(cmdHash, reader))
            {
                return true;
            }
            return false;
        }

        // ----------------------------- Sync Events --------------------------------

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void SendEventInternal(NetworkWriter writer, int channelId, string eventName)
        {
            if (!NetworkServer.active)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("SendEvent no server?"); }
                return;
            }

            writer.FinishMessage();
            NetworkServer.SendWriterToReady(gameObject, writer, channelId);

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.SyncEvent, eventName);
#endif
        }

        /// <summary>
        /// Manually invoke a SyncEvent.
        /// </summary>
        /// <param name="cmdHash">Hash of the SyncEvent name.</param>
        /// <param name="reader">Parameters to pass to the SyncEvent.</param>
        /// <returns>Returns true if successful.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool InvokeSyncEvent(int cmdHash, NetworkReader reader)
        {
            if (InvokeSyncEventDelegate(cmdHash, reader))
            {
                return true;
            }
            return false;
        }

        // ----------------------------- Sync Lists --------------------------------

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool InvokeSyncList(int cmdHash, NetworkReader reader)
        {
            if (InvokeSyncListDelegate(cmdHash, reader))
            {
                return true;
            }
            return false;
        }

        // ----------------------------- Code Gen Path Helpers  --------------------------------
        /// <summary>
        /// Delegate for Command functions.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="reader"></param>
        public delegate void CmdDelegate(NetworkBehaviour obj, NetworkReader reader);
        /// <summary>
        /// Delegate for Event functions.
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="reader"></param>
        protected delegate void EventDelegate(List<Delegate> targets, NetworkReader reader);

        protected enum UNetInvokeType
        {
            Command,
            ClientRpc,
            SyncEvent,
            SyncList
        };

        protected class Invoker
        {
            public UNetInvokeType invokeType;
            public Type invokeClass;
            public CmdDelegate invokeFunction;

            public string DebugString()
            {
                return invokeType + ":" +
                    invokeClass + ":" +
                    invokeFunction.GetMethodName();
            }
        };

        static Dictionary<int, Invoker> s_CmdHandlerDelegates = new Dictionary<int, Invoker>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        static protected void RegisterCommandDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
        {
            if (s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return;
            }
            Invoker inv = new Invoker();
            inv.invokeType = UNetInvokeType.Command;
            inv.invokeClass = invokeClass;
            inv.invokeFunction = func;
            s_CmdHandlerDelegates[cmdHash] = inv;
            if (LogFilter.logDev) { Debug.Log("RegisterCommandDelegate hash:" + cmdHash + " " + func.GetMethodName()); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        static protected void RegisterRpcDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
        {
            if (s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return;
            }
            Invoker inv = new Invoker();
            inv.invokeType = UNetInvokeType.ClientRpc;
            inv.invokeClass = invokeClass;
            inv.invokeFunction = func;
            s_CmdHandlerDelegates[cmdHash] = inv;
            if (LogFilter.logDev) { Debug.Log("RegisterRpcDelegate hash:" + cmdHash + " " + func.GetMethodName()); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        static protected void RegisterEventDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
        {
            if (s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return;
            }
            Invoker inv = new Invoker();
            inv.invokeType = UNetInvokeType.SyncEvent;
            inv.invokeClass = invokeClass;
            inv.invokeFunction = func;
            s_CmdHandlerDelegates[cmdHash] = inv;
            if (LogFilter.logDev) { Debug.Log("RegisterEventDelegate hash:" + cmdHash + " " + func.GetMethodName()); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        static protected void RegisterSyncListDelegate(Type invokeClass, int cmdHash, CmdDelegate func)
        {
            if (s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return;
            }
            Invoker inv = new Invoker();
            inv.invokeType = UNetInvokeType.SyncList;
            inv.invokeClass = invokeClass;
            inv.invokeFunction = func;
            s_CmdHandlerDelegates[cmdHash] = inv;
            if (LogFilter.logDev) { Debug.Log("RegisterSyncListDelegate hash:" + cmdHash + " " + func.GetMethodName()); }
        }

        internal static string GetInvoker(int cmdHash)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return null;
            }

            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            return inv.DebugString();
        }

        // wrapper fucntions for each type of network operation
        internal static bool GetInvokerForHashCommand(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
        {
            return GetInvokerForHash(cmdHash, UNetInvokeType.Command, out invokeClass, out invokeFunction);
        }

        internal static bool GetInvokerForHashClientRpc(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
        {
            return GetInvokerForHash(cmdHash, UNetInvokeType.ClientRpc, out invokeClass, out invokeFunction);
        }

        internal static bool GetInvokerForHashSyncList(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
        {
            return GetInvokerForHash(cmdHash, UNetInvokeType.SyncList, out invokeClass, out invokeFunction);
        }

        internal static bool GetInvokerForHashSyncEvent(int cmdHash, out Type invokeClass, out CmdDelegate invokeFunction)
        {
            return GetInvokerForHash(cmdHash, UNetInvokeType.SyncEvent, out invokeClass, out invokeFunction);
        }

        static bool GetInvokerForHash(int cmdHash, NetworkBehaviour.UNetInvokeType invokeType, out Type invokeClass, out CmdDelegate invokeFunction)
        {
            Invoker invoker = null;
            if (!s_CmdHandlerDelegates.TryGetValue(cmdHash, out invoker))
            {
                if (LogFilter.logDev) { Debug.Log("GetInvokerForHash hash:" + cmdHash + " not found"); }
                invokeClass = null;
                invokeFunction = null;
                return false;
            }

            if (invoker == null)
            {
                if (LogFilter.logDev) { Debug.Log("GetInvokerForHash hash:" + cmdHash + " invoker null"); }
                invokeClass = null;
                invokeFunction = null;
                return false;
            }

            if (invoker.invokeType != invokeType)
            {
                if (LogFilter.logError) { Debug.LogError("GetInvokerForHash hash:" + cmdHash + " mismatched invokeType"); }
                invokeClass = null;
                invokeFunction = null;
                return false;
            }

            invokeClass = invoker.invokeClass;
            invokeFunction = invoker.invokeFunction;
            return true;
        }

        internal static void DumpInvokers()
        {
            Debug.Log("DumpInvokers size:" + s_CmdHandlerDelegates.Count);
            foreach (var inv in s_CmdHandlerDelegates)
            {
                Debug.Log("  Invoker:" + inv.Value.invokeClass + ":" + inv.Value.invokeFunction.GetMethodName() + " " + inv.Value.invokeType + " " + inv.Key);
            }
        }

        internal bool ContainsCommandDelegate(int cmdHash)
        {
            return s_CmdHandlerDelegates.ContainsKey(cmdHash);
        }

        internal bool InvokeCommandDelegate(int cmdHash, NetworkReader reader)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return false;
            }

            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            if (inv.invokeType != UNetInvokeType.Command)
            {
                return false;
            }

            if (GetType() != inv.invokeClass)
            {
                if (GetType().IsSubclassOf(inv.invokeClass))
                {
                    // allowed, commands function is on a base class.
                }
                else
                {
                    return false;
                }
            }

            inv.invokeFunction(this, reader);
            return true;
        }

        internal bool InvokeRpcDelegate(int cmdHash, NetworkReader reader)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return false;
            }

            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            if (inv.invokeType != UNetInvokeType.ClientRpc)
            {
                return false;
            }

            if (GetType() != inv.invokeClass)
            {
                if (GetType().IsSubclassOf(inv.invokeClass))
                {
                    // allowed, rpc function is on a base class.
                }
                else
                {
                    return false;
                }
            }

            inv.invokeFunction(this, reader);
            return true;
        }

        internal bool InvokeSyncEventDelegate(int cmdHash, NetworkReader reader)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return false;
            }

            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            if (inv.invokeType != UNetInvokeType.SyncEvent)
            {
                return false;
            }

            inv.invokeFunction(this, reader);
            return true;
        }

        internal bool InvokeSyncListDelegate(int cmdHash, NetworkReader reader)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return false;
            }

            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            if (inv.invokeType != UNetInvokeType.SyncList)
            {
                return false;
            }

            if (GetType() != inv.invokeClass)
            {
                return false;
            }

            inv.invokeFunction(this, reader);
            return true;
        }

        static internal string GetCmdHashHandlerName(int cmdHash)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return cmdHash.ToString();
            }
            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            return inv.invokeType + ":" + inv.invokeFunction.GetMethodName();
        }

        static string GetCmdHashPrefixName(int cmdHash, string prefix)
        {
            if (!s_CmdHandlerDelegates.ContainsKey(cmdHash))
            {
                return cmdHash.ToString();
            }
            Invoker inv = s_CmdHandlerDelegates[cmdHash];
            var name = inv.invokeFunction.GetMethodName();

            int index = name.IndexOf(prefix);
            if (index > -1)
            {
                name = name.Substring(prefix.Length);
            }
            return name;
        }

        internal static string GetCmdHashCmdName(int cmdHash)
        {
            return GetCmdHashPrefixName(cmdHash, "InvokeCmd");
        }

        internal static string GetCmdHashRpcName(int cmdHash)
        {
            return GetCmdHashPrefixName(cmdHash, "InvokeRpc");
        }

        internal static string GetCmdHashEventName(int cmdHash)
        {
            return GetCmdHashPrefixName(cmdHash, "InvokeSyncEvent");
        }

        internal static string GetCmdHashListName(int cmdHash)
        {
            return GetCmdHashPrefixName(cmdHash, "InvokeSyncList");
        }

        // ----------------------------- Helpers  --------------------------------

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void SetSyncVarGameObject(GameObject newGameObject, ref GameObject gameObjectField, uint dirtyBit, ref NetworkInstanceId netIdField)
        {
            if (m_SyncVarGuard)
                return;

            NetworkInstanceId newGameObjectNetId = new NetworkInstanceId();
            if (newGameObject != null)
            {
                var uv = newGameObject.GetComponent<NetworkIdentity>();
                if (uv != null)
                {
                    newGameObjectNetId = uv.netId;
                    if (newGameObjectNetId.IsEmpty())
                    {
                        if (LogFilter.logWarn) { Debug.LogWarning("SetSyncVarGameObject GameObject " + newGameObject + " has a zero netId. Maybe it is not spawned yet?"); }
                    }
                }
            }

            NetworkInstanceId oldGameObjectNetId = new NetworkInstanceId();
            if (gameObjectField != null)
            {
                oldGameObjectNetId = gameObjectField.GetComponent<NetworkIdentity>().netId;
            }

            if (newGameObjectNetId != oldGameObjectNetId)
            {
                if (LogFilter.logDev) { Debug.Log("SetSyncVar GameObject " + GetType().Name + " bit [" + dirtyBit + "] netfieldId:" + oldGameObjectNetId + "->" + newGameObjectNetId); }
                SetDirtyBit(dirtyBit);
                gameObjectField = newGameObject;
                netIdField = newGameObjectNetId;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void SetSyncVar<T>(T value, ref T fieldValue, uint dirtyBit)
        {
            bool changed = false;
            if (value == null)
            {
                if (fieldValue != null)
                    changed = true;
            }
            else
            {
                changed = !value.Equals(fieldValue);
            }
            if (changed)
            {
                if (LogFilter.logDev) { Debug.Log("SetSyncVar " + GetType().Name + " bit [" + dirtyBit + "] " + fieldValue + "->" + value); }
                SetDirtyBit(dirtyBit);
                fieldValue = value;
            }
        }

        /// <summary>
        /// Used to set the behaviour as dirty, so that a network update will be sent for the object.
        /// </summary>
        /// <param name="dirtyBit">Bit mask to set.</param>
        // these are masks, not bit numbers, ie. 0x004 not 2
        public void SetDirtyBit(uint dirtyBit)
        {
            m_SyncVarDirtyBits |= dirtyBit;
        }

        /// <summary>
        /// This clears all the dirty bits that were set on this script by SetDirtyBits();
        /// <para>This is automatically invoked when an update is sent for this object, but can be called manually as well.</para>
        /// </summary>
        public void ClearAllDirtyBits()
        {
            m_LastSendTime = Time.time;
            m_SyncVarDirtyBits = 0;
        }

        internal int GetDirtyChannel()
        {
            if (Time.time - m_LastSendTime > GetNetworkSendInterval())
            {
                if (m_SyncVarDirtyBits != 0)
                {
                    return GetNetworkChannel();
                }
            }
            return -1;
        }

        /// <summary>
        /// Virtual function to override to send custom serialization data. The corresponding function to send serialization data is OnDeserialize().
        /// <para>The initialState flag is useful to differentiate between the first time an object is serialized and when incremental updates can be sent. The first time an object is sent to a client, it must include a full state snapshot, but subsequent updates can save on bandwidth by including only incremental changes. Note that SyncVar hook functions are not called when initialState is true, only for incremental updates.</para>
        /// <para>If a class has SyncVars, then an implementation of this function and OnDeserialize() are added automatically to the class. So a class that has SyncVars cannot also have custom serialization functions.</para>
        /// <para>The OnSerialize function should return true to indicate that an update should be sent. If it returns true, then the dirty bits for that script are set to zero, if it returns false then the dirty bits are not changed. This allows multiple changes to a script to be accumulated over time and sent when the system is ready, instead of every frame.</para>
        /// </summary>
        /// <param name="writer">Writer to use to write to the stream.</param>
        /// <param name="initialState">If this is being called to send initial state.</param>
        /// <returns>True if data was written.</returns>
        public virtual bool OnSerialize(NetworkWriter writer, bool initialState)
        {
            if (!initialState)
            {
                writer.WritePackedUInt32(0);
            }
            return false;
        }

        /// <summary>
        /// Virtual function to override to receive custom serialization data. The corresponding function to send serialization data is OnSerialize().
        /// </summary>
        /// <param name="reader">Reader to read from the stream.</param>
        /// <param name="initialState">True if being sent initial state.</param>
        public virtual void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if (!initialState)
            {
                reader.ReadPackedUInt32();
            }
        }

        /// <summary>
        /// An internal method called on client objects to resolve GameObject references.
        /// <para>It is not safe to put user code in this function as it may be replaced by the network system's code generation process.</para>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void PreStartClient()
        {
        }

        /// <summary>
        /// This is invoked on clients when the server has caused this object to be destroyed.
        /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// class Bomb : <see cref="NetworkBehaviour">NetworkBehaviour</see>
        /// {
        ///    public override void OnNetworkDestroy()
        ///    {
        ///        // play explosion sound
        ///    }
        /// }
        /// </code>
        /// </summary>
        public virtual void OnNetworkDestroy()
        {
        }

        /// <summary>
        /// This is invoked for NetworkBehaviour objects when they become active on the server.
        /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
        /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
        /// </summary>
        public virtual void OnStartServer()
        {
        }

        /// <summary>
        /// Called on every NetworkBehaviour when it is activated on a client.
        /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
        /// </summary>
        public virtual void OnStartClient()
        {
        }

        /// <summary>
        /// Called when the local player object has been set up.
        /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
        /// </summary>
        public virtual void OnStartLocalPlayer()
        {
        }

        /// <summary>
        /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.localPlayerAuthority">'NetworkIdentity.localPlayerAuthority.'</see>
        /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and <see cref="OnStartClient">OnStartClient.</see></para>
        /// <para>When NetworkIdentity.AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with NetworkServer.SpawnWithClientAuthority, this will be called on the client that owns the object.</para>
        /// </summary>
        public virtual void OnStartAuthority()
        {
        }

        /// <summary>
        /// This is invoked on behaviours when authority is removed.
        /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
        /// </summary>
        public virtual void OnStopAuthority()
        {
        }

        /// <summary>
        /// Callback used by the visibility system to (re)construct the set of observers that can see this object.
        /// <para>Implementations of this callback should add network connections of players that can see this object to the observers set.</para>
        /// </summary>
        /// <param name="observers">The new set of observers for this object.</param>
        /// <param name="initialize">True if the set of observers is being built for the first time.</param>
        /// <returns>Return true if this function did work.</returns>
        public virtual bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
        {
            return false;
        }

        /// <summary>
        /// Callback used by the visibility system for objects on a host.
        /// <para>Objects on a host (with a local client) cannot be disabled or destroyed when they are not visibile to the local client. So this function is called to allow custom code to hide these objects. A typical implementation will disable renderer components on the object. This is only called on local clients on a host.</para>
        /// </summary>
        /// <param name="vis">New visibility state.</param>
        public virtual void OnSetLocalVisibility(bool vis)
        {
        }

        /// <summary>
        /// Callback used by the visibility system to determine if an observer (player) can see this object.
        /// <para>If this function returns true, the network connection will be added as an observer.</para>
        /// </summary>
        /// <param name="conn">Network connection of a player.</param>
        /// <returns>True if the player can see this object.</returns>
        public virtual bool OnCheckObserver(NetworkConnection conn)
        {
            return true;
        }

        /// <summary>
        /// This virtual function is used to specify the QoS channel to use for SyncVar updates for this script.
        /// <para>Using the NetworkSettings custom attribute causes this function to be implemented for this script, but developers can also implement it themselves.</para>
        /// </summary>
        /// <returns>The QoS channel for this script.</returns>
        public virtual int GetNetworkChannel()
        {
            return Channels.DefaultReliable;
        }

        /// <summary>
        /// This virtual function is used to specify the send interval to use for SyncVar updates for this script.
        /// <para>Using the NetworkSettings custom attribute causes this function to be implemented for this script, but developers can also implement it themselves.</para>
        /// </summary>
        /// <returns>The time in seconds between updates.</returns>
        public virtual float GetNetworkSendInterval()
        {
            return k_DefaultSendInterval;
        }
    }
}
