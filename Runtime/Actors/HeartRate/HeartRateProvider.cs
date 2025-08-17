/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System.ComponentModel;
using OmiLAXR.Actors.StressLevel;
using UnityEngine;

namespace OmiLAXR.Actors.HeartRate
{
    /// <summary>
    /// Abstract base class for heart rate monitoring and stress level assessment.
    /// Provides a framework for different heart rate data sources (wearables, sensors, APIs)
    /// and converts heart rate measurements into normalized stress level indicators.
    /// 
    /// This class serves as a bridge between physiological heart rate data and the
    /// stress level detection system, using heart rate variability as a key stress marker.
    /// </summary>
    [Description("Monitors heart rate value provided by a Heart Rate Provider.")]
    public abstract class HeartRateProvider : StressLevelDataProvider
    {
        /// <summary>
        /// Gets the current heart rate value in beats per minute (BPM).
        /// Provides read-only access to the internally tracked heart rate measurement.
        /// This value is updated by derived classes through the SetHeartRate method.
        /// </summary>
        /// <returns>Current heart rate in BPM, or 0 if no reading is available</returns>
        public int GetHeartRate() => heartRate;

        /// <summary>
        /// Identifier name for this data provider within the stress level system.
        /// Used for logging, debugging, and distinguishing this provider from others.
        /// </summary>
        /// <returns>The string "HeartRate" as the provider identifier</returns>
        public override string Name => "HeartRate";

        /// <summary>
        /// Weighting factor for heart rate's contribution to overall stress calculation.
        /// Uses standard weight (1.0f) as heart rate is considered a baseline physiological indicator.
        /// Can be adjusted in derived classes if specific heart rate sources have different reliability.
        /// </summary>
        /// <returns>Weight factor of 1.0f for heart rate contribution</returns>
        public override float Weight => 1.0f;
        
        /// <summary>
        /// Current heart rate value in beats per minute (BPM).
        /// This field is protected and can only be modified through the SetHeartRate method.
        /// Displayed as read-only in the Unity Inspector for monitoring purposes.
        /// 
        /// Typical ranges:
        /// - Resting: 60-100 BPM (adults)
        /// - Light activity: 100-120 BPM
        /// - Moderate activity: 120-160 BPM
        /// - High stress/activity: 160+ BPM
        /// </summary>
        [ReadOnly, SerializeField]
        private int heartRate;

        /// <summary>
        /// Protected method for derived classes to update the current heart rate value.
        /// This should be called whenever new heart rate data becomes available from the
        /// underlying monitoring system (sensor, API, wearable device, etc.).
        /// 
        /// The method provides encapsulation while allowing derived classes to control
        /// when and how heart rate updates occur based on their specific data sources.
        /// </summary>
        /// <param name="hr">New heart rate value in beats per minute (BPM)</param>
        protected void SetHeartRate(int hr)
            => heartRate = hr;

        /// <summary>
        /// Calculates and returns the current stress level based on heart rate analysis.
        /// Converts the raw heart rate measurement into a normalized stress indicator
        /// using physiological principles of stress response and heart rate elevation.
        /// 
        /// The calculation considers that stress typically causes heart rate elevation
        /// above resting levels, with higher rates indicating increased stress states.
        /// </summary>
        /// <returns>
        /// Nullable float representing stress level:
        /// - null: Unable to determine stress level (no heart rate data, sensor error)
        /// - 0.0f: Normal/resting heart rate, minimal stress indication
        /// - 1.0f: Significantly elevated heart rate, high stress indication
        /// - Values between 0.0f and 1.0f indicate proportional stress levels
        /// </returns>
        public override float? GetStressLevel()
            => NormalizeHeartRate(heartRate);

        /// <summary>
        /// Static utility method that converts raw heart rate values into normalized stress indicators.
        /// Uses a simplified linear normalization algorithm based on common physiological principles.
        /// 
        /// The normalization assumes:
        /// - 60 BPM as baseline (typical resting heart rate lower bound)
        /// - 120 BPM as high stress threshold (60 BPM above baseline)
        /// - Linear relationship between heart rate elevation and stress level
        /// 
        /// This is a simplistic approach that could be enhanced with:
        /// - Individual baseline calibration
        /// - Age and fitness level adjustments
        /// - Heart rate variability analysis
        /// - Context-aware interpretation (activity vs. stress)
        /// </summary>
        /// <param name="hr">Heart rate value in beats per minute to normalize</param>
        /// <returns>
        /// Normalized stress level between 0.0f and 1.0f:
        /// - 0.0f: Heart rate at or below 60 BPM (resting)
        /// - 0.5f: Heart rate around 90 BPM (mild elevation)
        /// - 1.0f: Heart rate at or above 120 BPM (significant elevation)
        /// </returns>
        private static float NormalizeHeartRate(float hr)
        {
            // Simplistic linear normalization from resting (60 BPM) to elevated (120 BPM)
            // Formula: (current_hr - baseline) / range
            // Baseline: 60 BPM (typical resting heart rate)
            // Range: 60 BPM (from 60 to 120, covering normal to elevated stress response)
            // Clamp01 ensures the result stays within valid [0, 1] bounds
            return Mathf.Clamp01((hr - 60f) / 60f); // Simplistic normalization
        }
    }
}