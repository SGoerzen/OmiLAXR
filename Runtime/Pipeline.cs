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
    [AddComponentMenu("OmiLAXR / 0) Pipelines / Pipeline")]
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

        public List<PipelineExtension> Extensions = new List<PipelineExtension>();

        public ActorDataProvider[] ActorDataProviders { get; protected set; }

        public static T GetPipeline<T>() where T : Pipeline
            => FindObjectOfType<T>();

        public static Pipeline GetAll() => FindObjectOfType<Pipeline>();
        
        public T GetDataProvider<T>() where T : DataProvider
            => DataProviders.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetTrackingBehaviour<T>() where T : PipelineComponent, ITrackingBehaviour
            => TrackingBehaviours.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetFilters<T>() where T : Filter
            => Filters.OfType<T>().Select(dp => dp as T).FirstOrDefault();
        
        public T GetListener<T>() where T : Listener
            => Listeners.OfType<T>().Select(listener => listener as T).FirstOrDefault();

        public event Action<Pipeline> AfterInit;
        public event Action<Pipeline> AfterStarted;
        public event Action<Pipeline> OnCollect; 
        public event Action<Object[]> AfterFoundObjects;
        public event Action<Object[]> AfterFilteredObjects;
        public event ComposerAction<IStatement, bool> AfterComposedStatement; 
        public event EndpointAction<IStatement> BeforeSendStatement; 
        public event EndpointAction<IStatement> AfterSendStatement;
        public event Action<Pipeline> BeforeStoppedPipeline; 
        public event Action<Pipeline> AfterStoppedPipeline; 

        public readonly List<Object> trackingObjects = new List<Object>();

        public void Add(PipelineComponent comp)
        {
            var type = comp.GetType();
            if (type.IsSubclassOf(typeof(Listener)))
                Listeners.Add(comp as Listener);
            else if (type.IsSubclassOf(typeof(Filter)))
                Filters.Add(comp as Filter);
            else if (type.IsSubclassOf(typeof(ITrackingBehaviour)))
                TrackingBehaviours.Add(comp as ITrackingBehaviour);
            else if (type.IsSubclassOf(typeof(PipelineExtension)))
            {
                var ext = comp as PipelineExtension;
                ext!.Extend(this);
                Extensions.Add(ext);
            }
        }

        private Actor FindActor()
        {
            var actorGroup = GetComponent<ActorGroup>();
            return actorGroup ?? GetComponent<Actor>();
        }
        
        protected void Awake()
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
            DataProviders.AddRange(FindObjectsOfType<DataProvider>(false));
            
            // Bind after and before send events
            foreach (var dp in DataProviders)
            {
                foreach (var c in dp.Composers.Where(c => c.IsEnabled))
                {
                    c.AfterComposed += (composer, statement, immediate) =>
                    {
                        AfterComposedStatement?.Invoke(composer, statement, immediate);
                    };
                }
                foreach (var ep in dp.Endpoints.Where(e => e.enabled))
                {
                    ep.OnSendingStatement += (endpoint, statement) =>
                    {
                        BeforeSendStatement?.Invoke(endpoint, statement);
                    };
                    ep.OnSentStatement += (endpoint, statement) =>
                    {
                        AfterSendStatement?.Invoke(endpoint, statement);
                    };
                }
            }

            var composersCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Composers.Count);
            var hooksCount = DataProviders.Aggregate(0, (i, provider) => i + provider.Hooks.Count);
            
            OnCollect?.Invoke(this);
            
            Log($"Initialized with {Listeners.Count} listeners, {Filters.Count} filters, {composersCount} composers, {hooksCount} hooks and {DataProviders.Count} data providers" );
            
            AfterInit?.Invoke(this);
        }
        
        protected void CollectGesturesAndActions()
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
            CollectGesturesAndActions();
            trackingObjects.Clear();

            // 1) Start listening for events
            foreach (var listener in Listeners.Where(l => l.enabled))
            {
                listener.OnFoundObjects += FoundObjects;
                listener.StartListening();
            }
            
            Log($"Started Pipeline with {trackingObjects.Count} tracking target objects...");
            AfterStarted?.Invoke(this);
        }

        private void OnDisable()
        {
            BeforeStoppedPipeline?.Invoke(this);
            
            trackingObjects.Clear();
            
            // clear listenings
            foreach (var listener in Listeners.Where(l => l.enabled))
            {
                listener.OnFoundObjects -= FoundObjects;
            }

            Log("Stopped Pipeline!");
            
            AfterStoppedPipeline?.Invoke(this);
        }

        public void StartPipeline()
        {
            gameObject.SetActive(true);
        }

        public void StopPipeline()
        {
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