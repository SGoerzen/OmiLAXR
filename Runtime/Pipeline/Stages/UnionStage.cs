using System.Collections.Generic;
using System.Linq;

namespace OmiLAXR.Pipeline.Stages
{
    public class UnionStage : PipelineStage
    {
        public override PipelineData Pass(PipelineData input)
        {
            IEnumerable<object> objects = new List<object>();
            objects = jobs
                .Select(job => job.Pass(input))
                .Aggregate(objects, (current, o) => current.Union(o.Data));
            return PipelineData.From(objects);
        }
    }
}