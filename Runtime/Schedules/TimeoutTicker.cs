using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR.Schedules
{
    /// <summary>
    /// A scheduler that executes a single delayed action after a specified timeout.
    /// Useful for deferring execution rather than repeating it periodically.
    /// </summary>
    public class TimeoutTicker : Scheduler
    {
        [Serializable]
        public new class Settings : Scheduler.Settings
        {
            [Tooltip("Time in seconds before the action is triggered."), Min(0.01f)]
            public float timeoutSeconds = 1.0f;
        }
        
        private readonly Settings _settings;

        /// <summary>
        /// Creates a TimeoutTicker that triggers a one-time action after a delay.
        /// </summary>
        /// <param name="owner">The MonoBehaviour responsible for running the coroutine.</param>
        /// <param name="settings">Configuration for the timeout, including delay and activation flag.</param>
        /// <param name="onTick">The callback to invoke after the timeout.</param>
        /// <param name="onTickStart">Optional callback before waiting starts.</param>
        /// <param name="onTickEnd">Optional callback after execution completes.</param>
        public TimeoutTicker(MonoBehaviour owner, Settings settings, Action onTick, Action onTickStart = null, Action onTickEnd = null)
            : base(owner, settings, onTick, onTickStart, onTickEnd)
        {
            _settings = settings;
        }

        /// <summary>
        /// Executes the scheduled action once after the configured timeout.
        /// </summary>
        /// <returns>An IEnumerator for Unity's coroutine system.</returns>
        protected override IEnumerator Run()
        {
            if (!_settings.isActive)
                yield break;

            TriggerOnTickStart();

            yield return new WaitForSeconds(_settings.timeoutSeconds);

            if (_settings.isActive)
                TriggerOnTick();

            TriggerOnTickEnd();
        }
    }
}
