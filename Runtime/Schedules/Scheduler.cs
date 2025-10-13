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
    /// Scheduler class for scheduled operations in the OmiLAXR system.
    /// Combines tick-based and event-driven scheduling.
    /// Use Init(...) for runtime initialization.
    /// </summary>
    public abstract class Scheduler : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("Scheduler Configuration")]
        [Tooltip("Shows that the Scheduler is active.")]
        public bool isActive = true;

        [Tooltip("Don't wait for first tick.")]
        public bool tickImmediate = true;

        [Tooltip("Autostart Scheduler after creation.")]
        public bool runImmediate = false;

        /// <summary>
        /// The MonoBehaviour that owns and executes the coroutine.
        /// Must be set at runtime via Init(), because ScriptableObjects cannot store scene references.
        /// </summary>
        [NonSerialized]
        public MonoBehaviour owner;

        [NonSerialized]
        private Coroutine _coroutine;

        [NonSerialized]
        private bool _isRunning;

        /// <summary>
        /// True if the scheduler is currently running.
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Raised when the scheduler is stopped.
        /// </summary>
        public event Action OnStop;

        public event Action OnTick;
        public event Action OnTickStart;
        public event Action OnTickEnd;

        /// <summary>
        /// Runtime initialization for this scheduler.
        /// Call this after CreateInstance().
        /// </summary>
        public virtual void Init(MonoBehaviour owner, Action onTick = null, Action onTickStart = null, Action onTickEnd = null, bool runImmediate = false)
        {
            this.owner = owner;
            if (onTick != null) OnTick += onTick;
            if (onTickStart != null) OnTickStart += onTickStart;
            if (onTickEnd != null) OnTickEnd += onTickEnd;
            this.runImmediate = runImmediate;

            if (runImmediate)
                Start();
        }

        public virtual void Start()
        {
            if (_isRunning || owner == null)
                return;
            isActive = true;
            TriggerOnTickStart();
            _coroutine = owner.StartCoroutine(RunInternal(tickImmediate));
            _isRunning = true;
        }

        public virtual void Stop()
        {
            if (!_isRunning || owner == null)
                return;

            TriggerOnTickEnd();
            isActive = false;

            if (_coroutine != null)
            {
                owner.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _isRunning = false;
            TriggerOnStop();
        }

        private IEnumerator RunInternal(bool immediate)
        {
            if (immediate)
                TriggerOnTick();

            yield return Run();
        }

        /// <summary>
        /// Override this to implement custom tick logic.
        /// </summary>
        protected abstract IEnumerator Run();

        protected virtual void TriggerOnTick() => OnTick?.Invoke();
        protected virtual void TriggerOnTickStart() => OnTickStart?.Invoke();
        protected virtual void TriggerOnTickEnd() => OnTickEnd?.Invoke();
        protected virtual void TriggerOnStop() => OnStop?.Invoke();

        public virtual void OnBeforeSerialize() { }

        public virtual void OnAfterDeserialize()
        {
            owner = null;
            _coroutine = null;
            _isRunning = false;
        }

        /// <summary>
        /// Creates a clone of this scheduler with a new owner assigned.
        /// Event handlers and runtime state are not copied.
        /// </summary>
        /// <typeparam name="T">The scheduler subclass type.</typeparam>
        /// <param name="newOwner">The MonoBehaviour to assign to the clone.</param>
        /// <returns>A new instance of the scheduler with the same config and new owner.</returns>
        public Scheduler Clone(MonoBehaviour newOwner)
        {
            var clone = Instantiate(this);
            clone.owner = newOwner;
            clone._isRunning = false;
            clone._coroutine = null;

            // Note: Events are not copied, as UnityEvents/Actions are not serialized.
            // If desired, caller can reassign them via `Init()`.
            return clone;
        }
    }
}
