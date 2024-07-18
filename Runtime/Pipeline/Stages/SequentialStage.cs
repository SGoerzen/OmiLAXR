using System.Linq;

namespace OmiLAXR.Pipeline.Stages
{
    public class SequentialStage<TInput, TOutput> : PipelineStage<TInput, TOutput>
    {
        public override PipelineData<TOutput> Pass(PipelineData<TInput> input)
        {
            return Jobs.Aggregate(input, (cur, next) => next.Pass(cur).ConvertTo<TInput>())
                .ConvertTo<TOutput>();
        }
    }
}