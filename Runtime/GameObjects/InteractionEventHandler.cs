using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OmiLAXR
{
    /// <summary>
    /// Handles pointer interaction events for UI elements, supporting both mouse and XR interaction.
    /// Implements Unity's pointer interface to track hover, press, and release events.
    /// </summary>
    public class InteractionEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// Contains data about the pointer interaction events.
        /// </summary>
        public struct InteractionEventArgs
        {
            /// <summary>
            /// Total number of presses recorded since this component was initialized.
            /// </summary>
            public int TotalPresses { get; set; }
            
            /// <summary>
            /// Number of presses recorded during the current hover.
            /// </summary>
            public int PressesInHover { get; set; }
            
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
        private bool _isHovering = false;
        private bool _isPressing = false;
        
        // Sum of how often the button was pressed inside a hover
        private int _pressHoverSum = 0;
        private int _pressTotalSum = 0;

        /// <summary>
        /// Event triggered when the pointer starts hovering over this element.
        /// </summary>
        public event Action<InteractionEventArgs> OnHoverStarted;

        /// <summary>
        /// Event triggered when the pointer stops hovering over this element.
        /// </summary>
        public event Action<InteractionEventArgs> OnHoverEnded;
        
        /// <summary>
        /// Event triggered when the pointer begins pressing this element.
        /// </summary>
        public event Action<InteractionEventArgs> OnPressStarted;
        
        /// <summary>
        /// Event triggered when the pointer releases this element after pressing.
        /// </summary>
        public event Action<InteractionEventArgs> OnPressEnded;
        
        /// <summary>
        /// Event triggered when a complete click action occurs (press and release while hovering).
        /// Only fires when the element is released while the pointer is still hovering over it.
        /// </summary>
        public event Action<InteractionEventArgs> OnClicked;
        
        /// <summary>
        /// Whether the pointer is currently hovering over this element.
        /// </summary>
        public bool IsHovering => _isHovering;
        
        /// <summary>
        /// Whether the pointer is currently pressing this element.
        /// </summary>
        public bool IsPressing => _isPressing;

        /// <summary>
        /// Called when the pointer enters this UI element.
        /// Records the start time and logs the event.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            _hoverStartTime = Time.time;
            _isHovering = true;
            _pressHoverSum = 0;
            
            OnHoverStarted?.Invoke(new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = 0,
                PressDuration = 0
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

            OnHoverEnded?.Invoke(new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = Time.time - _hoverStartTime,
                PressDuration = _isPressing ? Time.time - _pressStartTime : 0
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

            OnPressStarted?.Invoke(new InteractionEventArgs()
            {
                TotalPresses = _pressTotalSum,
                PressesInHover = _pressHoverSum,
                HoverDuration = Time.time - _hoverStartTime,
                PressDuration = 0
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
                PressDuration = pressDuration
            };

            OnPressEnded?.Invoke(eventArgs);
            
            // If we're still hovering when the press ends, this is considered a "click"
            if (_isHovering)
            {
                OnClicked?.Invoke(eventArgs);
            }
        }
    }
}