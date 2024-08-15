using System;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class StatementComposer : MonoBehaviour
    {
        public event Action<IStatement> afterComposed;
        protected static T GetTrackingBehaviour<T>() where T : TrackingBehaviour => FindObjectOfType<T>();
        
        protected void SendStatement(IStatement statement)
        {
            afterComposed?.Invoke(statement);
        }
    }
}