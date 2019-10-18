using NUnit.Framework;
using UnityEngine.Networking;

#pragma warning disable 618
public class AuthSpawnableObject : NetworkBehaviour
{
    // this object is spawned with client Authority
    public override void OnStartAuthority()
    {
        Assert.IsTrue(hasAuthority);
    }

    public override void OnStopAuthority()
    {
        Assert.Fail("OnStopAuthority on AuthSpawnableObject should not be called");
    }
}
#pragma warning restore 618
