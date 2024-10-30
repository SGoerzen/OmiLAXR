using System;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public class PupilDilationData
    {
        public readonly double PupilDiameterMillimeters;
        public readonly DateTime? Timestamp; // Optional
        
        public PupilDilationData(double pupilDiameterMillimeters, DateTime? timestamp)
        {
            PupilDiameterMillimeters = pupilDiameterMillimeters;
            Timestamp = timestamp;
        }
    }
}