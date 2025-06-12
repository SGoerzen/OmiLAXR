using System;
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Session Tracking Behaviour")]
    [Description("Tracks the start and end of a session.")]
    public class SessionTrackingBehaviour : TrackingBehaviour
    {
        private bool _isRunning = false;
        public readonly TrackingBehaviourEvent<DateTime> OnSessionStarted = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime> OnSessionStopped = new TrackingBehaviourEvent<DateTime>();
        protected override void BeforeStartedPipeline(Pipeline pipeline)
        {
            if (_isRunning) return;
            OnSessionStarted.Invoke(this, DateTime.Now);
            _isRunning = true;
        }

        protected override void BeforeStoppedPipeline(Pipeline pipeline)
        {
            TriggerEnd();
        }

        private void TriggerEnd()
        {
            if (!_isRunning) return;
            OnSessionStopped.Invoke(this, DateTime.Now);
            _isRunning = false;
        }

        private void OnDestroy() => TriggerEnd();
        private void OnApplicationQuit() => TriggerEnd();
    }
}