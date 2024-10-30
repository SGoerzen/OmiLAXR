using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public abstract class EyeTrackingBehaviour : TrackingBehaviour<Object>
    {
        /// <summary>
        /// The eye moves on to focus on a particular point (fixation start).
        /// </summary>
        [Gesture("Eyes"), Action("Fixated")]
        public TrackingBehaviourEvent<FixationData> OnFixated = new TrackingBehaviourEvent<FixationData>();
        /// <summary>
        /// The eye pauses to focus on a particular point (fixation end).
        /// </summary>
        [Gesture("Eyes"), Action("Saccaded")]
        public TrackingBehaviourEvent<SaccadeData> OnSaccaded = new TrackingBehaviourEvent<SaccadeData>();
    }
}