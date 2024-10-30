using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct FixationData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly Duration Duration; 
        public readonly double? PupilDiameterMillimeters;
        public readonly GameObject Target;

        /// <summary>
        /// Attention data on specific Areas of Interest (AOIs), durations, and transitions.
        /// </summary>
        /// <param name="target">Target game object of scene.</param>
        /// <param name="startGazeCoordinates">Location of the fixation.</param>
        /// <param name="duration">How long the fixation lasted (computed when ending).</param>
        /// <param name="pupilDiameterMillimeters">Pupil diameter can indicate cognitive load.</param>
        public FixationData(GameObject target, Vector3 startGazeCoordinates, Duration duration, double? pupilDiameterMillimeters = null)
        {
            Target = target;
            StartGazeCoordinates = startGazeCoordinates;
            Duration = duration;
            PupilDiameterMillimeters = pupilDiameterMillimeters;
        }
    }
}