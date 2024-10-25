using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    [AddComponentMenu("OmiLAXR / 1) Listeners / <Collider> Objects Listener")]
    [Description("Provides all <Collider> components to pipeline.")]
    public class ColliderGameObjectsListener : Listener
    {
        public override void StartListening()
        {
            var colliders = FindObjectsOfType<Collider>();
            Found(colliders);
        }
    }
}