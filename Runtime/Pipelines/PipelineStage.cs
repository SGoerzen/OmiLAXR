using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipelines
{
    public class PipelineStages<TInput, TOutput> : List<PipelineStage<TInput, TOutput>> 
        where TInput : class
        where TOutput : class
    {
        
    }
    public abstract class PipelineStage<TInput, TOutput> : MonoBehaviour
        where TInput : class
        where TOutput : class
    {
        public readonly PipelineJobs<TInput, TOutput> Jobs = new PipelineJobs<TInput, TOutput>();
        public abstract PipelineData<TOutput> Pass(PipelineData<TInput> input);
    }
}