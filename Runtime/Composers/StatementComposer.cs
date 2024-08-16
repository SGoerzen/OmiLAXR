using System;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class StatementComposer : MonoBehaviour
    {
        private void OnEnable()
        {
            
        }

        protected abstract Author GetAuthor();
        public virtual bool IsHigherComposer => false;
        public event Action<IStatement> afterComposed;
        protected static T GetTrackingBehaviour<T>() where T : TrackingBehaviour => FindObjectOfType<T>();
        protected static T GetPipeline<T>() where T : Pipeline => FindObjectOfType<T>();
        
        protected void SendStatement(IStatement statement)
        {
            afterComposed?.Invoke(statement);
        }
    }
}