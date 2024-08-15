using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviors / System Tracking Behavior")]
    public class SystemTrackingBehaviour : TrackingBehaviour
    {
        public event TrackingBehaviourAction OnGameStarted;
        
        [RuntimeInitializeOnLoadMethod]
        private static void GameStarted()
        {
            var stbs = FindObjectsOfType<SystemTrackingBehaviour>();
            foreach (var stb in stbs)
            {
                stb.OnGameStarted?.Invoke(stb);
            }
        }
        
        
    }
}