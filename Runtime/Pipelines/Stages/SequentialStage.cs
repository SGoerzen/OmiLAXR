using System.Linq;

namespace OmiLAXR.Pipelines.Stages
{
    public class SequentialStage<TInput, TOutput> : PipelineStage<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        public override PipelineData<TOutput> Pass(PipelineData<TInput> input)
        {
            return Jobs.Aggregate(input, (cur, next) => next.Pass(cur).ConvertTo<TInput>())
                .ConvertTo<TOutput>();
        }
    }
}