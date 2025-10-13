/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*
* HeartRateTrackingBehaviour (commented)
* - Samples BPM at a fixed interval from a HeartRateProvider
* - Emits raw BPM via OnHeartBeat
* - Keeps a rolling time window of samples and computes:
*     mean/variance/stddev (BPM)
*     HRV estimates (from BPM→IBI approximation): SDNN, RMSSD, pNN50
*
* IMPORTANT: True HRV should be computed from beat-to-beat RR intervals.
*            Here we approximate IBI as 60000/BPM for convenience.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using OmiLAXR.Actors.HeartRate;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Monitors heart rate (BPM) at regular intervals and computes rolling-window statistics.
    /// Emits both raw BPM pulses and aggregate metrics (BPM stats and coarse HRV estimates).
    ///
    /// <remarks>
    /// PERFORMANCE:
    /// - Uses a single List<Sample> buffer and prunes old entries each sample.
    /// - Welford's algorithm provides numerically stable mean/variance in one pass.
    /// - HRV metrics are approximations using IBI = 60000/BPM; for research-grade HRV,
    ///   prefer true RR interval streams.
    /// </remarks>
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Heart Rate Tracking Behaviour")]
    [Description(
        "Tracks heart rate (BPM) and emits:\n" +
        "- OnHeartBeat(bpm): current BPM each interval\n" +
        "- OnStatsUpdated(stats): windowed stats incl. meanBpm, varBpm, stdBpm, minBpm, maxBpm\n" +
        "  and HRV estimates (approx.): meanIbiMs, sdnnMs, rmssdMs, pnn50"
    )]
    public class HeartRateTrackingBehaviour : TrackingBehaviour
    {
        // ──────────────────────────────────────────────────────────────────────
        // Inspector configuration
        // ──────────────────────────────────────────────────────────────────────

        [Header("Sampling")]
        [Tooltip("Time between heart rate samples (seconds). Choose a cadence your provider can sustain.")]
        public float intervalSeconds = 1.0f;

        [Header("Windowed Metrics")]
        [Tooltip("Compute statistics over this rolling window (seconds). Larger windows smooth more, react slower.")]
        [Range(5f, 300f)]
        public float windowSeconds = 30f;

        [Tooltip("Emit stats only once the window contains at least this many samples.")]
        [Range(2, 1024)]
        public int minSamplesForStats = 5;

        [Tooltip("Compute HRV estimates from BPM→IBI (coarse). Disable to save compute.")]
        public bool computeHrvEstimates = true;

        // ──────────────────────────────────────────────────────────────────────
        // Events
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fires each interval with the current heart rate in beats per minute.
        /// </summary>
        public readonly TrackingBehaviourEvent<int> OnHeartBeat = new TrackingBehaviourEvent<int>();

        /// <summary>
        /// Fires when windowed statistics are (re)computed. Cadence equals sampling cadence.
        /// </summary>
        public readonly TrackingBehaviourEvent<HeartRateStats> OnStatsUpdated = new TrackingBehaviourEvent<HeartRateStats>();

        // ──────────────────────────────────────────────────────────────────────
        // Internals
        // ──────────────────────────────────────────────────────────────────────

        private HeartRateProvider _provider;

        /// <summary>
        /// Rolling sample buffer. Each entry stores Unity time and BPM at that time.
        /// </summary>
        private readonly List<Sample> _samples = new List<Sample>(256);

        /// <summary>
        /// Lightweight POD for one sample in the rolling buffer.
        /// </summary>
        private struct Sample { public float t; public int bpm; }

        /// <summary>
        /// Aggregate statistics over the current window.
        /// </summary>
        [Serializable]
        public struct HeartRateStats
        {
            // Raw BPM stats
            public int   sampleCount;     // number of samples currently in window
            public float windowSeconds;   // configured window length (for reference)
            public float meanBpm;         // arithmetic mean of BPM
            public float varianceBpm;     // unbiased sample variance of BPM
            public float stdBpm;          // sqrt(varianceBpm)
            public int   minBpm;          // min BPM in window
            public int   maxBpm;          // max BPM in window

            // HRV estimates from BPM→IBI approximation (milliseconds)
            // NOTE: These are convenient, not research-grade.
            public float meanIbiMs;       // mean inter-beat interval (ms)
            public float sdnnMs;          // std dev of NN intervals (ms)
            public float rmssdMs;         // root mean square of successive differences (ms)
            public float pnn50;           // fraction of successive IBI diffs > 50ms (0..1)

            public float computedAt;      // Time.time when stats were computed
        }

        // ──────────────────────────────────────────────────────────────────────
        // Lifecycle
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Locates the provider and starts interval sampling.
        /// Component disables itself if no provider is found/enabled.
        /// </summary>
        private void Start()
        {
            _provider = GetComponentInParent<HeartRateProvider>();
            if (_provider == null || !_provider.enabled)
            {
                enabled = false;
                DebugLog.OmiLAXR.Warning(
                    $"Cannot find any <HeartRateProvider> in parent pipeline '{Pipeline.name}'. " +
                    "Heart Rate Tracking Behaviour was disabled.");
                return;
            }

            // Schedule periodic sampling (TrackingBehaviour.SetInterval is assumed to exist)
            SetInterval(SampleOnce, intervalSeconds);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Sampling & statistics
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Called on each sampling tick:
        /// - Pulls BPM from provider
        /// - Emits OnHeartBeat
        /// - Adds to rolling window and prunes out-of-window samples
        /// - Computes & emits windowed statistics (if enough data)
        /// </summary>
        private void SampleOnce()
        {
            // Defensive clamp: avoid 0 BPM → infinite IBI; adjust as needed for your provider.
            var bpm = Mathf.Max(1, _provider.GetHeartRate());

            // 1) Emit raw pulse
            OnHeartBeat?.Invoke(this, bpm);

            // 2) Add to rolling buffer
            var now = Time.time;
            _samples.Add(new Sample { t = now, bpm = bpm });

            // 3) Prune old entries (simple linear prune; typical windows are small)
            var cutoff = now - windowSeconds;
            var firstValid = 0;
            while (firstValid < _samples.Count && _samples[firstValid].t < cutoff)
                firstValid++;
            if (firstValid > 0)
                _samples.RemoveRange(0, firstValid);

            // 4) Compute stats if window sufficiently populated
            var required = Mathf.Max(2, minSamplesForStats);
            if (_samples.Count >= required)
            {
                var stats = ComputeStats(_samples, windowSeconds, computeHrvEstimates);
                OnStatsUpdated?.Invoke(this, stats);
            }
        }

        /// <summary>
        /// Computes rolling-window statistics.
        /// Uses Welford's algorithm for numerically stable mean/variance (BPM),
        /// then derives coarse HRV metrics by approximating IBI = 60000/BPM.
        /// </summary>
        private static HeartRateStats ComputeStats(List<Sample> buf, float windowSec, bool computeHrv)
        {
            var n = buf.Count;

            // ── BPM mean/variance via Welford ─────────────────────────────────
            var mean = 0f;     // running mean
            var m2   = 0f;     // running sum of squares of differences from the current mean
            int min = int.MaxValue, max = int.MinValue;

            for (var i = 0; i < n; i++)
            {
                var x = buf[i].bpm;

                // Track extrema for quick range insight
                if (x < min) min = x;
                if (x > max) max = x;

                // Welford update
                var delta = x - mean;
                mean += delta / (i + 1);
                m2   += delta * (x - mean);
            }

            var variance = (n > 1) ? (m2 / (n - 1)) : 0f;   // unbiased sample variance
            var std      = Mathf.Sqrt(Mathf.Max(0f, variance));

            // ── HRV approximations from BPM → IBI (ms) ───────────────────────
            // NOTE: These are convenient approximations; prefer RR intervals when available.
            float meanIbi = 0f, sdnn = 0f, rmssd = 0f, pnn50 = 0f;

            if (computeHrv)
            {
                // Convert each BPM to an IBI (ms). Clamp BPM into a physiological band to avoid extremes.
                const float MIN_BPM = 20f, MAX_BPM = 240f;
                var ibi = new float[n];

                for (var i = 0; i < n; i++)
                {
                    var b = Mathf.Clamp(buf[i].bpm, MIN_BPM, MAX_BPM);
                    ibi[i] = 60000f / b; // ms per beat
                }

                // Mean & SDNN (std dev of NN intervals)
                float meanI = 0f, m2I = 0f;
                for (var i = 0; i < n; i++)
                {
                    var delta = ibi[i] - meanI;
                    meanI += delta / (i + 1);
                    m2I   += delta * (ibi[i] - meanI);
                }
                meanIbi = meanI;
                sdnn    = (n > 1) ? Mathf.Sqrt(Mathf.Max(0f, m2I / (n - 1))) : 0f;

                // RMSSD & pNN50 from successive IBI differences
                if (n > 1)
                {
                    var sumSq = 0f;
                    var over50 = 0;
                    var pairs = n - 1;

                    for (var i = 1; i < n; i++)
                    {
                        var diff  = ibi[i] - ibi[i - 1];
                        var adiff = Mathf.Abs(diff);

                        sumSq += diff * diff;
                        if (adiff > 50f) over50++; // pNN50 threshold in ms
                    }

                    rmssd = Mathf.Sqrt(sumSq / pairs);
                    pnn50 = pairs > 0 ? (float)over50 / pairs : 0f;
                }
            }

            // Pack results
            return new HeartRateStats
            {
                sampleCount   = n,
                windowSeconds = windowSec,
                meanBpm       = mean,
                varianceBpm   = variance,
                stdBpm        = std,
                minBpm        = (min == int.MaxValue ? 0 : min),
                maxBpm        = (max == int.MinValue ? 0 : max),

                meanIbiMs = meanIbi,
                sdnnMs    = sdnn,
                rmssdMs   = rmssd,
                pnn50     = pnn50,

                computedAt = Time.time
            };
        }
    }
}