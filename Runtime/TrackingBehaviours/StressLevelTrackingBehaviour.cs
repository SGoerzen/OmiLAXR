/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using OmiLAXR.StressLevel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Monitors stress level changes and triggers events based on configurable thresholds.
    /// Tracks stress level updates, increases, decreases, and stress/relaxation states.
    /// </summary>
    public class StressLevelTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Minimum change required to trigger stress level change events.
        /// </summary>
        [Range(0f, 1f)]
        public float changeThreshold = 0.05f;

        /// <summary>
        /// Event triggered whenever the stress level value changes.
        /// </summary>
        public readonly TrackingBehaviourEvent<float, float> OnStressLevelUpdated = new TrackingBehaviourEvent<float, float>();
        
        /// <summary>
        /// Event triggered when stress level increases beyond the change threshold.
        /// </summary>
        public readonly TrackingBehaviourEvent<float, float> OnStressLevelIncreased = new TrackingBehaviourEvent<float, float>();
        
        /// <summary>
        /// Event triggered when stress level decreases beyond the change threshold.
        /// </summary>
        public readonly TrackingBehaviourEvent<float, float> OnStressLevelDecreased = new TrackingBehaviourEvent<float, float>();
        
        /// <summary>
        /// Event triggered when stress level crosses into high stress range (>0.75).
        /// </summary>
        public readonly TrackingBehaviourEvent<float> OnStressDetected = new TrackingBehaviourEvent<float>();
        
        /// <summary>
        /// Event triggered when stress level drops to relaxed range (<0.4).
        /// </summary>
        public readonly TrackingBehaviourEvent<float> OnRelaxationDetected = new TrackingBehaviourEvent<float>();

        /// <summary>
        /// Previously recorded stress level for change detection.
        /// </summary>
        private float _lastLevel;
        
        /// <summary>
        /// Flag tracking whether user is currently in stressed state.
        /// </summary>
        private bool _isStressed;

        /// <summary>
        /// Reference to the stress level data provider component.
        /// </summary>
        private StressLevelProvider _provider;

        /// <summary>
        /// Initialize stress level provider reference.
        /// </summary>
        private void Start()
        {
            _provider = GetComponent<StressLevelProvider>(); // Or inject
        }

        /// <summary>
        /// Monitor stress level changes each frame and trigger appropriate events.
        /// </summary>
        private void Update()
        {
            var current = _provider.GetStressLevel();

            // Skip processing if no significant change
            if (Math.Abs(current - _lastLevel) < 0.001f)
                return;
            // Check for significant increases or decreases
            var delta = current - _lastLevel;
            
            // Always notify of any stress level update
            OnStressLevelUpdated?.Invoke(this, current, delta);

    
            if (delta > changeThreshold)
                OnStressLevelIncreased?.Invoke(this, current, delta);
            else if (delta < -changeThreshold)
                OnStressLevelDecreased?.Invoke(this, current, delta);

            // Check for stress state transitions
            if (current > 0.75f && !_isStressed)
            {
                // Entered high stress state
                _isStressed = true;
                OnStressDetected?.Invoke(this, current);
            }
            else if (current < 0.4f && _isStressed)
            {
                // Returned to relaxed state
                _isStressed = false;
                OnRelaxationDetected?.Invoke(this, current);
            }

            _lastLevel = current;
        }
    }
}