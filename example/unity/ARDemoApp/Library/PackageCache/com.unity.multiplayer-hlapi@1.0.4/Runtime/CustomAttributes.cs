using System;


namespace UnityEngine.Networking
{
    /// <summary>
    /// This attribute is used to configure the network settings of scripts that are derived from the NetworkBehaviour base class.
    /// <code>
    /// using UnityEngine.Networking;
    ///
    /// [NetworkSettings(channel = 1, sendInterval = 0.2f)]
    /// class MyScript : NetworkBehaviour
    /// {
    ///    [SyncVar]
    ///    int value;
    /// }
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkSettingsAttribute : Attribute
    {
        /// <summary>
        /// The QoS channel to use for updates for this script.
        /// <para>Updates for SyncVar variables will be sent on the specified QoS channel. The default channel for scripts is zero.</para>
        /// </summary>
        public int channel = Channels.DefaultReliable;
        /// <summary>
        /// The sendInterval control how frequently updates are sent for this script.
        /// <para>If sendInterval is zero, updates will be sent at the end of the frame when dirty bits are set for that script. Note that setting the value of a SyncVar will automatically set dirty bits.</para>
        /// <para>If sendInterval is non-zero, updates are deferred until sendInterval seconds have passed since the last update for that script. So it can be used as a throttle in cases where the Sync value is changing constantly on the server, but you don't want it to be updated every frame.</para>
        /// <para>The default sendInterval for scripts is 0.1f seconds.</para>
        /// <para>The send interval can also be customized by implementing the virtual function GetNetworkSendInterval() on NetworkBehaviour.</para>
        /// </summary>
        public float sendInterval = 0.1f;
    }

    /// <summary>
    /// [SyncVar] is an attribute that can be put on member variables of NetworkBehaviour classes. These variables will have their values sychronized from the server to clients in the game that are in the ready state.
    /// <para>Setting the value of a [SyncVar] marks it as dirty, so it will be sent to clients at the end of the current frame. Only simple values can be marked as [SyncVars]. The type of the SyncVar variable cannot be from an external DLL or assembly.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Ship : NetworkBehaviour
    /// {
    ///    [SyncVar]
    ///    public int health = 100;
    ///
    ///    [SyncVar]
    ///    public float energy = 100;
    /// }
    /// </code>
    /// <para>The allowed SyncVar types are:</para>
    /// <list type="bullet">
    /// <item>
    /// <description>Basic type (byte, int, float, string, UInt64, etc)</description>
    /// </item>
    /// <item>
    /// <description>Built-in Unity math type (Vector3, Quaternion, etc), </description>
    /// </item>
    /// <item>
    /// <description>Structs containing allowable types.</description>
    /// </item>
    /// </list>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class SyncVarAttribute : Attribute
    {
        /// <summary>
        /// The hook attribute can be used to specify a function to be called when the sync var changes value on the client.
        /// <para>This ensures that all clients receive the proper variables from other clients.</para>
        /// <code>
        /// //Attach this to the GameObject you would like to spawn (the player).
        /// //Make sure to create a NetworkManager with an HUD component in your Scene. To do this, create a GameObject, click on it, and click on the Add Component button in the Inspector window.  From there, Go to Network>NetworkManager and Network>NetworkManagerHUD respectively.
        /// //Assign the GameObject you would like to spawn in the NetworkManager.
        /// //Start the server and client for this to work.
        ///
        /// //Use this script to send and update variables between Networked GameObjects
        /// using UnityEngine;
        /// using UnityEngine.Networking;
        ///
        /// public class Health : NetworkBehaviour
        /// {
        ///    public const int m_MaxHealth = 100;
        ///
        ///    //Detects when a health change happens and calls the appropriate function
        ///    [SyncVar(hook = "OnChangeHealth")]
        ///    public int m_CurrentHealth = m_MaxHealth;
        ///    public RectTransform healthBar;
        ///
        ///    public void TakeDamage(int amount)
        ///    {
        ///        if (!isServer)
        ///            return;
        ///        //Decrease the "health" of the GameObject
        ///        m_CurrentHealth -= amount;
        ///        //Make sure the health doesn't go below 0
        ///        if (m_CurrentHealth &lt;= 0)
        ///        {
        ///            m_CurrentHealth = 0;
        ///        }
        ///    }
        ///
        ///    void Update()
        ///    {
        ///        //If the space key is pressed, decrease the GameObject's own "health"
        ///        if (Input.GetKey(KeyCode.Space))
        ///        {
        ///            if (isLocalPlayer)
        ///                CmdTakeHealth();
        ///        }
        ///    }
        ///
        ///    void OnChangeHealth(int health)
        ///    {
        ///        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
        ///    }
        ///
        ///    //This is a Network command, so the damage is done to the relevant GameObject
        ///    [Command]
        ///    void CmdTakeHealth()
        ///    {
        ///        //Apply damage to the GameObject
        ///        TakeDamage(2);
        ///    }
        /// }
        /// </code>
        /// </summary>
        public string hook;
    }

    /// <summary>
    /// This is an attribute that can be put on methods of NetworkBehaviour classes to allow them to be invoked on the server by sending a command from a client.
    /// <para>[Command] functions are invoked on the player GameObject associated with a connection. This is set up in response to the "ready" message, by passing the player GameObject to the NetworkServer.PlayerIsReady() function. The arguments to the command call are serialized across the network, so that the server function is invoked with the same values as the function on the client. These functions must begin with the prefix "Cmd" and cannot be static.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Player : NetworkBehaviour
    /// {
    ///    int moveX = 0;
    ///    int moveY = 0;
    ///    float moveSpeed = 0.2f;
    ///    bool isDirty = false;
    /// 
    ///    void Update()
    ///    {
    ///        if (!isLocalPlayer)
    ///        {
    ///            return;
    ///        }
    ///        // input handling for local player only
    ///        int oldMoveX = moveX;
    ///        int oldMoveY = moveY;
    ///        moveX = 0;
    ///        moveY = 0;
    ///        if (Input.GetKey(KeyCode.LeftArrow))
    ///        {
    ///            moveX -= 1;
    ///        }
    ///        if (Input.GetKey(KeyCode.RightArrow))
    ///        {
    ///            moveX += 1;
    ///        }
    ///        if (Input.GetKey(KeyCode.UpArrow))
    ///        {
    ///            moveY += 1;
    ///        }
    ///        if (Input.GetKey(KeyCode.DownArrow))
    ///        {
    ///            moveY -= 1;
    ///        }
    ///        if (moveX != oldMoveX || moveY != oldMoveY)
    ///        {
    ///            CmdMove(moveX, moveY);
    ///        }
    ///    }
    ///
    ///    [Command]
    ///    public void CmdMove(int x, int y)
    ///    {
    ///        moveX = x;
    ///        moveY = y;
    ///        isDirty = true;
    ///    }
    ///
    ///    public void FixedUpdate()
    ///    {
    ///        if (NetworkServer.active)
    ///        {
    ///            transform.Translate(moveX * moveSpeed, moveY * moveSpeed, 0);
    ///        }
    ///    }
    /// }
    /// </code>
    /// <para>The allowed argument types are;</para>
    /// <list type="bullet">
    /// <item>
    /// <description>Basic type (byte, int, float, string, UInt64, etc)</description>
    /// </item>
    /// <item>
    /// <description>Built-in Unity math type (Vector3, Quaternion, etc), </description>
    /// </item>
    /// <item>
    /// <description>Arrays of basic types</description>
    /// </item>
    /// <item>
    /// <description>Structs containing allowable types </description>
    /// </item>
    /// <item>
    /// <description>NetworkIdentity </description>
    /// </item>
    /// <item>
    /// <description>NetworkInstanceId</description>
    /// </item>
    /// <item>
    /// <description>NetworkHash128</description>
    /// </item>
    /// <item>
    /// <description>GameObject with a NetworkIdentity component attached.</description>
    /// </item>
    /// </list>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// The QoS channel to use to send this command on, see <see cref="QosType"/>QosType.
        /// </summary>
        public int channel = Channels.DefaultReliable; // this is zero
    }

    /// <summary>
    /// This is an attribute that can be put on methods of NetworkBehaviour classes to allow them to be invoked on clients from a server.
    /// <para>[ClientRPC] functions are called by code on Unity Multiplayer servers, and then invoked on corresponding GameObjects on clients connected to the server. The arguments to the RPC call are serialized across the network, so that the client function is invoked with the same values as the function on the server. These functions must begin with the prefix "Rpc" and cannot be static.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Example : NetworkBehaviour
    /// {
    ///    int counter;
    ///    [ClientRpc]
    ///    public void RpcDoMagic(int extra)
    ///    {
    ///        Debug.Log("Magic = " + (123 + extra));
    ///    }
    ///
    ///    void Update()
    ///    {
    ///        counter += 1;
    ///        if (counter % 100 == 0 && NetworkServer.active)
    ///        {
    ///            RpcDoMagic(counter);
    ///        }
    ///    }
    /// }
    /// </code>
    /// <para>The allowed argument types are;</para>
    /// <list type="bullet">
    /// <item>
    /// <description>Basic type (byte, int, float, string, UInt64, etc)</description>
    /// </item>
    /// <item>
    /// <description>Built-in Unity math type (Vector3, Quaternion, etc), </description>
    /// </item>
    /// <item>
    /// <description>Arrays of basic types</description>
    /// </item>
    /// <item>
    /// <description>Structs containing allowable types </description>
    /// </item>
    /// <item>
    /// <description>NetworkIdentity </description>
    /// </item>
    /// <item>
    /// <description>NetworkInstanceId</description>
    /// </item>
    /// <item>
    /// <description>NetworkHash128</description>
    /// </item>
    /// <item>
    /// <description>GameObject with a NetworkIdentity component attached.</description>
    /// </item>
    /// </list>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ClientRpcAttribute : Attribute
    {
        /// <summary>
        /// The channel ID which this RPC transmission will use.
        /// </summary>
        public int channel = Channels.DefaultReliable; // this is zero
    }

    /// <summary>
    /// This is an attribute that can be put on methods of NetworkBehaviour classes to allow them to be invoked on clients from a server. Unlike the ClientRpc attribute, these functions are invoked on one individual target client, not all of the ready clients.
    /// <para>[TargetRpc] functions are called by user code on the server, and then invoked on the corresponding client object on the client of the specified NetworkConnection. The arguments to the RPC call are serialized across the network, so that the client function is invoked with the same values as the function on the server. These functions must begin with the prefix "Target" and cannot be static.</para>
    /// <para>The first argument to an TargetRpc function must be a NetworkConnection object.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Example : NetworkBehaviour
    /// {
    ///    [TargetRpc]
    ///    public void TargetDoMagic(NetworkConnection target, int extra)
    ///    {
    ///        Debug.Log("Magic = " + (123 + extra));
    ///    }
    ///
    ///    [Command]
    ///    void CmdTest()
    ///    {
    ///        TargetDoMagic(connectionToClient, 55);
    ///    }
    /// }
    /// </code>
    /// <para>The allowed argument types are;</para>
    /// <list type="bullet">
    /// <item>
    /// <description>Basic type (byte, int, float, string, UInt64, etc)</description>
    /// </item>
    /// <item>
    /// <description>Built-in Unity math type (Vector3, Quaternion, etc), </description>
    /// </item>
    /// <item>
    /// <description>Arrays of basic types</description>
    /// </item>
    /// <item>
    /// <description>Structs containing allowable types </description>
    /// </item>
    /// <item>
    /// <description>NetworkIdentity </description>
    /// </item>
    /// <item>
    /// <description>NetworkInstanceId</description>
    /// </item>
    /// <item>
    /// <description>NetworkHash128</description>
    /// </item>
    /// <item>
    /// <description>GameObject with a NetworkIdentity component attached.</description>
    /// </item>
    /// </list>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class TargetRpcAttribute : Attribute
    {
        /// <summary>
        /// The channel ID which this RPC transmission will use.
        /// </summary>
        public int channel = Channels.DefaultReliable; // this is zero
    }

    /// <summary>
    /// This is an attribute that can be put on events in NetworkBehaviour classes to allow them to be invoked on client when the event is called on the server.
    /// <para>[SyncEvent] events are called by user code on UNET servers, and then invoked on corresponding client objects on clients connected to the server. The arguments to the Event call are serialized across the network, so that the client event is invoked with the same values as the function on the server. These events must begin with the prefix "Event".</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class DamageClass : NetworkBehaviour
    /// {
    ///    public delegate void TakeDamageDelegate(int amount, float dir);
    ///
    ///    [SyncEvent]
    ///    public event TakeDamageDelegate EventTakeDamage;
    ///
    ///    [Command]
    ///    public void CmdDoMe(int val)
    ///    {
    ///        EventTakeDamage(val, 1.0f);
    ///    }
    /// }
    ///
    /// public class Other : NetworkBehaviour
    /// {
    ///    public DamageClass damager;
    ///    int health = 100;
    ///
    ///    void Start()
    ///    {
    ///        if (NetworkClient.active)
    ///            damager.EventTakeDamage += TakeDamage;
    ///    }
    ///
    ///    public void TakeDamage(int amount, float dir)
    ///    {
    ///        health -=  amount;
    ///    }
    /// }
    /// </code>
    /// <para>SyncEvents allow networked actions to be propagated to other scripts attached to the object. In the example above, the Other class registers for the TakeDamage event on the DamageClass. When the event happens on the DamageClass on the server, the TakeDamage() method will be invoked on the Other class on the client object. This allows modular network aware systems to be created, that can be extended by new scripts that respond to the events generated by them.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Event)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class SyncEventAttribute : Attribute
    {
        /// <summary>
        /// The UNET QoS channel that this event should be sent on.
        /// <para>This defaults to zero - the default reliable channel. This can be used to make events that are not essential for game play (such as effects) unreliable.</para>
        /// </summary>
        public int channel = Channels.DefaultReliable;  // this is zero
    }

    /// <summary>
    /// A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, to make them only run on servers.
    /// <para>A [Server] method returns immediately if NetworkServer.active is not true, and generates a warning on the console. This attribute can be put on member functions that are meant to be only called on server. This would be redundant for Command] functions, as being server-only is already enforced for them.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Example : NetworkBehaviour
    /// {
    ///    [Server]
    ///    public void Explode()
    ///    {
    ///        NetworkServer.Destroy(gameObject);
    ///    }
    /// }
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ServerAttribute : Attribute
    {
    }

    /// <summary>
    /// A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, to make them only run on servers, but not generate warnings.
    /// <para>This custom attribute is the same as the [Server] custom attribute, except that it does not generate a warning in the console if called on a client. This is useful to avoid spamming the console for functions that will be invoked by the engine, such as Update() or physics callbacks.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Example : MonoBehaviour
    /// {
    ///    float regenTimer = 0;
    ///    int heat = 100;
    ///
    ///    [ServerCallback]
    ///    void Update()
    ///    {
    ///        // heat dissipates over time
    ///        if (Time.time > regenTimer)
    ///        {
    ///            if (heat > 1)
    ///                heat -= 2;
    ///            regenTimer = Time.time + 1.0f;
    ///        }
    ///    }
    /// }
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ServerCallbackAttribute : Attribute
    {
    }

    /// <summary>
    /// A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, to make them only run on clients.
    /// <para>A [Client] method returns immediately if NetworkClient.active is not true, and generates a warning on the console. This attribute can be put on member functions that are meant to be only called on clients. This would redundant for [ClientRPC] functions, as being client-only is already enforced for them.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Example : MonoBehaviour
    /// {
    ///    [Client]
    ///    public void OnClientDisconnected(NetworkConnection conn, NetworkReader reader)
    ///    {
    ///        Debug.Log("Client Disconnected");
    ///        //ShutdownGame();
    ///        Application.LoadLevel("title");
    ///    }
    /// }
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ClientAttribute : Attribute
    {
    }

    /// <summary>
    /// A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, to make them only run on clients, but not generate warnings.
    /// <para>This custom attribute is the same as the Client custom attribute, except that it does not generate a warning in the console if called on a server. This is useful to avoid spamming the console for functions that will be invoked by the engine, such as Update() or physics callbacks.</para>
    /// <code>
    /// using UnityEngine;
    /// using UnityEngine.Networking;
    ///
    /// public class Example : MonoBehaviour
    /// {
    ///    [ClientCallback]
    ///    void OnTriggerEnter2D(Collider2D collider)
    ///    {
    ///        // make explosion
    ///    }
    /// }
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class ClientCallbackAttribute : Attribute
    {
    }
}
