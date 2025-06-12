using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Teleportation Tracking Behaviour"),
     Description(
         "Tracks teleportation events of <TeleportationProvider>, <TeleportationArea> and <TeleportationAnchor> components.")]
    public abstract class TeleportationTrackingBehaviour : TrackingBehaviour
    {
        public struct TeleportationPlayerState
        {
            public Vector3 FloorPosition;
            public Vector3 CameraGazeDirection;
        }

        public enum TeleportationTargetType
        {
            Floor,
            Anchor,
            Area,
            Object,
            Custom
        }

        public struct TeleportationArgs
        {
            public float CameraYOffset;
            public TeleportationPlayerState StartState;
            public TeleportationPlayerState EndState;
            public GameObject Target;
            public TeleportationTargetType TargetType;
            public Vector3 DestinationPosition;
            public Vector3 DestinationRotation;
            public float RequestTime;
            public Hand Hand;

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
            //public Vector3 DestinationPosition;
            //public Vector3 DestinationRotation;
            //public float RequestTime;
        }

        // as end teleportation throws same end position like start position, we are detecting change on our own

        [Gesture("XRController"), Action("Teleport")]
        public readonly TrackingBehaviourEvent<TeleportationArgs> OnTeleported
            = new TrackingBehaviourEvent<TeleportationArgs>();
    }
}