using UnityEngine;

namespace OmiLAXR
{
    public abstract class TrackingBehaviour : MonoBehaviour
    {
        protected MainTrackingBehaviour mainTrackingBehaviour;
        protected void Awake()
        {
            mainTrackingBehaviour = GetComponentInParent<MainTrackingBehaviour>();
        }

        public abstract void Listen(GameObject go);

        public void SendStatement()
        {
            mainTrackingBehaviour.SendToDataProviders();
        }
    }
}