/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.StressLevel
{
    /// <summary>
    /// Computes overall stress level by combining data from multiple stress providers.
    /// Uses weighted averaging to calculate final stress assessment.
    /// </summary>
    [AddComponentMenu("OmiLAXR / Actor Data / Stress Level Estimator")]
    [DisallowMultipleComponent]
    [Description("Sums stress levels of all providers and computes weighted average.")]
    public class StressLevelEstimator : StressLevelProvider
    {
        /// <summary>
        /// Time accumulator for 1-second update intervals
        /// </summary>
        private float _elapsedTime;
        
        /// <summary>
        /// Calculates weighted average stress level from all active providers.
        /// Returns null if no providers have valid data.
        /// </summary>
        /// <returns>Combined stress level (0-1) or null if no valid data</returns>
        public float? ComputeStressLevel()
        {
            var totalWeight = 0f;
            var weightedSum = 0f;

            // Iterate through all registered stress data providers
            foreach (var provider in providers)
            {
                // Skip inactive providers
                if (!provider.IsActive) continue;
                
                // Get stress level from current provider
                var level = provider.GetStressLevel();
                if (level.HasValue)
                {
                    // Add weighted contribution to sum
                    weightedSum += level.Value * provider.Weight;
                    totalWeight += provider.Weight;
                }
            }

            // Return weighted average, or null if no valid data
            return totalWeight > 0.0f ? weightedSum / totalWeight : (float?)null;
        }
        
        /// <summary>
        /// Updates stress level calculation every second using FixedUpdate for consistent timing
        /// </summary>
        private void FixedUpdate()
        {
            // Accumulate time for 1-second intervals
            _elapsedTime += Time.fixedDeltaTime;

            if (_elapsedTime < 1.0f)
                return;

            // Compute new stress level, defaulting to 0 if no data available
            var stressLevel = ComputeStressLevel() ?? 0f;
            SetStressLevel(stressLevel);
            
            // Reset timer for next update cycle
            _elapsedTime = 0f;
        }
    }
}