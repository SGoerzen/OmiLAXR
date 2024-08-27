using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    public abstract class Filter : MonoBehaviour
    {
        protected void OnEnable()
        {
            
        }

        public abstract Object[] Pass(Object[] gos);
    }
}