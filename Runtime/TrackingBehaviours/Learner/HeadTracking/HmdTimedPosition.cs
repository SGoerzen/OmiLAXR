using System;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    //value type for collecting the position of the hmd including the corresponding timestamp of sampling
    public struct HmdTimedPosition
    {
        public DateTime Timestamp;
        public Vector3 Position;

        public HmdTimedPosition(DateTime timestamp, Vector3 position)
        {
            Timestamp = timestamp;
            Position = position;
        }
    }

}