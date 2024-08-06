using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OmiLAXR.Pipelines.Filters
{
    public class BlacklistFilter : Filter
    {
        public List<string> blacklist;
        public override Object[] Pass(Object[] objects)
        {
            return objects.Where(go => !blacklist.Contains(go.GetTrackingName())).ToArray();
        }
    }
}