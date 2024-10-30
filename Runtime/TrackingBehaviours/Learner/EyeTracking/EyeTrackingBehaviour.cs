
namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    public abstract class EyeTrackingBehaviour : EventTrackingBehaviour
    {
        /// <summary>
        /// Notable change in pupil size occurs. Track cognitive load or emotional response.
        /// </summary>
        public abstract PupilDilationData? GetPupilDilationData();
        
        public abstract double? GetViewingAngle();
        
        /// <summary>
        /// The eye moves on to focus on a particular point (fixation). Log attention on specific Areas of Interest (AOIs), durations, and transitions.
        /// </summary>
        [Gesture("Eyes"), Action("Fixate")]
        public readonly TrackingBehaviourEvent<FixationData> OnFixated = new TrackingBehaviourEvent<FixationData>();
        /// <summary>
        /// Rapid movement occurs between two fixations. Track gaze shifts and identify scanning patterns.
        /// </summary>
        [Gesture("Eyes"), Action("Saccade")]
        public readonly TrackingBehaviourEvent<SaccadeData> OnSaccaded = new TrackingBehaviourEvent<SaccadeData>();
        /// <summary>
        /// Small involuntary movement happens during fixation. Study attention or response to stimuli within a fixation.
        /// </summary>
        [Gesture("Eyes"), Action("Micro Saccade")]
        public readonly TrackingBehaviourEvent<MicroSaccadeData> OnMicroSaccaded = new TrackingBehaviourEvent<MicroSaccadeData>();
        /// <summary>
        /// Gaze data is temporarily lost due to a blink. Monitor engagement, fatigue, or workload.
        /// </summary>
        [Gesture("Eyes"), Action("Blink")]
        public readonly TrackingBehaviourEvent<BlinkData> OnBlinked = new TrackingBehaviourEvent<BlinkData>();
        // /// <summary>
        // /// Notable change in pupil size occurs. Track cognitive load or emotional response.
        // /// </summary>
        // [Gesture("Eyes"), Action("Pupil Dilation")]
        // public readonly TrackingBehaviourEvent<PupilDilationData> OnPupilDilation = new TrackingBehaviourEvent<PupilDilationData>();
    }
}