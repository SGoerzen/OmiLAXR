using System.Collections.Generic;
using System.Linq;

namespace OmiLAXR.Pipeline.Stages
{
    public class UnionStage<TInput, TOutput> : PipelineStage<TInput, TOutput>
    {
        public override PipelineData<TOutput> Pass(PipelineData<TInput> input)
        {
            IEnumerable<TOutput> objects = new List<TOutput>();
            objects = Jobs
                .Select(job => job.Pass(input))
                .Aggregate(objects, (current, o) => current.Union(o.Data));
            return PipelineData<TOutput>.From(objects.ToArray());
        }
    }
}