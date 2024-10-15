using System;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / System Tracking Behaviour")]
    public class SystemTrackingBehaviour : EventTrackingBehaviour
    {
        public readonly TrackingBehaviourEvent<DateTime> OnStartedGame = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime> OnQuitGame = new TrackingBehaviourEvent<DateTime>();
        public readonly TrackingBehaviourEvent<DateTime, bool> OnFocusedGame = new TrackingBehaviourEvent<DateTime, bool>();
        public readonly TrackingBehaviourEvent<DateTime, bool> OnPausedGame = new TrackingBehaviourEvent<DateTime, bool>();
        
        [DefaultExecutionOrder(-10000)]
        protected static class SystemStartController
        {
            public static DateTime StartTime;
            [RuntimeInitializeOnLoadMethod]
            private static void GameStarted()
            {
                StartTime = DateTime.Now;
            }
        }

        private bool _isFirstRun = true;
        
        private void Start()
        {
            var stbs = FindObjectsOfType<SystemTrackingBehaviour>();
            foreach (var stb in stbs)
            {
                stb.OnStartedGame?.Invoke(stb, SystemStartController.StartTime);
            }
        }

        private void OnApplicationQuit()
        {
           OnQuitGame?.Invoke(this, DateTime.Now);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            OnFocusedGame?.Invoke(this, DateTime.Now, hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
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