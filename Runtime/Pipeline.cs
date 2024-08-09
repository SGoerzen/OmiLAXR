using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Data;
using OmiLAXR.Listeners;
using OmiLAXR.Pipelines.Filters;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Pipeline System containing many stages.
    /// </summary>
    public class Pipeline : MonoBehaviour
    {
        public PipelineActor actor;
        
        [HideInInspector]
        public List<Listener> listeners;
        [HideInInspector]
        public List<DataProvider> dataProviders;
        [HideInInspector]
        public List<TrackingBehaviour> trackingBehaviours;      
        [HideInInspector]
        public List<Filter> filters;
        
        public static T GetPipeline<T>() where T : Pipeline
            => FindObjectOfType<T>();

        public static Pipeline GetAll() => FindObjectOfType<Pipeline>();
        
        public T GetDataProvider<T>() where T : DataProvider
            => dataProviders.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetTrackingBehaviour<T>() where T : TrackingBehaviour
            => trackingBehaviours.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetFilters<T>() where T : Filter
            => filters.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetListener<T>() where T : Listener
            => listeners.OfType<T>().Select(listener => listener as T).FirstOrDefault();

        public event System.Action<Object[]> afterFoundObjects;
        public event System.Action<Object[]> afterFilteredObjects;
        public event System.Action<Object[]> afterBindTrackingBehavior;

        public readonly List<Object> trackingObjects = new List<Object>();
        
        private void Awake()
        {
            trackingBehaviours = GetComponentsInChildren<TrackingBehaviour>().ToList();
            
            // Find available listeners
            listeners = GetComponentsInChildren<Listener>().ToList();
            
            // Find available data providers
            filters = GetComponentsInChildren<Filter>().ToList();
            
            // Find available data providers
            dataProviders = FindObjectsOfType<DataProvider>().ToList();
            
            DebugLog.OmiLAXR.Print($"Started Pipeline {name} with {listeners.Count} listeners, {filters.Count} filters and {dataProviders.Count} data providers" );
        }

        protected void Log(string message, params object[] ps)
            => OmiLAXR.DebugLog.OmiLAXR.Print($"(Pipeline {name}) " + message);

        private void Start()
        {
            // 1) Start listening for events
            foreach (var listener in listeners)
            {
                listener.onFoundObjects += FoundObjects;
                listener.StartListening();
            }
        }

        public void Register(Listener listener)
            => listeners.Add(listener);

        public void Register(DataProvider dataProvider)
            => dataProviders.Add(dataProvider);

        public void Register(Filter filter)
            => filters.Add(filter);

        public void Register(TrackingBehaviour trackingBehaviour)
            => trackingBehaviours.Add(trackingBehaviour);

        private void FoundObjects(UnityEngine.Object[] objects)
        {
            afterFoundObjects?.Invoke(objects);
            
            Log($"Found {objects.Length} objects.");

            // 2) apply all filters
            objects = filters.Aggregate(objects, (gos, filter) => filter.Pass(gos));
            afterFilteredObjects?.Invoke(objects);
            
            trackingObjects.AddRange(objects);
        }
    }
}