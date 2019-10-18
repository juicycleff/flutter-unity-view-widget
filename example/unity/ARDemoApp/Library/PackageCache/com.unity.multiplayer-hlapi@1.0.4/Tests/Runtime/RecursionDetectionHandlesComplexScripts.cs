using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class RecursionDetectionHandlesComplexScripts
{
    [UnityTest]
    public IEnumerator RecursionDetectionHandlesComplexScriptsTest()
    {
        NetworkServer.Reset();
        NetworkClient.ShutdownAll();

        GameObject go = new GameObject();
        go.name = "objectWithComplexScript";
        go.AddComponent<UNetRecursionBehaviour>();
        yield return null;
        Assert.IsNotNull(GameObject.Find("objectWithComplexScript"));
        yield return null;

        Object.Destroy(go);
    }

    // Script with MANY MANY network attributes. Checks that the recursion detection in UNetWeaver wont generate a false positive on complex scripts.
    public partial class UNetRecursionBehaviour : NetworkBehaviour
    {
        // 31 syncvars
        [SyncVar]
        public int var000;
        [SyncVar]
        public int var001;
        [SyncVar]
        public int var002;
        [SyncVar]
        public int var003;
        [SyncVar]
        public int var004;
        [SyncVar]
        public int var005;
        [SyncVar]
        public int var006;
        [SyncVar]
        public int var007;
        [SyncVar]
        public int var008;
        [SyncVar]
        public int var009;

        [SyncVar]
        public int var010;
        [SyncVar]
        public int var011;
        [SyncVar]
        public int var012;
        [SyncVar]
        public int var013;
        [SyncVar]
        public int var014;
        [SyncVar]
        public int var015;
        [SyncVar]
        public int var016;
        [SyncVar]
        public int var017;
        [SyncVar]
        public int var018;
        [SyncVar]
        public int var019;

        [SyncVar]
        public int var020;
        [SyncVar]
        public int var021;
        [SyncVar]
        public int var022;
        [SyncVar]
        public int var023;
        [SyncVar]
        public int var024;
        [SyncVar]
        public int var025;
        [SyncVar]
        public int var026;
        [SyncVar]
        public int var027;
        [SyncVar]
        public int var028;
        [SyncVar]
        public int var029;

        [SyncVar]
        public int var030;

        // 50 Commands
        [Command]
        void CmdTest000(int p1, string p2) {}
        [Command]
        void CmdTest001(int p1, string p2) {}
        [Command]
        void CmdTest002(int p1, string p2) {}
        [Command]
        void CmdTest003(int p1, string p2) {}
        [Command]
        void CmdTest004(int p1, string p2) {}
        [Command]
        void CmdTest005(int p1, string p2) {}
        [Command]
        void CmdTest006(int p1, string p2) {}
        [Command]
        void CmdTest007(int p1, string p2) {}
        [Command]
        void CmdTest008(int p1, string p2) {}
        [Command]
        void CmdTest009(int p1, string p2) {}

        [Command]
        void CmdTest010(int p1, string p2) {}
        [Command]
        void CmdTest011(int p1, string p2) {}
        [Command]
        void CmdTest012(int p1, string p2) {}
        [Command]
        void CmdTest013(int p1, string p2) {}
        [Command]
        void CmdTest014(int p1, string p2) {}
        [Command]
        void CmdTest015(int p1, string p2) {}
        [Command]
        void CmdTest016(int p1, string p2) {}
        [Command]
        void CmdTest017(int p1, string p2) {}
        [Command]
        void CmdTest018(int p1, string p2) {}
        [Command]
        void CmdTest019(int p1, string p2) {}

        [Command]
        void CmdTest020(int p1, string p2) {}
        [Command]
        void CmdTest021(int p1, string p2) {}
        [Command]
        void CmdTest022(int p1, string p2) {}
        [Command]
        void CmdTest023(int p1, string p2) {}
        [Command]
        void CmdTest024(int p1, string p2) {}
        [Command]
        void CmdTest025(int p1, string p2) {}
        [Command]
        void CmdTest026(int p1, string p2) {}
        [Command]
        void CmdTest027(int p1, string p2) {}
        [Command]
        void CmdTest028(int p1, string p2) {}
        [Command]
        void CmdTest029(int p1, string p2) {}

        [Command]
        void CmdTest030(int p1, string p2) {}
        [Command]
        void CmdTest031(int p1, string p2) {}
        [Command]
        void CmdTest032(int p1, string p2) {}
        [Command]
        void CmdTest033(int p1, string p2) {}
        [Command]
        void CmdTest034(int p1, string p2) {}
        [Command]
        void CmdTest035(int p1, string p2) {}
        [Command]
        void CmdTest036(int p1, string p2) {}
        [Command]
        void CmdTest037(int p1, string p2) {}
        [Command]
        void CmdTest038(int p1, string p2) {}
        [Command]
        void CmdTest039(int p1, string p2) {}

        [Command]
        void CmdTest040(int p1, string p2) {}
        [Command]
        void CmdTest041(int p1, string p2) {}
        [Command]
        void CmdTest042(int p1, string p2) {}
        [Command]
        void CmdTest043(int p1, string p2) {}
        [Command]
        void CmdTest044(int p1, string p2) {}
        [Command]
        void CmdTest045(int p1, string p2) {}
        [Command]
        void CmdTest046(int p1, string p2) {}
        [Command]
        void CmdTest047(int p1, string p2) {}
        [Command]
        void CmdTest048(int p1, string p2) {}
        [Command]
        void CmdTest049(int p1, string p2) {}

        // 100 ClientRpcs

        [ClientRpc]
        void RpcTest000(int p1, string p2) {}
        [ClientRpc]
        void RpcTest001(int p1, string p2) {}
        [ClientRpc]
        void RpcTest002(int p1, string p2) {}
        [ClientRpc]
        void RpcTest003(int p1, string p2) {}
        [ClientRpc]
        void RpcTest004(int p1, string p2) {}
        [ClientRpc]
        void RpcTest005(int p1, string p2) {}
        [ClientRpc]
        void RpcTest006(int p1, string p2) {}
        [ClientRpc]
        void RpcTest007(int p1, string p2) {}
        [ClientRpc]
        void RpcTest008(int p1, string p2) {}
        [ClientRpc]
        void RpcTest009(int p1, string p2) {}

        [ClientRpc]
        void RpcTest010(int p1, string p2) {}
        [ClientRpc]
        void RpcTest011(int p1, string p2) {}
        [ClientRpc]
        void RpcTest012(int p1, string p2) {}
        [ClientRpc]
        void RpcTest013(int p1, string p2) {}
        [ClientRpc]
        void RpcTest014(int p1, string p2) {}
        [ClientRpc]
        void RpcTest015(int p1, string p2) {}
        [ClientRpc]
        void RpcTest016(int p1, string p2) {}
        [ClientRpc]
        void RpcTest017(int p1, string p2) {}
        [ClientRpc]
        void RpcTest018(int p1, string p2) {}
        [ClientRpc]
        void RpcTest019(int p1, string p2) {}

        [ClientRpc]
        void RpcTest020(int p1, string p2) {}
        [ClientRpc]
        void RpcTest021(int p1, string p2) {}
        [ClientRpc]
        void RpcTest022(int p1, string p2) {}
        [ClientRpc]
        void RpcTest023(int p1, string p2) {}
        [ClientRpc]
        void RpcTest024(int p1, string p2) {}
        [ClientRpc]
        void RpcTest025(int p1, string p2) {}
        [ClientRpc]
        void RpcTest026(int p1, string p2) {}
        [ClientRpc]
        void RpcTest027(int p1, string p2) {}
        [ClientRpc]
        void RpcTest028(int p1, string p2) {}
        [ClientRpc]
        void RpcTest029(int p1, string p2) {}

        [ClientRpc]
        void RpcTest030(int p1, string p2) {}
        [ClientRpc]
        void RpcTest031(int p1, string p2) {}
        [ClientRpc]
        void RpcTest032(int p1, string p2) {}
        [ClientRpc]
        void RpcTest033(int p1, string p2) {}
        [ClientRpc]
        void RpcTest034(int p1, string p2) {}
        [ClientRpc]
        void RpcTest035(int p1, string p2) {}
        [ClientRpc]
        void RpcTest036(int p1, string p2) {}
        [ClientRpc]
        void RpcTest037(int p1, string p2) {}
        [ClientRpc]
        void RpcTest038(int p1, string p2) {}
        [ClientRpc]
        void RpcTest039(int p1, string p2) {}

        [ClientRpc]
        void RpcTest040(int p1, string p2) {}
        [ClientRpc]
        void RpcTest041(int p1, string p2) {}
        [ClientRpc]
        void RpcTest042(int p1, string p2) {}
        [ClientRpc]
        void RpcTest043(int p1, string p2) {}
        [ClientRpc]
        void RpcTest044(int p1, string p2) {}
        [ClientRpc]
        void RpcTest045(int p1, string p2) {}
        [ClientRpc]
        void RpcTest046(int p1, string p2) {}
        [ClientRpc]
        void RpcTest047(int p1, string p2) {}
        [ClientRpc]
        void RpcTest048(int p1, string p2) {}
        [ClientRpc]
        void RpcTest049(int p1, string p2) {}

        [ClientRpc]
        void RpcTest100(int p1, string p2) {}
        [ClientRpc]
        void RpcTest101(int p1, string p2) {}
        [ClientRpc]
        void RpcTest102(int p1, string p2) {}
        [ClientRpc]
        void RpcTest103(int p1, string p2) {}
        [ClientRpc]
        void RpcTest104(int p1, string p2) {}
        [ClientRpc]
        void RpcTest105(int p1, string p2) {}
        [ClientRpc]
        void RpcTest106(int p1, string p2) {}
        [ClientRpc]
        void RpcTest107(int p1, string p2) {}
        [ClientRpc]
        void RpcTest108(int p1, string p2) {}
        [ClientRpc]
        void RpcTest109(int p1, string p2) {}

        [ClientRpc]
        void RpcTest110(int p1, string p2) {}
        [ClientRpc]
        void RpcTest111(int p1, string p2) {}
        [ClientRpc]
        void RpcTest112(int p1, string p2) {}
        [ClientRpc]
        void RpcTest113(int p1, string p2) {}
        [ClientRpc]
        void RpcTest114(int p1, string p2) {}
        [ClientRpc]
        void RpcTest115(int p1, string p2) {}
        [ClientRpc]
        void RpcTest116(int p1, string p2) {}
        [ClientRpc]
        void RpcTest117(int p1, string p2) {}
        [ClientRpc]
        void RpcTest118(int p1, string p2) {}
        [ClientRpc]
        void RpcTest119(int p1, string p2) {}

        [ClientRpc]
        void RpcTest120(int p1, string p2) {}
        [ClientRpc]
        void RpcTest121(int p1, string p2) {}
        [ClientRpc]
        void RpcTest122(int p1, string p2) {}
        [ClientRpc]
        void RpcTest123(int p1, string p2) {}
        [ClientRpc]
        void RpcTest124(int p1, string p2) {}
        [ClientRpc]
        void RpcTest125(int p1, string p2) {}
        [ClientRpc]
        void RpcTest126(int p1, string p2) {}
        [ClientRpc]
        void RpcTest127(int p1, string p2) {}
        [ClientRpc]
        void RpcTest128(int p1, string p2) {}
        [ClientRpc]
        void RpcTest129(int p1, string p2) {}

        [ClientRpc]
        void RpcTest130(int p1, string p2) {}
        [ClientRpc]
        void RpcTest131(int p1, string p2) {}
        [ClientRpc]
        void RpcTest132(int p1, string p2) {}
        [ClientRpc]
        void RpcTest133(int p1, string p2) {}
        [ClientRpc]
        void RpcTest134(int p1, string p2) {}
        [ClientRpc]
        void RpcTest135(int p1, string p2) {}
        [ClientRpc]
        void RpcTest136(int p1, string p2) {}
        [ClientRpc]
        void RpcTest137(int p1, string p2) {}
        [ClientRpc]
        void RpcTest138(int p1, string p2) {}
        [ClientRpc]
        void RpcTest139(int p1, string p2) {}

        [ClientRpc]
        void RpcTest140(int p1, string p2) {}
        [ClientRpc]
        void RpcTest141(int p1, string p2) {}
        [ClientRpc]
        void RpcTest142(int p1, string p2) {}
        [ClientRpc]
        void RpcTest143(int p1, string p2) {}
        [ClientRpc]
        void RpcTest144(int p1, string p2) {}
        [ClientRpc]
        void RpcTest145(int p1, string p2) {}
        [ClientRpc]
        void RpcTest146(int p1, string p2) {}
        [ClientRpc]
        void RpcTest147(int p1, string p2) {}
        [ClientRpc]
        void RpcTest148(int p1, string p2) {}
        [ClientRpc]
        void RpcTest149(int p1, string p2) {}
    }
}
#pragma warning restore 618
