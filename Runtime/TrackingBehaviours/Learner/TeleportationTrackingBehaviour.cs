/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System.ComponentModel;
using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    /// <summary>
    /// Base class for tracking teleportation events in VR environments.
    /// Captures teleportation start/end states and target information.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Teleportation Tracking Behaviour"),
     Description("Tracks teleportation events of <TeleportationProvider>, <TeleportationArea> and <TeleportationAnchor> components.")]
    public abstract class TeleportationTrackingBehaviour : TrackingBehaviour
    {
        /// <summary>
        /// Represents the player's state at a specific moment during teleportation.
        /// </summary>
        public struct TeleportationPlayerState
        {
            /// <summary>
            /// Floor position of the player.
            /// </summary>
            public Vector3 FloorPosition;
            
            /// <summary>
            /// Direction the camera is facing.
            /// </summary>
            public Vector3 CameraGazeDirection;
        }

        /// <summary>
        /// Types of teleportation targets available in the system.
        /// </summary>
        public enum TeleportationTargetType
        {
            Floor,
            Anchor,
            Area,
            Object,
            Custom
        }

        /// <summary>
        /// Complete teleportation event data including states, target, and timing information.
        /// </summary>
        public struct TeleportationArgs
        {
            /// <summary>
            /// Y-axis offset of the camera from floor position.
            /// </summary>
            public float CameraYOffset;
            
            /// <summary>
            /// Player state before teleportation.
            /// </summary>
            public TeleportationPlayerState StartState;
            
            /// <summary>
            /// Player state after teleportation.
            /// </summary>
            public TeleportationPlayerState EndState;
            
            /// <summary>
            /// The target GameObject for the teleportation.
            /// </summary>
            public GameObject Target;
            
            /// <summary>
            /// Type of teleportation target.
            /// </summary>
            public TeleportationTargetType TargetType;
            
            /// <summary>
            /// Final destination position.
            /// </summary>
            public Vector3 DestinationPosition;
            
            /// <summary>
            /// Final destination rotation.
            /// </summary>
            public Vector3 DestinationRotation;
            
            /// <summary>
            /// Time when teleportation was requested.
            /// </summary>
            public float RequestTime;
            
            /// <summary>
            /// Hand used to initiate teleportation.
            /// </summary>
            public Hand Hand;

            /// <summary>
            /// Creates teleportation event data with all necessary information.
            /// </summary>
            public TeleportationArgs(GameObject target, TeleportationTargetType targetType, float cameraYOffset,
                TeleportationPlayerState startState, TeleportationPlayerState endState, 
                Vector3? destinationPosition = null, Vector3? destinationRotation = null, Hand hand = Hand.Unknown)
            {
                Target = target;
                Hand = hand;
                TargetType = targetType;
                CameraYOffset = cameraYOffset;
                StartState = startState;
                EndState = endState;
                DestinationPosition = destinationPosition ?? Vector3.zero;
                DestinationRotation = destinationRotation ?? Vector3.zero;
                RequestTime = 0.0f;
            }
        }

        /// <summary>
        /// Event triggered when a teleportation occurs.
        /// </summary>
        [Gesture("XRController"), Action("Teleport")]
        public readonly TrackingBehaviourEvent<TeleportationArgs> OnTeleported
            = new TrackingBehaviourEvent<TeleportationArgs>();
    }
}