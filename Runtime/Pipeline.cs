using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Hooks;
using OmiLAXR.Listeners;
using OmiLAXR.Pipelines.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Pipeline System containing many stages.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Pipeline")]
    [DefaultExecutionOrder(0)]
    public class Pipeline : MonoBehaviour
    {
        public Actor actor;
        
        [HideInInspector] public List<Listener> listeners;
        [HideInInspector] public List<DataProvider> dataProviders;
        [HideInInspector] public List<TrackingBehaviour> trackingBehaviours;      
        [HideInInspector] public List<Filter> filters;
        
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
        public event System.Action<IStatement> afterComposedObjects; 
        public event System.Action<IStatement> beforeSendObjects; 
        public event System.Action<IStatement> afterSendObjects; 

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

            var composersCount = dataProviders.Aggregate(0, (i, provider) => i + provider.composers.Count);
            var hooksCount = dataProviders.Aggregate(0, (i, provider) => i + provider.hooks.Count);
            
            DebugLog.OmiLAXR.Print($"Started Pipeline {name} with {listeners.Count} listeners, {filters.Count} filters, {composersCount} composers, {hooksCount} hooks and {dataProviders.Count} data providers" );
        }

        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline {name}) " + message);

        private void Start()
        {
            // 1) Start listening for events
            foreach (var listener in listeners.Where(l => l.enabled))
            {
                listener.onFoundObjects += FoundObjects;
                listener.StartListening();
            }
        }

        private void FoundObjects(UnityEngine.Object[] objects)
        {
            afterFoundObjects?.Invoke(objects);

            var found = objects.Length;
            Log($"Found {found} objects.");

            // 2) apply all filters
            objects = filters.Aggregate(objects, (gos, filter) => filter.Pass(gos));
            afterFilteredObjects?.Invoke(objects);
            
            Log($"Filtered {found - objects.Length} objects.");
            
            trackingObjects.AddRange(objects);
        }
    }
}