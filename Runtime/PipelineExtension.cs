using OmiLAXR.Listeners;
using OmiLAXR.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-1)]
    public abstract class PipelineExtension<T> : MonoBehaviour
    where T : Pipeline
    {
        protected T Pipeline;
        private void Awake()
        {
            Pipeline = FindObjectOfType<T>();
            this.Pipeline.OnCollect += pipeline =>
            {
                Extend(this.Pipeline);
            }; 
            DebugLog.OmiLAXR.Print("Extended pipeline " + typeof(T));
        }

        protected void Add(Listener listener)
            => this.Pipeline.Listeners.Add(listener);

        protected void Add(Filter filter)
            => this.Pipeline.Filters.Add(filter);

        protected void Add(TrackingBehaviour trackingBehaviour)
            => this.Pipeline.TrackingBehaviours.Add(trackingBehaviour);

        protected abstract void Extend(T pipeline);
    }
}