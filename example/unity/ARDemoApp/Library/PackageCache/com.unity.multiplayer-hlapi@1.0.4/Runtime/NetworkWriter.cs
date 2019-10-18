using System;
using System.Text;
using UnityEngine;

namespace UnityEngine.Networking
{
    /*
    // Binary stream Writer. Supports simple types, buffers, arrays, structs, and nested types
        */
    /// <summary>
    /// General purpose serializer for UNET (for serializing data to byte arrays).
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
    public class NetworkWriter
    {
        const int k_MaxStringLength = 1024 * 32;
        NetBuffer m_Buffer;
        static Encoding s_Encoding;
        static byte[] s_StringWriteBuffer;

        /// <summary>
        /// Creates a new NetworkWriter object.
        /// </summary>
        public NetworkWriter()
        {
            m_Buffer = new NetBuffer();
            if (s_Encoding == null)
            {
                s_Encoding = new UTF8Encoding();
                s_StringWriteBuffer = new byte[k_MaxStringLength];
            }
        }

        /// <summary>
        /// Creates a new NetworkWriter object.
        /// </summary>
        /// <param name="buffer">A buffer to write into. This is not copied.</param>
        public NetworkWriter(byte[] buffer)
        {
            m_Buffer = new NetBuffer(buffer);
            if (s_Encoding == null)
            {
                s_Encoding = new UTF8Encoding();
                s_StringWriteBuffer = new byte[k_MaxStringLength];
            }
        }

        /// <summary>
        /// The current position of the internal buffer.
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for a code example.</para>
        /// </summary>
        public short Position { get { return (short)m_Buffer.Position; } }

        /// <summary>
        /// Returns a copy of internal array of bytes the writer is using, it copies only the bytes used.
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for a code example.</para>
        /// </summary>
        /// <returns>Copy of data used by the writer.</returns>
        public byte[] ToArray()
        {
            var newArray = new byte[m_Buffer.AsArraySegment().Count];
            Array.Copy(m_Buffer.AsArraySegment().Array, newArray, m_Buffer.AsArraySegment().Count);
            return newArray;
        }

        /// <summary>
        /// Returns the internal array of bytes the writer is using. This is NOT a copy.
        /// </summary>
        /// <returns>Internal buffer</returns>
        public byte[] AsArray()
        {
            return AsArraySegment().Array;
        }

        internal ArraySegment<byte> AsArraySegment()
        {
            return m_Buffer.AsArraySegment();
        }

        // http://sqlite.org/src4/doc/trunk/www/varint.wiki
        /// <summary>
        /// This writes the 32-bit value to the stream using variable-length-encoding.
        /// </summary>
        /// <param name="value">Value to write.</param>
        public void WritePackedUInt32(UInt32 value)
        {
            if (value <= 240)
            {
                Write((byte)value);
                return;
            }
            if (value <= 2287)
            {
                Write((byte)((value - 240) / 256 + 241));
                Write((byte)((value - 240) % 256));
                return;
            }
            if (value <= 67823)
            {
                Write((byte)249);
                Write((byte)((value - 2288) / 256));
                Write((byte)((value - 2288) % 256));
                return;
            }
            if (value <= 16777215)
            {
                Write((byte)250);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                return;
            }

            // all other values of uint
            Write((byte)251);
            Write((byte)(value & 0xFF));
            Write((byte)((value >> 8) & 0xFF));
            Write((byte)((value >> 16) & 0xFF));
            Write((byte)((value >> 24) & 0xFF));
        }

        /// <summary>
        /// This writes the 64-bit value to the stream using variable-length-encoding.
        /// </summary>
        /// <param name="value">Value to write.</param>
        public void WritePackedUInt64(UInt64 value)
        {
            if (value <= 240)
            {
                Write((byte)value);
                return;
            }
            if (value <= 2287)
            {
                Write((byte)((value - 240) / 256 + 241));
                Write((byte)((value - 240) % 256));
                return;
            }
            if (value <= 67823)
            {
                Write((byte)249);
                Write((byte)((value - 2288) / 256));
                Write((byte)((value - 2288) % 256));
                return;
            }
            if (value <= 16777215)
            {
                Write((byte)250);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                return;
            }
            if (value <= 4294967295)
            {
                Write((byte)251);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                Write((byte)((value >> 24) & 0xFF));
                return;
            }
            if (value <= 1099511627775)
            {
                Write((byte)252);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                Write((byte)((value >> 24) & 0xFF));
                Write((byte)((value >> 32) & 0xFF));
                return;
            }
            if (value <= 281474976710655)
            {
                Write((byte)253);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                Write((byte)((value >> 24) & 0xFF));
                Write((byte)((value >> 32) & 0xFF));
                Write((byte)((value >> 40) & 0xFF));
                return;
            }
            if (value <= 72057594037927935)
            {
                Write((byte)254);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                Write((byte)((value >> 24) & 0xFF));
                Write((byte)((value >> 32) & 0xFF));
                Write((byte)((value >> 40) & 0xFF));
                Write((byte)((value >> 48) & 0xFF));
                return;
            }

            // all others
            {
                Write((byte)255);
                Write((byte)(value & 0xFF));
                Write((byte)((value >> 8) & 0xFF));
                Write((byte)((value >> 16) & 0xFF));
                Write((byte)((value >> 24) & 0xFF));
                Write((byte)((value >> 32) & 0xFF));
                Write((byte)((value >> 40) & 0xFF));
                Write((byte)((value >> 48) & 0xFF));
                Write((byte)((value >> 56) & 0xFF));
            }
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(NetworkInstanceId value)
        {
            WritePackedUInt32(value.Value);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(NetworkSceneId value)
        {
            WritePackedUInt32(value.Value);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(char value)
        {
            m_Buffer.WriteByte((byte)value);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(byte value)
        {
            m_Buffer.WriteByte(value);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(sbyte value)
        {
            m_Buffer.WriteByte((byte)value);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(short value)
        {
            m_Buffer.WriteByte2((byte)(value & 0xff), (byte)((value >> 8) & 0xff));
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(ushort value)
        {
            m_Buffer.WriteByte2((byte)(value & 0xff), (byte)((value >> 8) & 0xff));
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(int value)
        {
            // little endian...
            m_Buffer.WriteByte4(
                (byte)(value & 0xff),
                (byte)((value >> 8) & 0xff),
                (byte)((value >> 16) & 0xff),
                (byte)((value >> 24) & 0xff));
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(uint value)
        {
            m_Buffer.WriteByte4(
                (byte)(value & 0xff),
                (byte)((value >> 8) & 0xff),
                (byte)((value >> 16) & 0xff),
                (byte)((value >> 24) & 0xff));
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(long value)
        {
            m_Buffer.WriteByte8(
                (byte)(value & 0xff),
                (byte)((value >> 8) & 0xff),
                (byte)((value >> 16) & 0xff),
                (byte)((value >> 24) & 0xff),
                (byte)((value >> 32) & 0xff),
                (byte)((value >> 40) & 0xff),
                (byte)((value >> 48) & 0xff),
                (byte)((value >> 56) & 0xff));
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(ulong value)
        {
            m_Buffer.WriteByte8(
                (byte)(value & 0xff),
                (byte)((value >> 8) & 0xff),
                (byte)((value >> 16) & 0xff),
                (byte)((value >> 24) & 0xff),
                (byte)((value >> 32) & 0xff),
                (byte)((value >> 40) & 0xff),
                (byte)((value >> 48) & 0xff),
                (byte)((value >> 56) & 0xff));
        }

        static UIntFloat s_FloatConverter;

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(float value)
        {
            s_FloatConverter.floatValue = value;
            Write(s_FloatConverter.intValue);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(double value)
        {
            s_FloatConverter.doubleValue = value;
            Write(s_FloatConverter.longValue);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(decimal value)
        {
            Int32[] bits = decimal.GetBits(value);
            Write(bits[0]);
            Write(bits[1]);
            Write(bits[2]);
            Write(bits[3]);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(string value)
        {
            if (value == null)
            {
                m_Buffer.WriteByte2(0, 0);
                return;
            }

            int len = s_Encoding.GetByteCount(value);

            if (len >= k_MaxStringLength)
            {
                throw new IndexOutOfRangeException("Serialize(string) too long: " + value.Length);
            }

            Write((ushort)(len));
            int numBytes = s_Encoding.GetBytes(value, 0, value.Length, s_StringWriteBuffer, 0);
            m_Buffer.WriteBytes(s_StringWriteBuffer, (ushort)numBytes);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(bool value)
        {
            if (value)
                m_Buffer.WriteByte(1);
            else
                m_Buffer.WriteByte(0);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="buffer">The byte buffer to write.</param>
        /// <param name="count">The number of bytes in the byte buffer to write.</param>
        public void Write(byte[] buffer, int count)
        {
            if (count > UInt16.MaxValue)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkWriter Write: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes."); }
                return;
            }
            m_Buffer.WriteBytes(buffer, (UInt16)count);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="buffer">The byte buffer to write.</param>
        /// <param name="offset">The byte buffer array element to start writing from.</param>
        /// <param name="count">The number of bytes in the byte buffer to write.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (count > UInt16.MaxValue)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkWriter Write: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes."); }
                return;
            }
            m_Buffer.WriteBytesAtOffset(buffer, (ushort)offset, (ushort)count);
        }

        /// <summary>
        /// This writes a 16-bit count and an array of bytes of that length to the stream.
        /// </summary>
        /// <param name="buffer">Array of bytes to write.</param>
        /// <param name="count">Number of bytes from the array to write.</param>
        public void WriteBytesAndSize(byte[] buffer, int count)
        {
            if (buffer == null || count == 0)
            {
                Write((UInt16)0);
                return;
            }

            if (count > UInt16.MaxValue)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkWriter WriteBytesAndSize: buffer is too large (" + count + ") bytes. The maximum buffer size is 64K bytes."); }
                return;
            }

            Write((UInt16)count);
            m_Buffer.WriteBytes(buffer, (UInt16)count);
        }

        /// <summary>
        /// This writes a 16-bit count and an array of bytes of that size to the stream.
        /// <para>Note that this will be the full allocated size of the array. So if the array is partially filled with data to send - then you should be using WriteBytesAndSize instead.</para>
        /// </summary>
        /// <param name="buffer">Bytes to write.</param>
        //NOTE: this will write the entire buffer.. including trailing empty space!
        public void WriteBytesFull(byte[] buffer)
        {
            if (buffer == null)
            {
                Write((UInt16)0);
                return;
            }
            if (buffer.Length > UInt16.MaxValue)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkWriter WriteBytes: buffer is too large (" + buffer.Length + ") bytes. The maximum buffer size is 64K bytes."); }
                return;
            }
            Write((UInt16)buffer.Length);
            m_Buffer.WriteBytes(buffer, (UInt16)buffer.Length);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Vector4 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Color value)
        {
            Write(value.r);
            Write(value.g);
            Write(value.b);
            Write(value.a);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Color32 value)
        {
            Write(value.r);
            Write(value.g);
            Write(value.b);
            Write(value.a);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Quaternion value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Rect value)
        {
            Write(value.xMin);
            Write(value.yMin);
            Write(value.width);
            Write(value.height);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>s
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Plane value)
        {
            Write(value.normal);
            Write(value.distance);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Ray value)
        {
            Write(value.direction);
            Write(value.origin);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Matrix4x4 value)
        {
            Write(value.m00);
            Write(value.m01);
            Write(value.m02);
            Write(value.m03);
            Write(value.m10);
            Write(value.m11);
            Write(value.m12);
            Write(value.m13);
            Write(value.m20);
            Write(value.m21);
            Write(value.m22);
            Write(value.m23);
            Write(value.m30);
            Write(value.m31);
            Write(value.m32);
            Write(value.m33);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(NetworkHash128 value)
        {
            Write(value.i0);
            Write(value.i1);
            Write(value.i2);
            Write(value.i3);
            Write(value.i4);
            Write(value.i5);
            Write(value.i6);
            Write(value.i7);
            Write(value.i8);
            Write(value.i9);
            Write(value.i10);
            Write(value.i11);
            Write(value.i12);
            Write(value.i13);
            Write(value.i14);
            Write(value.i15);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(NetworkIdentity value)
        {
            if (value == null)
            {
                WritePackedUInt32(0);
                return;
            }
            Write(value.netId);
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(Transform value)
        {
            if (value == null || value.gameObject == null)
            {
                WritePackedUInt32(0);
                return;
            }
            var uv = value.gameObject.GetComponent<NetworkIdentity>();
            if (uv != null)
            {
                Write(uv.netId);
            }
            else
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkWriter " + value + " has no NetworkIdentity"); }
                WritePackedUInt32(0);
            }
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="value">The object to write.</param>
        public void Write(GameObject value)
        {
            if (value == null)
            {
                WritePackedUInt32(0);
                return;
            }
            var uv = value.GetComponent<NetworkIdentity>();
            if (uv != null)
            {
                Write(uv.netId);
            }
            else
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkWriter " + value + " has no NetworkIdentity"); }
                WritePackedUInt32(0);
            }
        }

        /// <summary>
        /// This writes a reference to an object, value, buffer or network message, together with a NetworkIdentity component to the stream.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="msg">The network message to write.</param>
        public void Write(MessageBase msg)
        {
            msg.Serialize(this);
        }

        /// <summary>
        /// Seeks to the start of the internal buffer.
        /// </summary>
        public void SeekZero()
        {
            m_Buffer.SeekZero();
        }

        /// <summary>
        /// This begins a new message, which should be completed with FinishMessage() once the payload has been written.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// 
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        /// <param name="msgType">Message type.</param>
        public void StartMessage(short msgType)
        {
            SeekZero();

            // two bytes for size, will be filled out in FinishMessage
            m_Buffer.WriteByte2(0, 0);

            // two bytes for message type
            Write(msgType);
        }

        /// <summary>
        /// This fills out the size header of a message begun with StartMessage(), so that it can be send using Send() functions.
        /// <code>
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        /// 
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///    void Start()
        ///    {
        ///        short myMsgType = 444;
        ///        NetworkWriter writer = new NetworkWriter();
        ///        writer.StartMessage(myMsgType);
        ///        writer.Write("test data");
        ///        writer.FinishMessage();
        ///    }
        /// }
        /// </code>
        /// <para>See <see cref="NetworkWriter">NetworkWriter</see> for another code example.</para>
        /// </summary>
        public void FinishMessage()
        {
            // writes correct size into space at start of buffer
            m_Buffer.FinishMessage();
        }
    };
}
