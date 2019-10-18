using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class CustomNetworkManagerWithCallbacks : NetworkManager
{
    public List<string> actualListOfCallbacks = new List<string>();
    public bool isStartHostPartDone;
    public bool isStopHostPartDone;

    // ----- Start Host -----
    public override void OnStartHost()
    {
        actualListOfCallbacks.Add("OnStartHost");
    }

    public override void OnStartServer()
    {
        actualListOfCallbacks.Add("OnStartServer");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        actualListOfCallbacks.Add("OnServerConnect");
    }

    public override void OnStartClient(NetworkClient client)
    {
        actualListOfCallbacks.Add("OnStartClient");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        actualListOfCallbacks.Add("OnClientConnect");
        isStartHostPartDone = true;
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        actualListOfCallbacks.Add("OnServerReady");
        base.OnServerReady(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        actualListOfCallbacks.Add("OnServerAddPlayer");
    }

    // ----- Stop Host -----
    public override void OnStopHost()
    {
        actualListOfCallbacks.Add("OnStopHost");
    }

    public override void OnStopServer()
    {
        actualListOfCallbacks.Add("OnStopServer");
    }

    public override void OnStopClient()
    {
        actualListOfCallbacks.Add("OnStopClient");
        isStopHostPartDone = true;
    }
}
#pragma warning restore 618
