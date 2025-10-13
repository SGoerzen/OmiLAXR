using System;
using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    /// <summary>
    /// Transition between two gaze points / AOIs (a saccade), including timing and amplitude.
    /// Modeled after FixationData for a consistent data shape.
    /// </summary>
    public sealed class SaccadeData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly Vector3 EndGazeCoordinates;

        public readonly Duration Duration; 
        public readonly DateTime? StartTime;
        public readonly DateTime? EndTime;

        public readonly float SaccadeAmplitudeDegrees;
        public readonly float? PupilDiameterMillimeters; // Optional

        public readonly GazeHit Hit;

        /// <param name="hit">Gaze hit at the beginning of the saccade (source AOI).</param>
        /// <param name="startGazeCoordinates">World-space gaze origin at saccade start.</param>
        /// <param name="endGazeCoordinates">World-space gaze point at saccade end.</param>
        /// <param name="saccadeAmplitudeDegrees">Angular distance of the saccade in degrees.</param>
        /// <param name="pupilDiameterMillimeters">Optional pupil diameter (context for arousal/engagement).</param>
        /// <param name="startTime">Start timestamp of the saccade.</param>
        /// <param name="endTime">End timestamp of the saccade.</param>
        public SaccadeData(
            GazeHit hit,
            Vector3 startGazeCoordinates,
            Vector3 endGazeCoordinates,
            float saccadeAmplitudeDegrees,
            float? pupilDiameterMillimeters,
            DateTime? startTime,
            DateTime? endTime)
        {
            Hit = hit;

            StartGazeCoordinates = startGazeCoordinates;
            EndGazeCoordinates = endGazeCoordinates;

            SaccadeAmplitudeDegrees = saccadeAmplitudeDegrees;
            PupilDiameterMillimeters = pupilDiameterMillimeters;

            StartTime = startTime;
            EndTime = endTime;

            if (startTime.HasValue && endTime.HasValue)
            {
                var ms = (int)(endTime.Value - startTime.Value).TotalMilliseconds;
                Duration = Duration.FromMilliseconds(ms);
            }
        }
    }
}