using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct SaccadeData
    {
        public readonly Vector3 Trajectory;
        public readonly double SaccadeAmplitudeDegrees;
        public readonly double? PupilDiameterMillimeters; // Optional
        public readonly GameObject Target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">Target game object of scene.</param>
        /// <param name="trajectory">Defines the saccade trajectory.</param>
        /// <param name="saccadeAmplitudeDegrees">Angular distance of the saccade.</param>
        /// <param name="pupilDiameterMillimeters">Pupil diameter can describe Context for arousal or engagement.</param>
        public SaccadeData(GameObject target, Vector3 trajectory, double saccadeAmplitudeDegrees, double? pupilDiameterMillimeters)
        {
            Target = target;
            Trajectory = trajectory;
            SaccadeAmplitudeDegrees = saccadeAmplitudeDegrees;
            PupilDiameterMillimeters = pupilDiameterMillimeters;
        }
    }
}