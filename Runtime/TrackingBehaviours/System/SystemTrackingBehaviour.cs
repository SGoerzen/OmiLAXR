using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Scripting;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / System Tracking Behaviour")]
    [Description("Tracks states of game (started, quit, paused, resumed, focused, unfocused) and detects state changes.")]
    public class SystemTrackingBehaviour : TrackingBehaviour
    {
        public readonly TrackingBehaviourEvent<DateTime> OnStartedGame = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime> OnQuitGame = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime, bool> OnFocusedGame = new TrackingBehaviourEvent<DateTime, bool>();
        public readonly TrackingBehaviourEvent<DateTime, bool> OnPausedGame = new TrackingBehaviourEvent<DateTime, bool>();
        
        [Preserve]
        protected static class SystemStartController
        {
            public static DateTime? StartTime;
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
            private static void GameStarted()
            {
                StartTime = DateTime.Now;
            }
        }

        private bool _isFirstRun = true;
        private bool _sendStartSignal = false;
        private bool _isRunning = false;

        public void SendStartSignal()
        {
            if (_sendStartSignal)
                return;
            
            var now = SystemStartController.StartTime.HasValue ? SystemStartController.StartTime.Value : DateTime.Now;
            OnStartedGame?.Invoke(this, now);
            print("send start signal");

            _sendStartSignal = true;
        }

        protected override void OnStartedPipeline(Pipeline pipeline)
        {
            if (_isRunning) return;
            base.OnStartedPipeline(pipeline);
            SendStartSignal();
            _isRunning = true;
        }

        private void TriggerEnd()
        {
            if (!_isRunning) return;
            OnQuitGame.Invoke(this, DateTime.Now);
            _isRunning = false;
        }

        private void OnDestroy() => TriggerEnd();
        private void OnApplicationQuit() => TriggerEnd();

        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            if (!enabled)
                return;
            OnFocusedGame?.Invoke(this, DateTime.Now, hasFocus);
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (!enabled)
                return;
            
            if (_isFirstRun)
            {
                _isFirstRun = false;
                return;
            }
            OnPausedGame?.Invoke(this, DateTime.Now, pauseStatus);
        }
    }
}