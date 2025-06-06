using System;
using System.Linq;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Composers
{
    public abstract class Composer<T> : DataProviderPipelineComponent, IComposer
        where T : PipelineComponent, ITrackingBehaviour
    {
        [HideInInspector] public T[] trackingBehaviours;

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
        
        protected static TB[] GetTrackingBehaviours<TB>(bool includeInactive = false)
            where TB : Object, ITrackingBehaviour => FindObjects<TB>(includeInactive);

        protected void SendStatement(ITrackingBehaviour statementOwner, IStatement statement, bool immediate = false)
        {
            if (!IsEnabled)
                return;
            
            statement.SetOwner(statementOwner);
            statement.SetComposer(this);
            
            AfterComposed?.Invoke(this, statement, immediate);
        }
        protected void SendStatementImmediate(ITrackingBehaviour statementOwner, IStatement statement, bool immediate = false)
            => SendStatement(statementOwner, statement, immediate: true);
        
        [Obsolete("Use SendStatement(ITrackingBehaviour, IStatement, bool) instead.")]
        protected void SendStatement(IStatement statement, bool immediate = false)
        {
            SendStatement(trackingBehaviours.First(), statement, immediate);
        }
        [Obsolete("Use SendStatementImmediate(ITrackingBehaviour, IStatement) instead.")]
        protected void SendStatementImmediate(IStatement statement)
            => SendStatement(statement, immediate: true);

        protected override void Awake()
        {
            base.Awake();
            
            if (!IsEnabled)
                return;
            
            trackingBehaviours = GetTrackingBehaviours<T>(false);
        }

        protected virtual void Start()
        {
            foreach (var trackingBehaviour in trackingBehaviours)
                Compose(trackingBehaviour);
        }

        protected abstract void Compose(T tb);
    }
}