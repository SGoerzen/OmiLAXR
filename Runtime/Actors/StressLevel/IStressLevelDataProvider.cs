/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

namespace OmiLAXR.Actors.StressLevel
{
    /// <summary>
    /// Abstract base class defining the contract for stress level data sources in the OmiLAXR system.
    /// This class serves as the foundation for all components that can contribute stress level measurements
    /// from various physiological, behavioral, or environmental sources.
    /// 
    /// Implementations of this class represent different stress indicators such as:
    /// - Physiological sensors (heart rate, skin conductance, facial expressions)
    /// - Behavioral analysis (interaction patterns, movement data, voice stress)
    /// - Environmental factors (noise levels, lighting conditions, task complexity)
    /// - Cognitive load indicators (reaction times, error rates, attention metrics)
    /// 
    /// The provider system allows for a modular, extensible approach to stress detection
    /// where multiple data sources can be combined using weighted averaging algorithms
    /// to produce more accurate and robust stress level assessments.
    /// </summary>
    public interface IStressLevelDataProvider
    {
        /// <summary>
        /// Unique identifier for this specific stress level data provider.
        /// Used for logging, debugging, configuration, and system identification.
        /// 
        /// This name should be descriptive and unique among all stress level providers
        /// in the system to avoid conflicts and ensure proper identification in logs
        /// and configuration systems.
        /// 
        /// Examples:
        /// - "HeartRate" for heart rate-based stress detection
        /// - "FacialExpression" for facial analysis-based stress detection
        /// - "SkinConductance" for galvanic skin response measurements
        /// - "VoiceStress" for vocal stress analysis
        /// </summary>
        /// <returns>A descriptive string identifier for this provider</returns>
        public string Name { get; }

        /// <summary>
        /// Indicates whether this data provider is currently active and providing valid measurements.
        /// By default, returns the enabled state of the Unity component, but can be overridden
        /// to implement more sophisticated activation logic.
        /// 
        /// Providers might be inactive due to:
        /// - Hardware sensor disconnection or failure
        /// - Insufficient environmental conditions (e.g., poor lighting for facial analysis)
        /// - Calibration requirements not being met
        /// - User privacy settings or consent restrictions
        /// - Resource constraints or performance optimization
        /// 
        /// The stress level estimation system uses this flag to exclude inactive providers
        /// from the weighted average calculation, ensuring only reliable data sources
        /// contribute to the final stress assessment.
        /// </summary>
        /// <returns>True if the provider is active and can provide reliable stress measurements</returns>
        public bool IsActive { get; }

        /// <summary>
        /// Calculates and returns the current stress level as measured by this specific provider.
        /// This is the core method that each provider implementation must define to contribute
        /// to the overall stress level assessment system.
        /// 
        /// The returned value follows a standardized scale:
        /// - 0.0f: Completely relaxed state, no stress indicators detected
        /// - 0.2f: Minimal stress, slight elevation from baseline
        /// - 0.5f: Moderate stress levels, noticeable physiological/behavioral changes
        /// - 0.8f: High stress levels, significant stress response indicators
        /// - 1.0f: Maximum stress levels, extreme stress response detected
        /// 
        /// Null return values indicate that the provider cannot currently determine
        /// a reliable stress level measurement. This might occur due to:
        /// - Sensor calibration in progress
        /// - Insufficient data points for reliable calculation
        /// - Environmental conditions preventing accurate measurement
        /// - Hardware malfunction or data corruption
        /// 
        /// Implementations should ensure proper normalization of their raw data
        /// to fit within the [0, 1] range using appropriate scaling algorithms
        /// based on the specific physiological or behavioral metrics being measured.
        /// </summary>
        /// <returns>
        /// Nullable float representing normalized stress level:
        /// - null: Unable to determine stress level (sensor issues, calibration, etc.)
        /// - 0.0f: No stress detected, completely relaxed state
        /// - 1.0f: Maximum stress detected, extreme stress response
        /// - Values between 0.0f and 1.0f: Proportional stress levels
        /// </returns>
        public float? GetStressLevel();

        /// <summary>
        /// Weighting factor that determines this provider's influence in the overall stress calculation.
        /// Higher weights indicate more reliable, accurate, or important stress indicators.
        /// 
        /// The weight system allows the stress estimation algorithm to prioritize certain
        /// data sources over others based on their reliability, accuracy, or clinical relevance.
        /// 
        /// Typical weighting considerations:
        /// - Physiological accuracy: Heart rate (1.0), Skin conductance (0.8), Movement (0.6)
        /// - Measurement reliability: Direct sensors (1.0), Derived metrics (0.7), Heuristics (0.5)
        /// - Individual differences: Personalized weights based on user calibration
        /// - Contextual relevance: Task-specific weighting adjustments
        /// - Scientific validation: Evidence-based weighting from stress research
        /// 
        /// Common weight ranges:
        /// - 0.5f: Low confidence indicators or experimental measurements
        /// - 1.0f: Standard baseline weight for established stress indicators
        /// - 1.5f: High confidence indicators with strong scientific backing
        /// - 2.0f: Critical indicators that should dominate the stress calculation
        /// 
        /// The final stress level is calculated using weighted average:
        /// final_stress = sum(stress_value * weight) / sum(weights)
        /// </summary>
        /// <returns>
        /// Float value representing the confidence or importance weight:
        /// - Values less than 1.0f reduce this provider's influence
        /// - Value of 1.0f represents standard baseline influence
        /// - Values greater than 1.0f increase this provider's influence
        /// </returns>
        public float Weight { get; }
    }
}