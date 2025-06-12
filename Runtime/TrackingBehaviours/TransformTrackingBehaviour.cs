using System.ComponentModel;
using OmiLAXR.Schedules;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Transform Tracking Behaviour")]
    [Description("Tracks position, rotation and scale changes in a game object holding <TransformWatcher> component.")]
    public class TransformTrackingBehaviour : TrackingBehaviour<TransformWatcher>
    {
        public bool detectOnChange = true;
        public IntervalTicker.Settings intervalSettings;
        public TransformWatcher.TransformIgnore ignore;
        
        [Gesture("Movement"), Action("Translation")]
        public readonly TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedPosition =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        [Gesture("Movement"), Action("Rotation")]
        public readonly TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedRotation =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        [Gesture("Movement"), Action("Scale")]
        public readonly TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedScale =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();

        protected override void OnStartedPipeline(Pipeline pipeline)
        {
            SetInterval(() =>
            {
                foreach (var tw in AllFilteredObjects)
                {
                    if (!ignore.position)
                    {
                        var posState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousPosition,
                            NewValue = tw.CurrentPosition,
                        };
                        OnChangedPosition.Invoke(this, tw, posState);
                    }

                    if (!ignore.rotation)
                    {
                        var rotState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousRotation,
                            NewValue = tw.CurrentRotation,
                        };
                        OnChangedRotation.Invoke(this, tw, rotState);
                    }

                    if (!ignore.scale)
                    {
                        var scaleState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousScale,
                            NewValue = tw.CurrentScale,
                        };
                        OnChangedScale.Invoke(this, tw, scaleState);
                    }
                }        
            }, intervalSettings);
        }
        
        protected override void AfterFilteredObjects(TransformWatcher[] transformWatchers)
        {
            if (!detectOnChange)
                return; // Skip if on change is not wished
            foreach (var tw in transformWatchers)
            {
                OnChangedPosition.Bind(tw.onChangedPosition, tc =>
                {
                    if (!ignore.position)
                        OnChangedPosition.Invoke(this, tw, tc);
                });
                OnChangedRotation.Bind(tw.onChangedRotation, tc =>
                {
                    if (!ignore.rotation)
                        OnChangedRotation.Invoke(this, tw, tc);
                });
                OnChangedScale.Bind(tw.onChangedScale, tc =>
                {
                    if (!ignore.scale)
                        OnChangedScale.Invoke(this, tw, tc);
                });
            }
        }
    }
}