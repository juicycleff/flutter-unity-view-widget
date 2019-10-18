using UnityEngine.Networking;

#pragma warning disable 618
public class UnetPlayerWithGetCurrentRTTCallScript : NetworkBehaviour
{
    public bool isDone;

    public void Start()
    {
        byte error;
        if (isServer)
        {
            NetworkTransport.GetCurrentRTT(NetworkServer.serverHostId, connectionToClient.connectionId, out error);
            if ((NetworkError)error != NetworkError.Ok)
                isDone = true;
        }
    }
}
#pragma warning restore 618
