using System;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    public class GazeDetector : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [Tooltip("Welche Layer sollen für den Gaze-Raycast berücksichtigt werden?")]
        public LayerMask layersToInclude = ~0;

        [Tooltip("Maximale Raycast-Distanz in Metern.")]
        public float rayDistance = 10.0f;

        [Tooltip("Wie viele Treffer sollen maximal in den Buffer geschrieben werden?")]
        [SerializeField] private int maxHits = 8;

        [Tooltip("Wie sollen Trigger-Collider behandelt werden?")]
        [SerializeField] private QueryTriggerInteraction triggerMode = QueryTriggerInteraction.Ignore;

        [Tooltip("Kleiner Vorwärts-Offset (Meter), um Self-Hits zu vermeiden (z.B. 0.01).")]
        [SerializeField] private float originForwardOffset = 0.0f;

        public GazeHit LastHit { get; private set; }

        public event GazeHitHandler OnEnter;
        public event GazeHitHandler OnLeave;
        public event GazeHitHandler OnUpdate;

        private Ray _cachedRay;
        private RaycastHit[] _hits;

        [field: SerializeField, ReadOnly]
        public Component OwnerComponent { get; private set; }

        private void Awake()
        {
            maxHits = Mathf.Max(1, maxHits);
            _hits = new RaycastHit[maxHits];
        }

        // Bequeme Overloads
        public GazeHit PerformRaycast(bool updateState = false)
            => PerformRaycast(layersToInclude, rayDistance, updateState);

        public GazeHit PerformRaycast(float rayDis = 10.0f, bool updateState = false)
            => PerformRaycast(layersToInclude, rayDis, updateState);

        /// <summary>
        /// Führt einen garbage-freien Raycast in Blickrichtung aus und wählt deterministisch den nächstgelegenen Treffer.
        /// Optional aktualisiert die Methode internen Zustand und feuert Events.
        /// </summary>
        public GazeHit PerformRaycast(LayerMask layers, float rayDis = 10.0f, bool updateState = false)
        {
            // Blickstrahl setzen (ohne Allokation)
            var gazeDirection = transform.forward;
            var gazeOrigin    = transform.position;

            if (originForwardOffset > 0f)
                gazeOrigin += gazeDirection * originForwardOffset;

            _cachedRay.origin    = gazeOrigin;
            _cachedRay.direction = gazeDirection;

            // Einmaliger NonAlloc-Raycast in den wiederverwendbaren Buffer
            var hitCount = Physics.RaycastNonAlloc(_cachedRay, _hits, rayDis, layers, triggerMode);

            if (hitCount > 0)
            {
                // Aus dem bereits gefüllten Buffer den nächsten Treffer bestimmen
                var closestHit = GetClosestHitFromBuffer(hitCount);

                var gazeHit = new GazeHit(this, closestHit, gazeDirection, gazeOrigin);

                if (updateState)
                    UpdateStateWith(gazeHit);

                return gazeHit;
            }

            if (LastHit != null && updateState)
                TriggerLeave();

            return null;
        }

        /// <summary>
        /// Wählt den nächstgelegenen Treffer aus dem vorhandenen _hits-Buffer (Indexbereich [0, hitCount)).
        /// </summary>
        private RaycastHit GetClosestHitFromBuffer(int hitCount)
        {
            var closest = _hits[0];
            for (var i = 1; i < hitCount; i++)
            {
                // NaN-Schutz ist i.d.R. nicht nötig, kann aber bei fehlerhaften Collidern helfen
                if (_hits[i].distance < closest.distance)
                    closest = _hits[i];
            }
            return closest;
        }

        private void UpdateStateWith(GazeHit gazeHit)
        {
            // WICHTIG: Collider statt GameObject vergleichen (Objekte können mehrere Collider haben)
            var newCol  = gazeHit.RayHit.collider;
            var lastCol = LastHit?.RayHit.collider;

            var isNew = LastHit == null || newCol != lastCol;

            if (isNew)
            {
                if (LastHit != null)
                    TriggerLeave();

                LastHit = gazeHit;
                OnEnter?.Invoke(gazeHit);
            }

            LastHit = gazeHit;
            OnUpdate?.Invoke(gazeHit);
        }

        private void TriggerLeave()
        {
            if (LastHit == null)
                return;

            OnLeave?.Invoke(LastHit);
            LastHit = null;
        }

        public void AssignOwner(Component component) => OwnerComponent = component;

        public void AssignOwner<T>() where T : Component
        {
            var comp = gameObject.GetComponent<T>();
            OwnerComponent = comp;
        }

        public T GetOwner<T>() where T : Component => (T)OwnerComponent;
    }
}
