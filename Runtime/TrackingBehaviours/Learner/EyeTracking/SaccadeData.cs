using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct SaccadeData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly Vector3 EndGazeCoordinates;
        public readonly Duration Duration;
        public readonly double SaccadeAmplitudeDegrees;
        public readonly double? PupilDiameterMillimeters; // Optional
        public readonly GameObject Target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">Target game object of scene.</param>
        /// <param name="duration">Duration of saccade.</param>
        /// <param name="startGazeCoordinates">Eye coordinates when gaze started.</param>
        /// <param name="endGazeCoordinates">Eye coordinates when gaze stopped.</param>
        /// <param name="saccadeAmplitudeDegrees">Angular distance of the saccade.</param>
        /// <param name="pupilDiameterMillimeters">Pupil diameter can describe Context for arousal or engagement.</param>
        public SaccadeData(GameObject target, Duration duration, Vector3 startGazeCoordinates, Vector3 endGazeCoordinates, double saccadeAmplitudeDegrees, double? pupilDiameterMillimeters)
        {
            Target = target;
            Duration = duration;
            StartGazeCoordinates = startGazeCoordinates;
            EndGazeCoordinates = endGazeCoordinates;
            SaccadeAmplitudeDegrees = saccadeAmplitudeDegrees;
            PupilDiameterMillimeters = pupilDiameterMillimeters;
            StartGazeCoordinates = startGazeCoordinates;
            EndGazeCoordinates = endGazeCoordinates;
        }
    }
}