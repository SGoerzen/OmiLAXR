using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR
{
    public class GameObjectStateWatcher : MonoBehaviour
    {
        public UnityEvent<GameObject> OnDestroyed = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> OnEnabled = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> OnDisabled = new UnityEvent<GameObject>();
        
        private void OnDestroy()
        {
            OnDestroyed?.Invoke(gameObject);
        }

        private void OnEnable()
        {
            OnEnabled?.Invoke(gameObject);
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke(gameObject);
        }
    }
}