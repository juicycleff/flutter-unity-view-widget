using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This is a list of strings that will be synchronized from the server to clients.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public sealed class SyncListString : SyncList<string>
    {
        protected override void SerializeItem(NetworkWriter writer, string item)
        {
            writer.Write(item);
        }

        protected override string DeserializeItem(NetworkReader reader)
        {
            return reader.ReadString();
        }
        
        [System.Obsolete("ReadReference is now used instead")]
        static public SyncListString ReadInstance(NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            var result = new SyncListString();
            for (ushort i = 0; i < count; i++)
            {
                result.AddInternal(reader.ReadString());
            }
            return result;
        }

        /// <summary>
        /// An internal function used for serializing SyncList member variables.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="syncList"></param>
        static public void ReadReference(NetworkReader reader, SyncListString syncList)
        {
            ushort count = reader.ReadUInt16();
            syncList.Clear();
            for (ushort i = 0; i < count; i++)
            {
                syncList.AddInternal(reader.ReadString());
            }
        }

        static public void WriteInstance(NetworkWriter writer, SyncListString items)
        {
            writer.Write((ushort)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                writer.Write(items[i]);
            }
        }
    }

    /// <summary>
    /// A list of floats that will be synchronized from server to clients.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public sealed class SyncListFloat : SyncList<float>
    {
        protected override void SerializeItem(NetworkWriter writer, float item)
        {
            writer.Write(item);
        }

        protected override float DeserializeItem(NetworkReader reader)
        {
            return reader.ReadSingle();
        }

        [System.Obsolete("ReadReference is now used instead")]
        static public SyncListFloat ReadInstance(NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            var result = new SyncListFloat();
            for (ushort i = 0; i < count; i++)
            {
                result.AddInternal(reader.ReadSingle());
            }
            return result;
        }

        /// <summary>
        /// An internal function used for serializing SyncList member variables.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="syncList"></param>
        static public void ReadReference(NetworkReader reader, SyncListFloat syncList)
        {
            ushort count = reader.ReadUInt16();
            syncList.Clear();
            for (ushort i = 0; i < count; i++)
            {
                syncList.AddInternal(reader.ReadSingle());
            }
        }

        static public void WriteInstance(NetworkWriter writer, SyncListFloat items)
        {
            writer.Write((ushort)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                writer.Write(items[i]);
            }
        }
    }

    /// <summary>
    /// A list of integers that will be synchronized from server to clients.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class SyncListInt : SyncList<int>
    {
        protected override void SerializeItem(NetworkWriter writer, int item)
        {
            writer.WritePackedUInt32((uint)item);
        }

        protected override int DeserializeItem(NetworkReader reader)
        {
            return (int)reader.ReadPackedUInt32();
        }

        [System.Obsolete("ReadReference is now used instead")]
        static public SyncListInt ReadInstance(NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            var result = new SyncListInt();
            for (ushort i = 0; i < count; i++)
            {
                result.AddInternal((int)reader.ReadPackedUInt32());
            }
            return result;
        }

        /// <summary>
        /// An internal function used for serializing SyncList member variables.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="syncList"></param>
        static public void ReadReference(NetworkReader reader, SyncListInt syncList)
        {
            ushort count = reader.ReadUInt16();
            syncList.Clear();
            for (ushort i = 0; i < count; i++)
            {
                syncList.AddInternal((int)reader.ReadPackedUInt32());
            }
        }

        static public void WriteInstance(NetworkWriter writer, SyncListInt items)
        {
            writer.Write((ushort)items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                writer.WritePackedUInt32((uint)items[i]);
            }
        }
    }

    /// <summary>
    /// A list of unsigned integers that will be synchronized from server to clients.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class SyncListUInt : SyncList<uint>
    {
        protected override void SerializeItem(NetworkWriter writer, uint item)
        {
            writer.WritePackedUInt32(item);
        }

        protected override uint DeserializeItem(NetworkReader reader)
        {
            return reader.ReadPackedUInt32();
        }

        [System.Obsolete("ReadReference is now used instead")]
        static public SyncListUInt ReadInstance(NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            var result = new SyncListUInt();
            for (ushort i = 0; i < count; i++)
            {
                result.AddInternal(reader.ReadPackedUInt32());
            }
            return result;
        }

        /// <summary>
        /// An internal function used for serializing SyncList member variables.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="syncList"></param>
        static public void ReadReference(NetworkReader reader, SyncListUInt syncList)
        {
            ushort count = reader.ReadUInt16();
            syncList.Clear();
            for (ushort i = 0; i < count; i++)
            {
                syncList.AddInternal(reader.ReadPackedUInt32());
            }
        }

        static public void WriteInstance(NetworkWriter writer, SyncListUInt items)
        {
            writer.Write((ushort)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                writer.WritePackedUInt32(items[i]);
            }
        }
    }

    /// <summary>
    /// A list of booleans that will be synchronized from server to clients.
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class SyncListBool : SyncList<bool>
    {
        protected override void SerializeItem(NetworkWriter writer, bool item)
        {
            writer.Write(item);
        }

        protected override bool DeserializeItem(NetworkReader reader)
        {
            return reader.ReadBoolean();
        }

        [System.Obsolete("ReadReference is now used instead")]
        static public SyncListBool ReadInstance(NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            var result = new SyncListBool();
            for (ushort i = 0; i < count; i++)
            {
                result.AddInternal(reader.ReadBoolean());
            }
            return result;
        }

        /// <summary>
        /// An internal function used for serializing SyncList member variables.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="syncList"></param>
        static public void ReadReference(NetworkReader reader, SyncListBool syncList)
        {
            ushort count = reader.ReadUInt16();
            syncList.Clear();
            for (ushort i = 0; i < count; i++)
            {
                syncList.AddInternal(reader.ReadBoolean());
            }
        }

        static public void WriteInstance(NetworkWriter writer, SyncListBool items)
        {
            writer.Write((ushort)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                writer.Write(items[i]);
            }
        }
    }

    /// <summary>
    /// This class is used for lists of structs that are synchronized from the server to clients.
    /// <para>To use SyncListStruct, derive a new class with your struct as the generic parameter.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class SyncListStruct<T> : SyncList<T> where T : struct
    {
        new public void AddInternal(T item)
        {
            base.AddInternal(item);
        }

        protected override void SerializeItem(NetworkWriter writer, T item)
        {
        }

        protected override T DeserializeItem(NetworkReader reader)
        {
            return new T();
        }

        public T GetItem(int i)
        {
            return base[i];
        }

        new public ushort Count { get { return (ushort)base.Count; } }
    }

    /// <summary>
    /// This is the base class for type-specific SyncList classes.
    /// <para>A SyncList can only be of the following type;</para>
    /// 
    /// <list type="bullet">
    /// <item>
    /// <description>Basic type (byte, int, float, string, UInt64, etc)</description>
    /// </item>
    /// <item>
    /// <description>Built-in Unity math type (Vector3, Quaternion, etc),</description>
    /// </item>
    /// <item>
    /// <description>NetworkIdentity</description>
    /// </item>
    /// <item>
    /// <description>NetworkInstanceId</description>
    /// </item>
    /// <item>
    /// <description>NetworkHash128</description>
    /// </item>
    /// <item>
    /// <description>GameObject with a NetworkIdentity component attached.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    abstract public class SyncList<T> : IList<T>
    {
        /// <summary>
        /// A delegate that can be populated to recieve callbacks when the list changes.
        /// <para>For example this function is called when the m_ints list changes:</para>
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public  class MyBehaviour : NetworkBehaviour
        /// {
        ///    public SyncListInt m_ints = new SyncListInt();
        ///
        ///    private void OnIntChanged(SyncListInt.Operation op, int index)
        ///    {
        ///        Debug.Log("list changed " + op);
        ///    }
        ///
        ///    public override void OnStartClient()
        ///    {
        ///        m_ints.Callback = OnIntChanged;
        ///    }
        /// }
        /// </code>
        /// <para>It is best to populate the delagate during the OnStartClient() callback function. Doing it earlier can lead to it being lost when the initial list value is applied.</para>
        /// </summary>
        /// <param name="op"></param>
        /// <param name="itemIndex"></param>
        public delegate void SyncListChanged(Operation op, int itemIndex);

        List<T> m_Objects = new List<T>();

        /// <summary>
        /// Returns the number of elements in this SyncList<T>.
        /// </summary>
        public int Count { get { return m_Objects.Count; } }
        /// <summary>
        /// Reports whether the SyncList<T> is read-only.
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// The delegate type used for SyncListChanged.
        /// </summary>
        public SyncListChanged Callback { get { return m_Callback; } set { m_Callback = value; } }

        /// <summary>
        /// The types of operations that can occur for SyncLists.
        /// </summary>
        public enum Operation
        {
            /// <summary>
            /// Item was added to the list.
            /// </summary>
            OP_ADD,
            /// <summary>
            /// The list was cleared.
            /// </summary>
            OP_CLEAR,
            /// <summary>
            /// An item was inserted into the list.
            /// </summary>
            OP_INSERT,
            /// <summary>
            /// An item was removed from the list.
            /// </summary>
            OP_REMOVE,
            /// <summary>
            /// An item was removed at an index from the list.
            /// </summary>
            OP_REMOVEAT,
            /// <summary>
            /// An item was set to a new value in the list.
            /// </summary>
            OP_SET,
            /// <summary>
            /// An item in the list was manually marked dirty.
            /// </summary>
            OP_DIRTY
        };

        NetworkBehaviour m_Behaviour;
        int m_CmdHash;
        SyncListChanged m_Callback;

        /// <summary>
        /// This is used to write a value object from a SyncList to a stream.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        /// <param name="item">Item to write.</param>
        abstract protected void SerializeItem(NetworkWriter writer, T item);
        /// <summary>
        /// This method is used when deserializing SyncList items from a stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        /// <returns>New instance of the SyncList value type.</returns>
        abstract protected T DeserializeItem(NetworkReader reader);


        /// <summary>
        /// Internal function.
        /// </summary>
        /// <param name="beh">The behaviour the list belongs to.</param>
        /// <param name="cmdHash">Identifies this list.</param>
        public void InitializeBehaviour(NetworkBehaviour beh, int cmdHash)
        {
            m_Behaviour = beh;
            m_CmdHash = cmdHash;
        }

        void SendMsg(Operation op, int itemIndex, T item)
        {
            if (m_Behaviour == null)
            {
                if (LogFilter.logError) { Debug.LogError("SyncList not initialized"); }
                return;
            }

            var uv = m_Behaviour.GetComponent<NetworkIdentity>();
            if (uv == null)
            {
                if (LogFilter.logError) { Debug.LogError("SyncList no NetworkIdentity"); }
                return;
            }

            if (!uv.isServer)
            {
                // object is not spawned yet, so no need to send updates.
                return;
            }

            NetworkWriter writer = new NetworkWriter();
            writer.StartMessage(MsgType.SyncList);
            writer.Write(uv.netId);
            writer.WritePackedUInt32((uint)m_CmdHash);
            writer.Write((byte)op);
            writer.WritePackedUInt32((uint)itemIndex);
            SerializeItem(writer, item);
            writer.FinishMessage();

            NetworkServer.SendWriterToReady(uv.gameObject, writer, m_Behaviour.GetNetworkChannel());

#if UNITY_EDITOR
            Profiler.IncrementStatOutgoing(MsgType.SyncList, op.ToString());
#endif

            // ensure it is invoked on host
            if (m_Behaviour.isServer && m_Behaviour.isClient && m_Callback != null)
            {
                m_Callback.Invoke(op, itemIndex);
            }
        }

        void SendMsg(Operation op, int itemIndex)
        {
            SendMsg(op, itemIndex, default(T));
        }

        public void HandleMsg(NetworkReader reader)
        {
            byte op = reader.ReadByte();
            int itemIndex = (int)reader.ReadPackedUInt32();
            T item = DeserializeItem(reader);

            switch ((Operation)op)
            {
                case Operation.OP_ADD:
                    m_Objects.Add(item);
                    break;

                case Operation.OP_CLEAR:
                    m_Objects.Clear();
                    break;

                case Operation.OP_INSERT:
                    m_Objects.Insert(itemIndex, item);
                    break;

                case Operation.OP_REMOVE:
                    m_Objects.Remove(item);
                    break;

                case Operation.OP_REMOVEAT:
                    m_Objects.RemoveAt(itemIndex);
                    break;

                case Operation.OP_SET:
                case Operation.OP_DIRTY:
                    m_Objects[itemIndex] = item;
                    break;
            }
            if (m_Callback != null)
            {
                m_Callback.Invoke((Operation)op, itemIndex);
            }
        }

        // used to bypass Add message.
        internal void AddInternal(T item)
        {
            m_Objects.Add(item);
        }

        /// <summary>
        /// Same as List:Add() but the item is added on clients.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            m_Objects.Add(item);
            SendMsg(Operation.OP_ADD, m_Objects.Count - 1, item);
        }

        /// <summary>
        /// Same as List:Clear() but the list is cleared on clients.
        /// </summary>
        public void Clear()
        {
            m_Objects.Clear();
            SendMsg(Operation.OP_CLEAR, 0);
        }

        /// <summary>
        /// Determines whether the list contains item item.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>True if item contain</returns>
        public bool Contains(T item)
        {
            return m_Objects.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the SyncList<T> to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">Array to copy elements to.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int index)
        {
            m_Objects.CopyTo(array, index);
        }

        /// <summary>
        /// Determines the index of a specific item in the SyncList<T>.
        /// </summary>
        /// <param name="item">The item to return the index for.</param>
        /// <returns>Index of the item</returns>
        public int IndexOf(T item)
        {
            return m_Objects.IndexOf(item);
        }

        /// <summary>
        /// Same as List::Insert() but also inserts into list on clients.
        /// </summary>
        /// <param name="index">Where to insert the item.</param>
        /// <param name="item">Item to insert.</param>
        public void Insert(int index, T item)
        {
            m_Objects.Insert(index, item);
            SendMsg(Operation.OP_INSERT, index, item);
        }

        /// <summary>
        /// Same as List:Remove except removes on clients also.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            var result = m_Objects.Remove(item);
            if (result)
            {
                SendMsg(Operation.OP_REMOVE, 0, item);
            }
            return result;
        }

        /// <summary>
        /// Same as List:Remove except it removes the index on clients also.
        /// </summary>
        /// <param name="index">Index to remove.</param>
        public void RemoveAt(int index)
        {
            m_Objects.RemoveAt(index);
            SendMsg(Operation.OP_REMOVEAT, index);
        }

        /// <summary>
        /// Marks an item in the list as dirty, so it will be updated on clients.
        /// </summary>
        /// <param name="index">Index of item to dirty.</param>
        public void Dirty(int index)
        {
            SendMsg(Operation.OP_DIRTY, index, m_Objects[index]);
        }

        public T this[int i]
        {
            get { return m_Objects[i]; }
            set
            {
                bool changed = false;
                if (m_Objects[i] == null)
                {
                    if (value == null)
                        return;
                    else
                        changed = true;
                }
                else
                {
                    changed = !m_Objects[i].Equals(value);
                }

                m_Objects[i] = value;
                if (changed)
                {
                    SendMsg(Operation.OP_SET, i, value);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the SyncList<T>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
