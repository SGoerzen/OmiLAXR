/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Actors.HeartRate;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Tracking behavior specifically designed to monitor heart rate data at regular intervals.
    /// Requires a HeartRateProvider component in the parent pipeline to function properly.
    /// Automatically disables itself if no valid provider is found.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Heart Rate Tracking Behaviour")]
    [Description("Tracks the heart rate of an actor.")]
    public class HeartRateTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Time interval in seconds between heart rate measurements.
        /// Determines how frequently the OnHeartBeat event is triggered.
        /// </summary>
        public float intervalSeconds = 1.0f;
        
        /// <summary>
        /// Event triggered at regular intervals with the current heart rate value.
        /// Provides heart rate as an integer value (beats per minute).
        /// </summary>
        public readonly TrackingBehaviourEvent<int> OnHeartBeat = new TrackingBehaviourEvent<int>();
        
        /// <summary>
        /// Initializes the heart rate tracking by locating the provider and setting up the interval timer.
        /// Validates that a HeartRateProvider exists and is enabled before starting tracking.
        /// </summary>
        private void Start()
        {
            // Search for HeartRateProvider in parent hierarchy
            var provider = GetComponentInParent<HeartRateProvider>();
            if (provider == null || !provider.enabled)
            {
                // No valid provider found - disable this component
                enabled = false;
                DebugLog.OmiLAXR.Warning($"Cannot find any <HeartRateProvider> in parent pipeline '{Pipeline.name}'. The Heart Rate Tracking Behaviour was disabled.");
                return;
            }
            
            // Set up interval-based heart rate monitoring
            SetInterval(() =>
            {
                // Get current heart rate from provider and trigger event
                OnHeartBeat?.Invoke(this, provider.GetHeartRate());
            }, intervalSeconds);
        }
    }
}