using System;
using System.Collections.Generic;
using System.Linq;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Composers
{
    [DefaultExecutionOrder(-100)]
    public abstract class Composer<T> : DataProviderPipelineComponent, IComposer
        where T : PipelineComponent, ITrackingBehaviour
    {
        [HideInInspector] public T[] trackingBehaviours;

        protected override void OnEnable()
        {
            base.OnEnable();

            _name = GetType().Name.Replace("Composer", "");

            trackingBehaviours = GetTrackingBehaviours<T>();

            foreach (var trackingBehaviour in trackingBehaviours)
                Compose(trackingBehaviour);

            HandleWaitList();
        }

        public abstract Author GetAuthor();

        private string _name;
        public virtual string GetName() => _name;
        public virtual bool IsHigherComposer => false;
        public event ComposerAction<IStatement> AfterComposed;

        private List<IStatement> _waitList = new List<IStatement>();

        protected static TB[] GetTrackingBehaviours<TB>(bool includeInactive = false)
            where TB : Object, ITrackingBehaviour => FindObjects<TB>(includeInactive);

        [Obsolete(
            "Use SendStatement(ITrackingBehaviour, IStatement) instead. Immediate is not needed anymore due efficient thread queue handling.",
            true)]
        protected void SendStatement(ITrackingBehaviour statementOwner, IStatement statement, bool immediate)
            => SendStatement(statementOwner, statement);

        protected void SendStatement(ITrackingBehaviour statementOwner, IStatement statement)
        {
            statement.SetOwner(statementOwner);
            statement.SetComposer(this);

            if (AfterComposed?.GetInvocationList().Length < 1)
            {
                print("Enqueued statement: " + statement.ToShortString() + " in waitlist.");
                _waitList.Add(statement);
                return;
            }
            
            AfterComposed?.Invoke(this, statement);
        }

        [Obsolete(
            "Use SendStatement(ITrackingBehaviour, IStatement) instead. Immediate is not needed anymore due efficient thread queue handling.",
            true)]
        protected void SendStatementImmediate(ITrackingBehaviour statementOwner, IStatement statement)
            => SendStatement(statementOwner, statement, immediate: true);

        [Obsolete("Use SendStatement(ITrackingBehaviour, IStatement, bool) instead.")]
        protected void SendStatement(IStatement statement, bool immediate = false)
        {
            SendStatement(trackingBehaviours.First(), statement, immediate);
        }

        [Obsolete("Use SendStatementImmediate(ITrackingBehaviour, IStatement) instead.")]
        protected void SendStatementImmediate(IStatement statement)
            => SendStatement(statement, immediate: true);

        protected abstract void Compose(T tb);

        private void HandleWaitList()
        {
            if (_waitList.Count > 0 && AfterComposed?.GetInvocationList().Length > 0)
            {
                foreach (var statement in _waitList)
                {
                    AfterComposed?.Invoke(this, statement);
                }

                _waitList.Clear();
            }
        }

        private void Update()
        {
            HandleWaitList();
        }
    }
}