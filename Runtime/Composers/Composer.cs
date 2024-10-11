using System;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class Composer : PipelineComponent
    {
        private void OnEnable()
        {
            
        }
        
        protected abstract Author GetAuthor();
        public virtual bool IsHigherComposer => false;
        public event ComposerAction<IStatement> AfterComposed;
        protected static T GetTrackingBehaviour<T>(bool includeInactive = false) where T : TrackingBehaviour => FindObjectOfType<T>(includeInactive);
        protected static T GetPipeline<T>() where T : Pipeline => FindObjectOfType<T>(true);
        
        protected void SendStatement(IStatement statement)
        {
            AfterComposed?.Invoke(this, statement);
        }
    }
}