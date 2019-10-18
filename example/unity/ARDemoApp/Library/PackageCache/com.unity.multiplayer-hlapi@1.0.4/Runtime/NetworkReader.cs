using System;
using System.Text;
using UnityEngine;

namespace UnityEngine.Networking
{
    /// <summary>
    /// General purpose serializer for UNET (for reading byte arrays).
    /// <para>This class works with NetworkWriter and is used for serializing data for UNet commands, RPC calls, events and low level messages.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class ExampleScript : MonoBehaviour
    /// {
    ///    // Writing data to a NetworkWriter and then
    ///    // Converting this to a NetworkReader.
    ///    void Start()
    ///    {
    ///        // The data you add to your writer must be prefixed with a message type.
    ///        // This is in the form of a short.
    ///        short myMsgType = 143;
    ///        NetworkWriter writer = new NetworkWriter();
    ///        // You start the message in your writer by passing in the message type.
    ///        // This is a short meaning that it will take up 2 bytes at the start of
    ///        // your message.
    ///        writer.StartMessage(myMsgType);
    ///        // You can now begin your message. In this case we will just use strings.
    ///        writer.Write("Test data 1");
    ///        writer.Write("Test data 2");
    ///        writer.Write("Test data 3");
    ///        // Make sure to end your message with FinishMessage()
    ///        writer.FinishMessage();
    ///        // You can now access the data in your writer. ToArray() returns a copy
    ///        // of the bytes that the writer is using and AsArray() returns the
    ///        // internal array of bytes, not a copy.
    ///        byte[] writerData = writer.ToArray();
    ///        CreateNetworkReader(writerData);
    ///    }
    ///
    ///    void CreateNetworkReader(byte[] data)
    ///    {
    ///        // We will create the NetworkReader using the data from our previous
    ///        // NetworkWriter.
    ///        NetworkReader networkReader = new NetworkReader(data);
    ///        // The first two bytes in the buffer represent the size
    ///        // of the message. This is equal to the NetworkReader.Length
    ///        // minus the size of the prefix.
    ///        byte[] readerMsgSizeData = networkReader.ReadBytes(2);
    ///        short readerMsgSize = (short)((readerMsgSizeData[1] &lt;&lt; 8) + readerMsgSizeData[0]);
    ///        Debug.Log(readerMsgSize);
    ///        // The message type added in NetworkWriter.StartMessage
    ///        // is to be read now. It is a short and so consists of
    ///        // two bytes. It is the second two bytes on the buffer.
    ///        byte[] readerMsgTypeData = networkReader.ReadBytes(2);
    ///        short readerMsgType = (short)((readerMsgTypeData[1] &lt;&lt; 8) + readerMsgTypeData[0]);
    ///        Debug.Log(readerMsgType);
    ///        // If all of your data is of the same type (in this case the
    ///        // data on our buffer is comprised of only strings) you can
    ///        // read all the data from the buffer using a loop like so.
    ///        while (networkReader.Position &lt; networkReader.Length)
    ///        {
    ///            Debug.Log(networkReader.ReadString());
    ///        }
    ///    }
    /// }
    /// </code>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkReader
    {
        NetBuffer m_buf;

        const int k_MaxStringLength = 1024 * 32;
        const int k_InitialStringBufferSize = 1024;
        static byte[] s_StringReaderBuffer;
        static Encoding s_Encoding;

        /// <summary>
        /// Creates a new NetworkReader object.
        /// </summary>
        public NetworkReader()
        {
            m_buf = new NetBuffer();
            Initialize();
        }

        /// <summary>
        /// Creates a new NetworkReader object.
        /// </summary>
        /// <param name="writer">A buffer to construct the reader with, this buffer is NOT copied.</param>
        public NetworkReader(NetworkWriter writer)
        {
            m_buf = new NetBuffer(writer.AsArray());
            Initialize();
        }

        public NetworkReader(byte[] buffer)
        {
            m_buf = new NetBuffer(buffer);
            Initialize();
        }

        static void Initialize()
        {
            if (s_Encoding == null)
            {
                s_StringReaderBuffer = new byte[k_InitialStringBufferSize];
                s_Encoding = new UTF8Encoding();
            }
        }

        /// <summary>
        /// The current position within the buffer.
        /// <para>See <see cref="NetworkReader">NetworkReader</see> for a code example.</para>
        /// </summary>
        public uint Position { get { return m_buf.Position; } }
        /// <summary>
        /// The current length of the buffer.
        /// <para>See <see cref="NetworkReader">NetworkReader</see> for a code example.</para>
        /// </summary>
        public int Length { get { return m_buf.Length; } }

        /// <summary>
        /// Sets the current position of the reader's stream to the start of the stream.
        /// </summary>
        public void SeekZero()
        {
            m_buf.SeekZero();
        }

        internal void Replace(byte[] buffer)
        {
            m_buf.Replace(buffer);
        }

        // http://sqlite.org/src4/doc/trunk/www/varint.wiki
        // NOTE: big endian.

        /// <summary>
        /// Reads a 32-bit variable-length-encoded value.
        /// </summary>
        /// <returns>The 32 bit value read.</returns>
        public UInt32 ReadPackedUInt32()
        {
            byte a0 = ReadByte();
            if (a0 < 241)
            {
                return a0;
            }
            byte a1 = ReadByte();
            if (a0 >= 241 && a0 <= 248)
            {
                return (UInt32)(240 + 256 * (a0 - 241) + a1);
            }
            byte a2 = ReadByte();
            if (a0 == 249)
            {
                return (UInt32)(2288 + 256 * a1 + a2);
            }
            byte a3 = ReadByte();
            if (a0 == 250)
            {
                return a1 + (((UInt32)a2) << 8) + (((UInt32)a3) << 16);
            }
            byte a4 = ReadByte();
            if (a0 >= 251)
            {
                return a1 + (((UInt32)a2) << 8) + (((UInt32)a3) << 16) + (((UInt32)a4) << 24);
            }
            throw new IndexOutOfRangeException("ReadPackedUInt32() failure: " + a0);
        }

        /// <summary>
        /// Reads a 64-bit variable-length-encoded value.
        /// </summary>
        /// <returns>The 64 bit value read.</returns>
        public UInt64 ReadPackedUInt64()
        {
            byte a0 = ReadByte();
            if (a0 < 241)
            {
                return a0;
            }
            byte a1 = ReadByte();
            if (a0 >= 241 && a0 <= 248)
            {
                return 240 + 256 * (a0 - ((UInt64)241)) + a1;
            }
            byte a2 = ReadByte();
            if (a0 == 249)
            {
                return 2288 + (((UInt64)256) * a1) + a2;
            }
            byte a3 = ReadByte();
            if (a0 == 250)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16);
            }
            byte a4 = ReadByte();
            if (a0 == 251)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24);
            }


            byte a5 = ReadByte();
            if (a0 == 252)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32);
            }


            byte a6 = ReadByte();
            if (a0 == 253)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32) + (((UInt64)a6) << 40);
            }


            byte a7 = ReadByte();
            if (a0 == 254)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32) + (((UInt64)a6) << 40) + (((UInt64)a7) << 48);
            }


            byte a8 = ReadByte();
            if (a0 == 255)
            {
                return a1 + (((UInt64)a2) << 8) + (((UInt64)a3) << 16) + (((UInt64)a4) << 24) + (((UInt64)a5) << 32) + (((UInt64)a6) << 40) + (((UInt64)a7) << 48)  + (((UInt64)a8) << 56);
            }
            throw new IndexOutOfRangeException("ReadPackedUInt64() failure: " + a0);
        }

        /// <summary>
        /// Reads a NetworkInstanceId from the stream.
        /// </summary>
        /// <returns>The NetworkInstanceId read.</returns>
        public NetworkInstanceId ReadNetworkId()
        {
            return new NetworkInstanceId(ReadPackedUInt32());
        }

        /// <summary>
        /// Reads a NetworkSceneId from the stream.
        /// </summary>
        /// <returns>The NetworkSceneId read.</returns>
        public NetworkSceneId ReadSceneId()
        {
            return new NetworkSceneId(ReadPackedUInt32());
        }

        /// <summary>
        /// Reads a byte from the stream.
        /// </summary>
        /// <returns>The value read.</returns>
        public byte ReadByte()
        {
            return m_buf.ReadByte();
        }

        /// <summary>
        /// Reads a signed byte from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public sbyte ReadSByte()
        {
            return (sbyte)m_buf.ReadByte();
        }

        /// <summary>
        /// Reads a signed 16 bit integer from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public short ReadInt16()
        {
            ushort value = 0;
            value |= m_buf.ReadByte();
            value |= (ushort)(m_buf.ReadByte() << 8);
            return (short)value;
        }

        /// <summary>
        /// Reads an unsigned 16 bit integer from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public ushort ReadUInt16()
        {
            ushort value = 0;
            value |= m_buf.ReadByte();
            value |= (ushort)(m_buf.ReadByte() << 8);
            return value;
        }

        /// <summary>
        /// Reads a signed 32bit integer from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public int ReadInt32()
        {
            uint value = 0;
            value |= m_buf.ReadByte();
            value |= (uint)(m_buf.ReadByte() << 8);
            value |= (uint)(m_buf.ReadByte() << 16);
            value |= (uint)(m_buf.ReadByte() << 24);
            return (int)value;
        }

        /// <summary>
        /// Reads an unsigned 32 bit integer from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public uint ReadUInt32()
        {
            uint value = 0;
            value |= m_buf.ReadByte();
            value |= (uint)(m_buf.ReadByte() << 8);
            value |= (uint)(m_buf.ReadByte() << 16);
            value |= (uint)(m_buf.ReadByte() << 24);
            return value;
        }

        /// <summary>
        /// Reads a signed 64 bit integer from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public long ReadInt64()
        {
            ulong value = 0;

            ulong other = m_buf.ReadByte();
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 8;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 16;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 24;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 32;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 40;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 48;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 56;
            value |= other;

            return (long)value;
        }

        /// <summary>
        /// Reads an unsigned 64 bit integer from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public ulong ReadUInt64()
        {
            ulong value = 0;
            ulong other = m_buf.ReadByte();
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 8;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 16;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 24;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 32;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 40;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 48;
            value |= other;

            other = ((ulong)m_buf.ReadByte()) << 56;
            value |= other;
            return value;
        }

        /// <summary>
        /// Reads a decimal from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public decimal ReadDecimal()
        {
            Int32[] bits = new Int32[4];

            bits[0] = ReadInt32();
            bits[1] = ReadInt32();
            bits[2] = ReadInt32();
            bits[3] = ReadInt32();

            return new decimal(bits);
        }

        /// <summary>
        /// Reads a float from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public float ReadSingle()
        {
            uint value = ReadUInt32();
            return FloatConversion.ToSingle(value);
        }

        /// <summary>
        /// Reads a double from the stream.
        /// </summary>
        /// <returns>Value read</returns>
        public double ReadDouble()
        {
            ulong value = ReadUInt64();
            return FloatConversion.ToDouble(value);
        }

        /// <summary>
        /// Reads a string from the stream. (max of 32k bytes).
        /// <para>See <see cref="NetworkReader">NetworkReader</see> for a code example.</para>
        /// </summary>
        /// <returns>Value read.</returns>
        public string ReadString()
        {
            UInt16 numBytes = ReadUInt16();
            if (numBytes == 0)
                return "";

            if (numBytes >= k_MaxStringLength)
            {
                throw new IndexOutOfRangeException("ReadString() too long: " + numBytes);
            }

            while (numBytes > s_StringReaderBuffer.Length)
            {
                s_StringReaderBuffer = new byte[s_StringReaderBuffer.Length * 2];
            }

            m_buf.ReadBytes(s_StringReaderBuffer, numBytes);

            char[] chars = s_Encoding.GetChars(s_StringReaderBuffer, 0, numBytes);
            return new string(chars);
        }

        /// <summary>
        /// Reads a char from the stream.
        /// </summary>
        /// <returns>Value read.</returns>
        public char ReadChar()
        {
            return (char)m_buf.ReadByte();
        }

        /// <summary>
        /// Reads a boolean from the stream.
        /// </summary>
        /// <returns>The value read.</returns>
        public bool ReadBoolean()
        {
            int value = m_buf.ReadByte();
            return value == 1;
        }

        /// <summary>
        /// Reads a number of bytes from the stream.
        /// <para>See <see cref="NetworkReader">NetworkReader</see> for a code example.</para>
        /// </summary>
        /// <param name="count">Number of bytes to read.</param>
        /// <returns>Bytes read. (this is a copy).</returns>
        public byte[] ReadBytes(int count)
        {
            if (count < 0)
            {
                throw new IndexOutOfRangeException("NetworkReader ReadBytes " + count);
            }
            //TODO: Allocation!
            byte[] value = new byte[count];
            m_buf.ReadBytes(value, (uint)count);
            return value;
        }

        /// <summary>
        /// This read a 16-bit byte count and a array of bytes of that size from the stream.
        /// <para>The format used by this function is the same as NetworkWriter.WriteBytesAndSize().</para>
        /// </summary>
        /// <returns>The bytes read from the stream.</returns>
        public byte[] ReadBytesAndSize()
        {
            ushort sz = ReadUInt16();
            if (sz == 0)
                return new byte[0];

            return ReadBytes(sz);
        }

        /// <summary>
        /// Reads a Unity Vector2 object.
        /// </summary>
        /// <returns>The vector read from the stream.</returns>
        public Vector2 ReadVector2()
        {
            return new Vector2(ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a Unity Vector3 objects.
        /// </summary>
        /// <returns>The vector read from the stream.</returns>
        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a Unity Vector4 object.
        /// </summary>
        /// <returns>The vector read from the stream</returns>
        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a unity Color objects.
        /// </summary>
        /// <returns>The color read from the stream.</returns>
        public Color ReadColor()
        {
            return new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a unity color32 objects.
        /// </summary>
        /// <returns>The color read from the stream.</returns>
        public Color32 ReadColor32()
        {
            return new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());
        }

        /// <summary>
        /// Reads a Unity Quaternion object.
        /// </summary>
        /// <returns>The quaternion read from the stream.</returns>
        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a Unity Rect object.
        /// </summary>
        /// <returns>The rect read from the stream.</returns>
        public Rect ReadRect()
        {
            return new Rect(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        /// <summary>
        /// Reads a unity Plane object.
        /// </summary>
        /// <returns>The plane read from the stream.</returns>
        public Plane ReadPlane()
        {
            return new Plane(ReadVector3(), ReadSingle());
        }

        /// <summary>
        /// Reads a Unity Ray object.
        /// </summary>
        /// <returns>The ray read from the stream.</returns>
        public Ray ReadRay()
        {
            return new Ray(ReadVector3(), ReadVector3());
        }

        /// <summary>
        /// Reads a unity Matrix4x4 object.
        /// </summary>
        /// <returns>The matrix read from the stream.</returns>
        public Matrix4x4 ReadMatrix4x4()
        {
            Matrix4x4 m = new Matrix4x4();
            m.m00 = ReadSingle();
            m.m01 = ReadSingle();
            m.m02 = ReadSingle();
            m.m03 = ReadSingle();
            m.m10 = ReadSingle();
            m.m11 = ReadSingle();
            m.m12 = ReadSingle();
            m.m13 = ReadSingle();
            m.m20 = ReadSingle();
            m.m21 = ReadSingle();
            m.m22 = ReadSingle();
            m.m23 = ReadSingle();
            m.m30 = ReadSingle();
            m.m31 = ReadSingle();
            m.m32 = ReadSingle();
            m.m33 = ReadSingle();
            return m;
        }

        /// <summary>
        /// Reads a NetworkHash128 assetId.
        /// </summary>
        /// <returns>The assetId object read from the stream.</returns>
        public NetworkHash128 ReadNetworkHash128()
        {
            NetworkHash128 hash;
            hash.i0 = ReadByte();
            hash.i1 = ReadByte();
            hash.i2 = ReadByte();
            hash.i3 = ReadByte();
            hash.i4 = ReadByte();
            hash.i5 = ReadByte();
            hash.i6 = ReadByte();
            hash.i7 = ReadByte();
            hash.i8 = ReadByte();
            hash.i9 = ReadByte();
            hash.i10 = ReadByte();
            hash.i11 = ReadByte();
            hash.i12 = ReadByte();
            hash.i13 = ReadByte();
            hash.i14 = ReadByte();
            hash.i15 = ReadByte();
            return hash;
        }

        /// <summary>
        /// Reads a reference to a Transform from the stream.
        /// <para>The game object of this Transform must have a NetworkIdentity.</para>
        /// </summary>
        /// <returns>The transform object read.</returns>
        public Transform ReadTransform()
        {
            NetworkInstanceId netId = ReadNetworkId();
            if (netId.IsEmpty())
            {
                return null;
            }
            GameObject go = ClientScene.FindLocalObject(netId);
            if (go == null)
            {
                if (LogFilter.logDebug) { Debug.Log("ReadTransform netId:" + netId); }
                return null;
            }

            return go.transform;
        }

        /// <summary>
        /// Reads a reference to a GameObject from the stream.
        /// </summary>
        /// <returns>The GameObject referenced.</returns>
        public GameObject ReadGameObject()
        {
            NetworkInstanceId netId = ReadNetworkId();
            if (netId.IsEmpty())
            {
                return null;
            }

            GameObject go;
            if (NetworkServer.active)
            {
                go = NetworkServer.FindLocalObject(netId);
            }
            else
            {
                go = ClientScene.FindLocalObject(netId);
            }
            if (go == null)
            {
                if (LogFilter.logDebug) { Debug.Log("ReadGameObject netId:" + netId + "go: null"); }
            }

            return go;
        }

        /// <summary>
        /// Reads a reference to a NetworkIdentity from the stream.
        /// </summary>
        /// <returns>The NetworkIdentity object read.</returns>
        public NetworkIdentity ReadNetworkIdentity()
        {
            NetworkInstanceId netId = ReadNetworkId();
            if (netId.IsEmpty())
            {
                return null;
            }
            GameObject go;
            if (NetworkServer.active)
            {
                go = NetworkServer.FindLocalObject(netId);
            }
            else
            {
                go = ClientScene.FindLocalObject(netId);
            }
            if (go == null)
            {
                if (LogFilter.logDebug) { Debug.Log("ReadNetworkIdentity netId:" + netId + "go: null"); }
                return null;
            }

            return go.GetComponent<NetworkIdentity>();
        }

        /// <summary>
        /// Returns a string representation of the reader's buffer.
        /// </summary>
        /// <returns>Buffer contents.</returns>
        public override string ToString()
        {
            return m_buf.ToString();
        }

        /// <summary>
        /// This is a utility function to read a typed network message from the stream.
        /// </summary>
        /// <typeparam name="TMsg">The type of the Network Message, must be derived from MessageBase.</typeparam>
        /// <returns></returns>
        public TMsg ReadMessage<TMsg>() where TMsg : MessageBase, new()
        {
            var msg = new TMsg();
            msg.Deserialize(this);
            return msg;
        }
    };
}
