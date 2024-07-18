using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipeline.Stages.ObjectFilters
{
    public class BlacklistFilter : PipelineJob<GameObject, GameObject>
    {
        public List<string> blacklist;
        public override PipelineData<GameObject> Pass(PipelineData<GameObject> data)
        {
            return data.Filter(d => blacklist.Contains(d.name));
        }
    }
}