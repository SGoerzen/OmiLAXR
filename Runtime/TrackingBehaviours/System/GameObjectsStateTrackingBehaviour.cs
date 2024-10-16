using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Game Objects State Tracking Behaviour")]
    [Description("Tracks if a game object state is changed (e.g. is destroyed).")]
    public class GameObjectsStateTrackingBehaviour : TrackingBehaviour<GameObject>
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
        
        protected override void AfterFilteredObjects(GameObject[] gameObjects)
        {
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