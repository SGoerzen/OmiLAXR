using System.Linq;

namespace OmiLAXR.Pipeline.Stages
{
    public class SequentialStage : PipelineStage
    {
        public override PipelineData Pass(PipelineData input)
        {
            return jobs.Aggregate(input, (data, next) => next.Pass(data)).Clone();
        }
    }
}