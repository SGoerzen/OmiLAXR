using System;
using UnityEngine;

namespace OmiLAXR.Tracking.Behaviours
{
    public class SystemTrackingBehaviour : TrackingBehaviour
    {
        private static SystemTrackingBehaviour _instance;
        //[Description("Collection of tracking subsystems.")]
        //public readonly TrackingSubSystems SubSystems = new TrackingSubSystems();

        /// <summary>
        /// Singleton instance of the MainTrackingBehavior. Only one can exist at a time.
        /// </summary>
        public static SystemTrackingBehaviour Instance
            => _instance ??= FindObjectOfType<SystemTrackingBehaviour>();
        
        public event Action OnGameStarted;
        
        [RuntimeInitializeOnLoadMethod]
        private static void GameStarted()
        {
            Instance.OnGameStarted.Invoke();
        }
        
        public override void Listen(GameObject go)
        {
            
        }
    }
}