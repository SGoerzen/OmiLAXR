namespace OmiLAXR.TrackingBehaviours
{
    public interface IntervalTrackingBehaviour
    {
        IntervalSettings GetIntervalSettings();
        void OnStartTimer();
        void OnTick();
        void OnStopTimer();
    }
}