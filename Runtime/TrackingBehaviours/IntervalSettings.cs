using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    [CreateAssetMenu(menuName = "OmiLAXR / Create Interval Settings")]
    public class IntervalSettings : ScriptableObject
    {
        public float intervalSeconds = 1.0f;
        public bool randomizeInterval = false;
    }
}