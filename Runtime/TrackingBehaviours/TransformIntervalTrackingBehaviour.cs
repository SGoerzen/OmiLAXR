
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Transform Interval Tracking Behaviour")]
    [Description("Tracks position, rotation and scal of game objects holding <TransformWatcher> component in an interval.")]
    public class TransformIntervalTrackingBehaviour : TransformTrackingBehaviour, IntervalTrackingBehaviour
    {
        public IntervalSettings intervalSettings;
        public IntervalSettings GetIntervalSettings() => intervalSettings;

        protected override void AfterFilteredObjects(TransformWatcher[] transformWatchers)
        {
            // overwrite bind of TransformTrackingBehaviour
        }


        public void OnStartTimer()
        {
           
        }

        public void OnTick()
        {
            
        }

        public void OnStopTimer()
        {
            
        }
    }
}