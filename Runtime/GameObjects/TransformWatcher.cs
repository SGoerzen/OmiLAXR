using System;
using UnityEngine;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Game Objects / Transform Watcher")]
    [DisallowMultipleComponent]
    public class TransformWatcher : MonoBehaviour
    {
        public delegate void TransformChangedEvent(Vector3 newValue, Vector3 oldValue);
        
        public float positionThreshold = .5f;
        public float rotationThreshold = 1.0f;
        public float scaleThreshold = 0.1f;
        
        private Vector3 _lastPosition;
        private Vector3 _lastScale;
        private Vector3 _lastRotation;

        public event TransformChangedEvent OnChangedPosition;
        public event TransformChangedEvent OnChangedScale;
        public event TransformChangedEvent OnChangedRotation;

        private bool DetectChange(ref Vector3 curValue, Vector3 newValue, float threshold)
        {
            var dis = Vector3.Distance(newValue, Vector3.one * threshold);
            if (dis <= threshold)
                return false;
            curValue = newValue;
            return true;
        }
        
        private void Update()
        {
            var pos = transform.position;
            var scale = transform.localScale;
            var rotation = transform.eulerAngles;

            if (DetectChange(ref _lastPosition, pos, positionThreshold))
            {
                OnChangedPosition?.Invoke(pos, _lastPosition);
            }
            if (DetectChange(ref _lastRotation, rotation, rotationThreshold))
            {
                OnChangedRotation?.Invoke(rotation, _lastRotation);
            }
            if (DetectChange(ref _lastScale, scale, scaleThreshold))
            {
                OnChangedScale?.Invoke(scale, _lastScale);
            }
        }
    }
}