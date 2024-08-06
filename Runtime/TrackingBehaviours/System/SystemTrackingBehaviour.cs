using System;
using UnityEngine;

namespace OmiLAXR.Tracking.Behaviours
{
    public class SystemTrackingBehaviour : TrackingBehaviour
    {
        private static SystemTrackingBehaviour _instance;
        
        public static SystemTrackingBehaviour Instance
            => _instance ??= FindObjectOfType<SystemTrackingBehaviour>();
        
        public event Action OnGameStarted;
        
        [RuntimeInitializeOnLoadMethod]
        private static void GameStarted()
        {
            
        }
        
    }
}