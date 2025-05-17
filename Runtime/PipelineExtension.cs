using System;
using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Extensions;
using OmiLAXR.Listeners;
using OmiLAXR.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-100)]
    public abstract class PipelineExtension<T> : PipelineComponent, IPipelineExtension
    where T : Pipeline
    {
        public Pipeline Pipeline { get; protected set; }
        public Pipeline GetPipeline() => Pipeline;
        public readonly List<Listener> Listeners = new List<Listener>();
        public readonly List<ITrackingBehaviour> TrackingBehaviours = new List<ITrackingBehaviour>();
        public readonly List<Filter> Filters = new List<Filter>();
        private void Awake()
        {
            var pipeline = FindObject<T>();
            Extend(pipeline);
        }

        public void Extend(Pipeline pipeline)
        {
            Pipeline = pipeline;

            var listeners = gameObject.GetComponentsInChildren<Listener>();
            var tbs = gameObject.GetComponentsInChildren<ITrackingBehaviour>();
            var filters = gameObject.GetComponentsInChildren<Filter>();
            
            Listeners.AddRange(listeners);
            TrackingBehaviours.AddRange(tbs);
            Filters.AddRange(filters);
            
            Pipeline.Listeners.AddRange(listeners);
            Pipeline.TrackingBehaviours.AddRange(tbs);
            Pipeline.Filters.AddRange(filters);
            Pipeline.Extensions.Add(this);

            DebugLog.OmiLAXR.Print("Extended pipeline " + pipeline);
        }
        
    }
}