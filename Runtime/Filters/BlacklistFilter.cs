using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace OmiLAXR.Filters
{
    [AddComponentMenu("OmiLAXR / 2) Filters / Blacklist Filter")]
    [Description("Filters found objects by a blacklist of tracking names. The names are get with go.GetTrackingName().")]
    public class BlacklistFilter : Filter
    {
        public List<string> blacklist;
        public override Object[] Pass(Object[] objects)
        {
            return objects.Where(go => !blacklist.Contains(go.GetTrackingName())).ToArray();
        }
    }
}