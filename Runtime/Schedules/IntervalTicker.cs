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
    /// Executes actions at fixed time intervals using Unity coroutines.
    /// Interval and tick count can be set as asset fields.
    /// </summary>
    [CreateAssetMenu(
        fileName = "IntervalTicker",
        menuName = "OmiLAXR/Scheduler/IntervalTicker",
        order = 0)]
    public sealed class IntervalTicker : Scheduler
    {
        [Header("Time in seconds in what interval the action is triggered."), Min(0.01f)]
        public float intervalSeconds = 1.0f;

        [Header("How many ticks the action is triggered for. -1 = infinite.")]
        public int tTLTicks = -1;

        private int _tickCount;

        [NonSerialized] private WaitForSeconds _wait;
        [NonSerialized] private float _cachedInterval = -1f;

        /// <summary>
        /// Additional initialization (besides Scheduler.Init).
        /// </summary>
        public override void Init(
            MonoBehaviour owner,
            Action onTick,
            Action onTickStart = null,
            Action onTickEnd = null,
            bool runImmediate = false,
            bool resetHandlers = true)
        {
            base.Init(owner, onTick, onTickStart, onTickEnd, runImmediate, resetHandlers);
        }

        protected override void TriggerOnTickStart()
        {
            _tickCount = 0;
            base.TriggerOnTickStart();
        }

        protected override void TriggerOnTick()
        {
            base.TriggerOnTick();
            _tickCount++;

            if (tTLTicks > -1 && _tickCount >= tTLTicks)
            {
                DebugLog.OmiLAXR.Warning("IntervalTicker: Ticks reached limit. Stopping.");
                Stop();
            }
        }

        /// <summary>
        /// Implements the abstract Run method from the Scheduler base class.
        /// Repeatedly waits for the interval and then invokes the tick action.
        /// </summary>
        protected override IEnumerator Run()
        {
            while (isActive)
            {
                if (!Mathf.Approximately(_cachedInterval, intervalSeconds) || _wait == null)
                {
                    _cachedInterval = intervalSeconds;
                    _wait = new WaitForSeconds(_cachedInterval);
                }

                yield return _wait;

                if (!isActive)
                    yield break;

                TriggerOnTick();
            }
        }

        /// <summary>
        /// Static factory for programmatic creation.
        /// </summary>
        public static IntervalTicker Create(
            MonoBehaviour owner,
            float interval,
            Action onTick,
            int tTLTicks = -1,
            Action onTickStart = null,
            Action onTickEnd = null,
            bool runImmediate = false)
        {
            var ticker = CreateInstance<IntervalTicker>();
            ticker.intervalSeconds = interval;
            ticker.tTLTicks = tTLTicks;
            ticker.Init(owner, onTick, onTickStart, onTickEnd, runImmediate, resetHandlers: true);
            return ticker;
        }
    }
}