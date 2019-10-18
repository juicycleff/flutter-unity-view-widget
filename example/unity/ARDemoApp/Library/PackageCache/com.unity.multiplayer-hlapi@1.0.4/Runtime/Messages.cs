using System;
using System.Collections.Generic;

#pragma warning disable 618
namespace UnityEngine.Networking
{
    /// <summary>
    /// Network message classes should be derived from this class. These message classes can then be sent using the various Send functions of NetworkConnection, NetworkClient and NetworkServer.
    /// <para>Public data fields of classes derived from MessageBase will be automatically serialized with the class. The virtual methods Serialize and Deserialize may be implemented by developers for precise control, but if they are not implemented, then implementations will be generated for them.</para>
    /// <para><b>Note :</b> Unity uses its own network serialization system. It doesn't support the NonSerialized attribute. Instead, use private variables.</para>
    /// <para>In the example below, the methods have implementations, but if those methods were not implemented, the message would still be usable.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// class SpawnMessage : MessageBase
    /// {
    ///    public uint netId;
    ///    public NetworkHash128 assetId;
    ///    public Vector3 position;
    ///    public byte[] payload;
    ///
    ///    // This method would be generated
    ///    public override void Deserialize(NetworkReader reader)
    ///    {
    ///        netId = reader.ReadPackedUInt32();
    ///        assetId = reader.ReadNetworkHash128();
    ///        position = reader.ReadVector3();
    ///        payload = reader.ReadBytesAndSize();
    ///    }
    ///
    ///    // This method would be generated
    ///    public override void Serialize(NetworkWriter writer)
    ///    {
    ///        writer.WritePackedUInt32(netId);
    ///        writer.Write(assetId);
    ///        writer.Write(position);
    ///        writer.WriteBytesFull(payload);
    ///    }
    /// }
    /// </code>
    /// </summary>
    // This can't be an interface because users don't need to implement the
    // serialization functions, we'll code generate it for them when they omit it.
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public abstract class MessageBase
    {
        /// <summary>
        /// This method is used to populate a message object from a NetworkReader stream.
        /// <para>Developers may implement this method for precise control of serialization, but they do no have to. An implemenation of this method will be generated for derived classes.</para>
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        // De-serialize the contents of the reader into this message
        public virtual void Deserialize(NetworkReader reader) {}

        /// <summary>
        /// The method is used to populate a NetworkWriter stream from a message object.
        /// <para>Developers may implement this method for precise control of serialization, but they do no have to. An implemenation of this method will be generated for derived classes.</para>
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        // Serialize the contents of this message into the writer
        public virtual void Serialize(NetworkWriter writer) {}
    }
}

namespace UnityEngine.Networking.NetworkSystem
{
    // ---------- General Typed Messages -------------------
    /// <summary>
    /// This is a utility class for simple network messages that contain only a string.
    /// <para>This example sends a message with the name of the scene.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    /// using UnityEngine.Networking.NetworkSystem;
    ///
    /// public class Test
    /// {
    ///    void SendSceneName(string sceneName)
    ///    {
    ///        var msg = new StringMessage(sceneName);
    ///        NetworkServer.SendToAll(MsgType.Scene, msg);
    ///    }
    /// }
    /// </code>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class StringMessage : MessageBase
    {
        /// <summary>
        /// The string that will be serialized.
        /// </summary>
        public string value;

        public StringMessage()
        {
        }

        public StringMessage(string v)
        {
            value = v;
        }

        public override void Deserialize(NetworkReader reader)
        {
            value = reader.ReadString();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(value);
        }
    }

    /// <summary>
    /// A utility class to send simple network messages that only contain an integer.
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    /// using UnityEngine.Networking.NetworkSystem;
    ///
    /// public class Test
    /// {
    ///    void SendValue(int value)
    ///    {
    ///        var msg = new IntegerMessage(value);
    ///        NetworkServer.SendToAll(MsgType.Scene, msg);
    ///    }
    /// }
    /// </code>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class IntegerMessage : MessageBase
    {
        /// <summary>
        /// The integer value to serialize.
        /// </summary>
        public int value;

        public IntegerMessage()
        {
        }

        public IntegerMessage(int v)
        {
            value = v;
        }

        public override void Deserialize(NetworkReader reader)
        {
            value = (int)reader.ReadPackedUInt32();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32((uint)value);
        }
    }

    /// <summary>
    /// A utility class to send a network message with no contents.
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    /// using UnityEngine.Networking.NetworkSystem;
    /// 
    /// public class Test
    /// {
    ///    void SendNotification()
    ///    {
    ///        var msg = new EmptyMessage();
    ///        NetworkServer.SendToAll(667, msg);
    ///    }
    /// }
    /// </code>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class EmptyMessage : MessageBase
    {
        public override void Deserialize(NetworkReader reader)
        {
        }

        public override void Serialize(NetworkWriter writer)
        {
        }
    }

    // ---------- Public System Messages -------------------
    /// <summary>
    /// This is passed to handler functions registered for the SYSTEM_ERROR built-in message.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ErrorMessage : MessageBase
    {
        /// <summary>
        /// The error code.
        /// <para>This is a value from the UNETError enumeration.</para>
        /// </summary>
        public int errorCode;

        public override void Deserialize(NetworkReader reader)
        {
            errorCode = reader.ReadUInt16();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write((ushort)errorCode);
        }
    }

    /// <summary>
    /// This is passed to handler funtions registered for the SYSTEM_READY built-in message.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ReadyMessage : EmptyMessage
    {
    }

    /// <summary>
    /// This is passed to handler funtions registered for the SYSTEM_NOT_READY built-in message.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NotReadyMessage : EmptyMessage
    {
    }

    /// <summary>
    /// This is passed to handler funtions registered for the AddPlayer built-in message.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class AddPlayerMessage : MessageBase
    {
        /// <summary>
        /// The playerId of the new player.
        /// <para>This is specified by the client when they call NetworkClient.AddPlayer(someId).</para>
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public short playerControllerId;
        /// <summary>
        /// The size of the extra message data included in the AddPlayerMessage.
        /// </summary>
        public int msgSize;
        /// <summary>
        /// The extra message data included in the AddPlayerMessage.
        /// </summary>
        public byte[] msgData;

        public override void Deserialize(NetworkReader reader)
        {
            playerControllerId = (short)reader.ReadUInt16();
            msgData = reader.ReadBytesAndSize();
            if (msgData == null)
            {
                msgSize = 0;
            }
            else
            {
                msgSize = msgData.Length;
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write((ushort)playerControllerId);
            writer.WriteBytesAndSize(msgData, msgSize);
        }
    }

    /// <summary>
    /// This is passed to handler funtions registered for the SYSTEM_REMOVE_PLAYER built-in message.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class RemovePlayerMessage : MessageBase
    {
        /// <summary>
        /// The player ID of the player GameObject which should be removed.
        /// <para>This is specified by the client when they call NetworkClient.RemovePlayer(someId).</para>
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public short playerControllerId;

        public override void Deserialize(NetworkReader reader)
        {
            playerControllerId = (short)reader.ReadUInt16();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write((ushort)playerControllerId);
        }
    }

    /// <summary>
    /// Information about a change in authority of a non-player in the same network game.
    /// <para>This information is cached by clients and used during host-migration.</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class PeerAuthorityMessage : MessageBase
    {
        /// <summary>
        /// The connection Id (on the server) of the peer whose authority is changing for the object.
        /// </summary>
        public int connectionId;
        /// <summary>
        /// The network id of the object whose authority state changed.
        /// </summary>
        public NetworkInstanceId netId;
        /// <summary>
        /// The new state of authority for the object referenced by this message.
        /// </summary>
        public bool authorityState;

        public override void Deserialize(NetworkReader reader)
        {
            connectionId = (int)reader.ReadPackedUInt32();
            netId = reader.ReadNetworkId();
            authorityState = reader.ReadBoolean();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32((uint)connectionId);
            writer.Write(netId);
            writer.Write(authorityState);
        }
    }

    /// <summary>
    /// A structure used to identify player object on other peers for host migration.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public struct PeerInfoPlayer
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
    }

    /// <summary>
    /// Information about another participant in the same network game.
    /// <para>This information is cached by clients and used during host-migration.</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class PeerInfoMessage : MessageBase
    {
        /// <summary>
        /// The id of the NetworkConnection associated with the peer.
        /// </summary>
        public int connectionId;
        /// <summary>
        /// The IP address of the peer.
        /// </summary>
        public string address;
        /// <summary>
        /// The network port being used by the peer.
        /// </summary>
        public int port;
        /// <summary>
        /// True if this peer is the host of the network game.
        /// </summary>
        public bool isHost;
        /// <summary>
        /// True if the peer if the same as the current client.
        /// </summary>
        public bool isYou;
        /// <summary>
        /// The players for this peer.
        /// </summary>
        public PeerInfoPlayer[] playerIds;

        public override void Deserialize(NetworkReader reader)
        {
            connectionId = (int)reader.ReadPackedUInt32();
            address = reader.ReadString();
            port = (int)reader.ReadPackedUInt32();
            isHost = reader.ReadBoolean();
            isYou = reader.ReadBoolean();

            uint numPlayers = reader.ReadPackedUInt32();
            if (numPlayers > 0)
            {
                List<PeerInfoPlayer> ids = new List<PeerInfoPlayer>();
                for (uint i = 0; i < numPlayers; i++)
                {
                    PeerInfoPlayer info;
                    info.netId = reader.ReadNetworkId();
                    info.playerControllerId = (short)reader.ReadPackedUInt32();
                    ids.Add(info);
                }
                playerIds = ids.ToArray();
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32((uint)connectionId);
            writer.Write(address);
            writer.WritePackedUInt32((uint)port);
            writer.Write(isHost);
            writer.Write(isYou);
            if (playerIds == null)
            {
                writer.WritePackedUInt32(0);
            }
            else
            {
                writer.WritePackedUInt32((uint)playerIds.Length);
                for (int i = 0; i < playerIds.Length; i++)
                {
                    writer.Write(playerIds[i].netId);
                    writer.WritePackedUInt32((uint)playerIds[i].playerControllerId);
                }
            }
        }

        public override string ToString()
        {
            return "PeerInfo conn:" + connectionId + " addr:" + address + ":" + port + " host:" + isHost + " isYou:" + isYou;
        }
    }

    /// <summary>
    /// Internal UNET message for sending information about network peers to clients.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class PeerListMessage : MessageBase
    {
        /// <summary>
        /// The list of participants in a networked game.
        /// </summary>
        public PeerInfoMessage[] peers;
        /// <summary>
        /// The connectionId of this client on the old host.
        /// </summary>
        public int oldServerConnectionId;

        public override void Deserialize(NetworkReader reader)
        {
            oldServerConnectionId = (int)reader.ReadPackedUInt32();
            int numPeers = reader.ReadUInt16();
            peers = new PeerInfoMessage[numPeers];
            for (int i = 0; i < peers.Length; ++i)
            {
                var peerInfo = new PeerInfoMessage();
                peerInfo.Deserialize(reader);
                peers[i] = peerInfo;
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32((uint)oldServerConnectionId);
            writer.Write((ushort)peers.Length);
            for (int i = 0; i < peers.Length; i++)
            {
                peers[i].Serialize(writer);
            }
        }
    }

    /// <summary>
    /// This network message is used when a client reconnect to the new host of a game.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ReconnectMessage : MessageBase
    {
        /// <summary>
        /// This client's connectionId on the old host.
        /// </summary>
        public int oldConnectionId;
        /// <summary>
        /// The playerControllerId of the player that is rejoining.
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public short playerControllerId;
        /// <summary>
        /// The networkId of this player on the old host.
        /// </summary>
        public NetworkInstanceId netId;
        /// <summary>
        /// Size of additional data.
        /// </summary>
        public int msgSize;
        /// <summary>
        /// Additional data.
        /// </summary>
        public byte[] msgData;

        public override void Deserialize(NetworkReader reader)
        {
            oldConnectionId = (int)reader.ReadPackedUInt32();
            playerControllerId = (short)reader.ReadPackedUInt32();
            netId = reader.ReadNetworkId();
            msgData = reader.ReadBytesAndSize();
            msgSize = msgData.Length;
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32((uint)oldConnectionId);
            writer.WritePackedUInt32((uint)playerControllerId);
            writer.Write(netId);
            writer.WriteBytesAndSize(msgData, msgSize);
        }
    }

    // ---------- System Messages requried for code gen path -------------------
    /* These are not used directly but manually serialized, these are here for reference.

    public struct CommandMessage
    {
        public int cmdHash;
        public string cmdName;
        public byte[] payload;
    }
    public struct RPCMessage
    {
        public NetworkId netId;
        public int cmdHash;
        public byte[] payload;
    }
    public struct SyncEventMessage
    {
        public NetworkId netId;
        public int cmdHash;
        public byte[] payload;
    }

    internal class SyncListMessage<T> where T: struct
    {
        public NetworkId netId;
        public int cmdHash;
        public byte operation;
        public int itemIndex;
        public T item;
    }

*/

    // ---------- Internal System Messages -------------------
    class ObjectSpawnMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public NetworkHash128 assetId;
        public Vector3 position;
        public byte[] payload;
        public Quaternion rotation;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            assetId = reader.ReadNetworkHash128();
            position = reader.ReadVector3();
            payload = reader.ReadBytesAndSize();

            uint extraPayloadSize = sizeof(uint) * 4;
            if ((reader.Length - reader.Position) >= extraPayloadSize)
            {
                rotation = reader.ReadQuaternion();
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.Write(assetId);
            writer.Write(position);
            writer.WriteBytesFull(payload);

            writer.Write(rotation);
        }
    }

    class ObjectSpawnSceneMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public NetworkSceneId sceneId;
        public Vector3 position;
        public byte[] payload;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            sceneId = reader.ReadSceneId();
            position = reader.ReadVector3();
            payload = reader.ReadBytesAndSize();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.Write(sceneId);
            writer.Write(position);
            writer.WriteBytesFull(payload);
        }
    }

    class ObjectSpawnFinishedMessage : MessageBase
    {
        public uint state;

        public override void Deserialize(NetworkReader reader)
        {
            state = reader.ReadPackedUInt32();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(state);
        }
    }

    class ObjectDestroyMessage : MessageBase
    {
        public NetworkInstanceId netId;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
        }
    }

    class OwnerMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public short playerControllerId;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            playerControllerId = (short)reader.ReadPackedUInt32();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.WritePackedUInt32((uint)playerControllerId);
        }
    }

    class ClientAuthorityMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public bool authority;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            authority = reader.ReadBoolean();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.Write(authority);
        }
    }

    class OverrideTransformMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public byte[] payload;
        public bool teleport;
        public int time;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            payload = reader.ReadBytesAndSize();
            teleport = reader.ReadBoolean();
            time = (int)reader.ReadPackedUInt32();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.WriteBytesFull(payload);
            writer.Write(teleport);
            writer.WritePackedUInt32((uint)time);
        }
    }

    class AnimationMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public int      stateHash;      // if non-zero, then Play() this animation, skipping transitions
        public float    normalizedTime;
        public byte[]   parameters;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            stateHash = (int)reader.ReadPackedUInt32();
            normalizedTime = reader.ReadSingle();
            parameters = reader.ReadBytesAndSize();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.WritePackedUInt32((uint)stateHash);
            writer.Write(normalizedTime);

            if (parameters == null)
                writer.WriteBytesAndSize(parameters, 0);
            else
                writer.WriteBytesAndSize(parameters, parameters.Length);
        }
    }

    class AnimationParametersMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public byte[]   parameters;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            parameters = reader.ReadBytesAndSize();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);

            if (parameters == null)
                writer.WriteBytesAndSize(parameters, 0);
            else
                writer.WriteBytesAndSize(parameters, parameters.Length);
        }
    }

    class AnimationTriggerMessage : MessageBase
    {
        public NetworkInstanceId netId;
        public int      hash;

        public override void Deserialize(NetworkReader reader)
        {
            netId = reader.ReadNetworkId();
            hash = (int)reader.ReadPackedUInt32();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(netId);
            writer.WritePackedUInt32((uint)hash);
        }
    }

    class LobbyReadyToBeginMessage : MessageBase
    {
        public byte slotId;
        public bool readyState;

        public override void Deserialize(NetworkReader reader)
        {
            slotId = reader.ReadByte();
            readyState = reader.ReadBoolean();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(slotId);
            writer.Write(readyState);
        }
    }

    struct CRCMessageEntry
    {
        public string name;
        public byte channel;
    }

    class CRCMessage : MessageBase
    {
        public CRCMessageEntry[] scripts;

        public override void Deserialize(NetworkReader reader)
        {
            int numScripts = reader.ReadUInt16();
            scripts = new CRCMessageEntry[numScripts];
            for (int i = 0; i < scripts.Length; ++i)
            {
                var entry = new CRCMessageEntry();
                entry.name = reader.ReadString();
                entry.channel = reader.ReadByte();
                scripts[i] = entry;
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write((ushort)scripts.Length);
            for (int i = 0; i < scripts.Length; i++)
            {
                writer.Write(scripts[i].name);
                writer.Write(scripts[i].channel);
            }
        }
    }
}
#pragma warning restore 618
