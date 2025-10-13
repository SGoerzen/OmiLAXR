using UnityEngine;

namespace OmiLAXR.Components.Gaze.Pursuit
{
    public delegate void PursuitStartedHandler(GazeHit gazeHit, PursuitData data);
    public delegate void PursuitEndedHandler(GazeHit gazeHit, PursuitData data);

    [RequireComponent(typeof(GazeDetector))]
    public sealed class PursuitDetector : MonoBehaviour
    {
        [Tooltip("Smooth Pursuit Logic (ScriptableObject)")]
        public PursuitLogic pursuitLogic;

        [Tooltip("Transform of the XR HMD (Camera Rig)")]
        public Transform hmdTransform;

        public event PursuitStartedHandler OnPursuitStarted;
        public event PursuitEndedHandler OnPursuitEnded;

        private GazeDetector _gazeDetector;
        private bool _hasPrev;
        private Vector3 _prevEyeDir;
        private Vector3 _prevTargetDir;

        private void Awake()
        {
            _gazeDetector = GetComponent<GazeDetector>();

            if (pursuitLogic == null)
                pursuitLogic = PursuitLogic.GetDefault();

            if (hmdTransform == null && Camera.main != null)
                hmdTransform = Camera.main.transform;

            if (pursuitLogic == null)
            {
                DebugLog.OmiLAXR.Error("There is no Pursuit Logic assigned.");
                return;
            }
            
            pursuitLogic.ResetLogic();

            if (_gazeDetector)
            {
                _gazeDetector.OnUpdate += HandleGazeUpdate;
                _gazeDetector.OnLeave  += HandleLeave;
            }
        }

        private void HandleGazeUpdate(GazeHit hit)
        {
            if (hmdTransform == null || pursuitLogic == null) return;

            var origin = hmdTransform.position;

            // Blickrichtung aus HMD → Gaze-Schnittpunkt (Fallback: HMD-Forward)
            var gazePoint = (hit != null && hit.RayHit.collider != null)
                ? hit.RayHit.point
                : (origin + hmdTransform.forward);
            var currEyeDir = (gazePoint - origin).normalized;

            // Zielrichtung aus HMD → AOI-Pivot (Collider-Transform)
            var targetTf = (hit != null && hit.RayHit.collider != null)
                ? hit.RayHit.collider.transform
                : null;

            var targetPos = targetTf != null ? targetTf.position : (origin + hmdTransform.forward);
            var currTargetDir = (targetPos - origin).normalized;

            if (!_hasPrev)
            {
                _prevEyeDir = currEyeDir;
                _prevTargetDir = currTargetDir;
                _hasPrev = true;
                return;
            }

            if (pursuitLogic.TryUpdatePursuit(
                    hit,
                    _prevEyeDir,    currEyeDir,
                    _prevTargetDir, currTargetDir,
                    Time.deltaTime,
                    out var isStart,
                    out var data))
            {
                if (isStart)
                    OnPursuitStarted?.Invoke(hit, data);
                else if (data != null)
                    OnPursuitEnded?.Invoke(hit, data);
            }

            _prevEyeDir = currEyeDir;
            _prevTargetDir = currTargetDir;
        }
        
        private void HandleLeave(GazeHit _) => pursuitLogic?.ResetLogic();

        private void OnDisable()
        {
            if (_gazeDetector)
            {
                _gazeDetector.OnUpdate -= HandleGazeUpdate;
                _gazeDetector.OnLeave  -= HandleLeave;
            }
        }
    }
}
