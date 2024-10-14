using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    public class TransformTrackingBehaviour : TrackingBehaviour
    {
        [Gesture("Movement"), Action("Translation")]
        public TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedPosition =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        [Gesture("Movement"), Action("Rotation")]
        public TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedRotation =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        [Gesture("Movement"), Action("Scale")]
        public TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedScale =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            var transformWatchers = Select<TransformWatcher>(objects);

            foreach (var tw in transformWatchers)
            {
                OnChangedPosition.Bind(tw.onChangedPosition, tc =>
                {
                    OnChangedPosition.Invoke(this, tw, tc);
                });
                OnChangedRotation.Bind(tw.onChangedRotation, tc =>
                {
                    OnChangedRotation.Invoke(this, tw, tc);
                });
                OnChangedScale.Bind(tw.onChangedScale, tc =>
                {
                    OnChangedScale.Invoke(this, tw, tc);
                });
            }
        }
    }
}