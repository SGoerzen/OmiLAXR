using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class BindedStatementComposer<T> : StatementComposer 
        where T : TrackingBehaviour
    {
        [HideInInspector]
        public T trackingBehaviour;

        protected virtual void Awake()
        {
            trackingBehaviour = GetTrackingBehaviour<T>();
        }
    }
}