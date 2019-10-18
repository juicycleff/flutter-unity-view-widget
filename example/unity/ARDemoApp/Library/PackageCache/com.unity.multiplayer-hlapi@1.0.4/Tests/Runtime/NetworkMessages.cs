using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class MessageTypes
{
    public const short CSHelloMsgType = MsgType.Highest + 1;
    public const short CSUpdateMsgType = MsgType.Highest + 2;
    public const short SCUpdateMsgType = MsgType.Highest + 3;
}

public class CSHelloMessage : MessageBase
{
    public int connectionID;

    public CSHelloMessage() {}
    public CSHelloMessage(int ID) { this.connectionID = ID; }

    public override void Deserialize(NetworkReader reader)
    {
        connectionID = reader.ReadInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.StartMessage(MessageTypes.CSHelloMsgType);
        writer.Write(connectionID);
        writer.FinishMessage();
    }
}

public class CSUpdateMessage : MessageBase
{
    public byte ID;
    public Vector3 position;

    public CSUpdateMessage() {}
    public CSUpdateMessage(byte ID, Vector3 position)
    {
        this.ID = ID;
        this.position = position;
    }

    public override void Deserialize(NetworkReader reader)
    {
        ID = reader.ReadByte();
        position = reader.ReadVector3();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.StartMessage(MessageTypes.CSUpdateMsgType);
        writer.Write(ID);
        writer.Write(position);
        writer.FinishMessage();
    }
}

public class SCUpdateMessage : MessageBase
{
    public byte ID;
    public bool status;

    public SCUpdateMessage() {}
    public SCUpdateMessage(byte ID, bool status)
    {
        this.ID = ID;
        this.status = status;
    }

    public override void Deserialize(NetworkReader reader)
    {
        ID = reader.ReadByte();
        status = reader.ReadBoolean();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.StartMessage(MessageTypes.SCUpdateMsgType);
        writer.Write(ID);
        writer.Write(status);
        writer.FinishMessage();
    }
}
#pragma warning restore 618
