using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipeline.Stages.ObjectFilters
{
    public class BlacklistFilter : PipelineJob
    {
        public List<string> blacklist;
        public override PipelineData Pass(PipelineData data)
        {
            return data.Filter<GameObject>(d => blacklist.Contains(d.name));
        }
    }
}