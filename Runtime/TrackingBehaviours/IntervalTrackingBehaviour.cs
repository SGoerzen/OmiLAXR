using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    public abstract class IntervalTrackingBehaviour : IntervalTrackingBehaviour<Object>
    {
        protected override void AfterFilteredObjects(Object[] objects)
        {
            
        }
    }
    /// <summary>
    /// Base class for tracking behaviors that operate on a timed interval
    /// rather than processing every frame. Derived classes should implement
    /// ProcessObjectsAtInterval to define the interval-based behavior.
    /// </summary>
    public abstract class IntervalTrackingBehaviour<T> : TrackingBehaviour<T>
        where T : Object
    {
        public bool isIntervalActivated = true;
        /// <summary>
        /// Time interval in seconds between processing operations.
        /// </summary>
        [Tooltip("Time in seconds between processing operations")]
        public float intervalSeconds = 0.3f;
        
        /// <summary>
        /// Whether to process immediately when the behavior is enabled,
        /// rather than waiting for the first interval.
        /// </summary>
        [Tooltip("Process immediately when enabled")]
        public bool processOnEnable = true;
        
        // Last time processing occurred
        private float _lastProcessTime = 0f;
        
        // Whether this is the first processing since enable
        private bool _isFirstProcess = true;
        
        /// <summary>
        /// Unity's Update method. Checks if the interval has elapsed and
        /// calls the interval processing method when appropriate.
        /// </summary>
        protected virtual void Update()
        {
            if (!isIntervalActivated)
                return;
            
            // Check if it's time to process based on interval
            if (_isFirstProcess && processOnEnable)
            {
                IntervalUpdate();
                _lastProcessTime = Time.time;
                _isFirstProcess = false;
                return;
            }
            
            if (Time.time - _lastProcessTime >= intervalSeconds)
            {
                IntervalUpdate();
                _lastProcessTime = Time.time;
                _isFirstProcess = false;
            }
        }
        
        /// <summary>
        /// Abstract method that derived classes must implement to define
        /// the processing that should occur at each interval.
        /// </summary>
        protected abstract void IntervalUpdate();
        
        /// <summary>
        /// Resets the interval timer, causing the next interval to start from now.
        /// </summary>
        protected void ResetInterval()
        {
            _lastProcessTime = Time.time;
        }
        
        /// <summary>
        /// Force processes immediately, regardless of interval.
        /// </summary>
        protected void ForceProcess()
        {
            IntervalUpdate();
            _lastProcessTime = Time.time;
            _isFirstProcess = false;
        }
        
        /// <summary>
        /// Gets the time remaining until the next interval processing.
        /// </summary>
        /// <returns>Time in seconds until next processing.</returns>
        protected float GetTimeUntilNextProcess()
        {
            return Mathf.Max(0, intervalSeconds - (Time.time - _lastProcessTime));
        }
        
        /// <summary>
        /// Gets the progress toward the next interval (0-1 range).
        /// </summary>
        /// <returns>A value from 0 to 1 representing progress toward next interval.</returns>
        protected float GetIntervalProgress()
        {
            return Mathf.Clamp01((Time.time - _lastProcessTime) / intervalSeconds);
        }
        
        /// <summary>
        /// Called when the behavior is enabled.
        /// </summary>
        protected override void OnEnable()
        {
            _isFirstProcess = true;
            _lastProcessTime = 0f;
        }
        
        /// <summary>
        /// Called when the behavior is disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            // Can be overridden by derived classes
        }
    }
}