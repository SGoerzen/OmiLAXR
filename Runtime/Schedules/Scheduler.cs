using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR.Schedules
{
    /// <summary>
    /// Abstract base class for all schedulers in the OmiLAXR system.
    /// Provides common functionality for starting, stopping, and managing scheduled operations.
    /// Requires derived classes to implement the core timing logic through the Run method.
    /// </summary>
    public abstract class Scheduler
    {
        [Serializable]
        public abstract class Settings
        {
            public bool isActive = true;
            public bool tickImmediate = true;
            public bool runOnStart = true;
        }
        /// <summary>
        /// Reference to the MonoBehaviour that will own and execute the coroutine.
        /// This is necessary because only MonoBehaviours can start coroutines in Unity.
        /// </summary>
        protected readonly MonoBehaviour Owner;
        
        /// <summary>
        /// Reference to the running coroutine. Null when the scheduler is not active.
        /// Used to keep track of and stop the currently running coroutine when needed.
        /// </summary>
        protected Coroutine Coroutine;

        /// <summary>
        /// The handler that receives scheduling events (start, tick, end).
        /// Implements IScheduleHandler to provide a consistent interface for all scheduler types.
        /// </summary>
        protected readonly Action OnTick;
        protected readonly Action OnTickStart;
        protected readonly Action OnTickEnd;

        private Settings _settings;

        /// <summary>
        /// Initializes a new instance of the Scheduler class.
        /// </summary>
        /// <param name="owner">The MonoBehaviour that will own and execute the coroutine.</param>
        /// <param name="onTickStart"></param>
        /// <param name="onTickEnd"></param>
        /// <param name="onTick"></param>
        protected Scheduler(MonoBehaviour owner, Settings settings, Action onTick, Action onTickStart = null, Action onTickEnd = null)
        {
            Owner = owner;
            OnTick = onTick;
            OnTickStart = onTickStart;
            OnTickEnd = onTickEnd;
            _settings = settings;
        }
        
        /// <summary>
        /// Starts the scheduler. If already running, stops the current scheduler first.
        /// Calls the handler's OnTickStart method and initiates the coroutine.
        /// </summary>
        /// <param name="immediate">If true, executes the callback immediately before the first scheduled event. Default is true.</param>
        public void Start()
        {
            if (!_settings.runOnStart)
                return;
            _settings.isActive = true;
            Stop();
            OnTickStart?.Invoke();
            Coroutine = Owner.StartCoroutine(Run(_settings.tickImmediate));
        }
        
        /// <summary>
        /// Stops the scheduler if it's currently running.
        /// Calls the handler's OnTickEnd method and cleans up the coroutine reference.
        /// </summary>
        public void Stop()
        {
            OnTickEnd?.Invoke();
            if (Coroutine != null)
            {
                Owner.StopCoroutine(Coroutine);
                Coroutine = null;
            }
        }

        /// <summary>
        /// Private coroutine wrapper that handles the immediate execution option
        /// before delegating to the derived class's Run implementation.
        /// </summary>
        /// <param name="immediate">If true, executes the callback immediately before waiting for the first scheduled event.</param>
        /// <returns>IEnumerator required for coroutine functionality.</returns>
        private IEnumerator Run(bool immediate)
        {
            // Execute immediately if specified
            if (immediate)
            {
                OnTick?.Invoke();
            }

            yield return Run();
        }

        /// <summary>
        /// Abstract method that derived scheduler classes must implement.
        /// Defines the core timing logic for when the handler's OnTick method is called.
        /// </summary>
        /// <returns>An IEnumerator for Unity's coroutine system.</returns>
        protected abstract IEnumerator Run();
    }
}