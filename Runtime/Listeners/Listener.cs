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
            pipeline = GetComponentInParent<Pipeline>();
        }

        protected void OnEnable()
        {
            
        }

        protected void Found<T>(params T[] objects) where T : Object
        {
            OnFoundObjects?.Invoke(objects);
        }
    }
}