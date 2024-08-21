using System.Collections.Generic;
using System.Linq;
using OmiLAXR.Composers;
using OmiLAXR.Listeners;
using OmiLAXR.Filters;
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

        public readonly List<Listener> Listeners = new List<Listener>();
        public readonly List<DataProvider> DataProviders = new List<DataProvider>();
        public readonly List<TrackingBehaviour> TrackingBehaviours = new List<TrackingBehaviour>();
        public readonly List<Filter> Filters = new List<Filter>();
        
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

        public event System.Action<Pipeline> AfterInit;
        public event System.Action<Pipeline> AfterStarted;
        public event System.Action<Pipeline> OnCollect; 
        public event System.Action<Object[]> AfterFoundObjects;
        public event System.Action<Object[]> AfterFilteredObjects;
        public event System.Action<IStatement> AfterComposedObjects; 
        public event System.Action<IStatement> BeforeSendObjects; 
        public event System.Action<IStatement> AfterSendObjects;
        public event System.Action OnStartedPipeline;
        public event System.Action OnStoppedPipeline; 

        public readonly List<Object> trackingObjects = new List<Object>();

        public void Add(Listener listener)
            => this.Listeners.Add(listener);

        public void Add(Filter filter)
            => this.Filters.Add(filter);

        public void Add(TrackingBehaviour trackingBehaviour)
            => this.TrackingBehaviours.Add(trackingBehaviour);
        
        private void Awake()
        {
            if (actor == null)
                actor = GetComponent<Actor>();
            
            TrackingBehaviours.AddRange(GetComponentsInChildren<TrackingBehaviour>());
            
            // Find available listeners
            Listeners.AddRange(GetComponentsInChildren<Listener>());
            
            // Find available data providers
            Filters.AddRange(GetComponentsInChildren<Filter>());
            
            // Find available data providers
            DataProviders.AddRange(FindObjectsOfType<DataProvider>());

            var composersCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Composers.Count);
            var hooksCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Hooks.Count);
            
            OnCollect?.Invoke(this);
            
            DebugLog.OmiLAXR.Print($"Initialized Pipeline {name} with {Listeners.Count} listeners, {Filters.Count} filters, {composersCount} composers, {hooksCount} hooks and {DataProviders.Count} data providers" );
            
            AfterInit?.Invoke(this);
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
            
            AfterStarted?.Invoke(this);
        }

        private void FoundObjects(UnityEngine.Object[] objects)
        {
            AfterFoundObjects?.Invoke(objects);

            var found = objects.Length;
            Log($"Found {found} objects.");

            // 2) apply all filters
            objects = Filters.Aggregate(objects, (gos, filter) => filter.Pass(gos));
            AfterFilteredObjects?.Invoke(objects);
            
            Log($"Filtered {found - objects.Length} objects.");
            
            trackingObjects.AddRange(objects);
        }

        protected virtual void OnAfterComposedObjects(IStatement obj)
        {
            AfterComposedObjects?.Invoke(obj);
        }

        protected virtual void OnBeforeSendObjects(IStatement obj)
        {
            BeforeSendObjects?.Invoke(obj);
        }
    }
}