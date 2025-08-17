/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Tracks XR device input events using Unity's Input System.
    /// Monitors button presses and releases across all connected input devices.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Input System Tracking Behaviour"), 
     Description("Tracks XR device inputs by using <InputSystem> instance.")]
    public class InputSystemTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Contains information about an input tracking event including device and button details.
        /// </summary>
        public struct InputTrackingBehaviourArgs
        {
            /// <summary>
            /// Unique identifier of the input device.
            /// </summary>
            public int DeviceId;
            
            /// <summary>
            /// Display name of the input device.
            /// </summary>
            public string DeviceName;
            
            /// <summary>
            /// Name of the button that was pressed or released.
            /// </summary>
            public string ButtonName;
        }
        
        /// <summary>
        /// Event triggered when any button is pressed on any tracked device.
        /// </summary>
        [Gesture("XRController"), Action("Press")]
        public readonly TrackingBehaviourEvent<InputTrackingBehaviourArgs> OnPressedAnyButton = new TrackingBehaviourEvent<InputTrackingBehaviourArgs>();
        
        /// <summary>
        /// Event triggered when any button is released on any tracked device.
        /// </summary>
        [Gesture("XRController"), Action("Release")]
        public readonly TrackingBehaviourEvent<InputTrackingBehaviourArgs> OnReleasedAnyButton = new TrackingBehaviourEvent<InputTrackingBehaviourArgs>();

        /// <summary>
        /// Updates each frame to check for input events across all connected devices.
        /// </summary>
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
                    
                    // TODO: Add invoking of OnReleasedAnyButton
                }
            }
        }
    }
}