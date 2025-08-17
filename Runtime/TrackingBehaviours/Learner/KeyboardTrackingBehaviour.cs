/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Tracks keyboard input events for common keys including letters, numbers, and function keys.
    /// Supports both legacy Input Manager and new Input System.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Keyboard Tracking Behaviour"),
     Description("Tracks keyboard presses and releases.")]
    public class KeyboardTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Contains information about a keyboard event including key state and identifier.
        /// </summary>
        public struct KeyboardTrackingBehaviourArgs
        {
            /// <summary>
            /// Whether the key is currently pressed (true) or released (false).
            /// </summary>
            public readonly bool IsDown;
            
            /// <summary>
            /// String identifier of the key that triggered the event.
            /// </summary>
            public readonly string Key;

            /// <summary>
            /// Initializes keyboard event arguments.
            /// </summary>
            /// <param name="isDown">True if key is pressed, false if released</param>
            /// <param name="key">Key identifier string</param>
            public KeyboardTrackingBehaviourArgs(bool isDown, string key)
            {
                this.IsDown = isDown;
                this.Key = key;
            }
        }

        /// <summary>
        /// Event triggered when any tracked key is pressed or released.
        /// </summary>
        [Gesture("Keyboard"), Action("Pressed")]
        public TrackingBehaviourEvent<KeyboardTrackingBehaviourArgs> OnPressed =
            new TrackingBehaviourEvent<KeyboardTrackingBehaviourArgs>();

        /// <summary>
        /// Dictionary tracking the previous state of each monitored key.
        /// </summary>
        private readonly Dictionary<KeyCode, bool> _wasDown = new Dictionary<KeyCode, bool>();
        
        /// <summary>
        /// List of keys being actively monitored for input changes.
        /// </summary>
        private List<KeyCode> _keys = new List<KeyCode>();

        /// <summary>
        /// Initializes the set of keys to monitor when the component is enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Initialize tracking for common keyboard keys
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                // Filter to include only commonly used keys
                if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z ||
                    keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9 ||
                    keyCode >= KeyCode.F1 && keyCode <= KeyCode.F15 ||
                    keyCode >= KeyCode.Keypad0 && keyCode <= KeyCode.Keypad9 ||
                    keyCode >= KeyCode.UpArrow && keyCode <= KeyCode.RightArrow ||
                    keyCode == KeyCode.Space || keyCode == KeyCode.Return ||
                    keyCode == KeyCode.Backspace || keyCode == KeyCode.Tab ||
                    keyCode == KeyCode.Escape || keyCode == KeyCode.Delete)
                {
#if UNITY_2021_1_OR_NEWER
                    _wasDown.TryAdd(keyCode, false);
#else 
                    // Fallback for older Unity versions
                    if (!_wasDown.ContainsKey(keyCode))
                        _wasDown.Add(keyCode, false);
#endif
                }
            }

            _keys = new List<KeyCode>(_wasDown.Keys);
        }

        /// <summary>
        /// Processes key state changes and triggers events for press/release transitions.
        /// </summary>
        /// <param name="n">Key name string</param>
        /// <param name="isDown">Current key state</param>
        /// <param name="wasDown">Previous key state</param>
        private void HandleKey(string n, bool isDown, ref bool wasDown)
        {
            // Detect key press (transition from up to down)
            if (!wasDown && isDown)
                OnPressed?.Invoke(this, new KeyboardTrackingBehaviourArgs(true, n));
            // Detect key release (transition from down to up)
            else if (wasDown && !isDown)
                OnPressed?.Invoke(this, new KeyboardTrackingBehaviourArgs(false, n));

            wasDown = isDown;
        }

        /// <summary>
        /// Updates keyboard input tracking each frame.
        /// </summary>
        protected virtual void Update()
        {
            // Check each monitored key for state changes
            foreach (var key in _keys)
            {
                bool isDown = false;

#if ENABLE_INPUT_SYSTEM
                // Use Input System if available
                var inputKey = ConvertToInputSystemKey(key);
                if (inputKey != null)
                {
                    isDown = inputKey.isPressed;
                }
#else
                // Fall back to legacy Input Manager
                isDown = Input.GetKey(key);
#endif
                var wasDown = _wasDown[key];
                HandleKey(key.ToString(), isDown, ref wasDown);
                _wasDown[key] = wasDown;
            }
        }

#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// Converts legacy KeyCode values to Input System KeyControl references.
        /// </summary>
        /// <param name="keyCode">Legacy KeyCode to convert</param>
        /// <returns>Corresponding KeyControl, or null if not supported</returns>
        private KeyControl ConvertToInputSystemKey(KeyCode keyCode)
        {
            if (Keyboard.current == null) return null;

            // Map common KeyCodes to Input System KeyControls
            return keyCode switch
            {
                // Letter keys
                KeyCode.A => Keyboard.current.aKey,
                KeyCode.B => Keyboard.current.bKey,
                KeyCode.C => Keyboard.current.cKey,
                KeyCode.D => Keyboard.current.dKey,
                KeyCode.E => Keyboard.current.eKey,
                KeyCode.F => Keyboard.current.fKey,
                KeyCode.G => Keyboard.current.gKey,
                KeyCode.H => Keyboard.current.hKey,
                KeyCode.I => Keyboard.current.iKey,
                KeyCode.J => Keyboard.current.jKey,
                KeyCode.K => Keyboard.current.kKey,
                KeyCode.L => Keyboard.current.lKey,
                KeyCode.M => Keyboard.current.mKey,
                KeyCode.N => Keyboard.current.nKey,
                KeyCode.O => Keyboard.current.oKey,
                KeyCode.P => Keyboard.current.pKey,
                KeyCode.Q => Keyboard.current.qKey,
                KeyCode.R => Keyboard.current.rKey,
                KeyCode.S => Keyboard.current.sKey,
                KeyCode.T => Keyboard.current.tKey,
                KeyCode.U => Keyboard.current.uKey,
                KeyCode.V => Keyboard.current.vKey,
                KeyCode.W => Keyboard.current.wKey,
                KeyCode.X => Keyboard.current.xKey,
                KeyCode.Y => Keyboard.current.yKey,
                KeyCode.Z => Keyboard.current.zKey,

                // Special keys
                KeyCode.Space => Keyboard.current.spaceKey,
                KeyCode.Return => Keyboard.current.enterKey,
                KeyCode.Backspace => Keyboard.current.backspaceKey,
                KeyCode.Tab => Keyboard.current.tabKey,
                KeyCode.Escape => Keyboard.current.escapeKey,
                KeyCode.Delete => Keyboard.current.deleteKey,

                // Arrow keys
                KeyCode.UpArrow => Keyboard.current.upArrowKey,
                KeyCode.DownArrow => Keyboard.current.downArrowKey,
                KeyCode.LeftArrow => Keyboard.current.leftArrowKey,
                KeyCode.RightArrow => Keyboard.current.rightArrowKey,

                // Number keys
                KeyCode.Alpha0 => Keyboard.current.digit0Key,
                KeyCode.Alpha1 => Keyboard.current.digit1Key,
                KeyCode.Alpha2 => Keyboard.current.digit2Key,
                KeyCode.Alpha3 => Keyboard.current.digit3Key,
                KeyCode.Alpha4 => Keyboard.current.digit4Key,
                KeyCode.Alpha5 => Keyboard.current.digit5Key,
                KeyCode.Alpha6 => Keyboard.current.digit6Key,
                KeyCode.Alpha7 => Keyboard.current.digit7Key,
                KeyCode.Alpha8 => Keyboard.current.digit8Key,
                KeyCode.Alpha9 => Keyboard.current.digit9Key,

                _ => null // Unsupported key
            };
        }
#endif
    }
}