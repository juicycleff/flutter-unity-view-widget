using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
    /// <summary>
    /// This class holds information about which networked scripts use which QoS channels for updates.
    /// <para>This channel information is used to ensure that clients and servers are using compatible HLAPI script configurations.</para>
    /// </summary>
    [Obsolete("The high level API classes are deprecated and will be removed in the future.")]
    public class NetworkCRC
    {
        internal static NetworkCRC s_Singleton;

        Dictionary<string, int> m_Scripts = new Dictionary<string, int>();
        bool m_ScriptCRCCheck;

        static internal NetworkCRC singleton
        {
            get
            {
                if (s_Singleton == null)
                {
                    s_Singleton = new NetworkCRC();
                }
                return s_Singleton;
            }
        }
        /// <summary>
        /// A dictionary of script QoS channels.
        /// <para>This is used to compare script network configurations between clients and servers.</para>
        /// </summary>
        public Dictionary<string, int> scripts { get { return m_Scripts; } }

        /// <summary>
        /// Enables a CRC check between server and client that ensures the <see cref="NetworkBehaviour">NetworkBehaviour</see> scripts match.
        /// <para>This may not be appropriate in some cases, such a when the client and server are different Unity projects.</para>
        /// </summary>
        static public bool scriptCRCCheck
        {
            get
            {
                return singleton.m_ScriptCRCCheck;
            }
            set
            {
                singleton.m_ScriptCRCCheck = value;
            }
        }

        /// <summary>
        /// This can be used to reinitialize the set of script CRCs.
        /// <para>This is very rarely required - only when NetworkBehaviour scripts are dynamically loaded.</para>
        /// </summary>
        /// <param name="callingAssembly"></param>
        // The NetworkCRC cache contain entries from
        static public void ReinitializeScriptCRCs(Assembly callingAssembly)
        {
            singleton.m_Scripts.Clear();

            var types = callingAssembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (t.GetBaseType() == typeof(NetworkBehaviour))
                {
                    var cctor  = t.GetMethod(".cctor", BindingFlags.Static);
                    if (cctor != null)
                    {
                        cctor.Invoke(null, new object[] {});
                    }
                }
            }
        }

        /// <summary>
        /// This is used to setup script network settings CRC data.
        /// </summary>
        /// <param name="name">Script name.</param>
        /// <param name="channel">QoS Channel.</param>
        static public void RegisterBehaviour(string name, int channel)
        {
            singleton.m_Scripts[name] = channel;
        }

        internal static bool Validate(CRCMessageEntry[] scripts, int numChannels)
        {
            return singleton.ValidateInternal(scripts, numChannels);
        }

        bool ValidateInternal(CRCMessageEntry[] remoteScripts, int numChannels)
        {
            // check count against my channels
            if (m_Scripts.Count != remoteScripts.Length)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("Network configuration mismatch detected. The number of networked scripts on the client does not match the number of networked scripts on the server. This could be caused by lazy loading of scripts on the client. This warning can be disabled by the checkbox in NetworkManager Script CRC Check."); }
                Dump(remoteScripts);
                return false;
            }

            // check each script
            for (int i = 0; i < remoteScripts.Length; i++)
            {
                var script = remoteScripts[i];
                if (LogFilter.logDebug) { Debug.Log("Script: " + script.name + " Channel: " + script.channel); }

                if (m_Scripts.ContainsKey(script.name))
                {
                    int localChannel = m_Scripts[script.name];
                    if (localChannel != script.channel)
                    {
                        if (LogFilter.logError) { Debug.LogError("HLAPI CRC Channel Mismatch. Script: " + script.name + " LocalChannel: " + localChannel + " RemoteChannel: " + script.channel); }
                        Dump(remoteScripts);
                        return false;
                    }
                }
                if (script.channel >= numChannels)
                {
                    if (LogFilter.logError) { Debug.LogError("HLAPI CRC channel out of range! Script: " + script.name + " Channel: " + script.channel); }
                    Dump(remoteScripts);
                    return false;
                }
            }
            return true;
        }

        void Dump(CRCMessageEntry[] remoteScripts)
        {
            foreach (var script in m_Scripts.Keys)
            {
                Debug.Log("CRC Local Dump " + script + " : " + m_Scripts[script]);
            }

            foreach (var remote in remoteScripts)
            {
                Debug.Log("CRC Remote Dump " + remote.name + " : " + remote.channel);
            }
        }
    }
}
