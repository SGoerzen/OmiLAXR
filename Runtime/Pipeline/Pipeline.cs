using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipeline
{
    /// <summary>
    /// Pipeline System containing many stages.
    /// </summary>
    public abstract class Pipeline : MonoBehaviour
    {
        public readonly PipelineStages<object, object> Stages = new PipelineStages<object, object>();

        private void Start()
        {
            Debug.Log("[OmiLAXR] Pipeline started " + GetType());
        }

        protected abstract bool IsTriggered();
    }
}