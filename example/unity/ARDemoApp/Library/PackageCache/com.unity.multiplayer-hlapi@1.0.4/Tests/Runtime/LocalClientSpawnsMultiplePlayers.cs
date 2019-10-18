using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
public class LocalClientSpawnsMultiplePlayers : SpawningTestBase, IPrebuildSetup, IPostBuildCleanup
{
    const int kPlayerCount = 2;
    List<short> m_ReadyPlayers = new List<short>();
    int m_NumPlayers = 0;
    GameObject m_Obj;

    [UnityTest]
    public IEnumerator LocalClientSpawnsMultiplePlayersTest()
    {
        NetworkClient.ShutdownAll();
        NetworkServer.Reset();

        TestSetup();
        StartServer();
        StartLocalClient(kPlayerCount);

        while (m_ReadyPlayers.Count != kPlayerCount)
        {
            yield return null;
        }

        ClientScene.DestroyAllClientObjects();
        yield return null;
        NetworkServer.Destroy(m_Obj);
        NetworkServer.Destroy(playerObj);
    }

    public override void OnServerReady(GameObject player)
    {
        m_Obj = GameObject.Instantiate(GetBridgeScript.rocketPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(m_Obj);

        // rock + this player
        Assert.AreEqual(m_NumPlayers + 2, numStartServer);
        Assert.AreEqual(m_NumPlayers + 2, numStartClient);

        m_NumPlayers += 2;
    }

    public override void OnClientReady(short playerId)
    {
        // Sanity check. Make sure these are unique player IDs each time
        if (!m_ReadyPlayers.Contains(playerId))
        {
            m_ReadyPlayers.Add(playerId);
        }
        else
        {
            Assert.Fail("Player with such Id already exist");
        }
    }
}
#pragma warning restore 618
