using System.Linq;
using UnityEngine;

namespace OmiLAXR
{
    public abstract class PipelineStage : MonoBehaviour
    {
        protected Pipeline pipeline { get; private set; }
        public PipelineActor GetActor() => pipeline.actor;
        protected virtual void Awake()
        {
            pipeline = GetComponentInParent<Pipeline>();
            pipeline.afterFoundObjects += AfterFoundObjects;
            pipeline.afterFilteredObjects += AfterFilteredObjects;
            pipeline.afterBindTrackingBehavior += AfterBindObjects;
        }
        
        protected virtual void AfterFoundObjects(Object[] objects) {}
        protected virtual void AfterFilteredObjects(Object[] objects) {}
        protected virtual void AfterBindObjects(Object[] objects) {}
        
        protected void Log(string message, params object[] ps)
            => OmiLAXR.DebugLog.OmiLAXR.Print($"(Pipeline {pipeline.name}) " + message);
        
        protected T[] Select<T>(Object[] objects) where T : Object
            => objects.Where(o => o.GetType().IsSubclassOf(typeof(T))).Select(o => o as T).ToArray();
    }
}