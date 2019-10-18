#if UNITY_INPUT_SYSTEM
using UnityEngine.Scripting;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.XR.ARSubsystems
{
    /// <summary>
    /// A Handheld AR device layout, for use with the Input System, representing a mobile AR device.
    /// </summary>
    [Preserve]
    [InputControlLayout]
    public class HandheldARInputDevice : UnityEngine.InputSystem.InputDevice
    {
        /// <summary>
        /// The position in 3D space of the device.
        /// </summary>
        [Preserve]
        [InputControl]
        public Vector3Control devicePosition { get; private set; }
        /// <summary>
        /// The rotation in 3D space of the device.
        /// </summary>
        [Preserve]
        [InputControl]
        public QuaternionControl deviceRotation { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            devicePosition = GetChildControl<Vector3Control>("devicePosition");
            deviceRotation = GetChildControl<QuaternionControl>("deviceRotation");
        }
    }
}
#endif
