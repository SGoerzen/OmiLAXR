using UnityEngine;

namespace OmiLAXR.Listeners
{
    public class SceneGameObjectsListener : Listener
    {
        public bool includeInactive = true;
        public override void StartListening()
        {
            var gameObjects = FindObjectsOfType<GameObject>(includeInactive);
            Register(gameObjects);
        }
    }
}