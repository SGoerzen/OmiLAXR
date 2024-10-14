using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    public class TransformTrackingBehaviour : TrackingBehaviour
    {
        protected override void AfterFilteredObjects(Object[] objects)
        {
            var transformWatcher = Select<TransformWatcher>(objects);
        }
    }
}