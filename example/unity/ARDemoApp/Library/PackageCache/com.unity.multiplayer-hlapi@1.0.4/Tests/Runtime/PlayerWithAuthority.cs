using UnityEngine;
using NUnit.Framework;
using UnityEngine.Networking;

#pragma warning disable 618
public class PlayerWithAuthority : NetworkBehaviour
{
    GameObject spawned;
    public GameObject objAuthPrefab;
    public GameObject objNoAuthPrefab;

    public override void OnStartAuthority()
    {
        Assert.IsTrue(hasAuthority);
    }

    public override void OnStartLocalPlayer()
    {
        Assert.IsTrue(hasAuthority);
        Assert.IsTrue(isLocalPlayer);

        CmdSpawnObj();
    }

    [Command]
    void CmdSpawnObj()
    {
        // spawn auth object
        var objAuth = (GameObject)Instantiate(objAuthPrefab, Vector3.zero, objAuthPrefab.transform.rotation);
        NetworkServer.SpawnWithClientAuthority(objAuth, connectionToClient);

        // spawn no auth object
        var objNoAuth = (GameObject)Instantiate(objNoAuthPrefab, Vector3.zero, objNoAuthPrefab.transform.rotation);
        NetworkServer.Spawn(objNoAuth);

        objNoAuth.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        spawned = objNoAuth;
        Invoke("RemoveAuthority", 0.1f);
    }

    void RemoveAuthority()
    {
        spawned.GetComponent<NetworkIdentity>().RemoveClientAuthority(connectionToClient);
    }
}
#pragma warning restore 618
