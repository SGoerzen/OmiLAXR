using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OmiLAXR.Composers;
using OmiLAXR.Endpoints;
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
    [AddComponentMenu("OmiLAXR / Core / Pipeline")]
    [DefaultExecutionOrder(0)]
    public class Pipeline : MonoBehaviour
    {
        public bool IsRunning => gameObject.activeSelf;
        public Actor actor;
        public Instructor instructor;

        public readonly List<Listener> Listeners = new List<Listener>();
        public readonly List<DataProvider> DataProviders = new List<DataProvider>();
        public readonly List<ITrackingBehaviour> TrackingBehaviours = new List<ITrackingBehaviour>();
        public readonly List<Filter> Filters = new List<Filter>();

        public readonly Dictionary<string, List<ITrackingBehaviourEvent>> Actions = new Dictionary<string, List<ITrackingBehaviourEvent>>();
        public readonly Dictionary<string, List<ITrackingBehaviourEvent>> Gestures = new Dictionary<string, List<ITrackingBehaviourEvent>>();

        public List<IPipelineExtension> Extensions = new List<IPipelineExtension>();

        public ActorDataProvider[] ActorDataProviders { get; protected set; }

        #if UNITY_2023_1_OR_NEWER
        public static T GetPipeline<T>() where T : Pipeline
            => FindFirstObjectByType<T>();
        #else
        public static T GetPipeline<T>() where T : Pipeline
            => FindObjectOfType<T>();
        #endif

        #if UNITY_2023_1_OR_NEWER 
        public static Pipeline GetAll() => FindFirstObjectByType<Pipeline>();
        #else
        public static Pipeline GetAll() => FindObjectOfType<Pipeline>();
        #endif
        
        public T GetDataProvider<T>() where T : DataProvider
            => DataProviders.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetTrackingBehaviour<T>() where T : PipelineComponent, ITrackingBehaviour
            => TrackingBehaviours.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetFilters<T>() where T : Filter
            => Filters.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetListener<T>() where T : Listener
            => Listeners.OfType<T>().Select(listener => listener as T).FirstOrDefault();

        public event Action<Pipeline> AfterInit;
        public event Action<Pipeline> BeforeStartedPipeline;
        public event Action<Pipeline> AfterStartedPipeline;
        public event Action<Pipeline> OnCollect; 
        public event Action<Object[]> AfterFoundObjects;
        public event Action<Object[]> AfterFilteredObjects;
        public event ComposerAction<IStatement> AfterComposedStatement; 
        public event EndpointAction<IStatement> BeforeSendStatement; 
        public event EndpointAction<IStatement> AfterSendStatement;
        public event Action<Pipeline> BeforeStoppedPipeline; 
        public event Action<Pipeline> AfterStoppedPipeline; 

        public readonly List<Object> trackingObjects = new List<Object>();
        private bool _cleanupCalled = false;
        private bool _startupCalled = false;

        public void Add(PipelineComponent comp)
        {
            var type = comp.GetType();
            if (type.IsSubclassOf(typeof(Listener)))
            {
                if (!Listeners.Contains(comp))
                    Listeners.Add(comp as Listener);
            }
            else if (type.IsSubclassOf(typeof(Filter)))
            {
                if (!Filters.Contains(comp))
                    Filters.Add(comp as Filter);
            }
            else if (type.IsSubclassOf(typeof(ITrackingBehaviour)))
            {
                var tb = comp as ITrackingBehaviour;
                if (!TrackingBehaviours.Contains(tb))
                    TrackingBehaviours.Add(tb);
            }
            else if (type.IsSubclassOf(typeof(IPipelineExtension)))
            {
                var pc = comp as IPipelineComponent;
                if (pc != null)
                {
                    if (!Extensions.Contains(pc))
                    {
                        var ext = pc as IPipelineExtension;
                        ext?.Extend(this);
                        Extensions.Add(ext);
                    }
                }
            }
        }

        private Actor FindActor()
        {
            var actorGroup = GetComponent<ActorGroup>();
            return actorGroup ?? GetComponent<Actor>();
        }
        
        private void Init()
        {
            if (actor == null)
                actor = FindActor();

            ActorDataProviders = GetComponentsInChildren<ActorDataProvider>(false);
            
            TrackingBehaviours.AddRange(GetComponentsInChildren<ITrackingBehaviour>(false));
            
            // Find available listeners
            Listeners.AddRange(GetComponentsInChildren<Listener>(false));
            
            // Find available data providers
            Filters.AddRange(GetComponentsInChildren<Filter>(false));
            
            // Find available data providers
#if UNITY_2023_1_OR_NEWER
            DataProviders.AddRange(FindObjectsByType<DataProvider>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
#else
            DataProviders.AddRange(FindObjectsOfType<DataProvider>(false));
#endif
            
            // Bind after and before send events
            foreach (var dp in DataProviders)
            {
                foreach (var c in dp.Composers)
                {
                    c.AfterComposed += AfterComposed;
                }
                foreach (var ep in dp.Endpoints)
                {
                    ep.OnSendingStatement += OnSendingStatement;
                    ep.OnSentStatement += OnSentStatement;
                }
            }

            var composersCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Composers.Count);
            var hooksCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Hooks.Count);
            
            OnCollect?.Invoke(this);
            
            Log($"Initialized with {Listeners.Count} listeners, {Filters.Count} filters, {TrackingBehaviours.Count} tracking behaviours, {composersCount} composers, {hooksCount} hooks and {DataProviders.Count} data providers" );
            
            AfterInit?.Invoke(this);
        }

        private void AfterComposed(IComposer composer, IStatement statement)
            => AfterComposedStatement?.Invoke(composer, statement);
        private void OnSendingStatement(Endpoint endpoint, IStatement statement)
            => BeforeSendStatement?.Invoke(endpoint, statement);
        private void OnSentStatement(Endpoint endpoint, IStatement statement)
            => AfterSendStatement?.Invoke(endpoint, statement);
        
        private void CollectGesturesAndActions()
        {
            var tbs = TrackingBehaviours.ToArray();
            Actions.Clear();
            Gestures.Clear();

            foreach (var ts in tbs)
            {
                var properties = ts.GetTrackingBehaviourEvents();
                    
                foreach (var p in properties)
                {
                    var ev = p.GetValue(ts) as ITrackingBehaviourEvent;
                    var actionAttrs = p.GetCustomAttributes<ActionAttribute>();
                    var gestureAttrs = p.GetCustomAttributes<GestureAttribute>();

                    foreach (var actionAttr in actionAttrs)
                    {
                        if (Actions.ContainsKey(actionAttr.Name))
                            Actions[actionAttr.Name].Add(ev);
                        else 
                            Actions.Add(actionAttr.Name, new List<ITrackingBehaviourEvent>() { ev });
                    }

                    foreach (var gestureAttr in gestureAttrs)
                    {
                        if (Gestures.ContainsKey(gestureAttr.Name))
                            Gestures[gestureAttr.Name].Add(ev);
                        else 
                            Gestures.Add(gestureAttr.Name, new List<ITrackingBehaviourEvent>() { ev });
                    }
                }
            }
        }

        protected void Log(string message, params object[] ps)
            => DebugLog.OmiLAXR.Print($"<{GetType().Name}> " + message);

        private void OnEnable()
        {
            Startup();
        }

        private void Startup()
        {
            if (_startupCalled)
                return;
         
            Init();
            
            _startupCalled = true;
            
            CollectGesturesAndActions();
            trackingObjects.Clear();

            // 1) Start listening for events
            foreach (var listener in Listeners)
            {
                listener.OnFoundObjects += FoundObjects;
                if (listener.enabled)
                    listener.StartListening();
            }
            
            BeforeStartedPipeline?.Invoke(this);
            Log($"Started Pipeline with {trackingObjects.Count} tracking target objects...");
            AfterStartedPipeline?.Invoke(this);
            _cleanupCalled = false;
        }
        
        private void OnDisable()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (_cleanupCalled)
                return;
            
            _cleanupCalled = true;
            BeforeStoppedPipeline?.Invoke(this);
            
            trackingObjects.Clear();
            
            // clear listenings
            foreach (var listener in Listeners.Where(l => l.enabled))
            {
                if (listener == null)
                    continue;
                listener.OnFoundObjects -= FoundObjects;
            }

            foreach (var endpoint in DataProviders.SelectMany(dp => dp.Endpoints))
            {
                if (endpoint == null || !endpoint.enabled)
                    continue;
                endpoint.StopSending();
            }

            Log("Stopped Pipeline!");
            
            AfterStoppedPipeline?.Invoke(this);
            
            foreach (var dp in DataProviders)
            {
                foreach (var c in dp.Composers)
                {
                    c.AfterComposed -= AfterComposed;
                }
                foreach (var ep in dp.Endpoints)
                {
                    ep.OnSendingStatement -= OnSendingStatement;
                    ep.OnSentStatement -= OnSentStatement;
                }
            }
            
            _startupCalled = false;
        }

        public void StartPipeline()
        {
            if (IsRunning)
                return;
            
            gameObject.SetActive(true);
            Startup();
        }

        private void OnApplicationQuit()
        {
            Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        public void StopPipeline()
        {
            if (!IsRunning)
                return;
            
            Cleanup();
            _startupCalled = false;
            
            gameObject.SetActive(false);
        }

        public void SetDisabledActions(bool disabled, IEnumerable<string> names = null)
        {
            foreach (var pair in Actions.Where(p => names == null || names.Contains(p.Key)))
            {
                pair.Value.ForEach(v => v.IsDisabled = disabled);
            }
        }
        
        public void SetDisabledGestures(bool disabled, IEnumerable<string> names = null)
        {
            foreach (var pair in Gestures.Where(p => names == null || names.Contains(p.Key)))
            {
                pair.Value.ForEach(v => v.IsDisabled = disabled);
            }
        }

        private void FoundObjects(Object[] objects)
        {
            AfterFoundObjects?.Invoke(objects);

            // 2) apply all filters
            objects = Filters.Where(f => f.enabled).Aggregate(objects, (gos, filter) => filter.Pass(gos));
            AfterFilteredObjects?.Invoke(objects);
            
            trackingObjects.AddRange(objects);
        }
    }
}