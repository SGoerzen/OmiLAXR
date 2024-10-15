using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours
{
    [DefaultExecutionOrder(-1)]
    public abstract class TrackingBehaviour<T> : PipelineComponent, ITrackingBehaviour
    where T : Object
    {
        protected Pipeline pipeline { get; private set; }
        public Actor GetActor() => pipeline.actor;
        public Instructor GetInstructor() => pipeline.instructor;
        protected virtual void Awake()
        {
            pipeline = GetComponentInParent<Pipeline>(true);

            // cannot find a pipeline. Look for a Pipeline Extension
            if (!pipeline)
            {
                var pipelineExt = GetComponentInParent<IPipelineExtension>();
                pipeline = pipelineExt.GetPipeline();
            }
            
            pipeline.AfterFoundObjects += (obj) =>
            {
                AfterFoundObjects(Select<T>(obj));
            };
            pipeline.AfterFilteredObjects += (obj) =>
            {
                AfterFilteredObjects(Select<T>(obj));
            };
            pipeline.BeforeStoppedPipeline += (p) => Dispose(p.trackingObjects.ToArray());
        }
        
        protected virtual void AfterFoundObjects(T[] objects) {}
        protected abstract void AfterFilteredObjects(T[] objects);

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
        
        protected TS[] Select<TS>(Object[] objects) where TS : Object
            => objects
                .Where(o => o.GetType().IsSubclassOf(typeof(TS)))
                .Select(o => o as TS).ToArray();
    }
}