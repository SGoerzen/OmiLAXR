using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    public abstract class Filter : ActorPipelineComponent
    {
        protected void OnEnable()
        {
            
        }

        public abstract Object[] Pass(Object[] gos);
    }
}