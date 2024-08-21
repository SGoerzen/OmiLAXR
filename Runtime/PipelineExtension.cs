using OmiLAXR.Listeners;
using OmiLAXR.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-1)]
    public abstract class PipelineExtension<T> : MonoBehaviour, IPipelineExtension
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
            => this.Pipeline.Add(listener);

        protected void Add(Filter filter)
            => this.Pipeline.Add(filter);

        protected void Add(TrackingBehaviour trackingBehaviour)
            => this.Pipeline.Add(trackingBehaviour);

        protected abstract void Extend(T pipeline);
        public Pipeline GetPipeline() => Pipeline;
    }
}