using UnityEngine.Networking;

#pragma warning disable 618
public class SpawningBase_SpawnableObjectScript : NetworkBehaviour
{
    public override void OnStartServer()
    {
        SpawningTestBase.IncrementStartServer();
    }

    public override void OnStartClient()
    {
        SpawningTestBase.IncrementStartClient();
    }

    public override void OnNetworkDestroy()
    {
        SpawningTestBase.IncrementDestroyClient();
    }
}
#pragma warning restore 618
