using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TestFixture]
public class WeaverILGenerationTests
{
    [Test]
    public void TargetRPCServerClientChecks()
    {
        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_TargetRPCServerClientChecks), "CallTargetRpcTest", false, 1, 1,
            new MockInstruction(OpCodes.Isinst, "UnityEngine.Networking.ULocalConnectionToServer"),
            new MockInstruction(OpCodes.Brfalse),
            new MockInstruction(OpCodes.Ldstr),
            new MockInstruction(OpCodes.Call, "System.Void UnityEngine.Debug::LogError(System.Object)"),
            new MockInstruction(OpCodes.Ret)
            );

        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_TargetRPCServerClientChecks), "CallTargetRpcTest", false, 1, 1,
            new MockInstruction(OpCodes.Call, "System.Boolean UnityEngine.Networking.NetworkServer::get_active()"),
            new MockInstruction(OpCodes.Brtrue),
            new MockInstruction(OpCodes.Ldstr),
            new MockInstruction(OpCodes.Call, "System.Void UnityEngine.Debug::LogError(System.Object)"),
            new MockInstruction(OpCodes.Ret)
            );
    }

    [Test]
    public void RpcPassingEnumArrays()
    {
        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_TargetRPCServerClientChecks), "CallRpcWithEnumArray", false, 1, 1,
            new MockInstruction(OpCodes.Call, "System.Void Unity.GeneratedNetworkCode::_WriteArrayAttributeTargets_None(UnityEngine.Networking.NetworkWriter,System.AttributeTargets[])")
            );
    }

    [Test]
    public void SyncListsAreAutoInitializedInConstructor()
    {
        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_SyncLists), ".ctor", false, 0, 1,
            new MockInstruction(OpCodes.Ldarg_0),
            new MockInstruction(OpCodes.Newobj, "System.Void UnityEngine.Networking.SyncListInt::.ctor()"),
            new MockInstruction(OpCodes.Stfld, "UnityEngine.Networking.SyncListInt WeaverILGenerationTests_SyncLists::Inited")
            );

        /*WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_SyncLists), ".ctor", false, 0, 1,
            new MockInstruction(OpCodes.Ldarg_0),
            new MockInstruction(OpCodes.Newobj, "System.Void UnityEngine.Networking.SyncListInt::.ctor()"),
            new MockInstruction(OpCodes.Stfld, "UnityEngine.Networking.SyncListInt WeaverILGenerationTests_SyncLists::NotInited")
            );*/
    }

    [Test]
    public void SyncListsAreOnlySerializedOnce()
    {
        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_SyncLists), "OnSerialize", true, 2, 2,
            new MockInstruction(OpCodes.Ldfld, "UnityEngine.Networking.SyncListInt WeaverILGenerationTests_SyncLists::Inited"),
            new MockInstruction(OpCodes.Call, "System.Void UnityEngine.Networking.SyncListInt::WriteInstance(UnityEngine.Networking.NetworkWriter,UnityEngine.Networking.SyncListInt)")
            );

        /*WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_SyncLists), "OnSerialize", true, 2, 2,
            new MockInstruction(OpCodes.Ldfld, "UnityEngine.Networking.SyncListInt WeaverILGenerationTests_SyncLists::NotInited"),
            new MockInstruction(OpCodes.Call, "System.Void UnityEngine.Networking.SyncListInt::WriteInstance(UnityEngine.Networking.NetworkWriter,UnityEngine.Networking.SyncListInt)")
            );*/
    }

    [Test]
    public void SyncListsNetworkBehaviourWithSyncListCallsBaseClassAwakeMethod()
    {
        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_SyncLists), "Awake", false, 0, 1,
            new MockInstruction(OpCodes.Ldarg_0),
            new MockInstruction(OpCodes.Call, "System.Void WeaverILGenerationTests_SyncLists_Base::Awake()")
            );
    }

    [Test]
    public void SyncNetworkBehaviourBaseClassPreStartClientMethodFromSubclass()
    {
        WeaverILMatcher.MatchMethodIL(typeof(WeaverILGenerationTests_SyncLists), "PreStartClient", false, 0, 1,
            new MockInstruction(OpCodes.Ldarg_0),
            new MockInstruction(OpCodes.Call, "System.Void WeaverILGenerationTests_SyncLists_Base::PreStartClient()")
        );
    }
}
