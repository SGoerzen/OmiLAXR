using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.System
{
    public class GameObjectsStateTrackingBehaviour : TrackingBehaviour
    {
        public class GameObjectStateListener : MonoBehaviour
        {
            public UnityEvent<GameObject> onDestroyed;

            private void OnDestroy()
            {
                onDestroyed?.Invoke(gameObject);
            }
        }

        public bool watchOnDestroyed;
        public TrackingBehaviourEvent<GameObjectStateListener, GameObject> OnDestroyedGameObject; 
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            var gameObjects = Select<GameObject>(objects);
            foreach (var go in gameObjects)
            {
                var goStateListener = go.AddComponent<GameObjectStateListener>();
                
                if (watchOnDestroyed)
                {
                    OnDestroyedGameObject.Bind(goStateListener.onDestroyed, g =>
                    {
                        OnDestroyedGameObject?.Invoke(this, goStateListener, g);
                    });
                }
            }
        }
    }
}