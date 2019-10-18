#if UNITY_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;

using Inputs = UnityEngine.InputSystem.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.XR.ARCore
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class InputLayoutLoader
    {
#if UNITY_EDITOR      
        static InputLayoutLoader()
        {
            RegisterLayouts();
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RegisterLayouts()
        {
            Inputs.RegisterLayout<HandheldARInputDevice>(
                matches: new InputDeviceMatcher()
                    .WithInterface(XRUtilities.kXRInterfaceMatchAnyVersion)
                    .WithProduct("(ARCore)")
                );
        }      
    }
}
#endif
