using System;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Composers
{
    public abstract class Composer<T> : DataProviderPipelineComponent, IComposer
        where T : PipelineComponent, ITrackingBehaviour
    {
        [HideInInspector] public T[] trackingBehaviours;
        /// <summary>
        /// First matching tracking behaviour.
        /// </summary>
        [HideInInspector, Obsolete("Use tracking behaviour sender from TrackingBehaviourEvent.")] public T trackingBehaviour;

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
        
        public void SendStatement(ITrackingBehaviour statementOwner, IStatement statement, bool immediate = false)
        {
            if (!IsEnabled)
                return;
            statement.SetActor(statementOwner.GetActor());
            statement.SetInstructor(statementOwner.GetInstructor());
            AfterComposed?.Invoke(this, statement, immediate);
        }
        /// <summary>
        /// Send a statement to the pipeline. This shall only be used, if only one tracking behaviour is attached to the composer.
        /// Recommended to use SendStatement(TrackingBehaviour, IStatement) instead.
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="immediate"></param>
        [Obsolete("Recommended to use SendStatement(TrackingBehaviour statementOwner, IStatement stmt, bool immediate) instead.")]
        protected void SendStatement(IStatement statement, bool immediate = false)
        {
            SendStatement(trackingBehaviours[0], statement, immediate);;
        }
        [Obsolete("Use SendStatement(TrackingBehaviour, IStatement, bool immediate) instead. This method will be removed in a future version.", true)]
        protected void SendStatementImmediate(IStatement statement)
            => SendStatement(statement, immediate: true);
        
        protected override void Awake()
        {
            base.Awake();
            
            trackingBehaviours = GetTrackingBehaviours<T>(false);
            if (trackingBehaviours.Length < 1)
            {
                Debug.LogWarning($"{GetType().Name} was disabled as no tracking behaviours where found.");
            }
        }

        protected abstract void Compose(T tb);
    }
}