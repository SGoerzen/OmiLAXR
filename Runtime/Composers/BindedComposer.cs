using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class BindedComposer<T> : Composer 
        where T : TrackingBehaviour
    {
        [HideInInspector]
        public T trackingBehaviour;

        protected virtual void Awake()
        {
            trackingBehaviour = GetTrackingBehaviour<T>(true);
            Compose(trackingBehaviour);
        }

        protected abstract void Compose(T tb);
    }
}