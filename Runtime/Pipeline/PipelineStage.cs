using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipeline
{
    public class PipelineStages<TInput, TOutput> : List<PipelineStage<TInput, TOutput>> {}
    public abstract class PipelineStage<TInput, TOutput> : MonoBehaviour
    {
        public readonly PipelineJobs<TInput, TOutput> Jobs = new PipelineJobs<TInput, TOutput>();
        public abstract PipelineData<TOutput> Pass(PipelineData<TInput> input);
    }
}