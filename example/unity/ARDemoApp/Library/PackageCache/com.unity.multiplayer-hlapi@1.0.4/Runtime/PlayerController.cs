using System;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This represents a networked player.
    /// </summary>
    // This class represents the player entity in a network game, there can be multiple players per client
    // when there are multiple people playing on one machine
    // The server has one connection per client, and the connection has the player instances of that client
    // The client has player instances as member variables (should this be removed and just go though the connection like the server does?)
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class PlayerController
    {
        internal const short kMaxLocalPlayers = 8;

        /// <summary>
        /// The local player ID number of this player.
        /// <para>The HLAPI treats players and clients as separate GameObjects. In most cases, there is a single player for each client, but in some situations (for example, when there are multiple controllers connected to a console system) there might be multiple player GameObjects for a single connection. When there are multiple players for a single connection, use the playerControllerId property to tell them apart. This is an identifier that is scoped to the connection, so that it maps to the id of the controller associated with the player on that client.</para>
        /// </summary>
        public short playerControllerId = -1;
        /// <summary>
        /// The NetworkIdentity component of the player.
        /// </summary>
        public NetworkIdentity unetView;
        /// <summary>
        /// The game object for this player.
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// The maximum number of local players that a client connection can have.
        /// </summary>
        public const int MaxPlayersPerClient = 32;

        public PlayerController()
        {
        }

        /// <summary>
        /// Checks if this PlayerController has an actual player attached to it.
        /// </summary>
        public bool IsValid { get { return playerControllerId != -1; } }

        internal PlayerController(GameObject go, short playerControllerId)
        {
            gameObject = go;
            unetView = go.GetComponent<NetworkIdentity>();
            this.playerControllerId = playerControllerId;
        }

        /// <summary>
        /// String representation of the player objects state.
        /// </summary>
        /// <returns>String with the object state.</returns>
        public override string ToString()
        {
            return string.Format("ID={0} NetworkIdentity NetID={1} Player={2}", new object[] { playerControllerId, (unetView != null ? unetView.netId.ToString() : "null"), (gameObject != null ? gameObject.name : "null") });
        }
    }
}
