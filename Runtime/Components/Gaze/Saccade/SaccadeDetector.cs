using System;
using OmiLAXR.Components.Gaze.Saccade;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    [RequireComponent(typeof(GazeDetector))]
    public sealed class SaccadeDetector : MonoBehaviour
    {
        [Tooltip("Saccade Logic (ScriptableObject-based)")]
        public SaccadeLogic saccadeLogic;

        [Tooltip("Transform of the XR HMD (Camera Rig)")]
        public Transform hmdTransform;

        public event SaccadeStartedHandler OnSaccadeStarted;
        public event SaccadeEndedHandler OnSaccadeEnded;

        private GazeDetector _gazeDetector;
        private bool _hasPrev;
        private Vector3 _prevDir;
        private GazeHit _prevHit;

        private void Awake()
        {
            _gazeDetector = GetComponent<GazeDetector>();

            if (saccadeLogic == null)
                saccadeLogic = SaccadeLogic.GetDefault();

            if (hmdTransform == null && Camera.main != null)
                hmdTransform = Camera.main.transform;

            saccadeLogic?.ResetLogic();

            if (_gazeDetector)
            {
                _gazeDetector.OnUpdate += HandleGaze;
                _gazeDetector.OnLeave += HandleLeave;
            }
        }

        private void HandleLeave(GazeHit _) => saccadeLogic?.ResetLogic();

        private void HandleGaze(GazeHit hit)
        {
            if (hmdTransform == null || saccadeLogic == null) return;

            var origin = hmdTransform.position;
            var currentPoint = (hit != null && hit.RayHit.collider != null) ? hit.RayHit.point : (origin + hmdTransform.forward);
            var currentDir = (currentPoint - origin).normalized;

            if (!_hasPrev)
            {
                _prevDir = currentDir;
                _prevHit = hit;
                _hasPrev = true;
                return;
            }

            if (saccadeLogic.TryUpdateSaccade(
                    _prevHit, hit, _prevDir, currentDir,
                    Time.deltaTime, pupilDiameterMillimeters: null,
                    out var isStart, out var data))
            {
                if (isStart) OnSaccadeStarted?.Invoke(hit, data);
                else         OnSaccadeEnded?.Invoke(hit, data);
            }

            _prevDir = currentDir;
            _prevHit = hit;
        }

        private void OnDisable()
        {
            if (_gazeDetector)
            {
                _gazeDetector.OnUpdate -= HandleGaze;
                _gazeDetector.OnLeave -= HandleLeave;
            }
        }
    }
}
