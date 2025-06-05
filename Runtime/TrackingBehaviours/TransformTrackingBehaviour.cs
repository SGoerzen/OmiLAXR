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
        public TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedPosition =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        [Gesture("Movement"), Action("Rotation")]
        public TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedRotation =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();
        [Gesture("Movement"), Action("Scale")]
        public TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange> OnChangedScale =
            new TrackingBehaviourEvent<TransformWatcher, TransformWatcher.TransformChange>();

        private void Start()
        {
            SetInterval(() =>
            {
                foreach (var tw in SelectedObjects)
                {
                    if (!ignore.position)
                    {
                        var posState = new TransformWatcher.TransformChange()
                        {
                            OldValue = tw.PreviousPosition,
                            NewValue = tw.CurrentPosition,
                        };
                        OnChangedPosition.Invoke(this, tw, posState);
                        print("Position " + posState.NewValue);
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