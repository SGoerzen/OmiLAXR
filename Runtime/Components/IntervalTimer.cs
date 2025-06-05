using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Utility class for executing actions at fixed time intervals using Unity coroutines.
    /// Provides functionality to start, stop, and control timing of periodic callback execution.
    /// </summary>
    public class IntervalTimer
    {
        /// <summary>
        /// Reference to the MonoBehaviour that will own and execute the coroutine.
        /// </summary>
        private readonly MonoBehaviour _owner;
        
        /// <summary>
        /// Reference to the running coroutine. Null when the timer is not active.
        /// </summary>
        private Coroutine _coroutine;
        
        /// <summary>
        /// The time interval in seconds between callback executions.
        /// </summary>
        private readonly float _intervalSeconds;
        
        /// <summary>
        /// The action/callback to be executed at each interval.
        /// </summary>
        private readonly Action _callback;
        
        /// <summary>
        /// Initializes a new instance of the IntervalTimer class.
        /// </summary>
        /// <param name="owner">The MonoBehaviour that will own and execute the coroutine.</param>
        /// <param name="intervalSeconds">Time interval in seconds between callback executions.</param>
        /// <param name="callback">The action to be executed at each interval.</param>
        public IntervalTimer(MonoBehaviour owner, float intervalSeconds, Action callback)
        {
            _owner = owner;
            _intervalSeconds = intervalSeconds;
            _callback = callback;
        }

        /// <summary>
        /// Starts the interval timer. If already running, stops the current timer first.
        /// </summary>
        /// <param name="immediate">If true, executes the callback immediately before the first interval. Default is true.</param>
        public void Start(bool immediate = true)
        {
            Stop();
            _coroutine = _owner.StartCoroutine(Run(immediate));
        }

        /// <summary>
        /// Stops the interval timer if it's currently running.
        /// </summary>
        public void Stop()
        {
            if (_coroutine != null)
            {
                _owner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        /// <summary>
        /// Coroutine that handles the execution of the callback at specified intervals.
        /// </summary>
        /// <param name="immediate">If true, executes the callback immediately before waiting for the first interval.</param>
        /// <returns>IEnumerator required for coroutine functionality.</returns>
        private IEnumerator Run(bool immediate)
        {
            // Execute immediately if specified
            if (immediate)
            {
                _callback?.Invoke();
            }

            // Infinite loop that waits for the interval and then invokes the callback
            while (true)
            {
                yield return new WaitForSeconds(_intervalSeconds);
                _callback?.Invoke();
            }
        }
    }
}