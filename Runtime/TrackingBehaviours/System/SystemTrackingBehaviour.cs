using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / System Tracking Behaviour")]
    public class SystemTrackingBehaviour : TrackingBehaviour
    {
        public TrackingBehaviourEvent OnGameStarted = new TrackingBehaviourEvent();
        public TrackingBehaviourEvent OnGameQuit = new TrackingBehaviourEvent();
        
        [RuntimeInitializeOnLoadMethod]
        private static void GameStarted()
        {
            var stbs = FindObjectsOfType<SystemTrackingBehaviour>();
            foreach (var stb in stbs)
            {
                stb.OnGameStarted?.Invoke(stb);
            }
        }

        private void OnApplicationQuit()
        {
           OnGameQuit?.Invoke(this);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // todo
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // todo
        }

        protected override void AfterFilteredObjects(Object[] objects)
        {
            
        }
    }
}