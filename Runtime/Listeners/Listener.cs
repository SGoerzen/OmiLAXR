using UnityEngine;

namespace OmiLAXR.Listeners
{
    public abstract class Listener : PipelineStage
    {
        public event System.Action<Object[]> onFoundObjects;
        public abstract void StartListening();

        protected void Found<T>(T[] objects) where T : Object
        {
            onFoundObjects?.Invoke(objects);
        }
    }
}