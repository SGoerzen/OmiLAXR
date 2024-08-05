using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Tracking / Add restriction")]
    public class TrackingRestriction : MonoBehaviour
    {
        [Tooltip("Track position")] public bool trackPosition;

        [Tooltip("Track rotation")] public bool trackRotation;

        [Tooltip("Track scale")] public bool trackScale;

    }
}