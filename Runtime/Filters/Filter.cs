using UnityEngine;

namespace OmiLAXR.Pipelines.Filters
{
    public abstract class Filter : MonoBehaviour
    {
        public abstract Object[] Pass(Object[] gos);

    }
}