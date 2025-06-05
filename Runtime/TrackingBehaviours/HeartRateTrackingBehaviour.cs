
using OmiLAXR.Actors.HeartRate;
using OmiLAXR.Schedules;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Tracking behavior specifically designed to monitor heart rate data at regular intervals.
    /// Implements IntervalHandler to support scheduled execution via the IntervalTimer.
    /// </summary>
    public class HeartRateTrackingBehaviour : TrackingBehaviour
    {
        public readonly TrackingBehaviourEvent<int> OnHeartBeat = new TrackingBehaviourEvent<int>();
        
        private void Start()
        {
            var provider = GetComponentInParent<HeartRateProvider>();
            if (provider == null || !provider.enabled)
            {
                enabled = false;
                DebugLog.OmiLAXR.Error("Cannot find any <HeartRateProvider> in parent pipeline.");
                return;
            }
            SetInterval(() =>
            {
                OnHeartBeat?.Invoke(this, provider.GetHeartRate());
            }, 1.0f);
        }
    }
}