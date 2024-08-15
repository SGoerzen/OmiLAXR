using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / <Collider> Objects Listener")]
    public class ColliderGameObjectsListener : Listener
    {
        public override void StartListening()
        {
            var colliders = FindObjectsOfType<Collider>();
            Found(colliders);
        }
    }
}