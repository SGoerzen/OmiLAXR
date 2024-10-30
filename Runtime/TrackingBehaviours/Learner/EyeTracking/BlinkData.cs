
namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public struct BlinkData
    {
        public readonly Duration Duration;  
        public readonly BlinkEye BlinkedEye;
        
        public enum BlinkEye
        {
            Left, 
            Right, 
            Both
        }

        public BlinkData(BlinkEye blinkedEye, Duration duration)
        {
            BlinkedEye = blinkedEye;
            Duration = duration;
        }
    }
}