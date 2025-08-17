/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Tracks mouse input events including clicks, wheel scrolling, and movement.
    /// Supports both legacy Input Manager and new Input System.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Mouse Tracking Behaviour")]
    [Description("Tracks mouse clicks and wheel.")]
    public class MouseTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Contains information about a mouse event including button name and position.
        /// </summary>
        public struct MouseTrackingBehaviourArgs
        {
            /// <summary>
            /// Name of the mouse button that triggered the event.
            /// </summary>
            public readonly string MouseButton;
            
            /// <summary>
            /// Screen position where the mouse event occurred.
            /// </summary>
            public Vector3 MousePosition;

            /// <summary>
            /// Initializes mouse tracking event arguments.
            /// </summary>
            /// <param name="name">Button name (left, right, middle, wheel)</param>
            /// <param name="position">Mouse position in screen coordinates</param>
            public MouseTrackingBehaviourArgs(string name, Vector3 position)
            {
                MouseButton = name;
                MousePosition = position;
            }
        }

        /// <summary>
        /// Event triggered when a mouse button is clicked (pressed and released).
        /// </summary>
        [Gesture("Mouse"), Action("Click")]
        public readonly TrackingBehaviourEvent<MouseTrackingBehaviourArgs> OnClicked =
            new TrackingBehaviourEvent<MouseTrackingBehaviourArgs>();

        /// <summary>
        /// Event triggered when a mouse button is pressed down.
        /// </summary>
        [Gesture("Mouse"), Action("Press")]
        public readonly TrackingBehaviourEvent<MouseTrackingBehaviourArgs> OnPressedDown =
            new TrackingBehaviourEvent<MouseTrackingBehaviourArgs>();

        /// <summary>
        /// Event triggered when the mouse wheel is scrolled.
        /// </summary>
        [Gesture("Mouse"), Action("Scroll")]
        public readonly TrackingBehaviourEvent<MouseTrackingBehaviourArgs, float> OnScrolledWheel =
            new TrackingBehaviourEvent<MouseTrackingBehaviourArgs, float>();

        /// <summary>
        /// Event triggered when the mouse position changes significantly.
        /// </summary>
        [Gesture("Mouse"), Action("Move")]
        public TrackingBehaviourEvent<Vector3, Vector3> OnMousePositionChanged =
            new TrackingBehaviourEvent<Vector3, Vector3>();

        // Button state tracking for detecting press/release transitions
        private bool _isLeftDown;
        private bool _isRightDown;
        private bool _isWheelDown;

        /// <summary>
        /// Minimum movement distance in pixels to trigger position change events.
        /// </summary>
        public float movementThreshold = 3.0f;
        private Vector3 _lastMousePosition;

        private float _mouseWheel;
        /// <summary>
        /// Minimum wheel scroll delta to trigger scroll events.
        /// </summary>
        public float mouseWheelThreshold = 0.5f;

        /// <summary>
        /// Button names corresponding to mouse button indices.
        /// </summary>
        private static readonly string[] ButtonNames = { "left", "right", "middle" };

        /// <summary>
        /// Initialize mouse position tracking.
        /// </summary>
        protected virtual void Start()
        {
            _lastMousePosition = GetMousePosition();
        }

        /// <summary>
        /// Handles mouse button state changes and triggers appropriate events.
        /// </summary>
        /// <param name="index">Button index (0=left, 1=right, 2=middle)</param>
        /// <param name="wasDown">Previous button state</param>
        /// <param name="position">Current mouse position</param>
        private void HandleMouseClick(int index, ref bool wasDown, Vector3 position)
        {
            bool isDown = false;
#if ENABLE_INPUT_SYSTEM
            // Use Input System if available
            if (Mouse.current != null)
            {
                switch (index)
                {
                    case 0: isDown = Mouse.current.leftButton.isPressed; break;
                    case 1: isDown = Mouse.current.rightButton.isPressed; break;
                    case 2: isDown = Mouse.current.middleButton.isPressed; break;
                }
            }
#else
            // Fall back to legacy Input Manager
            isDown = Input.GetMouseButton(index);
#endif

            var n = ButtonNames[index];

            switch (wasDown)
            {
                // Button was just pressed
                case false when isDown:
                    OnPressedDown?.Invoke(this, new MouseTrackingBehaviourArgs(n, position));
                    break;
                // Button was just released (clicked)
                case true when !isDown:
                    OnClicked?.Invoke(this, new MouseTrackingBehaviourArgs(n, position));
                    break;
            }

            wasDown = isDown;
        }

        /// <summary>
        /// Updates mouse input tracking each frame.
        /// </summary>
        private void Update()
        {
            var mousePos = GetMousePosition();
            
            // Check all mouse buttons for state changes
            HandleMouseClick(0, ref _isLeftDown, mousePos);
            HandleMouseClick(1, ref _isRightDown, mousePos);
            HandleMouseClick(2, ref _isWheelDown, mousePos);

            // Detect mouse wheel scrolling
            float curMouseWheel = GetMouseScroll();
            float mouseWheelDis = Mathf.Abs(_mouseWheel - curMouseWheel);
            if (mouseWheelDis > mouseWheelThreshold)
            {
                _mouseWheel = curMouseWheel;
                OnScrolledWheel?.Invoke(this, new MouseTrackingBehaviourArgs("wheel", mousePos), curMouseWheel);
            }

            // Detect significant mouse movement
            float distance = Vector3.Distance(mousePos, _lastMousePosition);
            if (distance > movementThreshold)
            {
                OnMousePositionChanged?.Invoke(this, mousePos, _lastMousePosition);
                _lastMousePosition = mousePos;
            }
        }

        /// <summary>
        /// Gets current mouse position using appropriate input system.
        /// </summary>
        /// <returns>Mouse position in screen coordinates</returns>
        private Vector3 GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                var pos = Mouse.current.position.ReadValue();
                return new Vector3(pos.x, pos.y, 0f);
            }
            return Vector3.zero;
#else
            return Input.mousePosition;
#endif
        }

        /// <summary>
        /// Gets current mouse scroll wheel delta.
        /// </summary>
        /// <returns>Scroll wheel delta value</returns>
        private float GetMouseScroll()
        {
#if ENABLE_INPUT_SYSTEM
            return Mouse.current?.scroll.ReadValue().y ?? 0f;
#else
            return Input.mouseScrollDelta.y;
#endif
        }
    }
}