using System;
using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR.Components
{
    [RequireComponent(typeof(TransformWatcher))]
    public class TransformWatcherEvents : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when position changes exceed the defined threshold.
        /// Provides the old and new position values.
        /// </summary>
        [Tooltip("Event triggered when position changes exceed the threshold")]
        public UnityEvent<TransformWatcher.TransformChange> onChangedPosition = new UnityEvent<TransformWatcher.TransformChange>();
        
        /// <summary>
        /// Event triggered when scale changes exceed the defined threshold.
        /// Provides the old and new scale values.
        /// </summary>
        [Tooltip("Event triggered when scale changes exceed the threshold")]
        public UnityEvent<TransformWatcher.TransformChange> onChangedScale = new UnityEvent<TransformWatcher.TransformChange>();
        
        /// <summary>
        /// Event triggered when rotation changes exceed the defined threshold.
        /// Provides the old and new rotation values in Euler angles.
        /// </summary>
        [Tooltip("Event triggered when rotation changes exceed the threshold")]
        public UnityEvent<TransformWatcher.TransformChange> onChangedRotation = new UnityEvent<TransformWatcher.TransformChange>();
        
        [Tooltip("Event triggered when forward changes exceed the threshold")]
        public UnityEvent<TransformWatcher.TransformChange> onChangedForward = new UnityEvent<TransformWatcher.TransformChange>();

        private TransformWatcher _transformWatcher;

        private void Awake()
        {
            _transformWatcher = GetComponent<TransformWatcher>();
        }

        private void Update()
        {
            var state = _transformWatcher.GetTransformChangeState();
            
            if (state.Position.HasChanged)
                onChangedPosition?.Invoke(state.Position);
            
            if (state.Rotation.HasChanged)
                onChangedRotation?.Invoke(state.Rotation);
            
            if (state.Scale.HasChanged)
                onChangedScale?.Invoke(state.Scale);
            
            if (state.Forward.HasChanged)
                onChangedForward?.Invoke(state.Forward);
        }
    }
}