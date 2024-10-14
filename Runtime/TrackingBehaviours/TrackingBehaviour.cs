using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours
{
    [DefaultExecutionOrder(-1)]
    public abstract class TrackingBehaviour : PipelineComponent
    {
        protected Pipeline pipeline { get; private set; }
        public Actor GetActor() => pipeline.actor;
        protected virtual void Awake()
        {
            pipeline = GetComponentInParent<Pipeline>(true);

            // cannot find a pipeline. Look for a Pipeline Extension
            if (!pipeline)
            {
                var pipelineExt = GetComponentInParent<IPipelineExtension>();
                pipeline = pipelineExt.GetPipeline();
            }
            
            pipeline.AfterFoundObjects += AfterFoundObjects;
            pipeline.AfterFilteredObjects += AfterFilteredObjects;
            pipeline.BeforeStoppedPipeline += (p) => Dispose(p.trackingObjects.ToArray());
        }
        
        protected virtual void AfterFoundObjects(Object[] objects) {}
        protected abstract void AfterFilteredObjects(Object[] objects);

        protected void OnEnable()
        {
            
        }

        protected void DisposeAllTrackingEvents()
        {
            // Get all fields of type ITrackingBehaviourEvent
            var fields = GetTrackingBehaviourEvents();

            foreach (var field in fields)
            {
                // Get the value of the field from the current instance
                var fieldValue = field.GetValue(this) as ITrackingBehaviourEvent;

                // Call Dispose if the field is not null
                fieldValue?.UnbindAll();
            }
        }
        
        protected virtual void Dispose(Object[] objects)
        {
            DisposeAllTrackingEvents();
        }
        
        public FieldInfo[] GetTrackingBehaviourEvents()
        {
            return GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => typeof(ITrackingBehaviourEvent).IsAssignableFrom(f.FieldType))
                .ToArray();
        }
        
        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline '{pipeline.name}') " + message);
        
        protected T[] Select<T>(Object[] objects) where T : Object
            => objects
                .Where(o => o.GetType().IsSubclassOf(typeof(T)))
                .Select(o => o as T).ToArray();
    }
}