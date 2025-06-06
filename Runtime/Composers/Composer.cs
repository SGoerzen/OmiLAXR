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
            _name = GetType().Name.Replace("Composer", "");
        }

        public bool IsEnabled => enabled;

        public abstract Author GetAuthor();

        private string _name;
        public virtual string GetName() => _name;

        public virtual bool IsHigherComposer => false;
        public event ComposerAction<IStatement, bool> AfterComposed;
        
        protected static TB GetTrackingBehaviour<TB>(bool includeInactive = false)
            where TB : Object, ITrackingBehaviour => FindObject<TB>(includeInactive);
        
        protected void SendStatement(IStatement statement, bool immediate = false)
        {
            if (!IsEnabled)
                return;
            statement.SetComposer(this);
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
        }

        protected virtual void Start()
        {
            if (trackingBehaviour) 
                Compose(trackingBehaviour);
        }

        protected abstract void Compose(T tb);
    }
}