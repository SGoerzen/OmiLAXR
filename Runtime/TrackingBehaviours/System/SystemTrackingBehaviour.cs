using System;
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / System Tracking Behaviour")]
    [Description("Tracks states of game (started, quit, paused, resumed, focused, unfocused) and detects state changes.")]
    public class SystemTrackingBehaviour : ObjectlessTrackingBehaviour
    {
        public readonly TrackingBehaviourEvent<DateTime> OnStartedGame = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime> OnQuitGame = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime, bool> OnFocusedGame = new TrackingBehaviourEvent<DateTime, bool>();
        public readonly TrackingBehaviourEvent<DateTime, bool> OnPausedGame = new TrackingBehaviourEvent<DateTime, bool>();
        
        [DefaultExecutionOrder(-10000)]
        protected static class SystemStartController
        {
            public static DateTime? StartTime;
            [RuntimeInitializeOnLoadMethod]
            private static void GameStarted()
            {
                StartTime = DateTime.Now;
            }
        }

        private bool _isFirstRun = true;
        private bool _sendStartSignal = false;
        
        protected virtual void Start()
        {
            SendStartSignal();
        }

        protected void SendStartSignal()
        {
            if (_sendStartSignal)
                return;
            
            var now = SystemStartController.StartTime.HasValue ? SystemStartController.StartTime.Value : DateTime.Now;
            OnStartedGame?.Invoke(this, now);

            _sendStartSignal = true;
        }

        protected virtual void OnApplicationQuit()
        {
           OnQuitGame?.Invoke(this, DateTime.Now);
        }

        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            SendStartSignal();
            OnFocusedGame?.Invoke(this, DateTime.Now, hasFocus);
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (_isFirstRun)
            {
                _isFirstRun = false;
                return;
            }
            OnPausedGame?.Invoke(this, DateTime.Now, pauseStatus);
        }
    }
}