using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR
{
    public class GameObjectStateWatcher : MonoBehaviour
    {
        public UnityEvent<GameObject> onDestroyed = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> onEnabled = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> onDisabled = new UnityEvent<GameObject>();
        
        private void OnDestroy()
        {
            onDestroyed.Invoke(gameObject);
        }

        private void OnEnable()
        {
            onEnabled.Invoke(gameObject);
        }

        private void OnDisable()
        {
            onDisabled.Invoke(gameObject);
        }
    }
}