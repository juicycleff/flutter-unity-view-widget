using System;
using UnityEngine;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// An asset representing an emitted signal. A SignalAsset connects a SignalEmitter with a SignalReceiver.
    /// </summary>
    /// <seealso cref="UnityEngine.Timeline.SignalEmitter"/>
    /// <seealso cref="UnityEngine.Timeline.SignalReceiver"/>
    [AssetFileNameExtension("signal")]
    public class SignalAsset : ScriptableObject
    {
        internal static event Action<SignalAsset> OnEnableCallback;

        void OnEnable()
        {
            if (OnEnableCallback != null)
                OnEnableCallback(this);
        }
    }
}
