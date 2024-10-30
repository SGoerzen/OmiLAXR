using Object = UnityEngine.Object;

namespace OmiLAXR.Listeners
{
    public abstract class Listener : PipelineComponent
    {
        protected Pipeline pipeline { get; private set; }
        public Actor GetActor() => pipeline.actor;
        public event System.Action<Object[]> OnFoundObjects;
        public abstract void StartListening();
        
        protected virtual void Awake()
        {
            pipeline = GetComponentInParent<Pipeline>(true);
        }

        protected void OnEnable()
        {
            
        }

        /// <summary>
        /// Finds a group of objects and provide them to pipeline.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <typeparam name="T"></typeparam>
        protected void Detect<T>(bool includeInactive = false) where T : Object
        {
            var objects = FindObjectsOfType<T>(includeInactive);
            Found(objects);
        }
        
        protected void Found<T>(params T[] objects) where T : Object
        {
            if (!enabled || objects == null || objects.Length == 0)
                return;
            OnFoundObjects?.Invoke(objects);
        }
    }
}