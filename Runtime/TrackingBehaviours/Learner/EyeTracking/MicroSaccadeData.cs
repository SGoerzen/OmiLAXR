using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct MicroSaccadeData
    {
        public readonly double AmplitudeInDegrees;
        public readonly Duration Duration;
        public readonly double DirectionInDegrees;
        public readonly Vector2? StartPosition; // Optional
        public readonly Vector2? EndPosition;    // Optional
        public readonly GameObject Target;
        
        public MicroSaccadeData(GameObject target, double amplitudeInDegrees, Duration duration, double directionInDegrees, Vector2? startPosition, Vector2? endPosition)
        {
            Target = target;
            Duration = duration;
            AmplitudeInDegrees = amplitudeInDegrees;
            DirectionInDegrees = directionInDegrees;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        
    }
}