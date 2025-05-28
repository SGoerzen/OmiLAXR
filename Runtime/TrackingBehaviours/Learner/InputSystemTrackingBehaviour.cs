using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Input System Tracking Behaviour"), 
     Description("Tracks XR device inputs by using <InputSystem> instance.")]
    public class InputSystemTrackingBehaviour : TrackingBehaviour
    {
        public struct InputTrackingBehaviourArgs
        {
            public int DeviceId;
            public string DeviceName;
            public string ButtonName;
        }
        
        [Gesture("XRController"), Action("Press")]
        public readonly TrackingBehaviourEvent<InputTrackingBehaviourArgs> OnPressedAnyButton = new TrackingBehaviourEvent<InputTrackingBehaviourArgs>();
        
        protected virtual void Update()
        {
            // Check all devices
            foreach (var device in InputSystem.devices)
            {
                // Check all controls on the device
                foreach (var control in device.allControls)
                {
                    // If it's a button control and is pressed
                    var button = control as ButtonControl;
                    if (button == null || !button.wasPressedThisFrame)
                        continue;
                    // Fire the event
                    OnPressedAnyButton?.Invoke(this, new InputTrackingBehaviourArgs()
                    {
                        ButtonName = control.name,
                        DeviceId = device.deviceId,
                        DeviceName = device.displayName
                    });
                }
            }
        }
    }
}