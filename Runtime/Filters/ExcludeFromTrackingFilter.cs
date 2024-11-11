using System.ComponentModel;
using OmiLAXR.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Filters
{
    [AddComponentMenu("OmiLAXR / 2) Filters / <Exclude From Tracking> Filter")]
    [Description("Filters found objects that have the component <Exclude From Tracking>.")]
    public class ExcludeFromTrackingFilter : Filter
    {
        public override Object[] Pass(Object[] objects)
        {
            var b = objects.Exclude<ExcludeFromTracking>();
            return b;
        }
    }
}