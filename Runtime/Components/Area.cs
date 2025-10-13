using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Components
{
    [RequireComponent(typeof(Collider))]
    public class Area : MonoBehaviour
    {
        private Dictionary<int, bool> _insideObjects = new Dictionary<int, bool>();
        private Collider _collider;
        // public SnapshotCamera snapshotCamera;

        public event Action<Transform, Area, Vector3> OnEnter;
        public event Action<Transform, Area, Vector3> OnLeave;
        
        private void Start()
        {
            _collider = GetComponent<Collider>();
        }

        // Returns true if `point` lies inside `col` (within epsilon).
        public bool IsInside(Vector3 point, float epsilon = 1e-6f)
        {
            var closest = _collider.ClosestPoint(point);
            return (closest - point).sqrMagnitude <= (epsilon * epsilon);
        }

        public void DoCollisionCheck(Transform t)
        {
            if (t == null || !enabled || !gameObject.activeSelf)
                return;
            var point = t.position;
            var isInside = IsInside(point);
            var instanceId = transform.GetInstanceID();
            var keyExists = _insideObjects.ContainsKey(instanceId);
            var v = keyExists && _insideObjects[instanceId];
            if (isInside)
            {
                if (v)
                    return; // do nothing
                _insideObjects[instanceId] = true;
                OnEnter?.Invoke(t, this, point);
            }
            else
            {
                if (!v)
                    return;
                _insideObjects[instanceId] = false;
                OnLeave?.Invoke(t, this, point);
            }
        }
    }
}