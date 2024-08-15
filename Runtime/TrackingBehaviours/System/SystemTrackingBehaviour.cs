using System;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviors / System Tracking Behavior")]
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