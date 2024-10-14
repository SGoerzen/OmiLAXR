using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public interface IComposer
    {
        event ComposerAction<IStatement, bool> AfterComposed;
        bool IsHigherComposer { get; }
        bool IsEnabled { get; }
        Author GetAuthor();
    }

    public abstract class Composer<T> : PipelineComponent, IComposer
        where T : TrackingBehaviour
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
            where TB : TrackingBehaviour => FindObjectOfType<TB>(includeInactive);

        protected void SendStatement(IStatement statement, bool immediate = false)
        {
            AfterComposed?.Invoke(this, statement, immediate);
        }

        protected virtual void Awake()
        {
            trackingBehaviour = GetTrackingBehaviour<T>(true);
            Compose(trackingBehaviour);
        }

        protected abstract void Compose(T tb);
    }
}