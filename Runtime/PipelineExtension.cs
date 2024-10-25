using System;
using System.Linq;
using OmiLAXR.Extensions;
using OmiLAXR.Listeners;
using OmiLAXR.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-1)]
    public abstract class PipelineExtension : PipelineComponent
    {
        public Pipeline Pipeline { get; private set; }
        public void Extend(Pipeline pipeline)
        {
            var listeners = gameObject.GetComponentsInChildren<Listener>();
            var tbs = gameObject.GetComponentsInChildren<ITrackingBehaviour>();
            var filters = gameObject.GetComponentsInChildren<Filter>();
            var extensions = Array.Empty<PipelineComponent>();
            extensions = extensions.Concat(listeners).Concat(tbs as PipelineComponent[]).Concat(filters).ToArray();
            foreach (var ext in extensions)
            {
                // register in pipeline, just fyi
                var extWrapper = pipeline.gameObject.AddComponent<Extension>();
                extWrapper.extensionComponent = ext;
                extWrapper.pipelineExtension = this;
                pipeline.Add(ext);
            }

            Pipeline = pipeline;
            DebugLog.OmiLAXR.Print("Extended pipeline " + pipeline);
        }
        
    }
}