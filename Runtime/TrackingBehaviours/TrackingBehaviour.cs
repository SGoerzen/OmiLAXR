using System.Linq;
using System.Reflection;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours
{

    public abstract class TrackingBehaviour : TrackingBehaviour<Object>
    {
        protected override void AfterFilteredObjects(Object[] objects)
        {
            
        }
    }
    
    [DefaultExecutionOrder(-1)]
    public abstract class TrackingBehaviour<T> : ActorPipelineComponent, ITrackingBehaviour
    where T : Object
    {
        protected virtual void Awake()
        {
            Pipeline.AfterStartedPipeline += OnStartedPipeline;
            Pipeline.BeforeStoppedPipeline += OnStoppedPipeline;
            Pipeline.AfterFoundObjects += (objects) =>
            {
                if (!enabled)
                    return;
                // Skip Select<T> if not needed
                AfterFoundObjects(typeof(T) == typeof(Object) ? objects as T[] : Select<T>(objects));
            };
            Pipeline.AfterFilteredObjects += (objects) =>
            {
                if (!enabled)
                    return;
                // Skip Select<T> if not needed

                if (typeof(T) == typeof(Object) || objects.Length == 0)
                    AfterFilteredObjects(objects as T[]);
                else
                {
                    var selectedObjects = Select<T>(objects);
                    SelectedObjects = selectedObjects;
                    AfterFilteredObjects(selectedObjects);
                }
            };
            Pipeline.BeforeStoppedPipeline += (p) => Dispose(p.trackingObjects.ToArray());
        }
        
        protected virtual void OnStartedPipeline(Pipeline pipeline) {}
        protected virtual void OnStoppedPipeline(Pipeline pipeline) {}
        protected virtual void AfterFoundObjects(T[] objects) {}
        protected abstract void AfterFilteredObjects(T[] objects);
        
        protected T[] SelectedObjects = new T[0];

        protected virtual void OnEnable()
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
        
        protected TS[] Select<TS>(Object[] objects) where TS : Object
            => objects
                .Where(o => o.GetType() == typeof(TS) || o.GetType().IsSubclassOf(typeof(TS)))
                .Select(o => o as TS).ToArray();
        protected TS First<TS>(Object[] objects) where TS : Object
            => (TS)objects
                .FirstOrDefault(o => o.GetType() == typeof(TS) || o.GetType().IsSubclassOf(typeof(TS)));
    }
}