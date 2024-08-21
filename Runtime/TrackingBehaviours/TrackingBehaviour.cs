using System.Linq;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours
{
    [DefaultExecutionOrder(-1)]
    public abstract class TrackingBehaviour : MonoBehaviour
    {
        protected Pipeline pipeline { get; private set; }
        public Actor GetActor() => pipeline.actor;
        protected virtual void Awake()
        {
            pipeline = GetComponentInParent<Pipeline>();

            // cannot find a pipeline. Look for a Pipeline Extension
            if (!pipeline)
            {
                var pipelineExt = GetComponentInParent<IPipelineExtension>();
                pipeline = pipelineExt.GetPipeline();
            }
            
            pipeline.AfterFoundObjects += AfterFoundObjects;
            pipeline.AfterFilteredObjects += AfterFilteredObjects;
        }
        
        protected virtual void AfterFoundObjects(Object[] objects) {}
        protected abstract void AfterFilteredObjects(Object[] objects);
        
        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline '{pipeline.name}') " + message);
        
        protected T[] Select<T>(Object[] objects) where T : Object
            => objects.Where(o => o.GetType().IsSubclassOf(typeof(T))).Select(o => o as T).ToArray();
    }
}