using OmiLAXR.Extensions;
using OmiLAXR.Listeners;
using OmiLAXR.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-1)]
    public abstract class PipelineExtension<T> : PipelineComponent, IPipelineExtension
    where T : Pipeline
    {
        protected T Pipeline;
        private void Awake()
        {
            Pipeline = FindObjectOfType<T>(true);
            Pipeline.OnCollect += pipeline =>
            {
                var extensions = OnExtend();
                foreach (var ext in extensions)
                {
                    // add to lists
                    pipeline.Add(ext);
                    // register in pipeline, just fyi
                    var extWrapper = pipeline.gameObject.AddComponent<Extension>();
                    extWrapper.extensionComponent = ext;
                    extWrapper.pipelineExtension = this;
                }    
            }; 
            DebugLog.OmiLAXR.Print("Extended pipeline " + typeof(T));
        }

        protected void Add(Listener listener)
            => this.Pipeline.Add(listener);

        protected void Add(Filter filter)
            => this.Pipeline.Add(filter);

        protected void Add(TrackingBehaviour trackingBehaviour)
            => this.Pipeline.Add(trackingBehaviour);

        protected abstract PipelineComponent[] OnExtend();
        public Pipeline GetPipeline() => Pipeline;
    }
}