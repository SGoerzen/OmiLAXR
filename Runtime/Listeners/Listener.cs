using System;
using UnityEngine;

namespace OmiLAXR.Listeners
{
    public abstract class Listener : MonoBehaviour
    {
        public MainTrackingBehaviour mainTrackingBehaviour;
        private void Awake()
        {
            mainTrackingBehaviour = GetComponentInParent<MainTrackingBehaviour>();
        }

        public abstract void StartListening();

        public void Register(params GameObject[] gameObjects)
        {
            mainTrackingBehaviour.Register(gameObjects);
        }
    }
}