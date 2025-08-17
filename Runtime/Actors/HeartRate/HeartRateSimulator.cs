/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System.ComponentModel;
using UnityEngine;
using Random = System.Random;

namespace OmiLAXR.Actors.HeartRate
{
    /// <summary>
    /// Concrete implementation of HeartRateProvider that simulates realistic heart rate data.
    /// Generates random heart rate values within physiological ranges for testing and development purposes.
    /// Useful for prototyping stress level detection systems when real heart rate sensors are unavailable.
    /// 
    /// This simulator provides controlled, predictable heart rate data that can be used to:
    /// - Test stress level calculation algorithms
    /// - Demonstrate heart rate monitoring functionality
    /// - Validate data pipeline integration
    /// - Debug physiological data processing systems
    /// </summary>
    [AddComponentMenu("OmiLAXR / Actor Data / Heart Rate Simulator")]
    [DisallowMultipleComponent]
    [Description("Simulates heart rate beats.")]
    public class HeartRateSimulator : HeartRateProvider
    {
        /// <summary>
        /// Random number generator instance for heart rate simulation.
        /// Uses System.Random for consistent, controllable pseudorandom behavior.
        /// Initialized once to maintain deterministic seed behavior across the simulation lifecycle.
        /// 
        /// This provides more predictable behavior than UnityEngine.Random for testing scenarios
        /// where reproducible heart rate patterns may be desired for debugging.
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Accumulator for tracking elapsed time since the last heart rate update.
        /// Used to control the update frequency of simulated heart rate measurements.
        /// Resets to zero after each heart rate update cycle to maintain consistent timing.
        /// 
        /// This timing mechanism ensures heart rate updates occur at regular intervals,
        /// simulating the discrete nature of real heart rate monitoring devices.
        /// </summary>
        private float _elapsedTime;
        
        /// <summary>
        /// Unity's fixed timestep update method for consistent heart rate simulation timing.
        /// Uses FixedUpdate instead of Update to ensure reliable timing regardless of framerate.
        /// This is crucial for physiological data simulation where consistent sampling rates matter.
        /// 
        /// The method implements a simple timer-based approach:
        /// 1. Accumulates fixed delta time each frame
        /// 2. Checks if one second has elapsed (1.0f threshold)
        /// 3. Generates new random heart rate when timer expires
        /// 4. Resets timer for next cycle
        /// 
        /// This approach simulates real-world heart rate monitors that typically
        /// provide readings at 1 Hz (once per second) intervals.
        /// </summary>
        private void FixedUpdate()
        {
            // Accumulate time using fixed delta time for consistent timing
            // fixedDeltaTime ensures updates happen at regular intervals regardless of framerate
            // This is essential for physiological data that requires consistent sampling rates
            _elapsedTime += Time.fixedDeltaTime;

            // Check if one second has elapsed since last heart rate update
            // 1.0f second interval simulates typical heart rate monitor update frequency
            // Real devices often sample at 1 Hz to balance accuracy with power consumption
            if (_elapsedTime < 1.0f)
                return;

            // Generate random heart rate within physiologically realistic range
            // Range: 50-109 BPM covers resting to moderately active heart rates
            // 50 BPM: Lower bound for healthy resting heart rate (athletic individuals)
            // 110 BPM: Upper bound representing light to moderate activity/stress
            // This range excludes extreme values to maintain realistic simulation
            SetHeartRate(_random.Next(50, 110));
            
            // Reset elapsed time counter for next update cycle
            // This ensures consistent 1-second intervals between heart rate updates
            _elapsedTime = 0f;
        }
    }
}