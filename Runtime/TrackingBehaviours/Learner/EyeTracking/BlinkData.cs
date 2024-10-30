using System;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct BlinkData
    {
        public readonly int DurationInMilliseconds;  // Duration of blink in ms
        public readonly DateTime Timestamp;

        public BlinkData(int durationInMilliseconds, DateTime timestamp)
        {
            DurationInMilliseconds = durationInMilliseconds;
            Timestamp = timestamp;
        }
    }
}