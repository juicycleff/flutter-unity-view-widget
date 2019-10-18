using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine.Networking;

[TestFixture]
public class NetBufferTest
{
    const int bufferLength = 10;

    private byte[] bytes;
    private byte[] tempArray;
    private NetBuffer buffer;

    [SetUp]
    public void Setup()
    {
        tempArray = new byte[bufferLength];

        bytes = Enumerable.Range(1, bufferLength).Select(i => (byte)i).ToArray();
        buffer = new NetBuffer(bytes);
    }

    [Test]
    public void BufferCreated_InitialParametersAreSet()
    {
        Assert.AreEqual(64, new NetBuffer().Length, "NetBuffer initial size is not 64 bytes after buffer was created");
        Assert.AreEqual(0, new NetBuffer().Position, "NetBuffer initial position is not 0 after buffer was created");
    }

    [Test]
    public void BufferCreatedFromArrayOfBytesHasTheSameLengthAsOriginalArray()
    {
        Assert.AreEqual(0, buffer.Position, "NetBuffer initial position does not equal to 0 after buffer was created");
        Assert.AreEqual(buffer.Length, bufferLength,
            "NetBuffer's size doesn't equal to the size of byte array it was created from");
    }

    [Test]
    public void BufferReferencesTheArrayAndDoesNotCopyItsValues()
    {
        bytes[0] = 9;

        Assert.AreEqual(bytes[0], buffer.ReadByte(),
            "ReadByte function didn't change the value when the value is changed in referenced array");
    }

    [Test]
    public void CanReadOneByte()
    {
        Assert.AreEqual(bytes[0], buffer.ReadByte(), "ReadByte function read the first byte incorrectly");
        Assert.AreEqual(1, buffer.Position, "ReadByte function didn't changed buffer's position after byte was read");
    }

    [Test]
    public void AttemptToReadByteOutOfBufferBounds_OutOfRangeExceptionRaises()
    {
        var testBuffer = new NetBuffer(new byte[1]);
        testBuffer.ReadByte();

        Assert.That(() => testBuffer.ReadByte(),
            Throws.Exception.TypeOf<IndexOutOfRangeException>()
            .With.Message.EqualTo("NetworkReader:ReadByte out of range:NetBuf sz:1 pos:1"));
    }

    [Test]
    public void BunchOfBytesCanBeReadAtOnce()
    {
        buffer.ReadBytes(tempArray, bufferLength);

        for (var i = 0; i < bufferLength; i++)
        {
            Assert.AreEqual(bytes[i], tempArray[i], "ReadBytes function read a bunch of bytes at once and value of '" + i + "' byte is incorrect");
        }

        Assert.AreEqual(bufferLength, buffer.Position, "ReadBytes function changed position incorrectly after read a bunch of bytes");
    }

    [Test]
    public void AttemptToReadABunchOfBytesOutOfBufferBounds_OutOfRangeExceptionRaises()
    {
        var testBuffer = new NetBuffer(new byte[1]);

        Assert.That(() => testBuffer.ReadBytes(new byte[2], 2),
            Throws.Exception.TypeOf<IndexOutOfRangeException>()
            .With.Message.EqualTo("NetworkReader:ReadBytes out of range: (2) NetBuf sz:1 pos:0"));
    }

    [Test]
    public void CanWriteByte()
    {
        buffer.WriteByte(byte.MaxValue);

        Assert.AreEqual(byte.MaxValue, bytes[0], "WriteByte function cannot write byte to the provided array");
        Assert.AreEqual(1, buffer.Position, "WriteByte function didn't shift the NetBuffer's position");
    }

    [Test]
    public void CanWriteTwoBytes()
    {
        buffer.WriteByte2(255, 127);

        Assert.AreEqual(255, bytes[0], "WriteByte2 function wrote incorrect first byte to the provided array");
        Assert.AreEqual(127, bytes[1], "WriteByte2 function wrote incorrect second byte to the provided array");

        Assert.AreEqual(2, buffer.Position, "WriteByte2 function didn't shift the NetBuffer's position");
    }

    [Test]
    public void CanWriteFourBytes()
    {
        buffer.WriteByte4(10, 11, 12, 13);

        Assert.AreEqual(10, bytes[0], "WriteByte4 function wrote incorrect first byte to the provided array");
        Assert.AreEqual(11, bytes[1], "WriteByte4 function wrote incorrect second byte to the provided array");
        Assert.AreEqual(12, bytes[2], "WriteByte4 function wrote incorrect third byte to the provided array");
        Assert.AreEqual(13, bytes[3], "WriteByte4 function wrote incorrect fourth byte to the provided array");

        Assert.AreEqual(4, buffer.Position, "WriteByte4 function didn't shift the NetBuffer's position");
    }

    [Test]
    public void CanWriteEightBytes()
    {
        buffer.WriteByte8(100, 110, 120, 130, 140, 150, 160, 170);

        Assert.AreEqual(100, bytes[0], "WriteByte8 function wrote incorrect first byte to the provided array");
        Assert.AreEqual(110, bytes[1], "WriteByte8 function wrote incorrect second byte to the provided array");
        Assert.AreEqual(120, bytes[2], "WriteByte8 function wrote incorrect third byte to the provided array");
        Assert.AreEqual(130, bytes[3], "WriteByte8 function wrote incorrect fourth byte to the provided array");
        Assert.AreEqual(140, bytes[4], "WriteByte8 function wrote incorrect fifth byte to the provided array");
        Assert.AreEqual(150, bytes[5], "WriteByte8 function wrote incorrect sixth byte to the provided array");
        Assert.AreEqual(160, bytes[6], "WriteByte8 function wrote incorrect seventh byte to the provided array");
        Assert.AreEqual(170, bytes[7], "WriteByte8 function wrote incorrect eighth byte to the provided array");

        Assert.AreEqual(8, buffer.Position, "WriteByte8 function didn't shift the NetBuffer's position");
    }

    [TestCase(4, 14)]
    public void BufferSizeGrowsSeveralTimesUntilNewSizeCanHandleAllValuesToWrite(int length, int expectedLength)
    {
        var testBuffer = new NetBuffer(new byte[length]);
        testBuffer.WriteBytes(new byte[10], 10);

        Assert.AreEqual(expectedLength, testBuffer.Length, "New buffer length cannot handle all values");
    }

    [TestCase(bufferLength - 1, (ushort)(bufferLength - 1 - 5))] // array.Length < buffer.Length && bytes to write < array.Length
    [TestCase(bufferLength - 1, (ushort)(bufferLength - 1))]     // array.Length < buffer.Length && bytes to write == array.Length
    [TestCase(bufferLength,     (ushort)(bufferLength - 5))]     // array.Length == buffer.Length && bytes to write < array.Length
    [TestCase(bufferLength,     (ushort)(bufferLength))]         // array.Length == buffer.Length && bytes to write == array.Length
    [TestCase(bufferLength + 1, (ushort)(bufferLength + 1 - 5))] // array.Length > buffer.Length && bytes to write < array.Length
    [TestCase(bufferLength + 1, (ushort)(bufferLength + 1))]     // array.Length > buffer.Length && bytes to write == array.Length
    public void CanWriteABunchOfBytesAtOnce(int arraySize, ushort amountToWrite)
    {
        var sourceArray = Enumerable.Range(10, arraySize).Select(i => (byte)i).ToArray();

        buffer.WriteBytes(sourceArray, amountToWrite);

        Assert.AreEqual(amountToWrite, buffer.Position, "WriteBytes function changed position incorrectly after read a bunch of bytes");

        buffer.SeekZero();
        for (var i = 0; i < amountToWrite; i++)
        {
            Assert.AreEqual(sourceArray[i], buffer.ReadByte(), "WriteBytes function wrote a bunch of bytes at once and value of '" + i + "' byte is incorrect");
        }
    }

    [TestCase((ushort) 5, (ushort) 4)]   // offset < buffer size && offset + count < buffer size
    [TestCase((ushort) 10, (ushort) 10)] // offset == buffer size && offset + count > buffer size
    [TestCase((ushort) 20, (ushort) 20)] // offset  > buffer size
    [TestCase((ushort) 5, (ushort) 15)]  // offset < buffer size && count > buffer size
    [TestCase((ushort) 10, (ushort) 10)] // offset == buffer size && offset + count > buffer size
    [TestCase((ushort) 20, (ushort) 20)] // offset  > buffer size
    public void CanWriteAtOffset(ushort offset, ushort count)
    {
        var sourceArray = Enumerable.Range(100, count).Select(i => (byte)i).ToArray();
        buffer.WriteBytesAtOffset(sourceArray, offset, count);

        Assert.AreEqual(offset + count, buffer.Position, "WriteAtOffset didn't shift buffer's position");

        buffer.SeekZero();
        buffer.ReadBytes(new byte[offset], offset);

        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual(sourceArray[i], buffer.ReadByte(),
                "WriteAtOffset function wrote a bunch of bytes at offset '" + offset + "' " + "and value of '" + i + "' byte is incorrect");
        }
    }

    [Test]
    public void NewMemoryIsAllocated_ReferenceToOldArrayIsLost()
    {
        var sourceArray = Enumerable.Range(100, 10).Select(i => (byte)i).ToArray();
        buffer.WriteBytesAtOffset(sourceArray, (ushort)buffer.Length, 10); //size increased and new memory is allocated
        buffer.SeekZero();

        bytes[0] = 9;

        Assert.AreNotEqual(9, buffer.ReadByte(), "Link to previous referenced array is not lost and changes in referenced array affected internal buffer");
    }

    [Test]
    public void SeekZeroMovesPositionToZero()
    {
        buffer.WriteByte(127);
        buffer.SeekZero();
        Assert.AreEqual(0, buffer.Position, "SeekZero function didn't move position to zero");
    }

    [Test]
    public void ReplaceFunctionReplacesInternalBufferWithProvidedOne()
    {
        var testBuffer = new NetBuffer();
        testBuffer.ReadByte();
        testBuffer.Replace(new byte[] {255});

        Assert.AreEqual(0, testBuffer.Position, "Replace operation didn't move the postion of buffer to zero");
        Assert.AreEqual(1, testBuffer.Length, "New buffer length is not corresponding to the replaced array length");
        Assert.AreEqual(255, testBuffer.ReadByte(), "NetBuffer read incorrect value after replace operation");
    }

    [TestCase(100, 150)]
    [TestCase(3, 5)]
    [TestCase(1, 2)]
    public void BufferSizeGrows_NewBufferLengthRoundedUp(int length, int expectedLength)
    {
        var testBuffer = new NetBuffer(new byte[length]);

        for (var i = 0; i < length; i++)
        {
            testBuffer.WriteByte(byte.MaxValue);
        }

        Assert.AreEqual(expectedLength, testBuffer.Length, "New NetBuffer's length has unexpected value");
    }

    [Test]
    public void WriteBytesCopiesBytesIntoBufferFromCurrentPosition()
    {
        var sourceArray = Enumerable.Range(100, bufferLength).Select(i => (byte)i).ToArray();
        buffer.ReadBytes(new byte[2], 2); // shift position to 2
        buffer.WriteBytes(sourceArray, bufferLength); //count == buffer.Length
        buffer.SeekZero();

        Assert.AreEqual(bytes[0], buffer.ReadByte(), "WriteBytes touched the values outside buffer position. First byte is wrong");
        Assert.AreEqual(bytes[1], buffer.ReadByte(), "WriteBytes touched the values outside buffer position. Second byte is wrong");
    }
}
