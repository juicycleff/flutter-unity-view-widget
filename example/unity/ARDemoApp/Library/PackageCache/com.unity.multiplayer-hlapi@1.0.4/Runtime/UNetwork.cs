using System;

#pragma warning disable 618
namespace UnityEngine.Networking
{
    // Handles network messages on client and server
    public delegate void NetworkMessageDelegate(NetworkMessage netMsg);

    // Handles requests to spawn objects on the client
    public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);

    // Handles requests to unspawn objects on the client
    public delegate void UnSpawnDelegate(GameObject spawned);

    /// <summary>
    /// Container class for networking system built-in message types.
    /// </summary>
    // built-in system network messages
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class MsgType
    {
        // internal system messages - cannot be replaced by user code
        /// <summary>
        /// Internal networking system message for destroying objects.
        /// </summary>
        public const short ObjectDestroy = 1;
        /// <summary>
        /// Internal networking system message for sending a ClientRPC from server to client.
        /// </summary>
        public const short Rpc = 2;
        /// <summary>
        /// Internal networking system message for spawning objects.
        /// </summary>
        public const short ObjectSpawn = 3;
        /// <summary>
        /// Internal networking system message for telling clients they own a player object.
        /// </summary>
        public const short Owner = 4;
        /// <summary>
        /// Internal networking system message for sending a command from client to server.
        /// </summary>
        public const short Command = 5;
        /// <summary>
        /// Internal networking system message for sending tranforms from client to server.
        /// </summary>
        public const short LocalPlayerTransform = 6;
        /// <summary>
        /// Internal networking system message for sending a SyncEvent from server to client.
        /// </summary>
        public const short SyncEvent = 7;
        /// <summary>
        /// Internal networking system message for updating SyncVars on a client from a server.
        /// </summary>
        public const short UpdateVars = 8;
        /// <summary>
        /// Internal networking system message for sending a USyncList generic list.
        /// </summary>
        public const short SyncList = 9;
        /// <summary>
        /// Internal networking system message for spawning scene objects.
        /// </summary>
        public const short ObjectSpawnScene = 10;
        /// <summary>
        /// Internal networking system message for sending information about network peers to clients.
        /// </summary>
        public const short NetworkInfo = 11;
        /// <summary>
        /// Internal networking system messages used to tell when the initial contents of a scene is being spawned.
        /// </summary>
        public const short SpawnFinished = 12;
        /// <summary>
        /// Internal networking system message for hiding objects.
        /// </summary>
        public const short ObjectHide = 13;
        /// <summary>
        /// Internal networking system message for HLAPI CRC checking.
        /// </summary>
        public const short CRC = 14;
        /// <summary>
        /// Internal networking system message for setting authority to a client for an object.
        /// </summary>
        public const short LocalClientAuthority = 15;
        /// <summary>
        /// Internal networking system message for sending tranforms for client object from client to server.
        /// </summary>
        public const short LocalChildTransform = 16;
        /// <summary>
        /// Internal networking system message for identifying fragmented packets.
        /// </summary>
        public const short Fragment = 17;
        /// <summary>
        /// Internal networking system message for sending information about changes in authority for non-player objects to clients.
        /// </summary>
        public const short PeerClientAuthority = 18;

        // used for profiling
        internal const short UserMessage = 0;
        internal const short HLAPIMsg = 28;
        internal const short LLAPIMsg = 29;
        internal const short HLAPIResend = 30;
        internal const short HLAPIPending = 31;

        /// <summary>
        /// The highest value of internal networking system message ids. User messages must be above this value. User code cannot replace these handlers.
        /// </summary>
        public const short InternalHighest = 31;

        // public system messages - can be replaced by user code
        /// <summary>
        /// Internal networking system message for communicating a connection has occurred.
        /// <para>Ensure you use RegisterHandler on the client or server. Insert MsgType.Connect as a parameter to listen for connections.</para>
        /// </summary>
        public const short Connect = 32;
        /// <summary>
        /// Internal networking system message for communicating a disconnect has occurred.
        /// <para>To help understand the reason for a disconnect, an IntegerMessage number is written to the message body, which can be read and converted to the error enum.</para>
        /// </summary>
        public const short Disconnect = 33;
        /// <summary>
        /// Internal networking system message for communicating an error.
        /// </summary>
        public const short Error = 34;
        /// <summary>
        /// Internal networking system message for clients to tell server they are ready.
        /// </summary>
        public const short Ready = 35;
        /// <summary>
        /// Internal networking system message for server to tell clients they are no longer ready.
        /// <para>Can be used when switching scenes, to stop receiving network traffic during the switch.</para>
        /// </summary>
        public const short NotReady = 36;
        /// <summary>
        /// Internal networking system message for adding player objects to client instances.
        /// <para>This is sent to the server when a client calls NetworkClient.AddPlayer(). The server should have a handler for this message type to add the player object to the game and notify the client with NetworkServer.AddPlayer().</para>
        /// </summary>
        public const short AddPlayer = 37;
        /// <summary>
        /// Internal networking system message for removing a player object which was spawned for a client.
        /// </summary>
        public const short RemovePlayer = 38;
        /// <summary>
        /// Internal networking system message that tells clients which scene to load when they connect to a server.
        /// </summary>
        public const short Scene = 39;
        /// <summary>
        /// Internal networking system message for sending synchronizing animation state.
        /// <para>Used by the NetworkAnimation component.</para>
        /// </summary>
        public const short Animation = 40;
        /// <summary>
        /// Internal networking system message for sending synchronizing animation parameter state.
        /// <para>Used by the NetworkAnimation component.</para>
        /// </summary>
        public const short AnimationParameters = 41;
        /// <summary>
        /// Internal networking system message for sending animation triggers.
        /// <para>Used by the NetworkAnimation component.</para>
        /// </summary>
        public const short AnimationTrigger = 42;
        /// <summary>
        /// Internal networking system message for communicating a player is ready in the lobby.
        /// </summary>
        public const short LobbyReadyToBegin = 43;
        /// <summary>
        /// Internal networking system message for communicating a lobby player has loaded the game scene.
        /// </summary>
        public const short LobbySceneLoaded = 44;
        /// <summary>
        /// Internal networking system message for communicating failing to add lobby player.
        /// </summary>
        public const short LobbyAddPlayerFailed = 45;
        /// <summary>
        /// Internal networking system messages used to return the game to the lobby scene.
        /// </summary>
        public const short LobbyReturnToLobby = 46;
        /// <summary>
        /// Internal networking system message used when a client connects to the new host of a game.
        /// </summary>
        public const short ReconnectPlayer = 47;

        /// <summary>
        /// The highest value of built-in networking system message ids. User messages must be above this value.
        /// </summary>
        //NOTE: update msgLabels below if this is changed.
        public const short Highest = 47;

        static internal string[] msgLabels =
        {
            "none",
            "ObjectDestroy",
            "Rpc",
            "ObjectSpawn",
            "Owner",
            "Command",
            "LocalPlayerTransform",
            "SyncEvent",
            "UpdateVars",
            "SyncList",
            "ObjectSpawnScene", // 10
            "NetworkInfo",
            "SpawnFinished",
            "ObjectHide",
            "CRC",
            "LocalClientAuthority",
            "LocalChildTransform",
            "Fragment",
            "PeerClientAuthority",
            "",
            "", // 20
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "", // 30
            "", // - SystemInternalHighest
            "Connect", // 32,
            "Disconnect",
            "Error",
            "Ready",
            "NotReady",
            "AddPlayer",
            "RemovePlayer",
            "Scene",
            "Animation", // 40
            "AnimationParams",
            "AnimationTrigger",
            "LobbyReadyToBegin",
            "LobbySceneLoaded",
            "LobbyAddPlayerFailed", // 45
            "LobbyReturnToLobby", // 46
            "ReconnectPlayer", // 47
        };

        /// <summary>
        /// Returns the name of internal message types by their id.
        /// </summary>
        /// <param name="value">A internal message id value.</param>
        /// <returns>The name of the internal message.</returns>
        static public string MsgTypeToString(short value)
        {
            if (value < 0 || value > Highest)
            {
                return String.Empty;
            }
            string result =  msgLabels[value];
            if (string.IsNullOrEmpty(result))
            {
                result = "[" + value + "]";
            }
            return result;
        }
    }

    /// <summary>
    /// The details of a network message received by a client or server on a network connection.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkMessage
    {
        /// <summary>
        /// The size of the largest message in bytes that can be sent on a NetworkConnection.
        /// <para>Note that channels that are not Fragmented cannot send messages larger than the Maximum Transmission Unity (MTU) size, which is about 1400 bytes by default.</para>
        /// </summary>
        public const int MaxMessageSize = (64 * 1024) - 1;

        /// <summary>
        /// The id of the message type of the message.
        /// </summary>
        public short msgType;
        /// <summary>
        /// The connection the message was recieved on.
        /// </summary>
        public NetworkConnection conn;
        /// <summary>
        /// A NetworkReader object that contains the contents of the message.
        /// <para>For some built-in message types with no body, this can be null.</para>
        /// </summary>
        public NetworkReader reader;
        /// <summary>
        /// The transport layer channel the message was sent on.
        /// </summary>
        public int channelId;

        /// <summary>
        /// Returns a string with the numeric representation of each byte in the payload.
        /// </summary>
        /// <param name="payload">Network message payload to dump.</param>
        /// <param name="sz">Length of payload in bytes.</param>
        /// <returns>Dumped info from payload.</returns>
        public static string Dump(byte[] payload, int sz)
        {
            string outStr = "[";
            for (int i = 0; i < sz; i++)
            {
                outStr += (payload[i] + " ");
            }
            outStr += "]";
            return outStr;
        }

        /// <summary>
        /// ReadMessage is used to extract a typed network message from the NetworkReader of a NetworkMessage object.
        /// <para>For example in a handler for the AddPlayer message:</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// using UnityEngine.Networking.NetworkSystem;
        /// 
        /// public class MyManager : NetworkManager
        /// {
        ///    void OnServerAddPlayerMessageInternal(NetworkMessage netMsg)
        ///    {
        ///        var msg = netMsg.ReadMessage&lt;AddPlayerMessage&gt;();
        ///        OnServerAddPlayer(netMsg.conn, msg.playerControllerId);
        ///    }
        /// }
        /// </code>
        /// <para>The AddPlayerMessage that is created will be populated by calling DeSerialize(). So when it is returned form ReadMessage it is ready to use.</para>
        /// </summary>
        /// <typeparam name="TMsg">The type of the Network Message, must be derived from MessageBase.</typeparam>
        /// <returns></returns>
        public TMsg ReadMessage<TMsg>() where TMsg : MessageBase, new()
        {
            var msg = new TMsg();
            msg.Deserialize(reader);
            return msg;
        }

        public void ReadMessage<TMsg>(TMsg msg) where TMsg : MessageBase
        {
            msg.Deserialize(reader);
        }
    }

    /// <summary>
    /// Enumeration of Networking versions.
    /// </summary>
    public enum Version
    {
        /// <summary>
        /// The current UNET version.
        /// </summary>
        Current = 1
    }

    /// <summary>
    /// Class containing constants for default network channels.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class Channels
    {
        /// <summary>
        /// The id of the default reliable channel used by the UNet HLAPI, This channel is used for state updates and spawning.
        /// </summary>
        public const int DefaultReliable = 0;
        /// <summary>
        /// The id of the default unreliable channel used for the UNet HLAPI. This channel is used for movement updates.
        /// </summary>
        public const int DefaultUnreliable = 1;
    }

    /// <summary>
    /// An enumeration of the options that can be set on a network channel.
    /// </summary>
    public enum ChannelOption
    {
        /// <summary>
        /// The option to set the number of pending buffers for a channel.
        /// <para>These buffers are allocated dynamically as required when writes to the transport layer fail. Each buffer will be the size of maxPacketSize for the channel - usually around 1400 bytes. The default is 16 buffers.</para>
        /// <para>This only applies to reliable channels. If a reliable channel runs out of pnding buffers, data will be lost.</para>
        /// </summary>
        MaxPendingBuffers = 1,
        AllowFragmentation = 2,
        MaxPacketSize = 3
            // maybe add an InitialCapacity for Pending Buffers list if needed in the future
    }

#if UNITY_EDITOR
    class Profiler
    {
        internal static void IncrementStatOutgoing(short msgType)
        {
            IncrementStatOutgoing(msgType, "msg");
        }

        internal static void IncrementStatOutgoing(short msgType, string name)
        {
            UnityEditor.NetworkDetailStats.IncrementStat(
                UnityEditor.NetworkDetailStats.NetworkDirection.Outgoing,
                msgType, name, 1);
        }

        internal static void IncrementStatIncoming(short msgType)
        {
            IncrementStatIncoming(msgType, "msg");
        }

        internal static void IncrementStatIncoming(short msgType, string name)
        {
            UnityEditor.NetworkDetailStats.IncrementStat(
                UnityEditor.NetworkDetailStats.NetworkDirection.Incoming,
                msgType, name, 1);
        }

        internal static void SetStatOutgoing(short msgType, int value)
        {
            UnityEditor.NetworkDetailStats.SetStat(
                UnityEditor.NetworkDetailStats.NetworkDirection.Outgoing,
                msgType, "msg", value);
        }

        internal static void ResetAll()
        {
            UnityEditor.NetworkDetailStats.ResetAll();
        }

        internal static void NewProfilerTick()
        {
            UnityEditor.NetworkDetailStats.NewProfilerTick(Time.time);
        }
    }
#endif
}
#pragma warning disable 618