/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using OmiLAXR.TrackingBehaviours.Learner.HeadTracking;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Tracks head movements and detects nodding and shaking gestures using camera position data.
    /// Uses amplitude analysis and gesture pattern recognition for reliable head gesture detection.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Head Tracking Behaviour"), 
     Description("Detects nodding and shaking of the head by using <Camera>.")]
    public class HeadTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Contains comprehensive data about a detected head gesture event.
        /// </summary>
        public struct HeadTrackingBehaviourArgs
        {
            /// <summary>
            /// Duration of the gesture from start to completion.
            /// </summary>
            public readonly TimeSpan TimeSpan;
            
            /// <summary>
            /// Number of gesture cycles detected (e.g., number of nods or shakes).
            /// </summary>
            public readonly int NumberOfGesture;
            
            /// <summary>
            /// Raw HMD position data collected during the gesture.
            /// </summary>
            public readonly HmdTimedPosition[] HmdPositions;
            
            /// <summary>
            /// Modified position data used for shake detection (Y coordinate zeroed).
            /// </summary>
            public readonly HmdTimedPosition[] HmdPositionsManipulated;
            
            /// <summary>
            /// Distance calculations from origin for shake analysis.
            /// </summary>
            public readonly float[] DistanceHmdPositionsManipulated;
            
            /// <summary>
            /// Minimum distance threshold used for gesture detection.
            /// </summary>
            public readonly double MinDistanceOfGesture;

            /// <summary>
            /// Initializes head tracking event arguments with gesture data.
            /// </summary>
            /// <param name="hmdPositions">Raw position data array</param>
            /// <param name="hmdPositionsManipulated">Modified position data for shake detection</param>
            /// <param name="distanceHmdPositionsManipulated">Distance calculations array</param>
            /// <param name="minDistanceOfGesture">Minimum detection threshold</param>
            /// <param name="timesSpan">Total gesture duration</param>
            /// <param name="numberOfGesture">Number of detected gesture cycles</param>
            public HeadTrackingBehaviourArgs(
                HmdTimedPosition[] hmdPositions, 
                HmdTimedPosition[] hmdPositionsManipulated, 
                float[] distanceHmdPositionsManipulated,
                double minDistanceOfGesture, 
                TimeSpan timesSpan, 
                int numberOfGesture)
            {
                HmdPositions = hmdPositions;
                HmdPositionsManipulated = hmdPositionsManipulated;
                DistanceHmdPositionsManipulated = distanceHmdPositionsManipulated;
                MinDistanceOfGesture = minDistanceOfGesture;
                TimeSpan = timesSpan;
                NumberOfGesture = numberOfGesture;
            }
        }
        
        /// <summary>
        /// Event triggered when a nodding gesture is detected and completed.
        /// </summary>
        [Gesture("Head"), Action("Nod")]
        public TrackingBehaviourEvent<HeadTrackingBehaviourArgs> OnNodded =
            new TrackingBehaviourEvent<HeadTrackingBehaviourArgs>();
        
        /// <summary>
        /// Event triggered when a head shaking gesture is detected and completed.
        /// </summary>
        [Gesture("Head"), Action("Shake")]
        public TrackingBehaviourEvent<HeadTrackingBehaviourArgs> OnShook =
            new TrackingBehaviourEvent<HeadTrackingBehaviourArgs>();

        /// <summary>
        /// Enable or disable nodding gesture detection.
        /// </summary>
        public bool trackNoddingEnabled = true;
        
        /// <summary>
        /// Enable or disable head shaking gesture detection.
        /// </summary>
        public bool trackShakingEnabled = true;
        
        // Data collection arrays for position tracking
        private List<HmdTimedPosition> _hmdPositions = new List<HmdTimedPosition>();
        private List<HmdTimedPosition> _hmdPositionsManipulated = new List<HmdTimedPosition>();
        private List<float> _distanceHmdPositionsManipulated = new List<float>();
        private Vector3 _nullVector;

        // State tracking booleans for gesture recognition
        private bool _enoughSamplesCollected = false;
        private bool _trackingCurrentlyRunning = true;

        private bool _currentlyNodding = false;
        private bool _currentlyShaking = false;

        /***************
         * Tunable parameters for gesture detection algorithm
         * These values control sensitivity and accuracy of gesture recognition
         * Future optimization may be needed for different use cases
         ***************/
        
        /// <summary>
        /// Minimum Y-axis movement distance required to detect a nod gesture.
        /// </summary>
        private const double MinDistanceNodding = 0.01;
        
        /// <summary>
        /// Minimum horizontal movement distance required to detect a shake gesture.
        /// </summary>
        private const double MinDistanceShaking = 0.01;
        
        /// <summary>
        /// Minimum movement threshold to distinguish intentional gestures from normal head movement.
        /// </summary>
        private const double MinNormalHeadMovements = 0.05;
        
        /// <summary>
        /// Maximum time window for detecting continuous nodding gestures.
        /// </summary>
        private readonly TimeSpan _continuousTimeBetweenNods = new TimeSpan(0, 0, 0, 1, 0);
        
        /// <summary>
        /// Maximum time window for detecting continuous shaking gestures.
        /// </summary>
        private readonly TimeSpan _continuousTimeBetweenShakes = new TimeSpan(0, 0, 0, 1, 0);
        
        /// <summary>
        /// Minimum data collection time required before gesture detection can begin.
        /// </summary>
        private readonly TimeSpan _minTimeForGestures = new TimeSpan(0, 0, 0, 1, 0);

        /// <summary>
        /// Initialize tracking vectors and reference points.
        /// </summary>
        protected virtual void Start()
        {
            _nullVector = Vector3.zero;
        }

        /// <summary>
        /// Main update loop for head tracking - processes position data and detects gestures.
        /// </summary>
        protected virtual void Update()
        {
            // Get current HMD position data
            var addHmdPosition = HmdPosition.Instance.GetHmdPosition();
            if (Equals(addHmdPosition, default(HmdTimedPosition))) return;
            
            // Add position to raw tracking data
            _hmdPositions.Add(addHmdPosition);

            // Create modified position for shake detection (zero Y coordinate for horizontal-only analysis)
            addHmdPosition.Position.y = 0;
            _hmdPositionsManipulated.Add(addHmdPosition);

            // Calculate distance from origin for shake amplitude analysis
            _distanceHmdPositionsManipulated.Add(Vector3.Distance(_nullVector,
                _hmdPositionsManipulated[_hmdPositionsManipulated.Count - 1].Position));

            // Analyze current data for possible nod or shake gestures
            NodsOrShakes(ref _currentlyNodding, ref _currentlyShaking, ref _hmdPositions);

            switch (_currentlyNodding)
            {
                // Process detected nodding gesture
                case true when !_currentlyShaking:
                    Nodding(ref _currentlyNodding, ref _currentlyShaking);
                    break;
                // Process detected shaking gesture
                case false when _currentlyShaking:
                    Shaking(ref _currentlyNodding, ref _currentlyShaking);
                    break;
            }
        }

        /// <summary>
        /// Core gesture analysis method that determines if nodding or shaking is occurring.
        /// </summary>
        /// <param name="currentlyNodding">Reference to current nodding state</param>
        /// <param name="currentlyShaking">Reference to current shaking state</param>
        /// <param name="hmdPositions">Position data to analyze</param>
        private void NodsOrShakes(ref bool currentlyNodding, ref bool currentlyShaking,
            ref List<HmdTimedPosition> hmdPositions)
        {
            // Check if we have collected enough samples for reliable analysis
            HeadUtils.EnoughSamplesCollected(ref hmdPositions, _minTimeForGestures, ref _trackingCurrentlyRunning,
                ref _enoughSamplesCollected);

            // Exit early if insufficient data
            if (!_enoughSamplesCollected) return;
            
            // Check if movement is just normal head movement (looking around)
            if (HeadUtils.NormalHeadMovement("x", hmdPositions, MinNormalHeadMovements) &&
                HeadUtils.NormalHeadMovement("y", hmdPositions, MinNormalHeadMovements) &&
                HeadUtils.NormalHeadMovement("z", hmdPositions, MinNormalHeadMovements))
            {
                // Too much movement on all axes - likely just looking around
                currentlyNodding = false;
                currentlyShaking = false;
            }
            else
            {
                // Analyze for nodding gesture (primarily Y-axis movement)
                if (HeadUtils.NormalHeadMovement("x", hmdPositions, MinNormalHeadMovements) ||
                    HeadUtils.NormalHeadMovement("z", hmdPositions, MinNormalHeadMovements))
                {
                    // Too much X or Z movement for a pure nod
                    currentlyNodding = false;
                }
                else if (!(HeadUtils.EnoughAmplitude(hmdPositions, MinDistanceNodding, "y")))
                {
                    // Insufficient Y-axis amplitude for nodding
                    currentlyNodding = false;
                }
                else
                {
                    currentlyNodding = true;
                }

                if (!currentlyNodding)
                {
                    // Analyze for shaking gesture (horizontal movement)
                    if (HeadUtils.NormalHeadMovement("y", hmdPositions, MinNormalHeadMovements))
                    {
                        // Too much Y movement for a pure shake
                        currentlyShaking = false;
                    }
                    else if (!HeadUtils.EnoughAmplitude(_distanceHmdPositionsManipulated, MinDistanceShaking))
                    {
                        // Insufficient horizontal amplitude for shaking
                        currentlyShaking = false;
                    }
                    else
                    {
                        currentlyShaking = true;
                    }
                }

                switch (currentlyNodding)
                {
                    // No gesture detected - clear data for fresh start
                    case false when !currentlyShaking:
                        HeadUtils.ClearAllItems(ref hmdPositions, ref _hmdPositionsManipulated,
                            ref _distanceHmdPositionsManipulated);
                        break;
                    // Conflicting gestures detected - reset (shouldn't happen normally)
                    case true when currentlyShaking:
                        currentlyNodding = false;
                        currentlyShaking = false;
                        HeadUtils.ClearAllItems(ref hmdPositions, ref _hmdPositionsManipulated,
                            ref _distanceHmdPositionsManipulated);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Processes gesture completion and clears tracking data for next gesture.
        /// </summary>
        /// <param name="hmdPositions">Position data list to clear</param>
        /// <param name="hmdPositionsManipulated">Manipulated position data to clear</param>
        /// <param name="distanceHmdPositionsManipulated">Distance data to clear</param>
        /// <param name="currentlyNodding">Nodding state to reset</param>
        /// <param name="currentlyShaking">Shaking state to reset</param>
        /// <param name="minDistanceNodding">Minimum distance used for detection</param>
        /// <param name="numGestures">Number of gestures detected</param>
        /// <returns>Total gesture duration</returns>
        private static TimeSpan GetLogInfo(ref List<HmdTimedPosition> hmdPositions, ref List<HmdTimedPosition> hmdPositionsManipulated, ref List<float> distanceHmdPositionsManipulated, ref bool currentlyNodding, ref bool currentlyShaking, double minDistanceNodding, int numGestures)
        {
            var timeSpan = hmdPositions[hmdPositions.Count - 1].Timestamp - hmdPositions[0].Timestamp;

            currentlyNodding = false;
            currentlyShaking = false;

            // Clear all tracking data for next gesture detection
            HeadUtils.ClearAllItems(ref hmdPositions, ref hmdPositionsManipulated, ref distanceHmdPositionsManipulated);
            return timeSpan;
        }
        
        /// <summary>
        /// Processes a detected nodding gesture and triggers the OnNodded event when complete.
        /// </summary>
        /// <param name="currentlyNodding">Current nodding state</param>
        /// <param name="currentlyShaking">Current shaking state</param>
        private void Nodding(ref bool currentlyNodding, ref bool currentlyShaking)
        {
            if (!trackNoddingEnabled)
                return;
                
            // Check if nodding gesture is still continuing or has finished
            var positionContinuousNodding = HeadUtils.GetPositionContinuousGesture(_hmdPositions, _continuousTimeBetweenNods);
            if (positionContinuousNodding == -1) return;
            
            // Wait for gesture to complete
            if (!HeadUtils.GestureFinished(_hmdPositions, positionContinuousNodding, MinDistanceNodding, "y"))
                return;
                
            var numNods = HeadUtils.GetNumOfGestures(_hmdPositions, MinDistanceNodding, "y");
            if (numNods <= 0) return;
            
            var timeSpan = GetLogInfo(ref _hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref currentlyNodding, ref currentlyShaking, MinDistanceNodding, numNods);
            OnNodded?.Invoke(this, new HeadTrackingBehaviourArgs(_hmdPositions.ToArray(), _hmdPositionsManipulated.ToArray(), _distanceHmdPositionsManipulated.ToArray(), MinDistanceNodding, timeSpan, numNods));
        }
        
        /// <summary>
        /// Processes a detected shaking gesture and triggers the OnShook event when complete.
        /// </summary>
        /// <param name="currentlyNodding">Current nodding state</param>
        /// <param name="currentlyShaking">Current shaking state</param>
        private void Shaking(ref bool currentlyNodding, ref bool currentlyShaking)
        {
            if (!trackShakingEnabled)
                return;
                
            // Check if shaking gesture is still continuing or has finished
            var positionContinuousShaking = HeadUtils.GetPositionContinuousGesture(_hmdPositionsManipulated, _continuousTimeBetweenShakes);
            if (positionContinuousShaking == -1) return;
            
            // Wait for gesture to complete
            if (!HeadUtils.GestureFinished(_distanceHmdPositionsManipulated, positionContinuousShaking,
                    MinDistanceShaking)) return;
                    
            var numShakes = HeadUtils.GetNumOfGestures(_distanceHmdPositionsManipulated, MinDistanceShaking);
            if (numShakes <= 0)
                return;
                
            var timeSpan = GetLogInfo(ref _hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref currentlyNodding, ref currentlyShaking, MinDistanceShaking, numShakes);
            OnShook?.Invoke(this, new HeadTrackingBehaviourArgs(_hmdPositions.ToArray(), _hmdPositionsManipulated.ToArray(), _distanceHmdPositionsManipulated.ToArray(), MinDistanceShaking, timeSpan, numShakes));
        }
    }
}