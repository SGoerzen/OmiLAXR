/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner.HeadTracking
{
    /// <summary>
    /// Value type for storing HMD position data with timestamp for head tracking analysis.
    /// Used to collect positional samples over time for gesture recognition algorithms.
    /// </summary>
    public struct HmdTimedPosition
    {
        /// <summary>
        /// The timestamp when this position was recorded.
        /// </summary>
        public DateTime Timestamp;
        
        /// <summary>
        /// The 3D position of the HMD at the recorded timestamp.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Creates a new HmdTimedPosition with the specified timestamp and position.
        /// </summary>
        /// <param name="timestamp">When the position was recorded</param>
        /// <param name="position">The 3D position of the HMD</param>
        public HmdTimedPosition(DateTime timestamp, Vector3 position)
        {
            Timestamp = timestamp;
            Position = position;
        }
    }
}