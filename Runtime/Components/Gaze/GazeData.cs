using OmiLAXR.TrackingBehaviours.Learner.Gaze;
using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    public class GazeData
    {
        /// <summary>Normalized gaze direction in local space of the HMD (unit vector)</summary>
        public Vector3 LocalGazeDirection => (GazePointWorld - GazeOriginWorld).normalized;

        /// <summary>World-space gaze origin (e.g., eye socket center)</summary>
        public readonly Vector3 GazeOriginWorld;

        /// <summary>World-space gaze point (hit position on collider, if valid)</summary>
        public readonly Vector3 GazePointWorld;
        
        public float HitDistanceInMeters => (GazeOriginWorld - GazePointWorld).magnitude;
        
        public readonly GazeHit Hit;
        
        public readonly Frustum Frustum;
        
        public GazeData(
            GazeHit hit,
            Frustum frustum,
            Vector3 gazeOriginWorld,
            Vector3 gazePointWorld)
        {
            Hit = hit;
            GazeOriginWorld = gazeOriginWorld;
            GazePointWorld = gazePointWorld;
            Frustum = frustum;
        }
    }
}