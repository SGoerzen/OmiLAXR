using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct FixationData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly int DurationInMilliseconds; 
        public readonly double? PupilDiameterMillimeters;
        public readonly double? ViewingAngleDegrees;
        public readonly GameObject Target;

        /// <summary>
        /// Attention data on specific Areas of Interest (AOIs), durations, and transitions.
        /// </summary>
        /// <param name="startGazeCoordinates">Location of the fixation.</param>
        /// <param name="durationInMilliseconds">How long the fixation lasted (computed when ending).</param>
        /// <param name="pupilDiameterMillimeters">Indicates cognitive load.</param>
        /// <param name="viewingAngleDegrees">Adds context for orientation in 3D scenes.</param>
        public FixationData(GameObject target, Vector3 startGazeCoordinates, int durationInMilliseconds, double? pupilDiameterMillimeters = null, double? viewingAngleDegrees = null)
        {
            Target = target;
            StartGazeCoordinates = startGazeCoordinates;
            DurationInMilliseconds = durationInMilliseconds;
            ViewingAngleDegrees = viewingAngleDegrees;
            PupilDiameterMillimeters = pupilDiameterMillimeters;
        }
    }
}