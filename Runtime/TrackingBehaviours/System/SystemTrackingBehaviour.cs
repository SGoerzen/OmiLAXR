using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / System Tracking Behaviour")]
    public class SystemTrackingBehaviour : TrackingBehaviour
    {
        public event TrackingBehaviourAction OnGameStarted;
        public event TrackingBehaviourAction OnGameQuit;
        
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