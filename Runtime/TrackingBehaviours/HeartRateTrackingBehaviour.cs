using System.ComponentModel;
using OmiLAXR.Actors.HeartRate;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Tracking behavior specifically designed to monitor heart rate data at regular intervals.
    /// Implements IntervalHandler to support scheduled execution via the IntervalTimer.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Heart Rate Tracking Behaviour")]
    [Description("Tracks the heart rate of an actor.")]
    public class HeartRateTrackingBehaviour : TrackingBehaviour
    {
        public readonly TrackingBehaviourEvent<int> OnHeartBeat = new TrackingBehaviourEvent<int>();
        
        private void Start()
        {
            var provider = GetComponentInParent<HeartRateProvider>();
            if (provider == null || !provider.enabled)
            {
                enabled = false;
                DebugLog.OmiLAXR.Warning($"Cannot find any <HeartRateProvider> in parent pipeline '{Pipeline.name}'. The Heart Rate Tracking Behaviour was disabled.");
                return;
            }
            SetInterval(() =>
            {
                OnHeartBeat?.Invoke(this, provider.GetHeartRate());
            }, 1.0f);
        }
    }
}