using OmiLAXR.TrackingBehaviours;
using UnityEngine;

namespace OmiLAXR.Composers
{
    public abstract class StatementComposer : MonoBehaviour
    {
        protected static T GetTrackingBehaviour<T>() where T : TrackingBehaviour => FindObjectOfType<T>();
    }
}