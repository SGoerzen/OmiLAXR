using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-99999999)]
    [DisallowMultipleComponent]
    [AddComponentMenu("OmiLAXR / Global Settings")]
    public class GlobalSettings : PipelineComponent
    {
        private static GlobalSettings _instance;
        public static GlobalSettings Instance => GetInstance(ref _instance);
        
        [Tooltip("Whether the tracked name is by default the object name or the full name path to the object in hierarchy. This option is not applied if a [CustomTrackingName] component is attached.")]
        public TrackingNameBehaviour trackingNameBehaviour = TrackingNameBehaviour.ObjectName;

        public enum TrackingNameBehaviour
        {
            ObjectName,
            HierarchyTreePath
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}