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
    /// Abstract base class for all schedulers in the OmiLAXR system.
    /// Provides common functionality for starting, stopping, and managing scheduled operations.
    /// Requires derived classes to implement the core timing logic through the Run method.
    /// </summary>
    public abstract class Scheduler
    {
        [Serializable]
        public abstract class Settings
        {
            [Tooltip("Shows that the Schulder is active.")]
            public bool isActive = true;
            [Tooltip("Don't wait for first tick.")]
            public bool tickImmediate = true;
            [Tooltip("Autostart Scheduler after creation.")]
            public bool runImmediate = true;
        }
        /// <summary>
        /// Reference to the MonoBehaviour that will own and execute the coroutine.
        /// This is necessary because only MonoBehaviours can start coroutines in Unity.
        /// </summary>
        private readonly MonoBehaviour _owner;
        
        /// <summary>
        /// Reference to the running coroutine. Null when the scheduler is not active.
        /// Used to keep track of and stop the currently running coroutine when needed.
        /// </summary>
        private Coroutine _coroutine;

        /// <summary>
        /// The handler that receives scheduling events (start, tick, end).
        /// Implements IScheduleHandler to provide a consistent interface for all scheduler types.
        /// </summary>
        public event Action OnTick;
        public event Action OnTickStart;
        public event Action OnTickEnd;

        private readonly Settings _settings;

        private bool _isRunning;

        /// <summary>
        /// Initializes a new instance of the Scheduler class.
        /// </summary>
        /// <param name="owner">The MonoBehaviour that will own and execute the coroutine.</param>
        /// <param name="onTickStart"></param>
        /// <param name="onTickEnd"></param>
        /// <param name="settings"></param>
        /// <param name="onTick"></param>
        protected Scheduler(MonoBehaviour owner, Settings settings, Action onTick, Action onTickStart = null, Action onTickEnd = null)
        {
            _owner = owner;
            OnTick += onTick;
            OnTickStart += onTickStart;
            OnTickEnd += onTickEnd;
            _settings = settings;
        }

        protected virtual void TriggerOnTick() => OnTick?.Invoke();
        protected virtual void TriggerOnTickStart() => OnTickStart?.Invoke();
        protected virtual void TriggerOnTickEnd() => OnTickEnd?.Invoke();
        
        /// <summary>
        /// Starts the scheduler. If already running, stops the current scheduler first.
        /// Calls the handler's OnTickStart method and initiates the coroutine.
        /// </summary>
        public void Start()
        {
            if (_isRunning)
                return;
            _settings.isActive = true;
            OnTickStart?.Invoke();
            _coroutine = _owner.StartCoroutine(Run(_settings.tickImmediate));
            _isRunning = true;
        }
        
        /// <summary>
        /// Stops the scheduler if it's currently running.
        /// Calls the handler's OnTickEnd method and cleans up the coroutine reference.
        /// </summary>
        public void Stop()
        {
            if (!_isRunning)
                return;
            OnTickEnd?.Invoke();
            _settings.isActive = false;
            if (_coroutine != null)
            {
                _owner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _isRunning = false;
        }

        /// <summary>
        /// Private coroutine wrapper that handles the immediate execution option
        /// before delegating to the derived class's Run implementation.
        /// </summary>
        /// <param name="immediate">If true, executes the callback immediately before waiting for the first scheduled event.</param>
        /// <returns>IEnumerator required for coroutine functionality.</returns>
        private IEnumerator Run(bool immediate)
        { 
            if (immediate)
                TriggerOnTick();
            
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