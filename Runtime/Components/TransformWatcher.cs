/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR.Components
{
    /// <summary>
    /// Monitors a GameObject's transform for significant changes in position, rotation, and scale.
    /// Triggers events when changes exceed defined thresholds.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Game Objects / Transform Watcher")]
    [DisallowMultipleComponent]
    public sealed class TransformWatcher : MonoBehaviour
    {
        /// <summary>
        /// Data structure to hold information about a transform property change,
        /// including both the previous and current values.
        /// </summary>
        public struct TransformChange
        {
            public readonly bool HasChanged;
            /// <summary>
            /// The previous value before the change occurred.
            /// </summary>
            public readonly Vector3 OldValue;
            
            /// <summary>
            /// The new value after the change occurred.
            /// </summary>
            public readonly Vector3 NewValue;
            
            public TransformChange(Vector3 oldValue, Vector3 newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
                HasChanged = true;
            }
        }

        public class TransformChangeState
        {
            public TransformChange Position;
            public TransformChange Rotation;
            public TransformChange Scale;
            public TransformChange Forward;
        }

        [Serializable]
        public struct TransformIgnore
        {
            public bool position;
            public bool rotation;
            public bool scale;
            public bool forward;
        }

        public TransformIgnore ignore;
        
        /// <summary>
        /// Minimum position change (in units) required to trigger the position change event.
        /// </summary>
        [Tooltip("Minimum position change (in units) required to trigger events")]
        public float positionThreshold = .1f;
        
        /// <summary>
        /// Minimum rotation change (in degrees) required to trigger the rotation change event.
        /// </summary>
        [Tooltip("Minimum rotation change (in degrees) required to trigger events")]
        public float rotationThreshold = 1.0f;
        
        /// <summary>
        /// Minimum scale change required to trigger the scale change event.
        /// </summary>
        [Tooltip("Minimum scale change required to trigger events")]
        public float scaleThreshold = 0.1f;

        [Tooltip("Minimum forward change required to trigger events.")]
        public float forwardThreshold = 1.0f;
        
        /// <summary>
        /// The most recent position value that exceeded the threshold.
        /// </summary>
        private Vector3 _lastPosition;
        
        /// <summary>
        /// The most recent scale value that exceeded the threshold.
        /// </summary>
        private Vector3 _lastScale;
        
        /// <summary>
        /// The most recent rotation value that exceeded the threshold.
        /// </summary>
        private Vector3 _lastRotation;

        private Vector3 _lastForward;

        /// <summary>
        /// Gets the last position that exceeded the threshold.
        /// </summary>
        public Vector3 LastPosition => _lastPosition;
        
        /// <summary>
        /// Gets the last scale that exceeded the threshold.
        /// </summary>
        public Vector3 LastScale => _lastScale;
        
        /// <summary>
        /// Gets the last rotation that exceeded the threshold.
        /// </summary>
        public Vector3 LastRotation => _lastRotation;

        public Vector3 LastForward => _lastForward;
        
        /// <summary>
        /// Gets the current position of the transform.
        /// </summary>
        public Vector3 CurrentPosition => transform.position;
        
        /// <summary>
        /// Gets the current local scale of the transform.
        /// </summary>
        public Vector3 CurrentScale => transform.localScale;
        
        /// <summary>
        /// Gets the current rotation of the transform in Euler angles.
        /// </summary>
        public Vector3 CurrentRotation => transform.eulerAngles;

        public Vector3 CurrentForward => transform.forward;
        
        /// <summary>
        /// Gets or sets the position from the previous frame.
        /// </summary>
        public Vector3 PreviousPosition { get; private set; } = Vector3.zero;
        
        /// <summary>
        /// Gets or sets the scale from the previous frame.
        /// </summary>
        public Vector3 PreviousScale { get; private set; } = Vector3.zero;
        
        /// <summary>
        /// Gets or sets the rotation from the previous frame in Euler angles.
        /// </summary>
        public Vector3 PreviousRotation { get; private set; } = Vector3.zero;

        public Vector3 PreviousForward { get; private set; } = Vector3.zero;

        /// <summary>
        /// Event triggered when position changes exceed the defined threshold.
        /// Provides the old and new position values.
        /// </summary>
        [Tooltip("Event triggered when position changes exceed the threshold"), Obsolete("Use GetTransformChangeState() or <TransformWatcherEvents> instead.", true)]
        public UnityEvent<TransformChange> onChangedPosition = new UnityEvent<TransformChange>();
        
        /// <summary>
        /// Event triggered when scale changes exceed the defined threshold.
        /// Provides the old and new scale values.
        /// </summary>
        [Tooltip("Event triggered when scale changes exceed the threshold"), Obsolete("Use GetTransformChangeState() or <TransformWatcherEvents> instead.", true)]
        public UnityEvent<TransformChange> onChangedScale = new UnityEvent<TransformChange>();
        
        /// <summary>
        /// Event triggered when rotation changes exceed the defined threshold.
        /// Provides the old and new rotation values in Euler angles.
        /// </summary>
        [Tooltip("Event triggered when rotation changes exceed the threshold"), Obsolete("Use GetTransformChangeState() or <TransformWatcherEvents> instead.", true)]
        public UnityEvent<TransformChange> onChangedRotation = new UnityEvent<TransformChange>();

        /// <summary>
        /// Detects if a Vector3 value has changed beyond the specified threshold.
        /// </summary>
        /// <param name="curValue">Reference to the current value being tracked.</param>
        /// <param name="newValue">The new value to compare against.</param>
        /// <param name="threshold">The minimum distance required to register a change.</param>
        /// <returns>True if the change exceeds the threshold, false otherwise.</returns>
        private bool DetectChange(ref Vector3 curValue, Vector3 newValue, float threshold)
        {
            // Calculate the distance between the current and new values
            var dis = Vector3.Distance(newValue, curValue);
            
            // If the distance is less than or equal to the threshold, no significant change
            if (dis <= threshold)
                return false;
            
            // Update the current value to the new value
            curValue = newValue;
            
            // Return true to indicate a significant change was detected
            return true;
        }
        
        /// <summary>
        /// Monitors transform changes every frame and triggers events when changes exceed thresholds.
        /// </summary>
        public TransformChangeState
            GetTransformChangeState(TransformIgnore? ignoredChanges = null)
        {
            // Get the current transform values
            var pos = transform.position;
            var scale = transform.localScale;
            var rotation = transform.eulerAngles;
            var forward = transform.forward;

            var ign = ignoredChanges.HasValue ? ignoredChanges.Value : ignore;

            var state = new TransformChangeState();
            // Check for position changes and trigger events if needed
            if (!ign.position && DetectChange(ref _lastPosition, pos, positionThreshold))
            {
                state.Position = new TransformChange(_lastPosition, pos);
            }
            else state.Position = new TransformChange();
            
            // Check for rotation changes and trigger events if needed
            if (!ign.rotation && DetectChange(ref _lastRotation, rotation, rotationThreshold))
            {
                state.Rotation = new TransformChange(_lastRotation, rotation);
            }
            else state.Rotation = new TransformChange();
            
            // Check for scale changes and trigger events if needed
            if (!ign.scale && DetectChange(ref _lastScale, scale, scaleThreshold))
            {
                state.Scale = new TransformChange(_lastScale, scale);
            }
            else state.Scale = new TransformChange();

            if (!ign.forward && DetectChange(ref _lastForward, forward, forwardThreshold))
            {
                state.Forward = new TransformChange(_lastForward, forward);
            }
            else state.Forward = new TransformChange();
            
            // Update the previous frame values
            PreviousPosition = pos;
            PreviousScale = scale;
            PreviousRotation = rotation;
            PreviousForward = forward;
            return state;
        }
    }
}