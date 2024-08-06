using UnityEngine;

namespace OmiLAXR.Listeners
{
    public abstract class Listener : PipelineStage
    {
        public event System.Action<Object[]> onFoundObjects;
        public abstract void StartListening();

        protected override void Pipe(Object[] objects)
        {
            onFoundObjects?.Invoke(objects);
        }
    }
}