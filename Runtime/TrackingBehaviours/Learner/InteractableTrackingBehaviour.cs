using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    public struct InteractableEventArgs
    {
        public GameObject Target;
        public Hand Hand;

        public InteractableEventArgs(GameObject target, Hand hand = Hand.Unknown)
        {
            Target = target;
            Hand = hand;
        }
    }
    
    public abstract class InteractableTrackingBehaviour : TrackingBehaviour
    {
        [Gesture("XRController"), Action("Interacted")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnInteracted = new TrackingBehaviourEvent<InteractableEventArgs>();
        
        [Gesture("XRController"), Action("Touched")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnTouched = new TrackingBehaviourEvent<InteractableEventArgs>();
        
        [Gesture("XRController"), Action("Released")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnReleased = new TrackingBehaviourEvent<InteractableEventArgs>();

        [Gesture("XRController"), Action("Grabbed")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnGrabbed = new TrackingBehaviourEvent<InteractableEventArgs>();
    }
}