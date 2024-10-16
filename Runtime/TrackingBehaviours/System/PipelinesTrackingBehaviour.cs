
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Pipelines Tracking Behaviour")]
    public class PipelinesTrackingBehaviour : TrackingBehaviour<Pipeline>
    {
        protected override void AfterFilteredObjects(Pipeline[] objects)
        {
            
        }
    }
}