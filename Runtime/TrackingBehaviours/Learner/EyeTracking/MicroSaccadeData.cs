using System.Numerics;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct MicroSaccadeData
    {
        public readonly double AmplitudeInDegrees;
        public readonly double DirectionInDegrees;
        public readonly Vector2? StartPosition; // Optional
        public readonly Vector2? EndPosition;    // Optional
        
        public MicroSaccadeData(double amplitudeInDegrees, double directionInDegrees, Vector2? startPosition, Vector2? endPosition)
        {
            AmplitudeInDegrees = amplitudeInDegrees;
            DirectionInDegrees = directionInDegrees;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        
    }
}