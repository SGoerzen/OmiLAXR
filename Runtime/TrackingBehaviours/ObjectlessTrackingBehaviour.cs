using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    public class ObjectlessTrackingBehaviour : TrackingBehaviour<Object>
    {
        protected override void AfterFilteredObjects(Object[] objects)
        {
            // do nothing
        }
    }
}