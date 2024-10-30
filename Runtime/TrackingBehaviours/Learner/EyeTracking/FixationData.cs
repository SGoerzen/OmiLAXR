using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct FixationData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly Duration Duration; 
        public readonly double? PupilDiameterMillimeters;
        public readonly double? ViewingAngleDegrees;
        public readonly GameObject Target;

        /// <summary>
        /// Attention data on specific Areas of Interest (AOIs), durations, and transitions.
        /// </summary>
        /// <param name="target">Target game object of scene.</param>
        /// <param name="startGazeCoordinates">Location of the fixation.</param>
        /// <param name="duration">How long the fixation lasted (computed when ending).</param>
        /// <param name="pupilDiameterMillimeters">Pupil diameter can indicate cognitive load.</param>
        /// <param name="viewingAngleDegrees">Adds context for orientation in 3D scenes.</param>
        public FixationData(GameObject target, Vector3 startGazeCoordinates, Duration duration, double? pupilDiameterMillimeters = null, double? viewingAngleDegrees = null)
        {
            Target = target;
            StartGazeCoordinates = startGazeCoordinates;
            Duration = duration;
            ViewingAngleDegrees = viewingAngleDegrees;
            PupilDiameterMillimeters = pupilDiameterMillimeters;
        }
    }
}