using System;
using UnityEngine;

namespace OmiLAXR.Data
{
    public abstract class StatementComposer : MonoBehaviour
    {
        protected MainTrackingBehaviour mainTrackingBehaviour => MainTrackingBehaviour.Instance;
    }
}