using UnityEngine;

namespace OmiLAXR
{
    [DisallowMultipleComponent]
    [AddComponentMenu("OmiLAXR / Tracking / OmiLAXR Pipeline (Main)")]
    public class OmiLAXR_Pipeline : Pipeline.Pipeline
    {
        protected override bool IsTriggered()
            => true;
    }

}