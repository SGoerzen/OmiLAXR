using System;
using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR
{
    /// <summary>
    /// Monitors a GameObject's transform for significant changes in position, rotation, and scale.
    /// Triggers events when changes exceed defined thresholds.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Game Objects / Transform Watcher")]
    [DisallowMultipleComponent]
    public class TransformWatcher : MonoBehaviour
    {
        /// <summary>
        /// Data structure to hold information about a transform property change,
        /// including both the previous and current values.
        /// </summary>
        public struct TransformChange
        {
            /// <summary>
            /// The previous value before the change occurred.
            /// </summary>
            public Vector3 OldValue;
            
            /// <summary>
            /// The new value after the change occurred.
            /// </summary>
            public Vector3 NewValue;
        }

        [Serializable]
        public struct TransformIgnore
        {
            public bool position;
            public bool rotation;
            public bool scale;
        }

        public TransformIgnore ignore;
        
        /// <summary>
        /// Minimum position change (in units) required to trigger the position change event.
        /// </summary>
        [Tooltip("Minimum position change (in units) required to trigger events")]
        public float positionThreshold = .5f;
        
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
        
        /// <summary>
        /// Gets or sets the position from the previous frame.
        /// </summary>
        public Vector3 PreviousPosition { get; protected set; } = Vector3.zero;
        
        /// <summary>
        /// Gets or sets the scale from the previous frame.
        /// </summary>
        public Vector3 PreviousScale { get; protected set; } = Vector3.zero;
        
        /// <summary>
        /// Gets or sets the rotation from the previous frame in Euler angles.
        /// </summary>
        public Vector3 PreviousRotation { get; protected set; } = Vector3.zero;

        /// <summary>
        /// Event triggered when position changes exceed the defined threshold.
        /// Provides the old and new position values.
        /// </summary>
        [Tooltip("Event triggered when position changes exceed the threshold")]
        public UnityEvent<TransformChange> onChangedPosition = new UnityEvent<TransformChange>();
        
        /// <summary>
        /// Event triggered when scale changes exceed the defined threshold.
        /// Provides the old and new scale values.
        /// </summary>
        [Tooltip("Event triggered when scale changes exceed the threshold")]
        public UnityEvent<TransformChange> onChangedScale = new UnityEvent<TransformChange>();
        
        /// <summary>
        /// Event triggered when rotation changes exceed the defined threshold.
        /// Provides the old and new rotation values in Euler angles.
        /// </summary>
        [Tooltip("Event triggered when rotation changes exceed the threshold")]
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
        private void Update()
        {
            // Get the current transform values
            var pos = transform.position;
            var scale = transform.localScale;
            var rotation = transform.eulerAngles;

            // Check for position changes and trigger events if needed
            if (!ignore.position && DetectChange(ref _lastPosition, pos, positionThreshold))
            {
                onChangedPosition?.Invoke(new TransformChange()
                {
                    NewValue = pos,
                    OldValue = _lastPosition
                });
            }
            
            // Check for rotation changes and trigger events if needed
            if (!ignore.rotation && DetectChange(ref _lastRotation, rotation, rotationThreshold))
            {
                onChangedRotation?.Invoke(new TransformChange()
                {
                    NewValue = rotation,
                    OldValue = _lastRotation
                });
            }
            
            // Check for scale changes and trigger events if needed
            if (!ignore.scale && DetectChange(ref _lastScale, scale, scaleThreshold))
            {
                onChangedScale?.Invoke(new TransformChange()
                {
                    NewValue = scale,
                    OldValue = _lastScale
                });
            }
            
            // Update the previous frame values
            PreviousPosition = pos;
            PreviousScale = scale;
            PreviousRotation = rotation;
        }
    }
}