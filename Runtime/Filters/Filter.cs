using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    public abstract class Filter : PipelineComponent
    {
        protected void OnEnable()
        {
            
        }

        public abstract Object[] Pass(Object[] gos);
    }
}