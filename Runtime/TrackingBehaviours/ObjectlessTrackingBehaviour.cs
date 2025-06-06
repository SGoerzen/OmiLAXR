using System;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours
{
    [Obsolete("Please use 'TrackingBehaviour' instead. This class will be removed in future version.")]
    public class ObjectlessTrackingBehaviour : TrackingBehaviour<Object>
    {
        protected override void AfterFilteredObjects(Object[] objects)
        {
            // do nothing
        }
    }
}