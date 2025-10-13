using UnityEngine;

namespace OmiLAXR.Components.Gaze.Fixation
{
    [CreateAssetMenu(menuName = "OmiLAXR / Gaze / Fixation / Object Dwell (Strict)", fileName = "FixationLogicObjectDwellStrict")]
    public class FixationLogicObjectDwellStrict : FixationLogic
    {
        public float fixationTime = 0.3f;

        private Collider _target;
        private float _startTime;
        private bool _alreadyFired = false;

        public override void ResetLogic()
        {
            _target = null;
            _alreadyFired = false;
            _startTime = 0f;
        }

        public override bool TryUpdateFixation(RaycastHit hit, Transform hmdTransform, out bool isNewFixation)
        {
            isNewFixation = false;

            if (hit.collider != _target)
            {
                _target = hit.collider;
                _startTime = Time.time;
                _alreadyFired = false;
                return false;
            }

            if (_alreadyFired)
                return false;

            if (Time.time - _startTime >= fixationTime)
            {
                _alreadyFired = true;
                isNewFixation = true;
                return true;
            }

            return false;
        }
    }
}