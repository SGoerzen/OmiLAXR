/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OmiLAXR.Components
{
    /// <summary>
    /// Handles pointer interaction events for UI elements, supporting both mouse and XR interaction.
    /// Implements Unity's pointer interface to track hover, press, and release events.
    /// </summary>
    public class InteractionEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        public delegate void InteractionEventHandlerAction(InteractionEventHandler sender, InteractionEventArgs args);
        /// <summary>
        /// Contains data about the pointer interaction events.
        /// </summary>
        public struct InteractionEventArgs
        {
            public string Device { get; set; }
            /// <summary>
            /// Total number of presses recorded since this component was initialized.
            /// </summary>
            public uint TotalPresses { get; set; }
            
            /// <summary>
            /// Number of presses recorded during the current hover.
            /// </summary>
            public uint PressesInHover { get; set; }
            
            /// <summary>
            /// Duration in seconds that the pointer has been hovering over this element.
            /// </summary>
            public float HoverDuration { get; set; }
            
            /// <summary>
            /// Duration in seconds that the pointer has been pressing this element.
            /// </summary>
            public float PressDuration { get; set; }
        }
        
        // Timestamp when the pointer starts hovering over this element
        private float _hoverStartTime;
        
        // Timestamp when the pointer starts pressing this element
        private float _pressStartTime;

        // Flags to track the current interaction state
        private bool _isHovering;
        private bool _isPressing;
        
        // Sum of how often the button was pressed inside a hover
        private uint _pressHoverSum;
        private uint _pressTotalSum;

        /// <summary>
        /// Event triggered when the pointer starts hovering over this element.
        /// </summary>
        public event InteractionEventHandlerAction OnHoverStarted;

        /// <summary>
        /// Event triggered when the pointer stops hovering over this element.
        /// </summary>
        public event InteractionEventHandlerAction OnHoverEnded;
        
        /// <summary>
        /// Event triggered when the pointer begins pressing this element.
        /// </summary>
        public event InteractionEventHandlerAction OnPressStarted;
        
        /// <summary>
        /// Event triggered when the pointer releases this element after pressing.
        /// </summary>
        public event InteractionEventHandlerAction OnPressEnded;
        
        /// <summary>
        /// Event triggered when a complete click action occurs (press and release while hovering).
        /// Only fires when the element is released while the pointer is still hovering over it.
        /// </summary>
        public event InteractionEventHandlerAction OnClicked;
        
        /// <summary>
        /// Whether the pointer is currently hovering over this element.
        /// </summary>
        public bool IsHovering => _isHovering;
        
        /// <summary>
        /// Whether the pointer is currently pressing this element.
        /// </summary>
        public bool IsPressing => _isPressing;

        public static string GetDeviceName(PointerEventData eventData)
        {
            // 1. Prüfe pointerId (klassisch)
            if (eventData.pointerId == -1)
                return "Touch";
            if (eventData.pointerId == 0)
                return "Mouse";
            if (eventData.pointerId > 0)
                return $"Touch {eventData.pointerId}";

            // 2. Optional: Analyse des Raycast-Moduls
            var moduleType = eventData.pointerPressRaycast.module?.GetType().Name ?? "UnknownModule";

            // 3. Rückfall
            return $"Unknown input via {moduleType}";
        }
        
        /// <summary>
        /// Called when the pointer enters this UI element.
        /// Records the start time and logs the event.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            _hoverStartTime = Time.time;
            _isHovering = true;
            _pressHoverSum = 0;
            
            OnHoverStarted?.Invoke(this, new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = 0,
                PressDuration = 0,
                Device = GetDeviceName(eventData)
            });
        }

        /// <summary>
        /// Called when the pointer exits this UI element.
        /// Calculates the hover duration, logs the event, and triggers the OnHoverEnded event.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isHovering) 
                return;
            _isHovering = false;

            OnHoverEnded?.Invoke(this, new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = Time.time - _hoverStartTime,
                PressDuration = _isPressing ? Time.time - _pressStartTime : 0,
                Device = GetDeviceName(eventData)
            });
        }

        /// <summary>
        /// Called when the pointer is pressed down on this UI element.
        /// Records the start time and triggers the OnPressStarted event.
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            _pressStartTime = Time.time;
            _isPressing = true;

            OnPressStarted?.Invoke(this, new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = Time.time - _hoverStartTime,
                PressDuration = 0,
                Device = GetDeviceName(eventData)
            });
        }

        /// <summary>
        /// Called when the pointer is released on this UI element.
        /// Calculates the press duration, increments press counters, and triggers the OnPressEnded event.
        /// Also triggers OnClicked if the release happens while still hovering over the element.
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isPressing) 
                return;
            
            var pressDuration = Time.time - _pressStartTime;
            _isPressing = false;
            _pressHoverSum++;
            _pressTotalSum++;

            var eventArgs = new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = Time.time - _hoverStartTime,
                PressDuration = pressDuration,
                Device = GetDeviceName(eventData)
            };

            OnPressEnded?.Invoke(this, eventArgs);
            
            // If we're still hovering when the press ends, this is considered a "click"
            if (_isHovering)
            {
                OnClicked?.Invoke(this, eventArgs);
            }
        }
    }
}