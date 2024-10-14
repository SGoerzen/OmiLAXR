using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR
{
    [AddComponentMenu("OmiLAXR / Game Objects / Transform Watcher")]
    [DisallowMultipleComponent]
    public class TransformWatcher : MonoBehaviour
    {
        public struct TransformChange
        {
            public Vector3 OldValue;
            public Vector3 NewValue;
        }
        
        public float positionThreshold = .5f;
        public float rotationThreshold = 1.0f;
        public float scaleThreshold = 0.1f;
        
        private Vector3 _lastPosition;
        private Vector3 _lastScale;
        private Vector3 _lastRotation;

        public UnityEvent<TransformChange> onChangedPosition = new UnityEvent<TransformChange>();
        public UnityEvent<TransformChange> onChangedScale = new UnityEvent<TransformChange>();
        public UnityEvent<TransformChange> onChangedRotation = new UnityEvent<TransformChange>();

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
                onChangedPosition.Invoke(new TransformChange()
                {
                    NewValue = pos,
                    OldValue = _lastPosition
                });
            }
            if (DetectChange(ref _lastRotation, rotation, rotationThreshold))
            {
                onChangedRotation.Invoke(new TransformChange()
                {
                    NewValue = rotation,
                    OldValue = _lastRotation
                });
            }
            if (DetectChange(ref _lastScale, scale, scaleThreshold))
            {
                onChangedScale.Invoke(new TransformChange()
                {
                    NewValue = scale,
                    OldValue = _lastScale
                });
            }
        }
    }
}