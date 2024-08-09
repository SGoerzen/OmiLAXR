using System.Linq;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    public class ColliderGameObjectsListener : Listener
    {
        public override void StartListening()
        {
            var colliders = FindObjectsOfType<Collider>();
            Found(colliders);
        }
    }
}