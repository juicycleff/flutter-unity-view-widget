using System.Collections.Generic;
using UnityEngine.Networking;

#pragma warning disable 618
public class PlayerCallbacksOrderOnTheHostScript : NetworkBehaviour
{
    public List<string> actualListOfCallbacks = new List<string>();
    public bool isDone = false;

    public void Start()
    {
        actualListOfCallbacks.Add("Start");
    }

    public override void OnStartServer()
    {
        actualListOfCallbacks.Add("OnStartServer");
    }

    public override void OnStartClient()
    {
        actualListOfCallbacks.Add("OnStartClient");
    }

    public override void OnStartLocalPlayer()
    {
        actualListOfCallbacks.Add("OnStartLocalPlayer");
    }

    public override void OnStartAuthority()
    {
        actualListOfCallbacks.Add("OnStartAuthority");
    }

    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        actualListOfCallbacks.Add("OnRebuildObservers");
        return false;
    }

    public override void OnSetLocalVisibility(bool vis)
    {
        actualListOfCallbacks.Add("OnSetLocalVisibility");
        isDone = true;
    }
}
#pragma warning restore 618
