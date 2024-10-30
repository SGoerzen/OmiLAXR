using System.Numerics;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct SaccadeData
    {
        public readonly Vector3 StartGazeCoordinates;
        public readonly Vector3 EndGazeCoordinates;
        public readonly double SaccadeAmplitudeDegrees;
        public readonly double? PupilDiameterMillimeters; // Optional

        public SaccadeData(Vector3 startGazeCoordinates, Vector3 endGazeCoordinates, double saccadeAmplitudeDegrees, double? pupilDiameterMillimeters)
        {
            StartGazeCoordinates = startGazeCoordinates;
            EndGazeCoordinates = endGazeCoordinates;
            SaccadeAmplitudeDegrees = saccadeAmplitudeDegrees;
            PupilDiameterMillimeters = pupilDiameterMillimeters;
        }
    }
}