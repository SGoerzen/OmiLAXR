using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OmiLAXR.Composers;
using OmiLAXR.Listeners;
using OmiLAXR.Filters;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public readonly Dictionary<string, List<EventInfo>> Actions = new Dictionary<string, List<EventInfo>>();
        public readonly Dictionary<string, List<EventInfo>> Gestures = new Dictionary<string, List<EventInfo>>();
        
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
        public event System.Action<Pipeline> BeforeStoppedPipeline; 

        public readonly List<Object> trackingObjects = new List<Object>();

        public void Add(PipelineComponent comp)
        {
            var type = comp.GetType();
            if (type == typeof(Listener))
                Listeners.Add(comp as Listener);
            else if (type == typeof(Filter))
                Filters.Add(comp as Filter);
            else if (type == typeof(TrackingBehaviour))
                TrackingBehaviours.Add(comp as TrackingBehaviour);
        }
        public void Add(Listener listener)
            => Listeners.Add(listener);

        public void Add(Filter filter)
            => Filters.Add(filter);

        public void Add(TrackingBehaviour trackingBehaviour)
            => TrackingBehaviours.Add(trackingBehaviour);
        
        protected void Awake()
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
            
            Log($"Initialized with {Listeners.Count} listeners, {Filters.Count} filters, {composersCount} composers, {hooksCount} hooks and {DataProviders.Count} data providers" );
            
            AfterInit?.Invoke(this);
        }

        protected void CollectGesturesAndActions()
        {
            var tbs = TrackingBehaviours.ToArray();

            foreach (var ts in tbs)
            {
                var events = GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);
                    
                foreach (var ev in events)
                {
                    var actionAttrs = ev.GetCustomAttributes<ActionAttribute>();
                    var gestureAttrs = ev.GetCustomAttributes<GestureAttribute>();

                    foreach (var actionAttr in actionAttrs)
                    {
                        if (Actions.ContainsKey(actionAttr.Name))
                            Actions[actionAttr.Name].Add(ev);
                        else 
                            Actions.Add(actionAttr.Name, new List<EventInfo>() { ev });
                    }

                    foreach (var gestureAttr in gestureAttrs)
                    {
                        if (Gestures.ContainsKey(gestureAttr.Name))
                            Gestures[gestureAttr.Name].Add(ev);
                        else 
                            Gestures.Add(gestureAttr.Name, new List<EventInfo>() { ev });
                    }
                }
            }
        }

        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"<{GetType().Name}> " + message);

        private void OnEnable()
        {
            CollectGesturesAndActions();
            trackingObjects.Clear();

            // 1) Start listening for events
            foreach (var listener in Listeners.Where(l => l.enabled))
            {
                listener.onFoundObjects += FoundObjects;
                listener.StartListening();
            }
            
            Log($"Started Pipeline with {trackingObjects.Count} tracking target objects...");
            AfterStarted?.Invoke(this);
        }

        private void OnDisable()
        {
            BeforeStoppedPipeline?.Invoke(this);

            Log("Stopped Pipeline!");
        }

        public void StartPipeline()
        {
            gameObject.SetActive(true);
        }

        public void StopPipeline()
        {
            gameObject.SetActive(false);
        }

        private void FoundObjects(Object[] objects)
        {
            AfterFoundObjects?.Invoke(objects);

            // 2) apply all filters
            objects = Filters.Where(f => f.enabled).Aggregate(objects, (gos, filter) => filter.Pass(gos));
            AfterFilteredObjects?.Invoke(objects);
            
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