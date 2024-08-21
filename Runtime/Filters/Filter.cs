using UnityEngine;

namespace OmiLAXR.Filters
{
    public abstract class Filter : MonoBehaviour
    {
        public abstract Object[] Pass(Object[] gos);
    }
}