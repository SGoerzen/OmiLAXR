/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR.Schedules
{
    /// <summary>
    /// A scheduler that executes a single delayed action after a specified timeout.
    /// Useful for deferring execution rather than repeating it periodically.
    /// </summary>
    [CreateAssetMenu(
        fileName = "TimeoutTicker",
        menuName = "OmiLAXR/Scheduler/TimeoutTicker",
        order = 2)]
    public sealed class TimeoutTicker : Scheduler
    {
        [Tooltip("Time in seconds before the action is triggered."), Min(0.01f)]
        public float timeoutSeconds = 1.0f;

        /// <summary>
        /// Runtime initialization for the TimeoutTicker.
        /// </summary>
        public void Init(
            MonoBehaviour owner,
            float timeoutSeconds,
            Action onTick,
            Action onTickStart = null,
            Action onTickEnd = null,
            bool runImmediate = false)
        {
            this.timeoutSeconds = timeoutSeconds;
            base.Init(owner, onTick, onTickStart, onTickEnd, runImmediate);
        }

        /// <summary>
        /// Executes the scheduled action once after the configured timeout.
        /// </summary>
        protected override IEnumerator Run()
        {
            if (!isActive)
                yield break;

            TriggerOnTickStart();

            yield return new WaitForSeconds(timeoutSeconds);

            if (isActive)
                TriggerOnTick();

            TriggerOnTickEnd();
        }

        /// <summary>
        /// Factory for programmatic creation.
        /// </summary>
        public static TimeoutTicker Create(
            MonoBehaviour owner,
            float timeoutSeconds,
            Action onTick,
            Action onTickStart = null,
            Action onTickEnd = null,
            bool runImmediate = false)
        {
            var ticker = CreateInstance<TimeoutTicker>();
            ticker.Init(owner, timeoutSeconds, onTick, onTickStart, onTickEnd, runImmediate);
            return ticker;
        }
    }
}
