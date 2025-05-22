using System;
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Session Tracking Behaviour")]
    [Description("Tracks the start and end of a session.")]
    public class SessionTrackingBehaviour : ObjectlessTrackingBehaviour
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