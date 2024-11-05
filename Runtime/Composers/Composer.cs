using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class Composer<T> : DataProviderPipelineComponent, IComposer
        where T : PipelineComponent, ITrackingBehaviour
    {
        [HideInInspector] public T trackingBehaviour;

        private void OnEnable()
        {
        }

        public bool IsEnabled => enabled;

        public abstract Author GetAuthor();
        public virtual bool IsHigherComposer => false;
        public event ComposerAction<IStatement, bool> AfterComposed;

        protected static TB GetTrackingBehaviour<TB>(bool includeInactive = false)
            where TB : Object, ITrackingBehaviour => FindObjectOfType<TB>(includeInactive);

        protected void SendStatement(IStatement statement, bool immediate = false)
        {
            if (!IsEnabled)
                return;
            AfterComposed?.Invoke(this, statement, immediate);
        }
        protected void SendStatementImmediate(IStatement statement)
            => SendStatement(statement, immediate: true);

        protected override void Awake()
        {
            base.Awake();
            
            if (!IsEnabled)
                return;
            trackingBehaviour = GetTrackingBehaviour<T>(false);
            if (trackingBehaviour) 
                Compose(trackingBehaviour);
        }

        protected abstract void Compose(T tb);
    }
}