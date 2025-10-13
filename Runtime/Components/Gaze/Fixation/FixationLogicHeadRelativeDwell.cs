using UnityEngine;

namespace OmiLAXR.Components.Gaze.Fixation
{
    [CreateAssetMenu(menuName = "OmiLAXR / Gaze / Fixation / Head-Relative Dwell", fileName = "FixationLogicHeadRelativeDwell")]
    public class FixationLogicHeadRelativeDwell : FixationLogic
    {
        public float requiredDwellTime = 0.25f; // Sekunden
        public float maxAngularDeviation = 1.5f; // Grad

        private Vector3 _initialDirection;
        private float _startTime;
        private Collider _currentTarget;
        private bool _hasStarted = false;

        public override void ResetLogic()
        {
            _hasStarted = false;
            _currentTarget = null;
            _startTime = 0f;
        }

        public override bool TryUpdateFixation(RaycastHit hit, Transform hmdTransform, out bool isNewFixation)
        {
            isNewFixation = false;
            var currentDirection = (hit.point - hmdTransform.position).normalized;

            if (!_hasStarted || hit.collider != _currentTarget)
            {
                _hasStarted = true;
                _initialDirection = currentDirection;
                _startTime = Time.time;
                _currentTarget = hit.collider;
                return false;
            }

            var angle = Vector3.Angle(_initialDirection, currentDirection);
            var dwellTime = Time.time - _startTime;

            if (angle <= maxAngularDeviation && dwellTime >= requiredDwellTime)
            {
                isNewFixation = true;
                ResetLogic();
                return true;
            }

            if (angle > maxAngularDeviation)
            {
                ResetLogic();
            }

            return false;
        }
    }

}