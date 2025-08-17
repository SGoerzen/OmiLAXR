/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Contains event data for interactable object events, including target object and hand information.
    /// </summary>
    public struct InteractableEventArgs
    {
        /// <summary>
        /// The GameObject that was interacted with.
        /// </summary>
        public GameObject Target;
        
        /// <summary>
        /// The hand used for the interaction (if applicable).
        /// </summary>
        public Hand Hand;

        /// <summary>
        /// Initializes a new InteractableEventArgs with the specified target and hand.
        /// </summary>
        /// <param name="target">The GameObject being interacted with</param>
        /// <param name="hand">The hand performing the interaction (defaults to Unknown)</param>
        public InteractableEventArgs(GameObject target, Hand hand = Hand.Unknown)
        {
            Target = target;
            Hand = hand;
        }
    }
    
    /// <summary>
    /// Abstract base class for tracking behaviors that monitor XR controller interactions with objects.
    /// Provides standard interaction events: Interacted, Touched, Released, and Grabbed.
    /// </summary>
    public abstract class InteractableTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Event triggered when an object is interacted with by the controller.
        /// </summary>
        [Gesture("XRController"), Action("Interacted")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnInteracted = new TrackingBehaviourEvent<InteractableEventArgs>();
        
        /// <summary>
        /// Event triggered when an object is touched by the controller.
        /// </summary>
        [Gesture("XRController"), Action("Touched")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnTouched = new TrackingBehaviourEvent<InteractableEventArgs>();
        
        /// <summary>
        /// Event triggered when an object is released by the controller.
        /// </summary>
        [Gesture("XRController"), Action("Released")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnReleased = new TrackingBehaviourEvent<InteractableEventArgs>();

        /// <summary>
        /// Event triggered when an object is grabbed by the controller.
        /// </summary>
        [Gesture("XRController"), Action("Grabbed")]
        public TrackingBehaviourEvent<InteractableEventArgs> OnGrabbed = new TrackingBehaviourEvent<InteractableEventArgs>();
    }
}