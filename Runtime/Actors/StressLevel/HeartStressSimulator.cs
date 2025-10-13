/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/

using System.ComponentModel;
using OmiLAXR.Actors.HeartRate;
using UnityEngine;
using Random = System.Random;

namespace OmiLAXR.Actors.StressLevel
{
    /// <summary>
    /// Simulates cyclical stress patterns through heart rate changes.
    /// Alternates between high-stress (120-150 BPM) and recovery phases (60-75 BPM).
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("OmiLAXR / Actor Data / Heart Stress Simulator"),
     Description("Simulates heart rate to trigger and reduce stress levels.")]
    public sealed class HeartStressSimulator : HeartRateProvider
    {
        /// <summary>
        /// Random generator for heart rate values within defined ranges
        /// </summary>
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Time accumulator for 1-second update intervals
        /// </summary>
        private float _elapsedTime;
        
        /// <summary>
        /// Timer tracking current stress phase duration
        /// </summary>
        private float _stressTimer;

        /// <summary>
        /// Current simulation phase: true for stress, false for recovery
        /// </summary>
        private bool _isStressPhase = true;
        
        /// <summary>
        /// Prevents multiple loop scheduling in recovery phase
        /// </summary>
        private bool _hasTriggered;

        /// <summary>
        /// How long (seconds) the high-stress simulation lasts
        /// </summary>
        [Tooltip("Duration (in seconds) of high-stress simulation.")]
        public float stressDuration = 10f;

        /// <summary>
        /// Whether to continuously cycle between stress and recovery phases
        /// </summary>
        [Tooltip("If true, the stress simulation will repeat in cycles.")]
        public bool loopStress;
        
        /// <summary>
        /// Updates heart rate every second, cycling between stress and recovery phases
        /// </summary>
        private void FixedUpdate()
        {
            // Accumulate time for 1-second intervals
            _elapsedTime += Time.fixedDeltaTime;

            if (_elapsedTime < 1.0f)
                return;

            if (_isStressPhase)
            {
                // Stress phase: elevated heart rate (120-150 BPM)
                SetHeartRate(_random.Next(120, 150));
                _stressTimer += _elapsedTime;

                // End stress phase after specified duration
                if (_stressTimer >= stressDuration)
                {
                    _isStressPhase = false;
                    _stressTimer = 0f;
                }
            }
            else
            {
                // Recovery phase: normal resting heart rate (60-75 BPM)
                SetHeartRate(_random.Next(60, 75));

                // Schedule next stress cycle if looping enabled
                if (loopStress && !_hasTriggered)
                {
                    _hasTriggered = true;
                    Invoke(nameof(StartNextStressPhase), 5f); // 5-second recovery period
                }
            }

            _elapsedTime = 0f;
        }

        /// <summary>
        /// Begins a new stress phase cycle
        /// </summary>
        private void StartNextStressPhase()
        {
            _isStressPhase = true;
            _hasTriggered = false;
        }
    }
}