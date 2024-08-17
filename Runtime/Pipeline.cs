using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
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
        
        public Listener[] Listeners { get; private set; }
        public DataProvider[] DataProviders { get; private set; }
        public TrackingBehaviour[] TrackingBehaviours { get; private set; }   
        public Filter[] Filters { get; private set; }   
        
        public static T GetPipeline<T>() where T : Pipeline
            => FindObjectOfType<T>();

        public static Pipeline GetAll() => FindObjectOfType<Pipeline>();
        
        public T GetDataProvider<T>() where T : DataProvider
            => DataProviders.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetTrackingBehaviour<T>() where T : TrackingBehaviour
            => TrackingBehaviours.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetFilters<T>() where T : Filter
            => Filters.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetListener<T>() where T : Listener
            => Listeners.OfType<T>().Select(listener => listener as T).FirstOrDefault();

        public event System.Action<Object[]> afterFoundObjects;
        public event System.Action<Object[]> afterFilteredObjects;
        public event System.Action<IStatement> afterComposedObjects; 
        public event System.Action<IStatement> beforeSendObjects; 
        public event System.Action<IStatement> afterSendObjects;
        public event System.Action onStartedPipeline;
        public event System.Action onStoppedPipeline; 

        public readonly List<Object> trackingObjects = new List<Object>();
        
        private void Awake()
        {
            if (actor == null)
                actor = GetComponent<Actor>();
            
            TrackingBehaviours = GetComponentsInChildren<TrackingBehaviour>();
            
            // Find available listeners
            Listeners = GetComponentsInChildren<Listener>();
            
            // Find available data providers
            Filters = GetComponentsInChildren<Filter>();
            
            // Find available data providers
            DataProviders = FindObjectsOfType<DataProvider>();

            var composersCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Composers.Length);
            var hooksCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Hooks.Length);
            
            DebugLog.OmiLAXR.Print($"Started Pipeline {name} with {Listeners.Length} listeners, {Filters.Length} filters, {composersCount} composers, {hooksCount} hooks and {DataProviders.Length} data providers" );
        }

        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"(Pipeline {name}) " + message);

        private void Start()
        {
            // 1) Start listening for events
            foreach (var listener in Listeners.Where(l => l.enabled))
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
            objects = Filters.Aggregate(objects, (gos, filter) => filter.Pass(gos));
            afterFilteredObjects?.Invoke(objects);
            
            Log($"Filtered {found - objects.Length} objects.");
            
            trackingObjects.AddRange(objects);
        }
    }
}