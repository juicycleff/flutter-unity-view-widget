using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class WeaverILGenerationTests_SyncLists_Base : NetworkBehaviour
{
    [SyncVar] public GameObject baseSyncObject;
    void Awake()
    {
        Debug.Log("just here so compiler does not optimize away this");
    }
}

public class WeaverILGenerationTests_SyncLists : WeaverILGenerationTests_SyncLists_Base
{
    public SyncListInt Inited = new SyncListInt();

    // This can't be enabled by default as it will issue a warning from the weaver at compile time. This 
    // warning will appear in all projects including the package and can mess with CI or automation which checks for output
    // in the editor log.
    //[SyncVar]
    //public SyncListInt NotInited;

    [SyncVar]
    public GameObject syncObject;
}
#pragma warning restore 618
