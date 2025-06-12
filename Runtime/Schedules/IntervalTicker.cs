using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR.Schedules
{
    /// <summary>
    /// Utility class for executing actions at fixed time intervals using Unity coroutines.
    /// Provides functionality to start, stop, and control timing of periodic callback execution.
    /// Implements the Scheduler abstract class to provide interval-based scheduling.
    /// </summary>
    public class IntervalTicker : Scheduler
    {
        [Serializable]
        public new class Settings : Scheduler.Settings
        {
            [Header("Time in seconds in what interval the action is triggered."), Min(0.01f)]
            public float intervalSeconds = 1.0f;
            [Header("How many ticks the action is triggered for. -1 = infinite.")]
            public int tTLTicks = -1;
        }

        private readonly Settings _settings;
        
        private int _tickCount;

        /// <summary>
        /// Initializes a new instance of the IntervalTimer class.
        /// </summary>
        /// <param name="owner">The MonoBehaviour that will own and execute the coroutine.</param>
        /// <param name="settings">Settings that define the interval parameters.</param>
        /// <param name="onTick"></param>
        /// <param name="onTickStart"></param>
        /// <param name="onTickEnd"></param>
        public IntervalTicker(MonoBehaviour owner, Settings settings, Action onTick, Action onTickStart = null, Action onTickEnd = null)
        : base(owner, settings, onTick, onTickStart, onTickEnd)
        {
            _settings = settings;
            OnTickStart += () => _tickCount = 0;
        }
        
        protected override void TriggerOnTick()
        {
            base.TriggerOnTick();
            // Increment the counter (safely)
            _tickCount++;
        }

        /// <summary>
        /// Implements the abstract Run method from the Scheduler base class.
        /// Creates an infinite coroutine that waits for the specified interval
        /// before triggering the handler's OnTick method repeatedly.
        /// </summary>
        /// <returns>An IEnumerator for Unity's coroutine system.</returns>
        protected override IEnumerator Run()
        {
            // Infinite loop that waits for the interval and then invokes the callback
            while (_settings.isActive)
            {
                if (_settings.tTLTicks > -1 && _tickCount >= _settings.tTLTicks)
                {
                    DebugLog.OmiLAXR.Warning("IntervalTicker: Ticks reached limit. Stopping.");
                    Stop();
                    yield break;
                }
    
                yield return new WaitForSeconds(_settings.intervalSeconds);
                TriggerOnTick();
            }
        }
    }
}