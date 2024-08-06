using UnityEngine;

namespace OmiLAXR.Data
{
    public abstract class StatementComposer : MonoBehaviour
    {
        protected static T GetTrackingBehaviour<T>() where T : TrackingBehaviour => FindObjectOfType<T>();
    }
}