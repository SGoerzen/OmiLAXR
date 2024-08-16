using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.System
{
    public class GameObjectsStateTrackingBehaviour : TrackingBehaviour
    {
        public class GameObjectStateListener : MonoBehaviour
        {
            public event Action<GameObject> OnDestroyed;

            private void OnDestroy()
            {
                OnDestroyed?.Invoke(gameObject);
            }
            
            // todo more is possible
        }

        public bool watchOnDestroyed;
        public event TrackingBehaviourAction<GameObject> OnDestroyedGameObject; 
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            var gameObjects = Select<GameObject>(objects);
            foreach (var go in gameObjects)
            {
                var goStateListener = go.AddComponent<GameObjectStateListener>();
                
                if (watchOnDestroyed)
                    goStateListener.OnDestroyed += g => OnDestroyedGameObject?.Invoke(this, g);
            }
        }
    }
}