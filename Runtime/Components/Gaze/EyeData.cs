using OmiLAXR.TrackingBehaviours.Learner.Gaze;
using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    public sealed class EyeData : GazeData
    {
        public readonly Eye EyeSide;

        /// <summary>Openness value (0.0 = closed, 1.0 = fully open)</summary>
        public readonly float EyeOpenness;

        /// <summary>Confidence score of gaze data (0.0 - 1.0)</summary>
        public readonly float EyeConfidence;

        public readonly float EyeDepth;
        public readonly float EyeHeight;

        /// <summary>Measured pupil diameter in millimeters (optional, if supported)</summary>
        public float? PupilDiameterMillimeters;

        public float VergenceAngleDegrees;
        
        public EyeData(
            GazeHit hit,
            Vector3 gazeOriginWorld,
            Vector3 gazePointWorld,
            Eye eyeSide,
            Frustum frustum,
            float openness,
            float confidence, 
            float depth, float eyeHeight, float? pupilDiameter = null,
            float vergenceAngleDegrees = 0)
        : base(hit, frustum, gazeOriginWorld, gazePointWorld)
        {
            EyeSide = eyeSide;
            EyeOpenness = openness;
            EyeConfidence = confidence;
            EyeDepth = depth;
            EyeHeight = eyeHeight;
            PupilDiameterMillimeters = pupilDiameter;
            VergenceAngleDegrees = vergenceAngleDegrees;
        }
    }
}