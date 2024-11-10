using System.Linq;
using Object = UnityEngine.Object;

namespace OmiLAXR.Listeners
{
    public abstract class Listener : ActorPipelineComponent
    {
        public event System.Action<Object[]> OnFoundObjects;
        public abstract void StartListening();

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
            var objects = FindObjects<T>(includeInactive);
            Found(objects);
        }
        
        protected void Found<T>(params T[] objects) where T : Object
        {
            if (!enabled || objects == null || objects.Length == 0)
                return;
            OnFoundObjects?.Invoke(objects.Select(o => o as Object).ToArray());
        }
    }
}