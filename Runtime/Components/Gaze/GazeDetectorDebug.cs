using System;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    [RequireComponent(typeof(LineRenderer), typeof(GazeDetector))]
    public class GazeDetectorDebug : MonoBehaviour
    {
        [Serializable]
        public struct GazeDetectorDebugSettings
        {
            public bool enabled;
            public float rayWidth;
            public Color rayColor;
            public Color rayHoverColor;
            public GameObject hitPointGameObject;
            
            public static GazeDetectorDebugSettings Default => new GazeDetectorDebugSettings()
            {
                enabled = false,
                rayWidth = 0.01f,
                rayColor = Color.yellow,
                rayHoverColor = Color.red,
                hitPointGameObject = null
            };
        }
        private LineRenderer _lineRenderer;
        private GazeDetector _gazeDetector;
        private GameObject _hitPointGameObject;
        private bool _isSetup = false;
        
        public void Setup(GazeDetectorDebugSettings s, float rayDistance)
        {
            if (_isSetup)
                return;
            
            if (!_lineRenderer)
                _lineRenderer = GetComponent<LineRenderer>();
            
            if (!_gazeDetector)
                _gazeDetector = GetComponent<GazeDetector>();

            _hitPointGameObject = s.hitPointGameObject;
            
            _gazeDetector.OnEnter += hit =>
            {
                _lineRenderer.startColor = s.rayHoverColor;
                _lineRenderer.endColor = s.rayHoverColor;
            };
            
            _gazeDetector.OnLeave += hit =>
            {
                _lineRenderer.startColor = s.rayColor;
                _lineRenderer.endColor = s.rayColor;
            };
            
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = s.rayWidth;
            _lineRenderer.endWidth = s.rayWidth;
            _lineRenderer.startColor = s.rayColor;
            _lineRenderer.endColor = s.rayHoverColor;
            
            var shader = Shader.Find("Sprites/Default");
            var material = new Material(shader);
            _lineRenderer.material = material;
            
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));

            _isSetup = true;
        }

        private void Update()
        {
            if (!_hitPointGameObject)
                return;
            var lastHit = _gazeDetector.LastHit;
            if (lastHit == null)
            {
                _hitPointGameObject.transform.position = new Vector3(transform.position.x, transform.position.y,
                    transform.position.z + _gazeDetector.rayDistance);
                return;
            }

            _hitPointGameObject.transform.position = lastHit.RayHit.point;
        }
    }
}