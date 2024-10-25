using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.System
{
    [AddComponentMenu("OmiLAXR / 3) Tracking Behaviours / Game Objects State Tracking Behaviour")]
    [Description("Tracks if a game object state is changed (e.g. is destroyed).")]
    public class GameObjectStateTrackingBehaviour : TrackingBehaviour<GameObjectStateWatcher>
    {
        public bool watchOnDestroyed = true;
        public bool watchOnEnabled = true;
        public bool watchOnDisabled = true;
        
        public TrackingBehaviourEvent<GameObjectStateWatcher, GameObject> OnDestroyedGameObject = new TrackingBehaviourEvent<GameObjectStateWatcher, GameObject>(); 
        public TrackingBehaviourEvent<GameObjectStateWatcher, GameObject> OnEnabledGameObject = new TrackingBehaviourEvent<GameObjectStateWatcher, GameObject>(); 
        public TrackingBehaviourEvent<GameObjectStateWatcher, GameObject> OnDisabledGameObject = new TrackingBehaviourEvent<GameObjectStateWatcher, GameObject>(); 
        
        protected override void AfterFilteredObjects(GameObjectStateWatcher[] gameObjects)
        {
            foreach (var go in gameObjects)
            {
                if (watchOnDestroyed)
                {
                    OnDestroyedGameObject.Bind(go.onDestroyed, g =>
                    {
                        OnDestroyedGameObject.Invoke(this, go, g);
                    });
                }

                if (watchOnEnabled)
                {
                    OnEnabledGameObject.Bind(go.onEnabled, g =>
                    {
                        OnEnabledGameObject.Invoke(this, go, g);
                    });
                }

                if (watchOnDisabled)
                {
                    OnDisabledGameObject.Bind(go.onDisabled, g =>
                    {
                        OnDisabledGameObject.Invoke(this, go, g);
                    });
                }
            }
        }
    }
}