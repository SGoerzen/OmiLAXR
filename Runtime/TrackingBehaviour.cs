
using UnityEngine;

namespace OmiLAXR
{
    public abstract class TrackingBehaviour : PipelineStage
    {
        public event System.Action<UnityEngine.Object[]> onBind;
        protected override void Awake()
        {
            base.Awake();
            pipeline.afterFilteredObjects += Pipe;
        }

        protected override void Pipe(Object[] objects)
        {
            onBind?.Invoke(objects);
        }

    }
}