using System;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Session Tracking Behaviour")]
    public class SessionTrackingBehaviour : EventTrackingBehaviour
    {
        public readonly TrackingBehaviourEvent<DateTime> OnSessionStarted = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime> OnSessionStopped = new TrackingBehaviourEvent<DateTime>();
        protected override void OnStartedPipeline(Pipeline pipeline)
        {
            OnSessionStarted.Invoke(this, DateTime.Now);
        }

        protected override void OnStoppedPipeline(Pipeline pipeline)
        {
            OnSessionStopped.Invoke(this, DateTime.Now);
        }
        
    }
}