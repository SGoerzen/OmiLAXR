using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Listeners
{
    public abstract class Listener : PipelineComponent
    {
        protected Pipeline pipeline { get; private set; }
        public Actor GetActor() => pipeline.actor;
        public event System.Action<Object[]> onFoundObjects;
        public abstract void StartListening();
        
        protected virtual void Awake()
        {
            pipeline = GetComponentInParent<Pipeline>();
        }
        
        protected void Found<T>(T[] objects) where T : Object
        {
            onFoundObjects?.Invoke(objects);
        }
    }
}