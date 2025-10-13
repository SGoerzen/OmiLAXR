using System;
using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Gaze.Fixation
{
    public sealed class FixationData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly Duration Duration; 
        public readonly int TargetFixationCounts;
        public readonly DateTime? StartTime;
        public readonly DateTime? EndTime;
        public readonly GazeHit Hit;

        /// <summary>
        /// Attention data on specific Areas of Interest (AOIs), durations, and transitions.
        /// </summary>
        /// <param name="gazeHit">Gaze Hit where the fixation is made.</param>
        /// <param name="startGazeCoordinates">Location of the fixation.</param>
        /// <param name="targetFixationCounts">Amount of fixations that were made with these detector on the target object.</param>
        /// <param name="startTime">Start time when fixation has started.</param>
        /// <param name="endTime">End time when fixation has ended.</param>
        public FixationData(GazeHit gazeHit, Vector3 startGazeCoordinates, int targetFixationCounts, DateTime? startTime, DateTime? endTime)
        {
            Hit = gazeHit;
            StartGazeCoordinates = startGazeCoordinates;
            TargetFixationCounts = targetFixationCounts;
            StartTime = startTime;
            EndTime = endTime;
            if (startTime.HasValue && endTime.HasValue)
                Duration = Duration.FromMilliseconds((int) (endTime.Value - startTime.Value).TotalMilliseconds);
        }
    }
}