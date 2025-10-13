using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Components.Gaze.Fixation
{
    [CreateAssetMenu(menuName = "OmiLAXR / Gaze / Fixation / Angular Stability",
        fileName = "FixationLogicAngularStability")]
    public class FixationLogicAngularStability : FixationLogic
    {
        public int requiredStableFrames = 15;
        public float maxAnglePerFrame = 0.75f; // in Grad

        private readonly Queue<Vector3> _directionHistory = new Queue<Vector3>();
        private Collider _lastFixated = null;

        public override void ResetLogic()
        {
            _directionHistory.Clear();
            _lastFixated = null;
        }

        public override bool TryUpdateFixation(RaycastHit hit, Transform hmdTransform, out bool isNewFixation)
        {
            isNewFixation = false;

            // Richtung vom HMD zur Hitposition (Headspace)
            var direction = (hit.point - hmdTransform.position).normalized;
            _directionHistory.Enqueue(direction);

            if (_directionHistory.Count > requiredStableFrames)
                _directionHistory.Dequeue();

            if (_directionHistory.Count < requiredStableFrames)
                return false;

            // Cosinus des maximalen Winkels vorab berechnen
            float minDot = Mathf.Cos(maxAnglePerFrame * Mathf.Deg2Rad);

            Vector3? previous = null;
            foreach (var current in _directionHistory)
            {
                if (previous.HasValue)
                {
                    float dot = Vector3.Dot(previous.Value, current);
                    if (dot < minDot)
                        return false;
                }

                previous = current;
            }

            if (hit.collider != _lastFixated)
            {
                _lastFixated = hit.collider;
                _directionHistory.Clear();
                isNewFixation = true;
                return true;
            }

            return false;
        }
    }
}